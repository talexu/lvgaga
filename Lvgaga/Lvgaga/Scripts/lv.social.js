(function () {
    var getShareUri = function (tumblr) {
        return sprintf("http://api.bshare.cn/share/bsharesync?url=%s&summary=%s&publisherUuid=%s&pic=%s",
            encodeURIComponent("http://" + window.location.host + tumblr.Uri),
            encodeURIComponent(tumblr.Text),
            encodeURIComponent("35de718f-8cbf-4a01-8d69-486b3e6c3437"),
            encodeURIComponent(tumblr.MediaLargeUri));
    };

    lv.social.getShareUri = getShareUri;
})();