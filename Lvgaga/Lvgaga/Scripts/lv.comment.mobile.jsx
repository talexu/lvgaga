(function () {
    var that;
    var comSas;
    var continuationToken;
    var favSas;
    var takingCount;
    var tableNameOfFavorite;
    var tableNameOfComment;

    var loadFavorite = function (tumblr) {
        return lv.favorite.loadFavorite({
            favSas: favSas,
            tableNameOfFavorite: tableNameOfFavorite,
            tumblr: tumblr,
            onReceiveNewToken: function (token) {
                favSas = token;
            },
            done: function () {
                lv.refreshState(that);
            }
        });
    };

    var loadComments = function (e) {
        var button;
        if (e) button = e.target;

        var dataContext = that.state.dataContext;

        return lv.comment.loadComments({
            button: button,
            comSas: comSas,
            tableNameOfComment: tableNameOfComment,
            continuationToken: continuationToken,
            takingCount: takingCount,
            onReceiveNewToken: function (token) {
                comSas = token;
            },
            onReceiveNewContinuationToken: function (partitionKey, rowKey) {
                continuationToken = continuationToken || {};
                continuationToken.NextPartitionKey = partitionKey;
                continuationToken.NextRowKey = rowKey;
            },
            done: function (loadedComments) {
                dataContext.comments = dataContext.comments.concat(loadedComments);
                lv.refreshState(that);
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
                <div>
                    <button className={classNameOfFavorite} type="button" data-style="zoom-out"
                            data-spinner-color="#333" onClick={this.setFavorite}><span
                        className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span></button>
                    <a className="btn btn-default btn-share" href={dataContext.sharingUri} target="_blank"><span
                        className="glyphicon glyphicon-share-alt" aria-hidden="true"></span></a>
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
            if (!this.state.text.trim()) return;

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
                    <form>
                        <div className="input-group box-comment">
                            <input type="text" className="form-control txb-comment" value={this.state.text}
                                   onChange={this.handleChange}
                                   placeholder="评论"/>
                            <span className="input-group-btn">
                                <button className="btn btn-default ladda-button" type="button" data-style="slide-down"
                                        data-spinner-color="#333" onClick={this.postComment}>
                                    <span className="ladda-label" aria-hidden="true">评论</span>
                                </button>
                            </span>
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
                <div className="container-block">
                    <button type="button" className="btn btn-default btn-block ladda-button" data-style="zoom-out"
                            data-spinner-color="#333" style={btnStyle} onClick={loadComments}><span
                        className="ladda-label">加载更多</span></button>
                </div>
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
                <div>
                    <div>
                        <Tumblr dataContext={dataContext}/>

                        <div className="content">
                            <Functions dataContext={dataContext}/>
                        </div>

                        <div className="content">
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
                <div>
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