(function () {
    var createTumblr = function (dataEntity) {
        dataEntity.RowKey = lv.getInvertedTicks(dataEntity.RowKey);
        var id = sprintf("%s/%s", dataEntity.MediaType, dataEntity.RowKey);
        var base64Id = window.btoa(id);
        dataEntity.Id = id;
        dataEntity.Base64Id = base64Id;
        dataEntity.CreateTime = lv.getLocalTime(dataEntity.CreateTime);

        return dataEntity;
    };
    var createTumblrs = function (dataEntities) {
        $.each(dataEntities, function (index, dataEntity) {
            createTumblr(dataEntity);
        });

        return dataEntities;
    };

    var createComment = function(dataEntity) {
        dataEntity.CommentTime = lv.getLocalTime(dataEntity.CommentTime);

        return dataEntity;
    };
    var createComments = function(dataEntities) {
        $.each(dataEntities, function (index, dataEntity) {
            createComment(dataEntity);
        });

        return dataEntities;
    }

    lv.factory.createTumblrs = createTumblrs;
    lv.factory.createComment = createComment;
    lv.factory.createComments = createComments;
})();