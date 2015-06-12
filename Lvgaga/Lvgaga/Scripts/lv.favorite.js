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

    lv.favorite.postFavorite = postFavorite;
    lv.favorite.deleteFavorite = deleteFavorite;
    lv.favorite.setFavorite = setFavorite;
})();