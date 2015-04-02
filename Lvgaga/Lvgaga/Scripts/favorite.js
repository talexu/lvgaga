function favorite(pk, rk, btn) {
    $.post("/api/v1/favorites/".concat(pk, "/", rk)).done(function (data, textStatus, jqXHR) {
        switch (jqXHR.status) {
            case 201:
                btn.addClass("btn-selected");
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