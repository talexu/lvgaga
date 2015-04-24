lv.favorite = (function () {
    var exports = {};

    // 计算Cell宽度, 小于等于100且水平充满屏幕
    var expectedCellWidth = 100;
    var getLinksContainer = lv.singleton(function () {
        return $("#links");
    });
    var containerWidth = getLinksContainer().width();
    var cellWidth = containerWidth / Math.ceil(containerWidth / expectedCellWidth);

    var continuationToken = {};
    var sas;
    var tableNameOfFavorite;
    var favMediaType;

    var getLoadingButton = lv.singleton(function () {
        return $("#btn_load");
    });
    var getGallery = lv.singleton(function () {
        return $("#blueimp-gallery");
    });
    var getFavoriteButton = lv.singleton(function () {
        return $("#btn_favorite");
    });
    var getShareButton = lv.singleton(function () {
        return $("#btn_share");
    });
    var getCommentButton = lv.singleton(function () {
        return $("#btn_comment");
    });
    var getTumblrTextBox = lv.singleton(function () {
        return $("#p_text");
    });
    // 当前显示的Tumblr
    var curTumblr;

    // 填充数据
    var fillFavorite = function (tumblr) {
        $("<a/>")
            .append($("<img>").prop("src", tumblr.ThumbnailUri).width(cellWidth))
            .prop("href", tumblr.MediaUri)
            .prop("title", "")
            .attr("rowKey", tumblr.RowKey)
            .attr("mediaType", tumblr.MediaType)
            .attr("tumblrCategory", tumblr.TumblrCategory)
            .attr("tumblrText", tumblr.Text)
            .attr("createTime", lv.getLocalTime(tumblr.CreateTime))
            .prop("isFavorite", true)
            .attr("data-gallery", "")
            .appendTo(getLinksContainer());
    };

    // 初始化Gallery控件, 添加滑动时的事件处理
    var initGallery = function () {
        getGallery().on("slide", function (event, index, slide) {
            // Gallery slide event handler
            var element = $(getLinksContainer().children()[index]);
            curTumblr = element;
            var mediaUri = element.prop("href");
            var mediaType = element.attr("mediaType");
            var invertedTicks = lv.getInvertedTicks(element.attr("rowKey"));
            var commentUri = lv.getCommentUri(mediaType, invertedTicks);
            var tumblrText = element.attr("tumblrText");

            getFavoriteButton().attr("tp", mediaType);
            getFavoriteButton().attr("rk", invertedTicks);
            if (element.prop("isFavorite") === true) {
                getFavoriteButton().addClass("btn-selected");
                getFavoriteButton().removeClass("btn-white");
            } else {
                getFavoriteButton().addClass("btn-white");
                getFavoriteButton().removeClass("btn-selected");
            }

            getShareButton().prop("href", lv.getShareUri({ Uri: commentUri, Title: tumblrText, Summary: tumblrText, Pic: mediaUri }));

            getTumblrTextBox().text(tumblrText);

            getCommentButton().prop("href", commentUri);
            getCommentButton().addClass("btn-white");
        });
    };

    // 初始化收藏按钮的事件
    var initFav = function () {
        getFavoriteButton().on("touchend", function (event) {
            var btnCur = $(event.currentTarget);
            if (curTumblr.prop("isFavorite") === false) {
                lv.ajaxLadda(function () {
                    return lv.addFavorite({ pk: btnCur.attr("tp"), rk: btnCur.attr("rk") }).done(function () {
                        btnCur.removeClass("btn-white");
                        curTumblr.prop("isFavorite", true);
                    });
                }, btnCur);
            } else {
                lv.ajaxLadda(function () {
                    return lv.removeFavorite({ pk: btnCur.attr("tp"), rk: btnCur.attr("rk") }).done(function () {
                        btnCur.addClass("btn-white");
                        curTumblr.prop("isFavorite", false);
                    });
                }, btnCur);
            }
        });
    };

    // 读取收藏的Tumblr
    var loadFavs = function () {
        lv.retryExecute(function () {
            return lv.ajaxLadda(function () {
                return lv.queryAzureTable(sas, { continuationToken: continuationToken, filter: sprintf("RowKey ge '%s' and RowKey lt '%s'", favMediaType, favMediaType + 1), top: lv.defaultTakingCount }).done(function (data, textStatus, jqXhr) {
                    $.each(data.value, function (index, tumblr) {
                        fillFavorite(tumblr);
                    });

                    continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                    continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
                }).done(function () {
                    if (!continuationToken.NextPartitionKey || !continuationToken.NextRowKey) {
                        getLoadingButton().hide();
                    }
                });
            }, getLoadingButton());
        }, function () {
            return lv.ajaxLadda(function () {
                return lv.getToken([tableNameOfFavorite]).done(function (data) {
                    sas = data;
                });
            }, getLoadingButton());
        });
    };

    exports.initialize = function (p) {
        sas = p.sas;
        favMediaType = p.favMediaType;
        tableNameOfFavorite = p.tableNameOfFavorite;

        initGallery();
        initFav();
        loadFavs();
        getLoadingButton().on("touchend", function () {
            loadFavs();
        });
    };

    return exports;
})();