(function () {
    var sas = lv.dataContext.Sas;
    var favSas;
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
        loadFavorites: function (tumblrs) {
            var that = this;
            lv.retryExecute(function () {
                var from = tumblrs[0].RowKey;
                var to = tumblrs[tumblrs.length - 1].RowKey;
                return lv.queryAzureTable(favSas, {
                    filter: sprintf("RowKey ge '%s_%s' and RowKey le '%s_%s'", mediaType, from, mediaType, to),
                    select: "RowKey"
                }).done(function (data) {
                    var loadedFavs = {};
                    $.each(data.value, function (index, value) {
                        loadedFavs[lv.getInvertedTicks(value.RowKey)] = true;
                    });

                    $.each(tumblrs, function(index, value){
                        if(loadedFavs[value.RowKey]){
                            value.IsFavorited = true;
                        }
                    });
                    that.setState(that.state);
                });
            }, function () {
                return lv.getToken([tableNameOfFavorite]).done(function (data) {
                    favSas = data;
                });
            });
        },
        addFavorite: function(e, mediaType, rowKey){

        },
        componentDidMount: function () {
            this.loadFavorites(this.state.dataContext);
        },
        loadMoreTumblrs: function (e) {
            var that = this;
            var button = e.target;
            return lv.retryExecute(function () {
                return lv.ajaxLadda(function () {
                    return lv.queryAzureTable(sas, {
                        continuationToken: continuationToken,
                        filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
                        top: takingCount
                    }).done(function (data) {
                        $.each(data.value, function (index, tumblr) {
                            tumblr.RowKey = lv.getInvertedTicks(tumblr.RowKey);
                            var id = sprintf("%s/%s", tumblr.MediaType, tumblr.RowKey);
                            var base64id = window.btoa(id);
                            tumblr.Id = id;
                            tumblr.Base64Id = base64id;
                        });

                        that.state.dataContext = that.state.dataContext.concat(data.value);
                        that.setState(that.state);

                        that.loadFavorites(that.state.dataContext);
                    }).done(function (data, textStatus, jqXhr) {
                        continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                        continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");

                        if (!continuationToken.NextPartitionKey || !continuationToken.NextRowKey) {
                            button.style.display = "none";
                        }
                    });
                }, button);
            }, function () {
                return lv.ajaxLadda(function () {
                    return lv.getToken([tableNameOfTumblr]).done(function (data) {
                        sas = data;
                    });
                }, button);
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