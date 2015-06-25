import * as factory from '../business/factory.js';
import * as util from '../business/utility.js';
import * as token from '../business/token.js';
import * as favorite from '../business/favorite.js';
import {sprintf} from 'sprintf-js';

let reactRoot;
let comSas;
let continuationToken;
let favSas;
const takingCount = 20;
let tableNameOfFavorite;
let tableNameOfComment;

let loadFavorite = (tumblr) => {
    return util.retryExecute(() => {
        return util.queryAzureTable(favSas, {
            filter: sprintf("RowKey eq '%s_%s'", tumblr.MediaType, tumblr.RowKey),
            select: "RowKey"
        }).done((data) => {
            if (data.value.length > 0) {
                tumblr.IsFavorited = true;
                util.refreshState(reactRoot);
            }
        });
    }, () => {
        return token.getToken([tableNameOfFavorite]).done((data) => {
            favSas = data;
        });
    });
};

let loadComments = (e) => {
    let button;
    if (e) {
        button = e.target;
    }

    return util.retryExecuteLadda(() => {
        return util.queryAzureTable(comSas, {
            continuationToken: continuationToken,
            top: takingCount
        }).done((data, textStatus, jqXhr) => {
            continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
            continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
        }).done((data) => {
            if (data.value.length > 0) {
                reactRoot.state.dataContext.comments = reactRoot.state.dataContext.comments.concat(factory.createComments(data.value));
                util.refreshState(reactRoot);
            }
        });
    }, () => {
        return token.getToken([tableNameOfComment]).done((data) => {
            comSas = data;
        });
    }, button);
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
    comSasK:comSasV,
    continuationTokenK:continuationTokenV,
    favSasK:favSasV,
    tableNameOfFavoriteK:tableNameOfFavoriteV,
    tableNameOfCommentK:tableNameOfCommentV,
    hostK: hostV
    }) => {

    reactRoot = reactRootV;
    comSas = comSasV;
    continuationToken = continuationTokenV || {};
    favSas = favSasV;
    tableNameOfFavorite = tableNameOfFavoriteV;
    tableNameOfComment = tableNameOfCommentV;
    factory.setHost(hostV);
};

export * from '../business/factory.js';
export * from '../business/utility.js';
export * from '../business/favorite.js';
export{
    reactRoot, initialize, loadFavorite, loadComments, getLoadingButtonStyle
};