﻿import {sprintf} from 'sprintf-js';
var defaultRetryTime = 3;

var getToken = function (paths) {
    return $.get(sprintf("/api/v1/tokens/%s", paths.join("/"))).retry({
        times: defaultRetryTime
    });
};

export{
    getToken
};