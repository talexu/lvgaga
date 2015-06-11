(function () {
    var that;
    var comSas;
    var continuationToken;
    var favSas;
    var takingCount;
    var tableNameOfFavorite;
    var tableNameOfComment;

    var loadFavorite = function (tumblr) {
        return lv.retryExecute(function () {
            return lv.queryAzureTable(favSas, {
                filter: sprintf("RowKey eq '%s_%s'", tumblr.MediaType, tumblr.RowKey),
                select: "RowKey"
            }).done(function (data) {
                if (data.value.length > 0) {
                    tumblr.IsFavorited = true;

                    lv.refreshState(that);
                }
            });
        }, function () {
            return lv.token.getToken([tableNameOfFavorite]).done(function (data) {
                favSas = data;
            });
        });
    };

    var loadComments = function (e) {
        var button;
        if (e) {
            button = e.target;
        }

        var dataContext = that.state.dataContext;

        return lv.retryExecuteLadda(function () {
            return lv.queryAzureTable(comSas, {
                continuationToken: continuationToken,
                top: takingCount
            }).done(function (data, textStatus, jqXhr) {
                continuationToken = continuationToken || {};
                continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
            }).done(function (data) {
                if (data.value.length > 0) {
                    var comments = dataContext.comments.concat(lv.factory.createComments(data.value));
                    dataContext.comments = comments;

                    lv.refreshState(that);
                }
            });
        }, function () {
            return lv.token.getToken([tableNameOfComment]).done(function (data) {
                comSas = data;
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
                        <textarea className="form-control max-width-none" rows="3"
                                  value={this.state.text} onChange={this.handleChange}></textarea>

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
                        onClick={loadComments}><span class="ladda-label">加载更多</span></button>
            );
        }
    });

    var TumblrContainer = React.createClass({
        componentDidMount: function () {
            loadComments();
        },
        render: function () {
            var {dataContext} = this.props;

            return (
                <div className="box">
                    <div className="m-post photo">
                        <Tumblr dataContext={dataContext}/>
                        <Functions dataContext={dataContext}/>

                        <div>
                            <CommentForm dataContext={dataContext}/>
                            <CommentList dataContext={dataContext.comments}/>
                        </div>

                    </div>
                </div>
            );
        }
    });

    var TumblrContainerBox = React.createClass({
        getInitialState: function () {
            return {dataContext: lv.factory.createTumblr(this.props.Entity)};
        },
        componentDidMount: function () {
            loadFavorite(this.state.dataContext);
        },
        render: function () {
            that = this;

            return (
                <div className="g-mn">
                    <TumblrContainer dataContext={this.state.dataContext}/>
                    <LoadingMore/>
                </div>
            );
        }
    });

    var initialize = function (parameters) {
        comSas = parameters.Sas;
        takingCount = lv.defaultTakingCount;
        tableNameOfFavorite = parameters.tableNameOfFavorite;
        tableNameOfComment = parameters.tableNameOfComment;

        React.render(
            <TumblrContainerBox Entity={parameters}/>,
            document.getElementById('div_content')
        );
    };

    lv.comment.initialize = initialize;
})();