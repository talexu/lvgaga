(function () {
    var loadTumblrs = function (parameters) {
        var button = parameters.button;
        var tumSas = parameters.tumSas;
        var mediaType = parameters.mediaType;
        var tumblrCategory = parameters.tumblrCategory;
        var takingCount = parameters.takingCount;
        var continuationToken = parameters.continuationToken;
        var tableNameOfTumblr = parameters.tableNameOfTumblr;
        var onReceiveNewToken = parameters.onReceiveNewToken;
        var onReceiveNewContinuationToken = parameters.onReceiveNewContinuationToken;
        var done = parameters.done;

        return lv.retryExecuteLadda(function () {
            return lv.queryAzureTable(tumSas, {
                continuationToken: continuationToken,
                filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
                top: takingCount
            }).done(function (data, textStatus, jqXhr) {
                onReceiveNewContinuationToken(jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey"), jqXhr.getResponseHeader("x-ms-continuation-NextRowKey"));
            }).done(function (data) {
                done(data.value);
            });
        }, function () {
            return lv.token.getToken([tableNameOfTumblr]).done(function (data) {
                tumSas = data;
                onReceiveNewToken(data);
            });
        }, button);
    };

    lv.tumblr.loadTumblrs = loadTumblrs;
})();