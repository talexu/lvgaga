lv.tumblr = (function () {
    var that = {};
    var loadingButton = lv.singleton(function () {
        return $("#btn_load");
    });

    that.loadingButton = loadingButton();

    that.loadTumblrs = function () {

    }

    return that;
})();