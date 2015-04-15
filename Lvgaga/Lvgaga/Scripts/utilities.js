function favorite(pk, rk, btn, callback) {
    btn.attr("disabled", "disabled");
    $.post("/api/v1/favorites/".concat(pk, "/", rk)).retry({ times: 3 }).done(function (data, textStatus, jqXHR) {
        switch (jqXHR.status) {
            case 201:
                btn.addClass("btn-selected");
                if (callback) callback(data);
                break;
            case 200:
                var res = $.parseJSON(jqXHR.getResponseHeader("X-Responded-JSON"));
                if (res.status === 401) {
                    $(location).attr("href", res.headers.location.replace(/(ReturnUrl=)(.+)/, "$1" + encodeURIComponent(location.pathname)));
                }
                break;
            default:

        }
    }).always(function (data, textStatus, jqXHR) {
        btn.removeAttr("disabled");
    });
}

function unFavorite(pk, rk, btn, callback) {
    btn.attr("disabled", "disabled");
    $.ajax({
        url: "/api/v1/favorites/".concat(pk, "/", rk),
        type: "DELETE"
    }).retry({ times: 3 }).done(function (data, textStatus, jqXHR) {
        btn.removeClass("btn-selected");
        if (callback) callback(data);
    }).always(function (data, textStatus, jqXHR) {
        btn.removeAttr("disabled");
    });
}

function getFavoriteIndex(mediaType, from, to, callback) {
    $.get("/api/v1/favorites/".concat(mediaType, "?", "from=", from, "&", "to=", to)).retry({ times: 3 }).done(function (data, textStatus, jqXHR) {
        switch (jqXHR.status) {
            case 200:
                if (callback) callback(data);
                break;
            default:
        }
    });
}

function getFavorites(mediaType, top, callback) {
    $.get("/api/v1/favorites/".concat(mediaType, "?", "top=", top)).retry({ times: 3 }).done(function (data, textStatus, jqXHR) {
        switch (jqXHR.status) {
            case 200:
                if (callback) callback(data);
                break;
            default:
        }
    });
}

function combinePath(parameters) {
    return parameters.join("/");
}

function getInvertedTicks(rowKey) {
    return rowKey.slice(2);
}

function queryAzureTable(tableSasUrl, params) {
    var uri = tableSasUrl;
    if (params.continuationToken) {
        if (params.continuationToken.NextPartitionKey) {
            uri = uri.concat("&NextPartitionKey=", params.continuationToken.NextPartitionKey);
        }
        if (params.continuationToken.NextRowKey) {
            uri = uri.concat("&NextRowKey=", params.continuationToken.NextRowKey);
        }
    }
    if (params.top) {
        uri = uri.concat("&$top=", params.top);
    }
    if (params.filter) {
        uri = uri.concat("&$filter=", encodeURIComponent(params.filter));
    }
    return $.ajax({
            type: "GET",
            datatype: "json",
            url: uri,
            beforeSend: function(xhr) {
                xhr.setRequestHeader("MaxDataServiceVersion", "3.0");
                xhr.setRequestHeader("Accept", "application/json;odata=nometadata");
            }
        })
        .retry({ times: 3 });
}

function queryAzureTableWithLoadingButton(tableSasUrl, params) {
    var btnLoad = params.btn;
    if (btnLoad) {
        btnLoad.attr("disabled", "disabled");
    }

    return queryAzureTable(tableSasUrl, params)
        .done(function(data, textStatus, jqXHR) {
            if (btnLoad) {
                var nextPartitionKey = jqXHR.getResponseHeader("x-ms-continuation-NextPartitionKey");
                var nextRowKey = jqXHR.getResponseHeader("x-ms-continuation-NextRowKey");
                if (!nextPartitionKey || !nextRowKey) {
                    btnLoad.hide();
                }
            }
        })
        .always(function(data, textStatus, jqXHR) {
            if (btnLoad) {
                btnLoad.removeAttr("disabled");
            }
        });
}