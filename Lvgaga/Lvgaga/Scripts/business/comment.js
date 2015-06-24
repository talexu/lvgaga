import {sprintf} from 'sprintf-js';
import * as util from '../business/utility.js';
const defaultRetryTime = 3;

let postComment = ({
    tumblrK:tumblr,
    commentTextK:commentText,
    buttonK:button
    }) => {
    if (!commentText) return null;

    return util.ajaxLadda(() => {
        return util.authorizedExecute(() => {
            return $.post(sprintf("/api/v1/comments/%s/%s", tumblr.MediaType, tumblr.RowKey),
                {
                    "Text": commentText
                }).retry({times: defaultRetryTime});
        });
    }, button);
};

export{
    postComment
};