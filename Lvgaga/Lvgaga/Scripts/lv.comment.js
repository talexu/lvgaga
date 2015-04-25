﻿lv.comment = (function () {
    var exports = {};

    var continuationToken = {};
    var sas;
    var favSas;
    var mediaType;
    var rowKey;
    var text;
    var mediaUri;
    var tableNameOfComment;
    var tableNameOfFavorite;
    var takeCount = lv.defaultTakingCount;

    // 获取收藏按钮的实例
    var getFavoriteButton = lv.singleton(function () {
        return $("#btn_favorite");
    });
    // 获取分享按钮的实例
    var getShareButton = lv.singleton(function () {
        return $("#btn_share");
    });
    var getCommentButton = lv.singleton(function () {
        return $("#btn_send");
    });
    // 获取加载按钮的实例
    var getLoadingButton = lv.singleton(function () {
        return $("#btn_load");
    });
    // 获取评论容器
    var getCommentsContainer = lv.singleton(function () {
        return $("#div_comments");
    });
    // 获取评论文本框
    var getCommentTextBox = lv.singleton(function () {
        return $("#txb_comment");
    });
    // 获取评论内容
    var getCommentText = function () {
        return getCommentTextBox().val().trim();
    };

    var generateComment = function (data, createTime) {
        return $("<div></div>").
            append($("<hr></hr>").addClass("hr-comment")).
            append($("<a></a>").text(data.UserName)).
            append($("<p></p>").addClass("date-comment pull-right").text(createTime ? createTime : lv.getLocalTime(data.CommentTime))).
            append($("<p></p>").addClass("text-comment").text(data.Text));
    };

    // 读取并设置收藏按钮的状态
    var setFavs = function () {
        lv.retryExecute(function () {
            return lv.queryAzureTable(favSas, { filter: sprintf("RowKey eq '%s_%s'", mediaType, rowKey), select: "RowKey" }).done(function (data) {
                if (data.value.length > 0) {
                    getFavoriteButton().addClass("btn-selected");
                }
            });
        }, function () {
            return lv.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
            });
        });
    };

    // 收藏和取消收藏
    var initFav = function () {
        getFavoriteButton().on("touchend", function (event) {
            var btnCur = $(event.currentTarget);
            if (!btnCur.hasClass("btn-selected")) {
                lv.ajaxLadda(function () {
                    return lv.addFavorite({ pk: mediaType, rk: rowKey }).done(function () {
                        btnCur.addClass("btn-selected");
                    });
                }, btnCur);

            } else {
                lv.ajaxLadda(function () {
                    return lv.removeFavorite({ pk: mediaType, rk: rowKey }).done(function () {
                        btnCur.removeClass("btn-selected");
                    });
                }, btnCur);
            }
        });
    };

    // 读取评论
    var loadComments = function () {
        lv.retryExecute(function () {
            return lv.ajaxLadda(function () {
                return lv.queryAzureTable(sas, {
                    continuationToken: continuationToken,
                    top: takeCount
                }).done(function (data, textStatus, jqXhr) {
                    $.each(data.value, function (index, comment) {
                        getCommentsContainer().append(generateComment(comment));
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
                return lv.getToken([tableNameOfComment]).done(function (data) {
                    sas = data;
                });
            }, getLoadingButton());
        });
    };

    // 发送评论
    var sendComment = function () {
        var commentText = getCommentText();
        if (!commentText) return;

        lv.ajaxLadda(function () {
            return lv.authorizedExecute(function () {
                return $.post(sprintf("/api/v1/comments/%s/%s", mediaType, rowKey),
                {
                    "Text": commentText
                }).retry({ times: lv.defaultTakingCount });
            }).done(function (data) {
                getCommentsContainer().prepend(generateComment(data, "刚刚"));
                getCommentTextBox().val("");
            });
        }, getCommentButton());
    };

    exports.initialize = function (p) {
        sas = p.sas;
        mediaType = p.mediaType;
        rowKey = p.rowKey;
        text = p.text;
        mediaUri = p.mediaUri;
        tableNameOfComment = p.tableNameOfComment;
        tableNameOfFavorite = p.tableNameOfFavorite;

        loadComments();
        setFavs();
        initFav();
        getShareButton().prop("href", lv.getShareUri({ Uri: window.location.pathname, Title: text, Summary: text, Pic: mediaUri }));

        getCommentButton().on("touchend", function () {
            sendComment();
        });
        getLoadingButton().on("touchend", function () {
            loadComments();
        });
    };

    return exports;
})();