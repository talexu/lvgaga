﻿(function () {
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