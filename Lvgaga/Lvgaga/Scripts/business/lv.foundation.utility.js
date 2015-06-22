var defaultRetryTime = 3;

var retryExecute = function (func, handler, retry) {
    retry = retry === undefined ? defaultRetryTime - 1 : retry;
    if (retry < 0)
        return undefined;

    return func.apply(null, arguments).fail(function () {
        handler.apply(null, arguments).always(function () {
            retryExecute(func, handler, retry - 1);
        });
    });
};

var authorizedExecute = function (func) {
    var d = $.Deferred();
    func.apply(null, arguments).done(function (data, textStatus, jqXhr) {
        switch (jqXhr.status) {
            case 201:
                d.resolve(data, textStatus, jqXhr);
                break;
            case 200:
                var res = $.parseJSON(jqXhr.getResponseHeader("X-Responded-JSON"));
                if (res && res.status === 401) {
                    $(location).attr("href", res.headers.location.replace(/(ReturnUrl=)(.+)/, "$1" + encodeURIComponent(location.pathname)));
                    d.reject(data, textStatus, jqXhr);
                } else {
                    d.resolve(data, textStatus, jqXhr);
                }
                break;
            default:
                d.reject(data, textStatus, jqXhr);
        }
    }).fail(d.reject);

    return d;
};

var ajaxLadda = function (func, button) {
    if (!button)
        return func(null, arguments);

    var l;
    if (button instanceof jQuery) {
        l = Ladda.create(button.get(0));
    } else {
        l = Ladda.create(button);
    }
    l.start();
    return func.apply(null, arguments).always(function () {
        l.stop();
    });
};

var retryExecuteLadda = function (func, handler, button, retry) {
    return retryExecute(function () {
        return ajaxLadda(function () {
            return func.apply(null, arguments);
        }, button);
    }, function () {
        return ajaxLadda(function () {
            return handler.apply(null, arguments);
        }, button);
    }, retry);
};

var queryAzureTable = function (tableSasUrl, p) {
    if (!tableSasUrl || typeof tableSasUrl !== "string")
        return $.Deferred().reject();

    var uri = tableSasUrl;
    if (p.continuationToken) {
        if (p.continuationToken.NextPartitionKey) {
            uri += "&NextPartitionKey=" + p.continuationToken.NextPartitionKey;
        }
        if (p.continuationToken.NextRowKey) {
            uri += "&NextRowKey=" + p.continuationToken.NextRowKey;
        }
    }
    if (p.top) {
        uri += "&$top=" + p.top;
    }
    if (p.filter) {
        uri += "&$filter=" + encodeURIComponent(p.filter);
    }
    if (p.select) {
        uri += "&$select=" + encodeURIComponent(p.select);
    }
    return $.ajax({
        type: "GET",
        datatype: "json",
        url: uri,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("MaxDataServiceVersion", "3.0");
            xhr.setRequestHeader("Accept", "application/json;odata=nometadata");
        }
    }).retry({
        times: defaultRetryTime
    });
};

var refreshState = function (that) {
    that.setState(that.state);
};

export{
    retryExecute,
    authorizedExecute,
    ajaxLadda,
    retryExecuteLadda,
    queryAzureTable,
    refreshState
};