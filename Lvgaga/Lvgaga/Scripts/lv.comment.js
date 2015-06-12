(function () {
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
                    }).retry({ times: lv.defaultRetryTime });
            });
        }, button);
    };

    lv.comment.postComment = postComment;
})();