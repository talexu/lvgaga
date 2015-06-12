(function () {
    var retryTime = lv.defaultRetryTime;

    var postFavorite = function (tumblr) {
        return lv.authorizedExecute(function () {
            return $.post(sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey)).retry({times: retryTime});
        });
    };

    var deleteFavorite = function (tumblr) {
        return lv.authorizedExecute(function () {
            return $.ajax({
                url: sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey),
                type: "DELETE"
            }).retry({times: retryTime});
        });
    };

    var setFavorite = function (parameters) {
        var button = parameters.button;
        var tumblr = parameters.tumblr;
        var done = parameters.done;

        if (tumblr.IsFavorited) {
            return lv.ajaxLadda(function () {
                return deleteFavorite(tumblr).done(function () {
                    tumblr.IsFavorited = false;
                    done(tumblr);
                });
            }, button);
        } else {
            return lv.ajaxLadda(function () {
                return postFavorite(tumblr).done(function () {
                    tumblr.IsFavorited = true;
                    done(tumblr);
                });
            }, button);
        }
    };

    var loadFavorite = function (parameters) {
        var favSas = parameters.favSas;
        var tableNameOfFavorite = parameters.tableNameOfFavorite;
        var tumblr = parameters.tumblr;
        var onReceiveNewToken = parameters.onReceiveNewToken;
        var done = parameters.done;

        return lv.retryExecute(function () {
            return lv.queryAzureTable(favSas, {
                filter: sprintf("RowKey eq '%s_%s'", tumblr.MediaType, tumblr.RowKey),
                select: "RowKey"
            }).done(function (data) {
                if (data.value.length > 0) {
                    tumblr.IsFavorited = true;
                    done(tumblr);
                }
            });
        }, function () {
            return lv.token.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
                onReceiveNewToken(data);
            });
        });
    };

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

    lv.favorite.postFavorite = postFavorite;
    lv.favorite.deleteFavorite = deleteFavorite;
    lv.favorite.setFavorite = setFavorite;
    lv.favorite.loadFavorite = loadFavorite;
    lv.favorite.loadFavorites = loadFavorites;
})();