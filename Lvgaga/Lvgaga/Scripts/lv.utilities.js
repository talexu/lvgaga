﻿var lv = (function () {
    var exports = {};
    var defaultRetryTime = 3;

    exports.defaultTakingCount = 20;
    // 基础方法
    exports.singleton = function (func) {
        var instance;
        return (function () {
            return instance || (instance = func.apply(this, arguments));
        });
    };
    exports.retryExecute = function (func, handler, retry) {
        retry = retry === undefined ? defaultRetryTime - 1 : retry;
        if (retry < 0) return undefined;

        return func.apply(this, arguments).fail(function () {
            handler.apply(this, arguments).done(function () {
                exports.retryExecute(func, handler, retry - 1);
            });
        });
    };
    exports.ajaxLadda = function (func, button) {
        if (!button) return func(this, arguments);

        var l = Ladda.create(button.get(0));
        l.start();
        return func.apply(this, arguments).always(function () {
            l.stop();
        });
    };
    exports.getInvertedTicks = function (rowKey) {
        return rowKey.slice(2);
    };
    exports.getLocalTime = function (dataTime) {
        return moment.utc(dataTime).local().format("YYYY-MM-DD HH:mm:ss");
    };
    exports.getShareUri = function (p) {
        return sprintf("http://api.bshare.cn/share/sinaminiblog?url=%s&summary=%s&publisherUuid=%s&pic=%s", encodeURIComponent("http://" + window.location.host + p.Uri), encodeURIComponent(p.Summary), encodeURIComponent("35de718f-8cbf-4a01-8d69-486b3e6c3437"), encodeURIComponent(p.Pic));
    };

    // 获取Token
    exports.getToken = function (paths) {
        return $.get(sprintf("/api/v1/tokens/%s", paths.join("/"))).retry({ times: defaultRetryTime });
    };

    // 查询AzureTable
    exports.queryAzureTable = function (tableSasUrl, p) {
        var uri = tableSasUrl;
        if (p.continuationToken) {
            if (p.continuationToken.NextPartitionKey) {
                uri += "&NextPartitionKey=" + p.continuationToken.NextPartitionKey;
            }
            if (p.continuationToken.NextRowKey) {
                uri += "&NextRowKey=" + p.continuationToken.NextRowKey;
            }
        }
        if (p.top) {
            uri += "&$top=" + p.top;
        }
        if (p.filter) {
            uri += "&$filter=" + encodeURIComponent(p.filter);
        }
        if (p.select) {
            uri += "&$select=" + encodeURIComponent(p.select);
        }
        return $.ajax({
            type: "GET",
            datatype: "json",
            url: uri,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("MaxDataServiceVersion", "3.0");
                xhr.setRequestHeader("Accept", "application/json;odata=nometadata");
            }
        }).retry({ times: defaultRetryTime });
    };

    // 添加收藏
    exports.addFavorite = function (p, callback) {
        return $.post(sprintf("/api/v1/favorites/%s/%s", p.pk, p.rk)).retry({ times: defaultRetryTime }).done(function (data, textStatus, jqXhr) {
            switch (jqXhr.status) {
                case 201:
                    callback && callback(data);
                    break;
                case 200:
                    var res = $.parseJSON(jqXhr.getResponseHeader("X-Responded-JSON"));
                    if (res.status === 401) {
                        $(location).attr("href", res.headers.location.replace(/(ReturnUrl=)(.+)/, "$1" + encodeURIComponent(location.pathname)));
                    }
                    break;
                default:

            }
        });
    };
    // 移除收藏
    exports.removeFavorite = function (p, callback) {
        return $.ajax({
            url: sprintf("/api/v1/favorites/%s/%s", p.pk, p.rk),
            type: "DELETE"
        }).retry({ times: defaultRetryTime }).done(function (data) {
            callback && callback(data);
        });
    };

    return exports;
})();