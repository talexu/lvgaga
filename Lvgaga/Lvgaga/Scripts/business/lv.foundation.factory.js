import {sprintf} from 'sprintf-js';
import {Base64} from 'js-base64';
import moment from 'moment';

var getInvertedTicks = function (rowKey) {
    return rowKey.slice(2);
};

var getLocalTime = function (dataTime) {
    return moment.utc(dataTime).local().format("YYYY-MM-DD HH:mm:ss");
};

var getCommentUri = function (tumblr) {
    return ["/comments", tumblr.MediaType, tumblr.RowKey].join("/");
};

var getShareUri = function (tumblr) {
    return sprintf("http://api.bshare.cn/share/bsharesync?url=%s&summary=%s&publisherUuid=%s&pic=%s",
        encodeURIComponent("http://" + "qingyulu.azurewebsites.net" + tumblr.Uri),
        encodeURIComponent(tumblr.Text),
        encodeURIComponent("35de718f-8cbf-4a01-8d69-486b3e6c3437"),
        encodeURIComponent(tumblr.MediaLargeUri));
};

var createTumblr = function (dataEntity) {
    dataEntity.RowKey = getInvertedTicks(dataEntity.RowKey);
    let id = sprintf("%s/%s", dataEntity.MediaType, dataEntity.RowKey);
    let base64Id = Base64.encode(id);
    dataEntity.Id = id;
    dataEntity.Base64Id = base64Id;
    dataEntity.CreateTime = getLocalTime(dataEntity.CreateTime);
    dataEntity.Uri = getCommentUri(dataEntity);
    dataEntity.comments = dataEntity.comments || [];
    dataEntity.sharingUri = getShareUri(dataEntity);

    return dataEntity;
};
var createTumblrs = function (dataEntities, isFavorited = false) {
    for (let dataEntity of dataEntities) {
        createTumblr(dataEntity);
        dataEntity.IsFavorited = isFavorited;
    }

    return dataEntities;
};

var createComment = function (dataEntity) {
    dataEntity.CommentTime = lv.getLocalTime(dataEntity.CommentTime);

    return dataEntity;
};
var createComments = function (dataEntities) {
    for (let dataEntity of dataEntities) {
        createComment(dataEntity);
    }

    return dataEntities;
};

export{
    getInvertedTicks,
    getLocalTime,
    getCommentUri,
    getShareUri,
    createTumblr,
    createTumblrs,
    createComment,
    createComments
};