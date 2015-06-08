(function () {
    var getToken = function (paths) {
        return $.get(sprintf("/api/v1/tokens/%s", paths.join("/"))).retry({
            times: lv.defaultRetryTime
        });
    };

    lv.token.getToken = getToken;
})();