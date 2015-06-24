import {sprintf} from 'sprintf-js';
const defaultRetryTime = 3;

let getToken = (paths) => {
    return $.get(sprintf("/api/v1/tokens/%s", paths.join("/"))).retry({
        times: defaultRetryTime
    });
};

export{
    getToken
};