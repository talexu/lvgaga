function favorite(pk, rk, btn, callback) {
    btn.attr("disabled", "disabled");
    $.post("/api/v1/favorites/".concat(pk, "/", rk)).retry({ times: 3 }).done(function (data, textStatus, jqXHR) {
        switch (jqXHR.status) {
            case 201:
                btn.addClass("btn-selected");
                if (callback) callback();
                break;
            case 200:
                var res = $.parseJSON(jqXHR.getResponseHeader("X-Responded-JSON"));
                if (res.status === 401) {
                    $(location).attr("href", res.headers.location.replace(/(ReturnUrl=)(.+)/, "$1" + encodeURIComponent(location.pathname)));
                }
                break;
            default:

        }
    }).always(function (data, textStatus, jqXHR) {
        btn.removeAttr("disabled");
    });
}

function unFavorite(pk, rk, btn, callback) {
    btn.attr("disabled", "disabled");
    $.ajax({
        url: "/api/v1/favorites/".concat(pk, "/", rk),
        type: "DELETE"
    }).retry({ times: 3 }).done(function (data, textStatus, jqXHR) {
        btn.removeClass("btn-selected");
        if (callback) callback();
    }).always(function (data, textStatus, jqXHR) {
        btn.removeAttr("disabled");
    });
}

function getFavoriteIndex(mediaType, from, to, callback) {
    $.get("/api/v1/favorites/".concat(mediaType, "?", "from=", from, "&", "to=", to)).done(function (data, textStatus, jqXHR) {
        switch (jqXHR.status) {
            case 200:
                callback(data);
                break;
            default:
        }
    });
}