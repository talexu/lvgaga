import {sprintf} from 'sprintf-js';
import * as factory from '../business/lv.foundation.factory.js';
import * as util from '../business/lv.foundation.utility.js';
import * as token from '../business/lv.foundation.token.js';
var retryTime = 3;

var postFavorite = function (tumblr) {
    return util.authorizedExecute(function () {
        return $.post(sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey)).retry({times: retryTime});
    });
};

var deleteFavorite = function (tumblr) {
    return util.authorizedExecute(function () {
        return $.ajax({
            url: sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey),
            type: "DELETE"
        }).retry({times: retryTime});
    });
};

var setFavorite = function (parameters) {
    var button = parameters.button;
    var tumblr = parameters.tumblr;
    var done = parameters.done;

    if (tumblr.IsFavorited) {
        return util.ajaxLadda(function () {
            return deleteFavorite(tumblr).done(function () {
                tumblr.IsFavorited = false;
                done(tumblr);
            });
        }, button);
    } else {
        return util.ajaxLadda(function () {
            return postFavorite(tumblr).done(function () {
                tumblr.IsFavorited = true;
                done(tumblr);
            });
        }, button);
    }
};

export{
    postFavorite,
    deleteFavorite,
    setFavorite
};

var loadFavorite = function (parameters) {
    var favSas = parameters.favSas;
    var tableNameOfFavorite = parameters.tableNameOfFavorite;
    var tumblr = parameters.tumblr;
    var onReceiveNewToken = parameters.onReceiveNewToken;
    var done = parameters.done;

    return util.retryExecute(function () {
        return util.queryAzureTable(favSas, {
            filter: sprintf("RowKey eq '%s_%s'", tumblr.MediaType, tumblr.RowKey),
            select: "RowKey"
        }).done(function (data) {
            if (data.value.length > 0) {
                tumblr.IsFavorited = true;
                done(tumblr);
            }
        });
    }, function () {
        return token.getToken([tableNameOfFavorite]).done(function (data) {
            favSas = data;
            onReceiveNewToken(data);
        });
    });
};

var loadFavorites = function (parameters) {
    var favSas = parameters.favSas;
    var tableNameOfFavorite = parameters.tableNameOfFavorite;
    var tumblrs = parameters.tumblrs;
    var mediaType = parameters.mediaType;
    var onReceiveNewToken = parameters.onReceiveNewToken;
    var done = parameters.done;

    return util.retryExecute(function () {
        var from = tumblrs[0].RowKey;
        var to = tumblrs[tumblrs.length - 1].RowKey;

        return util.queryAzureTable(favSas, {
            filter: sprintf("RowKey ge '%s_%s' and RowKey le '%s_%s'", mediaType, from, mediaType, to),
            select: "RowKey"
        }).done(function (data) {
            var loadedFavs = {};
            $.each(data.value, function (index, value) {
                loadedFavs[util.getInvertedTicks(value.RowKey)] = true;
            });

            $.each(tumblrs, function (index, value) {
                if (loadedFavs[value.RowKey]) {
                    value.IsFavorited = true;
                }
            });

            done(tumblrs);
        });
    }, function () {
        return token.getToken([tableNameOfFavorite]).done(function (data) {
            favSas = data;
            onReceiveNewToken(data);
        });
    });
};

var loadFavoritesWithContinuationToken = function (parameters) {
    var button = parameters.button;
    var sasFav = parameters.sasFav;
    var tableNameOfFavorite = parameters.tableNameOfFavorite;
    var continuationToken = parameters.continuationToken;
    var mediaType = parameters.mediaType;
    var takingCount = parameters.takingCount;
    var onReceiveNewToken = parameters.onReceiveNewToken;
    var onReceiveNewContinuationToken = parameters.onReceiveNewContinuationToken;
    var done = parameters.done;

    return util.retryExecute(function () {
        return util.ajaxLadda(function () {
            return util.queryAzureTable(sasFav, {
                continuationToken: continuationToken,
                filter: sprintf("RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1),
                top: takingCount
            }).done(function (data, textStatus, jqXhr) {
                onReceiveNewContinuationToken(jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey"), jqXhr.getResponseHeader("x-ms-continuation-NextRowKey"));
            }).done(function (data) {
                done(factory.createTumblrs(data.value, true));
            });
        }, button);
    }, function () {
        return util.ajaxLadda(function () {
            return token.getToken([tableNameOfFavorite]).done(function (data) {
                sasFav = data;
                onReceiveNewToken(data);
            });
        }, button);
    });
};