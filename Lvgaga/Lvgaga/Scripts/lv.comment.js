lv.comment = (function () {
    var that = {};

    var tumblrTemplate;
    var continuationToken = {};
    var sas;
    var favSas;
    var mediaType;
    var tumblrCategory;
    var from;
    var to;
    var tableNameOfTumblr;
    var tableNameOfComment;
    var tableNameOfFavorite;

    // 获取加载按钮的实例
    var getLoadingButton = lv.singleton(function () {
        return $("#btn_load");
    });
    // 获取评论容器
    var getCommentsContainer = lv.singleton(function () {
        return $("#div_comments");
    });

    var generateComment = function (data, createTime) {
        return $("<div></div>").
		append($("<hr></hr>").addClass("hr-comment")).
		append($("<a></a>").text(data.UserName)).
		append($("<p></p>").addClass("date-comment pull-right").text(createTime ? createTime : lv.getLocalTime(data.CommentTime))).
		append($("<p></p>").addClass("text-comment").text(data.Text));
    }

    var loadComments = function () {
        lv.ajaxLadda(function() {
            return lv.retryExecute(function() {
                return lv.queryAzureTable(sas, {
                        continuationToken: continuationToken,
                        top: 20
                    })
                    .done(function(data, textStatus, jqXhr) {
                        $.each(data.value, function(index, comment) {
                            getCommentsContainer().append(generateComment(comment));
                        });

                        continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                        continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
                    });
            }, function() {
                return lv.getToken([tableNameOfComment])
                    .done(function(data) {
                        sas = data;
                    });
            });
        }, getLoadingButton());
        
    }

    that.initialize = function (p) {
        sas = p.sas;
        tableNameOfComment = p.tableNameOfComment;

        getLoadingButton().on("touchend", function () {
            loadComments();
        });
    };

    return that;
})();