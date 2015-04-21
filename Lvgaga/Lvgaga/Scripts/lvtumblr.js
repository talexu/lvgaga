lv.tumblr = (function () {
    var that = {};

    var tumblrTemplate;
    var continuationToken;
    var sas;
    var favSas;

    // 获取加载按钮的实例
    var getLoadingButton = lv.singleton(function () {
        return $("#btn_load");
    });
    // 获取Tumblrs的容器
    var getTumblrsDiv = lv.singleton(function () {
        return $("#div_tumblrs");
    });
    // 获取初始页面的收藏按钮
    var getInitialFavoriteButtons = lv.singleton(function () {
        return $(".btn-favorite");
    });

    // 启动图片延迟加载
    var initImgs = function (imgs) {
        imgs.lazyload({
            effect: "fadeIn"
        });
    };
    // 按本地时区格式化时间
    var initTime = function (times) {
        times.each(function () {
            var p = $(this);
            var utc = p.text();
            p.text(getLocalTime(utc));
        });
    }
    // 注册收藏按钮事件
    var initFavs = function (btns) {
        btns.on("touchend", function (event) {
            var btnCur = $(event.currentTarget);
            if (!btnCur.hasClass("btn-selected")) {
                favorite(btnCur.attr("tp"), btnCur.attr("rk"), btnCur);
            } else {
                unFavorite(btnCur.attr("tp"), btnCur.attr("rk"), btnCur);
            }
        });
    }
    // 设置重定向分享到链接
    var initShare = function (containers) {
        containers.each(function () {
            var container = $(this);
            var imgTumblr = container.find(".img-tumblr");
            var btnComment = container.find(".btn-comment");
            var pText = container.find(".text-tumblr");
            var btnShare = container.find(".btn-share");
            btnShare.prop("href", getShareUri({ Uri: btnComment.attr("href"), Title: pText.text(), Summary: pText.text(), Pic: imgTumblr.attr("data-original") }));
        });
    }

    that.loadTumblrs = function () {

    };

    that.initialize = function(p) {
        tumblrTemplate = p.tumblrTemplate || tumblrTemplate;
        continuationToken = p.continuationToken || continuationToken;
        sas = p.sas || sas;
    };

    return that;
})();