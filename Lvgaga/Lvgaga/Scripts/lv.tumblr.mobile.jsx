(function () {
    var that;
    var tumSas;
    var continuationToken;
    var favSas;
    var mediaType;
    var tumblrCategory;
    var takingCount;
    var tableNameOfTumblr;
    var tableNameOfFavorite;

    var loadFavorites = function (tumblrs) {
        return lv.tumblr.loadFavorites({
            favSas: favSas,
            tableNameOfFavorite: tableNameOfFavorite,
            tumblrs: tumblrs,
            mediaType: mediaType,
            onReceiveNewToken: function (token) {
                favSas = token;
            },
            done: function () {
                lv.refreshState(that);
            }
        });
    };

    var initTumblrs = function (entities) {
        that.state.dataContext = that.state.dataContext.concat(lv.factory.createTumblrs(entities));

        lv.refreshState(that);
        return entities;
    };

    var loadTumblrs = function (e) {
        return lv.tumblr.loadTumblrs({
            button: e.target,
            tumSas: tumSas,
            mediaType: mediaType,
            tumblrCategory: tumblrCategory,
            takingCount: takingCount,
            continuationToken: continuationToken,
            tableNameOfTumblr: tableNameOfTumblr,
            onReceiveNewToken: function (token) {
                tumSas = token;
            },
            onReceiveNewContinuationToken: function (partitionKey, rowKey) {
                continuationToken = continuationToken || {};
                continuationToken.NextPartitionKey = partitionKey;
                continuationToken.NextRowKey = rowKey;
            },
            done: function (entities) {
                var tumblrs = initTumblrs(entities);
                loadFavorites(tumblrs);
            }
        });
    };

    var Functions = React.createClass({
        setFavorite: function (e) {
            var {dataContext} = this.props;

            return lv.favorite.setFavorite({
                button: e.target,
                tumblr: dataContext,
                done: function () {
                    lv.refreshState(that);
                }
            });
        },
        render: function () {
            var {dataContext} = this.props;

            var classNameOfFavorite = "btn btn-default btn-favorite ladda-button";
            classNameOfFavorite += dataContext.IsFavorited ? " btn-selected" : "";

            return (
                <div className="content">
                    <button className={classNameOfFavorite} type="button" data-style="zoom-out"
                            data-spinner-color="#333" onClick={this.setFavorite}><span
                        className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span></button>
                    <a className="btn btn-default btn-share" href={dataContext.sharingUri} target="_blank"><span
                        className="glyphicon glyphicon-share-alt" aria-hidden="true"></span></a>
                    <a className="btn btn-default pull-right btn-comment" href={dataContext.Uri}><span
                        className="glyphicon glyphicon-th-list" aria-hidden="true"></span></a>
                </div>
            );
        }
    });

    var TumblrContainer = React.createClass({
        render: function () {
            var {dataContext} = this.props;

            return (
                <div className="container-tumblr">
                    <Tumblr dataContext={dataContext}/>
                    <Functions dataContext={dataContext}/>
                    <hr className="hr-tumblr"/>
                </div>
            );
        }
    });

    var TumblrContainerList = React.createClass({
        render: function () {
            var {dataContext} = this.props;

            var TumblrContainerNodes = dataContext.map(function (tumblr) {
                return (
                    <TumblrContainer dataContext={tumblr}/>
                );
            });
            return (
                <div>
                    {TumblrContainerNodes}
                </div>
            );
        }
    });

    var LoadingMore = React.createClass({
        render: function () {
            var btnStyle = {
                display: "inline"
            };
            if (continuationToken && (!continuationToken.NextPartitionKey || !continuationToken.NextRowKey)) {
                btnStyle.display = "none";
            }

            return (
                <div className="container-block">
                    <button type="button" className="btn btn-default btn-block ladda-button" data-style="zoom-out"
                            data-spinner-color="#333" style={btnStyle} onClick={loadTumblrs}><span
                        className="ladda-label">加载更多</span></button>
                </div>
            );
        }
    });

    var TumblrBox = React.createClass({
        getInitialState: function () {
            return {dataContext: lv.factory.createTumblrs(this.props.firstEntities)};
        },
        componentDidMount: function () {
            loadFavorites(this.state.dataContext);
        },
        render: function () {
            that = this;

            return (
                <div>
                    <TumblrContainerList dataContext={this.state.dataContext}/>
                    <LoadingMore/>
                </div>
            );
        }
    });

    var initialize = function (parameters) {
        tumSas = parameters.Sas;
        continuationToken = parameters.ContinuationToken;
        mediaType = parameters.MediaType;
        tumblrCategory = parameters.TumblrCategory;
        takingCount = lv.defaultTakingCount;
        tableNameOfTumblr = parameters.tableNameOfTumblr;
        tableNameOfFavorite = parameters.tableNameOfFavorite;

        React.render(
            <TumblrBox firstEntities={parameters.Tumblrs}/>,
            document.getElementById('div_content')
        );
    };

    lv.tumblr.initialize = initialize;
})();