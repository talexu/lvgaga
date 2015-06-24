import {sprintf} from 'sprintf-js';
import * as util from '../business/utility.js';
const defaultRetryTime = 3;

let postFavorite = (tumblr) => {
    return util.authorizedExecute(() => {
        return $.post(sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey)).retry({times: defaultRetryTime});
    });
};

let deleteFavorite = (tumblr) => {
    return util.authorizedExecute(() => {
        return $.ajax({
            url: sprintf("/api/v1/favorites/%s/%s", tumblr.MediaType, tumblr.RowKey),
            type: "DELETE"
        }).retry({times: defaultRetryTime});
    });
};

let setFavorite = ({
    buttonK:button,
    tumblrK:tumblr,
    doneK:done
    }) => {
    if (tumblr.IsFavorited) {
        return util.ajaxLadda(() => {
            return deleteFavorite(tumblr).done(() => {
                tumblr.IsFavorited = false;
                done(tumblr);
            });
        }, button);
    } else {
        return util.ajaxLadda(() => {
            return postFavorite(tumblr).done(() => {
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