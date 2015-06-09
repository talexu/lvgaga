(function () {
    var that;
    var sas = lv.dataContext.Sas;
    var favSas;
    var comSas;
    var continuationToken = lv.dataContext.ContinuationToken;
    var mediaType = lv.dataContext.MediaType;
    var tumblrCategory = lv.dataContext.TumblrCategory;
    var takingCount = lv.defaultTakingCount;
    var tableNameOfTumblr = lv.dataContext.tableNameOfTumblr;
    var tableNameOfFavorite = lv.dataContext.tableNameOfFavorite;
    var tableNameOfComment = lv.dataContext.tableNameOfComment;

    var initTumblrs = function (entities) {
        lv.factory.createTumblrs(entities);
        that.state.dataContext = that.state.dataContext.concat(entities);

        lv.refreshState(that);
        return entities;
    };

    var loadFavorites = function (tumblrs) {
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

                lv.refreshState(that);
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

    var setFavorite = function (tumblr, e) {
        var button = e.target;
        var isFavorited = tumblr.IsFavorited;

        if (isFavorited) {
            return lv.ajaxLadda(function () {
                return lv.favorite.deleteFavorite(tumblr).done(function () {
                    tumblr.IsFavorited = false;

                    lv.refreshState(that);
                });
            }, button);
        } else {
            return lv.ajaxLadda(function () {
                return lv.favorite.postFavorite(tumblr).done(function () {
                    tumblr.IsFavorited = true;

                    lv.refreshState(that);
                });
            }, button);
        }
    };

    var loadComments = function (tumblr, callback) {
        lv.retryExecute(function () {
            return lv.queryAzureTable(comSas, {
                filter: sprintf("PartitionKey eq '%s'", tumblr.RowKey),
                top: takingCount
            }).done(function (data, textStatus, jqXhr) {
                callback(lv.factory.createComments(data.value));
            });
        }, function () {
            return lv.token.getToken([tableNameOfComment]).done(function (data) {
                comSas = data;
            });
        });
    };

    lv.tumblr.initialize = function (parameters) {
        that = parameters.that;
    };
    lv.tumblr.initTumblrs = initTumblrs;
    lv.tumblr.loadFavorites = loadFavorites;
    lv.tumblr.loadTumblrs = loadTumblrs;
    lv.tumblr.setFavorite = setFavorite;
    lv.tumblr.loadComments = loadComments;
})();