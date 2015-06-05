(function () {
    var sas = lv.dataContext.Sas;
    var continuationToken = lv.dataContext.ContinuationToken;
    var mediaType = lv.dataContext.MediaType;
    var tumblrCategory = lv.dataContext.TumblrCategory;
    var takingCount = lv.defaultTakingCount;

    var eventHandlers = {
        loadMoreTumblrs: function (callback) {
            lv.queryAzureTable(sas, {
                continuationToken: continuationToken,
                filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
                top: takingCount
            }).done(function (data, textStatus, jqXhr) {
                if (callback) callback(data);
                continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
            });
        }
    };
    React.render(
        <TumblrContainerBox initialState={lv.dataContext.Tumblrs} eventHandlers={eventHandlers}/>,
        document.getElementById('div_tumblrs')
    );
})();