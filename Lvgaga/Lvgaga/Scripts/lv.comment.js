(function () {
    var postComment = function (parameters) {
        var tumblr = parameters.tumblr;
        var commentText = parameters.commentText;
        var button = parameters.button;
        var callback = parameters.callback;
        if (!commentText) return null;

        return lv.ajaxLadda(function () {
            return lv.authorizedExecute(function () {
                return $.post(sprintf("/api/v1/comments/%s/%s", tumblr.MediaType, tumblr.RowKey),
                    {
                        "Text": commentText
                    }).retry({times: lv.defaultTakingCount});
            }).done(function (data) {
                callback(data);
            });
        }, button);
    };

    lv.comment.postComment = postComment;
})();