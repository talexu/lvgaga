(function () {
    var getShareUri = function (p) {
        return sprintf("http://api.bshare.cn/share/sinaminiblog?url=%s&summary=%s&publisherUuid=%s&pic=%s",
            encodeURIComponent("http://" + window.location.host + p.Uri),
            encodeURIComponent(p.Summary),
            encodeURIComponent("35de718f-8cbf-4a01-8d69-486b3e6c3437"),
            encodeURIComponent(p.Pic));
    };

    lv.social.getShareUri = getShareUri;
})();