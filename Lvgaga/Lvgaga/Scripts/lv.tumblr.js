(function () {
    var loadFavorites = function (parameters) {
        var favSas = parameters.favSas;
        var tableNameOfFavorite = parameters.tableNameOfFavorite;
        var tumblrs = parameters.tumblrs;
        var mediaType = parameters.mediaType;
        var onReceiveNewToken = parameters.onReceiveNewToken;
        var done = parameters.done;

        return lv.retryExecute(function () {
            var from = tumblrs[0].RowKey;
            var to = tumblrs[tumblrs.length - 1].RowKey;

            return lv.queryAzureTable(favSas, {
                filter: sprintf("RowKey ge '%s_%s' and RowKey le '%s_%s'", mediaType, from, mediaType, to),
                select: "RowKey"
            }).done(function (data) {
                var loadedFavs = {};
                $.each(data.value, function (index, value) {
                    loadedFavs[lv.getInvertedTicks(value.RowKey)] = true;
                });

                $.each(tumblrs, function (index, value) {
                    if (loadedFavs[value.RowKey]) {
                        value.IsFavorited = true;
                    }
                });

                done(tumblrs);
            });
        }, function () {
            return lv.token.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
                onReceiveNewToken(data);
            });
        });
    };

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

    lv.tumblr.loadFavorites = loadFavorites;
    lv.tumblr.loadTumblrs = loadTumblrs;
})();