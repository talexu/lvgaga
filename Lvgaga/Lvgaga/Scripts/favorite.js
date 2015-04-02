function favorite(pk, rk, callback) {
    $.post("/api/v1/favorites/".concat(pk, "/", rk)).done(function (data, textStatus, jqXHR) {
        switch (jqXHR.status) {
            case 201:
                callback();
                break;
            case 200:
                var res = $.parseJSON(jqXHR.getResponseHeader("X-Responded-JSON"));
                if (res.status === 401) {
                    $(location).attr("href", res.headers.location.replace(/(ReturnUrl=)(.+)/, "$1" + encodeURIComponent(location.pathname)));
                }
                break;
            default:

        }
    });
}

function unFavorite(pk, rk, callback) {
    $.ajax({
        url: "/api/v1/favorites/".concat(pk, "/", rk),
        type: "DELETE"
    }).done(function (data, textStatus, jqXHR) {
        callback();
    });
}