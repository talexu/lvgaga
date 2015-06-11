(function () {
    var that;
    var tumSas;
    var favSas;
    var comSas;
    var continuationToken;
    var mediaType;
    var tumblrCategory;
    var takingCount;
    var commentTakingCount;
    var tableNameOfTumblr;
    var tableNameOfFavorite;
    var tableNameOfComment;

    var loadFavorites = function (tumblrs) {
        return lv.retryExecute(function () {
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

                $.each(tumblrs, function (index, value) {
                    if (loadedFavs[value.RowKey]) {
                        value.IsFavorited = true;
                    }
                });

                lv.refreshState(that);
            });
        }, function () {
            return lv.token.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
            });
        });
    };

    var initTumblrs = function (entities) {
        that.state.dataContext = that.state.dataContext.concat(lv.factory.createTumblrs(entities));

        lv.refreshState(that);
        return entities;
    };

    var loadTumblrs = function (e) {
        var button = e.target;

        return lv.retryExecuteLadda(function () {
            return lv.queryAzureTable(tumSas, {
                continuationToken: continuationToken,
                filter: sprintf("PartitionKey ge '%s' and PartitionKey lt '%s' and RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1, tumblrCategory, tumblrCategory + 1),
                top: takingCount
            }).done(function (data, textStatus, jqXhr) {
                continuationToken = continuationToken || {};
                continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
            }).done(function (data) {
                var entities = initTumblrs(data.value);
                loadFavorites(entities);
            });
        }, function () {
            return lv.token.getToken([tableNameOfTumblr]).done(function (data) {
                tumSas = data;
            });
        }, button);
    };

    var Functions = React.createClass({
        setFavorite: function (e) {
            var button = e.target;
            var {dataContext} = this.props;

            if (dataContext.IsFavorited) {
                return lv.ajaxLadda(function () {
                    return lv.favorite.deleteFavorite(dataContext).done(function () {
                        dataContext.IsFavorited = false;

                        lv.refreshState(that);
                    });
                }, button);
            } else {
                return lv.ajaxLadda(function () {
                    return lv.favorite.postFavorite(dataContext).done(function () {
                        dataContext.IsFavorited = true;

                        lv.refreshState(that);
                    });
                }, button);
            }
        },
        render: function () {
            var {dataContext} = this.props;

            var classNameOfFavorite = "btn btn-default btn-sm ladda-button";
            classNameOfFavorite += dataContext.IsFavorited ? " btn-selected" : "";

            return (
                <div>
                    <button type="button" className={classNameOfFavorite} data-style="zoom-out"
                            data-spinner-color="#333"
                            onClick={this.setFavorite}>
                        <span className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span>
                    </button>

                    <a className="btn btn-default btn-sm mar-left" href={dataContext.sharingUri} target="_blank">
                        <span className="glyphicon glyphicon-share-alt" aria-hidden="true"></span>
                    </a>

                    <button type="button display-block mar-right" className="btn btn-default btn-sm pull-right"
                            data-toggle="collapse"
                            data-target={"#"+dataContext.Base64Id} aria-expanded="false"
                            aria-controls={dataContext.Base64Id}>
                        <span className="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                    </button>
                </div>
            );
        }
    });

    var CommentForm = React.createClass({
        getInitialState: function () {
            return {
                text: ""
            };
        },
        handleChange: function (e) {
            this.setState({text: e.target.value});
        },
        postComment: function (e) {
            var {dataContext} = this.props;
            var self = this;

            lv.comment.postComment({
                tumblr: dataContext,
                commentText: this.state.text,
                button: e.target
            }).done(function (data) {
                dataContext.comments.unshift(lv.factory.createComment(data));
                self.setState(self.getInitialState());

                lv.refreshState(that);
            });
        },
        render: function () {
            return (
                <div>
                    <form role="form">
                        <div className="form-group mar-bottom">
                            <label for="comment">评论：</label>
                            <textarea className="form-control max-width-none" rows="3" value={this.state.text}
                                      onChange={this.handleChange}></textarea>

                            <div className="pull-right">
                                <button type="button" className="btn btn-default btn-sm ladda-button"
                                        data-style="zoom-out"
                                        data-spinner-color="#333" onClick={this.postComment}>
                                    <span class="ladda-label">发表评论</span>
                                </button>
                            </div>
                        </div>
                    </form>
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
                <button type="button" className="btn btn-default btn-lg btn-block btn-rectangle ladda-button"
                        data-style="zoom-out" data-spinner-color="#333" style={btnStyle}
                        onClick={loadTumblrs}><span class="ladda-label">加载更多</span></button>
            );
        }
    });

    var TumblrContainer = React.createClass({
        componentDidMount: function () {
            var {dataContext} = this.props;

            $('#' + dataContext.Base64Id).on('show.bs.collapse', function () {
                if (dataContext.comments.length <= 0) {
                    return lv.retryExecute(function () {
                        return lv.queryAzureTable(comSas, {
                            filter: sprintf("PartitionKey eq '%s'", dataContext.RowKey),
                            top: commentTakingCount
                        }).done(function (data) {
                            dataContext.comments = lv.factory.createComments(data.value);

                            lv.refreshState(that);
                        });
                    }, function () {
                        return lv.token.getToken([tableNameOfComment]).done(function (data) {
                            comSas = data;
                        });
                    });
                }
            });
        },
        render: function () {
            var {dataContext} = this.props;

            return (
                <div className="box">
                    <div className="m-post photo">
                        <Tumblr dataContext={dataContext}/>
                        <Functions dataContext={dataContext}/>

                        <div className="collapse" id={dataContext.Base64Id}>
                            <CommentForm dataContext={dataContext}/>
                            <CommentList dataContext={dataContext.comments}/>

                            <div className="info2">
                                <a href={dataContext.Uri} target="_blank">全文链接</a>
                            </div>
                        </div>

                    </div>
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

    var TumblrContainerBox = React.createClass({
        getInitialState: function () {
            return {dataContext: lv.factory.createTumblrs(this.props.firstEntities)};
        },
        componentDidMount: function () {
            loadFavorites(this.state.dataContext);
        },
        render: function () {
            that = this;

            return (
                <div className="g-mn">
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
        commentTakingCount = 5;
        tableNameOfTumblr = parameters.tableNameOfTumblr;
        tableNameOfFavorite = parameters.tableNameOfFavorite;
        tableNameOfComment = parameters.tableNameOfComment;

        React.render(
            <TumblrContainerBox firstEntities={parameters.Tumblrs}/>,
            document.getElementById('div_content')
        );
    };

    lv.tumblr.initialize = initialize;
})();