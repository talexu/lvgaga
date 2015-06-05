(function () {
    var sas = lv.dataContext.Sas;
    var continuationToken = lv.dataContext.ContinuationToken;
    var mediaType = lv.dataContext.MediaType;
    var tumblrCategory = lv.dataContext.TumblrCategory;
    var takingCount = lv.defaultTakingCount;
    var tableNameOfTumblr = lv.dataContext.tableNameOfTumblr;
    var tableNameOfFavorite = lv.dataContext.tableNameOfFavorite;

    var TumblrContainerBox = React.createClass({
        getInitialState: function () {
            return {dataContext: this.props.initialState};
        },
        loadMoreTumblrs: function (e) {
            var that = this;
            return lv.retryExecute(function () {
                return lv.ajaxLadda(function () {
                    return lv.queryAzureTable(sas, {
                        continuationToken: continuationToken,
                        filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
                        top: takingCount
                    }).done(function (data) {
                        $.each(data.value, function (index, tumblr) {
                            var rk = lv.getInvertedTicks(tumblr.RowKey);
                            var id = sprintf("%s/%s", tumblr.MediaType, rk);
                            var base64id = window.btoa(id);
                            tumblr.Id = id;
                            tumblr.Base64Id = base64id;
                        });

                        that.state.dataContext = that.state.dataContext.concat(data.value);
                        that.setState(that.state);
                    }).done(function (data, textStatus, jqXhr) {
                        continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                        continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
                    });
                }, e.target);
            }, function () {
                return lv.ajaxLadda(function () {
                    return lv.getToken([tableNameOfTumblr]).done(function (data) {
                        sas = data;
                    });
                }, e.target);
            });
        },
        render: function () {
            return (
                <div className="g-mn">
                    <TumblrContainerList dataContext={this.state.dataContext}/>
                    <LoadingMore eventHandler={this.loadMoreTumblrs}/>
                </div>
            );
        }
    });

    React.render(
        <TumblrContainerBox initialState={lv.dataContext.Tumblrs}/>,
        document.getElementById('div_tumblrs')
    );
})();