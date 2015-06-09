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

    lv.favorite.postFavorite = postFavorite;
    lv.favorite.deleteFavorite = deleteFavorite;
})();