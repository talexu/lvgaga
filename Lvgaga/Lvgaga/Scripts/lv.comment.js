(function () {
    var loadComments = function (parameters) {
        var button = parameters.button;
        var comSas = parameters.comSas;
        var tableNameOfComment = parameters.tableNameOfComment;
        var continuationToken = parameters.continuationToken;
        var takingCount = parameters.takingCount;
        var onReceiveNewToken = parameters.onReceiveNewToken;
        var onReceiveNewContinuationToken = parameters.onReceiveNewContinuationToken;
        var done = parameters.done;

        return lv.retryExecuteLadda(function () {
            return lv.queryAzureTable(comSas, {
                continuationToken: continuationToken,
                top: takingCount
            }).done(function (data, textStatus, jqXhr) {
                onReceiveNewContinuationToken(jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey"), jqXhr.getResponseHeader("x-ms-continuation-NextRowKey"));
            }).done(function (data) {
                if (data.value.length > 0) {
                    done(lv.factory.createComments(data.value));
                }
            });
        }, function () {
            return lv.token.getToken([tableNameOfComment]).done(function (data) {
                comSas = data;
                onReceiveNewToken(data);
            });
        }, button);
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
                    }).retry({times: lv.defaultRetryTime});
            });
        }, button);
    };

    lv.comment.loadComments = loadComments;
    lv.comment.postComment = postComment;
})();