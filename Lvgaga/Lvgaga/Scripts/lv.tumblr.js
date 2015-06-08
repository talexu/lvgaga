(function () {
    var that;
    var sas = lv.dataContext.Sas;
    var favSas;
    var continuationToken = lv.dataContext.ContinuationToken;
    var mediaType = lv.dataContext.MediaType;
    var tumblrCategory = lv.dataContext.TumblrCategory;
    var takingCount = lv.defaultTakingCount;
    var tableNameOfTumblr = lv.dataContext.tableNameOfTumblr;
    var tableNameOfFavorite = lv.dataContext.tableNameOfFavorite;

    var initTumblrs = function (entities) {
        lv.factory.createTumblrs(entities);
        that.state.dataContext = that.state.dataContext.concat(entities);
        that.setState(that.state);

        return entities;
    };

    var loadFavorites = function (tumblrs) {
        lv.retryExecute(function () {
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
                that.setState(that.state);
            });
        }, function () {
            return lv.token.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
            });
        });
    };

    var loadTumblrs = function (e) {
        var button = e.target;

        return lv.retryExecuteLadda(function () {
            return lv.queryAzureTable(sas, {
                continuationToken: continuationToken,
                filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
                top: takingCount
            }).done(function (data) {
                var entities = initTumblrs(data.value);
                loadFavorites(entities);
            }).done(function (data, textStatus, jqXhr) {
                continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");

                if (!continuationToken.NextPartitionKey || !continuationToken.NextRowKey) {
                    button.style.display = "none";
                }
            });
        }, function () {
            return lv.token.getToken([tableNameOfTumblr]).done(function (data) {
                sas = data;
            });
        }, button);
    };

    lv.tumblr.initialize = function (parameters) {
        that = parameters.that;
    };
    lv.tumblr.initTumblrs = initTumblrs;
    lv.tumblr.loadFavorites = loadFavorites;
    lv.tumblr.loadTumblrs = loadTumblrs;
})();