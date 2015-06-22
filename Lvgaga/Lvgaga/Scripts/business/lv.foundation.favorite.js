import {sprintf} from 'sprintf-js';
import * as factory from '../business/lv.foundation.factory.js';
import * as util from '../business/lv.foundation.utility.js';
import * as token from '../business/lv.foundation.token.js';
const retryTime = 3;

let postFavorite = function (tumblr) {
    return util.authorizedExecute(function () {
        return $.post(sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey)).retry({times: retryTime});
    });
};

let deleteFavorite = function (tumblr) {
    return util.authorizedExecute(function () {
        return $.ajax({
            url: sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey),
            type: "DELETE"
        }).retry({times: retryTime});
    });
};

let setFavorite = function ({
    buttonK:button,
    tumblrK:tumblr,
    doneK:done
    }) {
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

let loadFavorite = function (parameters) {
    let favSas = parameters.favSas;
    let tableNameOfFavorite = parameters.tableNameOfFavorite;
    let tumblr = parameters.tumblr;
    let onReceiveNewToken = parameters.onReceiveNewToken;
    let done = parameters.done;

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

let loadFavorites = function (parameters) {
    let favSas = parameters.favSas;
    let tableNameOfFavorite = parameters.tableNameOfFavorite;
    let tumblrs = parameters.tumblrs;
    let mediaType = parameters.mediaType;
    let onReceiveNewToken = parameters.onReceiveNewToken;
    let done = parameters.done;

    return util.retryExecute(function () {
        let from = tumblrs[0].RowKey;
        let to = tumblrs[tumblrs.length - 1].RowKey;

        return util.queryAzureTable(favSas, {
            filter: sprintf("RowKey ge '%s_%s' and RowKey le '%s_%s'", mediaType, from, mediaType, to),
            select: "RowKey"
        }).done(function (data) {
            let loadedFavs = {};
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

let loadFavoritesWithContinuationToken = function (parameters) {
    let button = parameters.button;
    let sasFav = parameters.sasFav;
    let tableNameOfFavorite = parameters.tableNameOfFavorite;
    let continuationToken = parameters.continuationToken;
    let mediaType = parameters.mediaType;
    let takingCount = parameters.takingCount;
    let onReceiveNewToken = parameters.onReceiveNewToken;
    let onReceiveNewContinuationToken = parameters.onReceiveNewContinuationToken;
    let done = parameters.done;

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