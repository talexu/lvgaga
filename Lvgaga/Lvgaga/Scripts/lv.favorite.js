(function () {
    var retryTime = lv.defaultRetryTime;

    var postFavorite = function (parameters) {
        return lv.authorizedExecute(function () {
            return $.post(sprintf("/api/v1/favorites/%s/%s", parameters.RowKey, parameters.PartitionKey)).retry({ times: retryTime });
        });
    };

    var deleteFavorite = function (parameters) {
        return lv.authorizedExecute(function() {
            return $.ajax({
                url: sprintf("/api/v1/favorites/%s/%s", parameters.RowKey, parameters.PartitionKey),
                type: "DELETE"
            }).retry({ times: retryTime });
        });
    };

    lv.favorite.postFavorite = postFavorite;
    lv.favorite.deleteFavorite = deleteFavorite;
})();