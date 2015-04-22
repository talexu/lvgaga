lv.comment = (function () {
    var that = {};

    var tumblrTemplate;
    var continuationToken = {};
    var sas;
    var favSas;
    var mediaType;
    var rowKey;
    var text;
    var mediaUri;
    var tableNameOfTumblr;
    var tableNameOfComment;
    var tableNameOfFavorite;

    // 获取收藏按钮的实例
    var getFavoriteButton = lv.singleton(function () {
        return $("#btn_favorite");
    });
    var getShareButton = lv.singleton(function () {
        return $("#btn_share");
    });
    // 获取加载按钮的实例
    var getLoadingButton = lv.singleton(function () {
        return $("#btn_load");
    });
    // 获取评论容器
    var getCommentsContainer = lv.singleton(function () {
        return $("#div_comments");
    });

    var generateComment = function (data, createTime) {
        return $("<div></div>").
		append($("<hr></hr>").addClass("hr-comment")).
		append($("<a></a>").text(data.UserName)).
		append($("<p></p>").addClass("date-comment pull-right").text(createTime ? createTime : lv.getLocalTime(data.CommentTime))).
		append($("<p></p>").addClass("text-comment").text(data.Text));
    }

    // 读取并设置收藏按钮的状态
    var setFavs = function () {
        lv.retryExecute(function () {
            return lv.queryAzureTable(favSas, { filter: sprintf("RowKey eq '%s_%s'", mediaType, rowKey), select: "RowKey" })
                .done(function (data) {
                    if (data.value.length > 0) {
                        getFavoriteButton().addClass("btn-selected");
                    }
                });
        }, function () {
            return lv.getToken([tableNameOfFavorite])
                .done(function (data) {
                    favSas = data;
                });
        });
    }

    // 收藏和取消收藏
    var initFav = function () {
        getFavoriteButton().on("touchend", function (event) {
            var btnCur = $(event.currentTarget);
            if (!btnCur.hasClass("btn-selected")) {
                lv.ajaxLadda(function () {
                    return lv.addFavorite({ pk: mediaType, rk: rowKey }, function () {
                        btnCur.addClass("btn-selected");
                    });
                }, btnCur);

            } else {
                lv.ajaxLadda(function () {
                    return lv.removeFavorite({ pk: mediaType, rk: rowKey }, function () {
                        btnCur.removeClass("btn-selected");
                    });
                }, btnCur);
            }
        });
    }

    var loadComments = function () {
        lv.ajaxLadda(function () {
            return lv.retryExecute(function () {
                return lv.queryAzureTable(sas, {
                    continuationToken: continuationToken,
                    top: 20
                })
                    .done(function (data, textStatus, jqXhr) {
                        $.each(data.value, function (index, comment) {
                            getCommentsContainer().append(generateComment(comment));
                        });

                        continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                        continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
                    });
            }, function () {
                return lv.getToken([tableNameOfComment])
                    .done(function (data) {
                        sas = data;
                    });
            });
        }, getLoadingButton());
    }

    that.initialize = function (p) {
        sas = p.sas;
        mediaType = p.mediaType;
        rowKey = p.rowKey;
        text = p.text;
        mediaUri = p.mediaUri;
        tableNameOfComment = p.tableNameOfComment;
        tableNameOfFavorite = p.tableNameOfFavorite;

        setFavs();
        initFav();
        getShareButton().prop("href", lv.getShareUri({ Uri: window.location.pathname, Title: text, Summary: text, Pic: mediaUri }));

        getLoadingButton().on("touchend", function () {
            loadComments();
        });
    };

    return that;
})();