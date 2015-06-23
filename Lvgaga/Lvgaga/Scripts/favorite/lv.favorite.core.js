import * as factory from '../business/lv.foundation.factory.js';
import * as util from '../business/lv.foundation.utility.js';
import * as token from '../business/lv.foundation.token.js';
import * as favorite from '../business/lv.foundation.favorite.js';
import {sprintf} from 'sprintf-js';

let reactRoot;
let sasFav;
let continuationToken = {};
let mediaType;
let takingCount;
let tableNameOfFavorite;
const expectedCellWidth = 300;
const loadingRow = 5;
let cellWidth;

let loadFavorites = (e) => {
    let button;
    if (e) {
        button = e.target;
    }

    return util.retryExecute(() => {
        return util.ajaxLadda(() => {
            return util.queryAzureTable(sasFav, {
                continuationToken: continuationToken,
                filter: sprintf("RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1),
                top: takingCount
            }).done((data, textStatus, jqXhr) => {
                continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
            }).done((data) => {
                reactRoot.state.tumblrs = reactRoot.state.tumblrs.concat(factory.createTumblrs(data.value, true));
                util.refreshState(reactRoot);
            });
        }, button);
    }, () => {
        return util.ajaxLadda(() => {
            return token.getToken([tableNameOfFavorite]).done((data) => {
                sasFav = data;
            });
        }, button);
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
    reactRootK : reactRootV,
    sasFavK : sasFavV,
    mediaTypeK : mediaTypeV,
    tableNameOfFavoriteK : tableNameOfFavoriteV
    }) => {

    reactRoot = reactRootV;
    sasFav = sasFavV;
    mediaType = mediaTypeV;
    tableNameOfFavorite = tableNameOfFavoriteV;
};

let setSize = (containerWidth)=> {

    let rowCapacity = Math.ceil(containerWidth / expectedCellWidth);
    cellWidth = containerWidth / rowCapacity;
    takingCount = rowCapacity * loadingRow || takingCount;
};

export{
    initialize, setSize, loadFavorites, getLoadingButtonStyle, cellWidth
};