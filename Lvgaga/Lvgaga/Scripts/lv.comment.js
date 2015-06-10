(function () {
    var that;
    var favSas;

    var getCommentUri = function (tumblr) {
        return ["/comments", tumblr.MediaType, tumblr.RowKey].join("/");
    };

    var postComment = function (parameters) {
        var tumblr = parameters.tumblr;
        var commentText = parameters.commentText;
        var button = parameters.button;
        if (!commentText) return null;

        return lv.ajaxLadda(function () {
            return lv.authorizedExecute(function () {
                return $.post(sprintf("/api/v1/comments/%s/%s", tumblr.MediaType, tumblr.RowKey),
                    {
                        "Text": commentText
                    }).retry({times: lv.defaultTakingCount});
            });
        }, button);
    };

    var loadFavorite = function (tumblr) {
        return lv.retryExecute(function () {
            return lv.queryAzureTable(favSas, {
                filter: sprintf("RowKey eq '%s_%s'", tumblr.MediaType, tumblr.RowKey),
                select: "RowKey"
            }).done(function (data) {
                if (data.value.length > 0) {
                    tumblr.IsFavorited = true;
                }

                lv.refreshState(that);
            });
        }, function () {
            return lv.token.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
            });
        });
    };

    lv.comment.getCommentUri = getCommentUri;
    lv.comment.postComment = postComment;
})();