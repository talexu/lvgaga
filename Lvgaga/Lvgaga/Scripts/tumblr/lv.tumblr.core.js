import * as factory from '../business/lv.foundation.factory.js';
import * as util from '../business/lv.foundation.utility.js';
import * as token from '../business/lv.foundation.token.js';
import {sprintf} from 'sprintf-js';

var reactRoot;
var tumSas;
var favSas;
var comSas;
var continuationToken;
var mediaType;
var tumblrCategory;
var takingCount = 20;
var commentTakingCount = 5;
var tableNameOfTumblr;
var tableNameOfFavorite;
var tableNameOfComment;

var loadTumblrs = function (e) {
    var button = e.target;

    return util.retryExecuteLadda(function () {
        return util.queryAzureTable(tumSas, {
            continuationToken: continuationToken,
            filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
            top: takingCount
        }).done(function (data, textStatus, jqXhr) {
            continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
            continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
        }).done(function (data) {
            reactRoot.state.dataContext = reactRoot.state.dataContext.concat(factory.createTumblrs(data.value));
            util.refreshState(reactRoot);
        });
    }, function () {
        return token.getToken([tableNameOfTumblr]).done(function (data) {
            tumSas = data;
        });
    }, button);
};

var initialize = function (reactRoot1,
                           tumSas1,
                           continuationToken1,
                           mediaType1,
                           tumblrCategory1,
                           tableNameOfTumblr1,
                           tableNameOfFavorite1,
                           tableNameOfComment1) {
    reactRoot = reactRoot1;
    tumSas = tumSas1;
    continuationToken = continuationToken1 || {};
    mediaType = mediaType1;
    tumblrCategory = tumblrCategory1;
    tableNameOfTumblr = tableNameOfTumblr1;
    tableNameOfFavorite = tableNameOfFavorite1;
    tableNameOfComment = tableNameOfComment1;
};

export * from '../business/lv.foundation.factory.js';
export
{
    initialize, loadTumblrs
};