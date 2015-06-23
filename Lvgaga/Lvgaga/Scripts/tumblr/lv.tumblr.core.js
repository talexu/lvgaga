import * as factory from '../business/lv.foundation.factory.js';
import * as util from '../business/lv.foundation.utility.js';
import * as token from '../business/lv.foundation.token.js';
import * as favorite from '../business/lv.foundation.favorite.js';
import {sprintf} from 'sprintf-js';

let reactRoot;
let tumSas;
let favSas;
let comSas;
let continuationToken;
let mediaType;
let tumblrCategory;
const takingCount = 20;
const commentTakingCount = 5;
let tableNameOfTumblr;
let tableNameOfFavorite;
let tableNameOfComment;

let initTumblrs = (entities) => {
    reactRoot.state.dataContext = reactRoot.state.dataContext.concat(factory.createTumblrs(entities));
    util.refreshState(reactRoot);
    return entities;
};

let loadFavorites = (tumblrs) => {
    return util.retryExecute(() => {
        let from = tumblrs[0].RowKey;
        let to = tumblrs[tumblrs.length - 1].RowKey;

        return util.queryAzureTable(favSas, {
            filter: sprintf("RowKey ge '%s_%s' and RowKey le '%s_%s'", mediaType, from, mediaType, to),
            select: "RowKey"
        }).done((data) => {
            let loadedFavs = {};

            // read all favorites
            for (let f of data.value) {
                loadedFavs[factory.getInvertedTicks(f.RowKey)] = true;
            }

            // set favorite for tumblrs
            for (let t of tumblrs) {
                if (loadedFavs[t.RowKey]) {
                    t.IsFavorited = true;
                }
            }

            // refresh UI
            util.refreshState(reactRoot);
        });
    }, () => {
        return token.getToken([tableNameOfFavorite]).done((data) => {
            favSas = data;
        });
    });
};

let loadTumblrs = (e) => {
    let button = e.target;

    return util.retryExecuteLadda(() => {
        return util.queryAzureTable(tumSas, {
            continuationToken: continuationToken,
            filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
            top: takingCount
        }).done((data, textStatus, jqXhr) => {
            continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
            continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
        }).done((data) => {
            initTumblrs(data.value);
            loadFavorites(data.value);
        });
    }, () => {
        return token.getToken([tableNameOfTumblr]).done((data) => {
            tumSas = data;
        });
    }, button);
};

let loadComments = (tumblr) => {
    return util.retryExecute(() => {
        return util.queryAzureTable(comSas, {
            filter: sprintf("PartitionKey eq '%s'", tumblr.RowKey),
            top: commentTakingCount
        }).done((data) => {
            tumblr.comments = factory.createComments(data.value);

            util.refreshState(reactRoot);
        });
    }, () => {
        return token.getToken([tableNameOfComment]).done((data) => {
            comSas = data;
        });
    });
};

let getLoadingButtonStyle = ()=> {
    let btnStyle = {
        display: "inline"
    };
    if (continuationToken && (!continuationToken.NextPartitionKey || !continuationToken.NextRowKey)) {
        btnStyle.display = "none";
    }

    return btnStyle;
};

let initialize = ({
    reactRootK:reactRootV,
    tumSasK:tumSasV,
    continuationTokenK:continuationTokenV,
    mediaTypeK:mediaTypeV,
    tumblrCategoryK:tumblrCategoryV,
    tableNameOfTumblrK:tableNameOfTumblrV,
    tableNameOfFavoriteK:tableNameOfFavoriteV,
    tableNameOfCommentK:tableNameOfCommentV
    }) => {

    reactRoot = reactRootV;
    tumSas = tumSasV;
    continuationToken = continuationTokenV || {};
    mediaType = mediaTypeV;
    tumblrCategory = tumblrCategoryV;
    tableNameOfTumblr = tableNameOfTumblrV;
    tableNameOfFavorite = tableNameOfFavoriteV;
    tableNameOfComment = tableNameOfCommentV;
};

export * from '../business/lv.foundation.factory.js';
export * from '../business/lv.foundation.utility.js';
export * from '../business/lv.foundation.favorite.js';
export
{
    reactRoot, initialize, loadTumblrs, loadFavorites, loadComments, getLoadingButtonStyle
};