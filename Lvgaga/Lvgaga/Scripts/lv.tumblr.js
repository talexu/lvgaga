lv.tumblr = (function () {
    var exports = {};

    var tumblrTemplate;
    var continuationToken;
    var sas;
    var favSas;
    var mediaType;
    var tumblrCategory;
    var from;
    var to;
    var tableNameOfTumblr;
    var tableNameOfFavorite;
    var takeCount = lv.defaultTakeCount;

    // 获取加载按钮的实例
    var getLoadingButton = lv.singleton(function () {
        return $("#btn_load");
    });
    // 获取Tumblrs的容器
    var getTumblrsDiv = lv.singleton(function () {
        return $("#div_tumblrs");
    });
    // 获取初始页面的收藏按钮
    var getFavoriteButtons = lv.singleton(function () {
        return $(".btn-favorite");
    });
    // 获取所有图片元素
    var getImages = lv.singleton(function () {
        return $("img.lazy");
    });
    // 获取所有时间元素
    var getTimes = lv.singleton(function () {
        return $(".date-tumblr");
    });
    // 获取所有Tumblr的容器
    var getTumblrContainers = lv.singleton(function () {
        return $(".container-tumblr");
    });

    // 启动图片延迟加载
    var initImgs = function () {
        getImages().lazyload({
            effect: "fadeIn"
        });
    };
    // 按本地时区格式化时间
    var initTime = function () {
        getTimes().each(function () {
            var p = $(this);
            var utc = p.text();
            p.text(lv.getLocalTime(utc));
        });
    }
    // 注册收藏按钮事件
    var initFavs = function () {
        getFavoriteButtons().on("touchend", function (event) {
            var btnCur = $(event.currentTarget);
            if (!btnCur.hasClass("btn-selected")) {
                lv.ajaxLadda(function () {
                    return lv.addFavorite({ pk: btnCur.attr("tp"), rk: btnCur.attr("rk") }, function () {
                        btnCur.addClass("btn-selected");
                    });
                }, btnCur);

            } else {
                lv.ajaxLadda(function () {
                    return lv.removeFavorite({ pk: btnCur.attr("tp"), rk: btnCur.attr("rk") }, function () {
                        btnCur.removeClass("btn-selected");
                    });
                }, btnCur);
            }
        });
    }
    // 设置重定向分享到链接
    var initShare = function () {
        getTumblrContainers().each(function () {
            var container = $(this);
            var imgTumblr = container.find(".img-tumblr");
            var btnComment = container.find(".btn-comment");
            var pText = container.find(".text-tumblr");
            var btnShare = container.find(".btn-share");
            btnShare.prop("href", lv.getShareUri({ Uri: btnComment.attr("href"), Title: pText.text(), Summary: pText.text(), Pic: imgTumblr.attr("data-original") }));
        });
    }
    // 读取并设置收藏按钮的状态
    var setFavs = function () {
        lv.retryExecute(function () {
            return lv.queryAzureTable(favSas, { filter: sprintf("RowKey ge '%s_%s' and RowKey le '%s_%s'", mediaType, from, mediaType, to), select: "RowKey" }).done(function (data) {
                var loadedFavs = {};
                $.each(data.value, function (index, value) {
                    loadedFavs[lv.getInvertedTicks(value.RowKey)] = true;
                });
                getFavoriteButtons().each(function () {
                    var btnCur = $(this);
                    var rk = btnCur.attr("rk");
                    if (loadedFavs[rk]) {
                        btnCur.addClass("btn-selected");
                    }
                });
            });
        }, function () {
            return lv.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
            });
        });
    }
    // 加载更多
    var loadTumblrs = function () {
        var last = $(".container-tumblr:last");
        return lv.retryExecute(function () {
            return lv.ajaxLadda(function () {
                return lv.queryAzureTable(sas, {
                    continuationToken: continuationToken,
                    filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
                    top: takeCount
                }).done(function (data, textStatus, jqXhr) {
                    $.each(data.value, function (index, tumblr) {
                        var rk = lv.getInvertedTicks(tumblr.RowKey);
                        var cp = tumblrTemplate.clone();
                        cp.find("img.img-tumblr").attr("data-original", tumblr.MediaUri);
                        cp.find("p.text-tumblr").text(tumblr.Text);
                        cp.find("p.date-tumblr").text(lv.getLocalTime(tumblr.CreateTime));
                        cp.find("button.btn-favorite").attr("rk", rk).attr("tp", tumblr.MediaType);
                        cp.find("a.btn-comment").prop("href", ["/comments", tumblr.MediaType, rk].join("/"));
                        cp.appendTo(getTumblrsDiv());
                    });

                    var all = last.nextAll();
                    getFavoriteButtons = lv.singleton(function () {
                        return all.find(".btn-favorite");
                    });
                    getImages = lv.singleton(function () {
                        return all.find("img.lazy");
                    });
                    getTumblrContainers = lv.singleton(function () {
                        return all;
                    });
                    initImgs();
                    initFavs();
                    initShare();

                    continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                    continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
                }).done(function (data) {
                    from = lv.getInvertedTicks(data.value[0].RowKey);
                    to = lv.getInvertedTicks(data.value[data.value.length - 1].RowKey);
                    setFavs();
                }).done(function () {
                    if (!continuationToken.NextPartitionKey || !continuationToken.NextRowKey) {
                        getLoadingButton().hide();
                    }
                });
            }, getLoadingButton());
        }, function () {
            return lv.getToken([tableNameOfTumblr]).done(function (data) {
                sas = data;
            });
        });
    };

    exports.initialize = function (p) {
        tumblrTemplate = p.tumblrTemplate;
        continuationToken = p.continuationToken;
        sas = p.sas;
        mediaType = p.mediaType;
        tumblrCategory = p.tumblrCategory;
        from = p.from;
        to = p.to;
        tableNameOfFavorite = p.tableNameOfFavorite;
        tableNameOfTumblr = p.tableNameOfTumblr;

        initImgs();
        initTime();
        initFavs();
        initShare();
        setFavs();
        getLoadingButton().on("touchend", function () {
            loadTumblrs();
        });
    };

    return exports;
})();