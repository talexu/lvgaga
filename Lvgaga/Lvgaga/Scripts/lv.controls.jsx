var Tumblr = React.createClass({
    render: function () {
        var {dataContext, ...other} = this.props;

        return (
            <div className="cont">
                <div className="pic">
                    <div className="img">
                        <img src={dataContext.MediaLargeUri}></img>
                    </div>
                </div>

                <div>
                    <div className="text text-1">
                        <p>{dataContext.Text}</p>
                    </div>
                    <div className="info2">
                        <p className="date">{dataContext.CreateTime}</p>
                    </div>
                </div>
            </div>
        );
    }
});

var Functions = React.createClass({
    setFavorite: function (e) {
        var {dataContext, eventHandlers, ...other} = this.props;

        eventHandlers.setFavorite(dataContext, e);
    },
    render: function () {
        var {dataContext, ...other} = this.props;

        var classNameOfFavorite = "btn btn-default btn-sm ladda-button";
        classNameOfFavorite += dataContext.IsFavorited ? " btn-selected" : "";

        return (
            <div>
                <button type="button" className={classNameOfFavorite} data-style="zoom-out" data-spinner-color="#333"
                        onClick={this.setFavorite}>
                    <span className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span>
                </button>
                <button type="button" className="btn btn-default btn-sm mar-left">
                    <span className="glyphicon glyphicon-share-alt" aria-hidden="true"></span>
                </button>
                <button type="button" className="btn btn-default btn-sm pull-right" data-toggle="collapse"
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
            commentText: ""
        };
    },
    handleChange: function (e) {
        this.setState({commentText: e.target.value});
    },
    clear: function () {
        this.setState(this.getInitialState());
    },
    postComment: function (e) {
        var postCommentEventHandler = this.props.postCommentEventHandler;
        postCommentEventHandler({
            commentText: this.state.commentText,
            e: e,
            finishing: this.clear
        });
    },
    render: function () {
        return (
            <div>
                <form role="form">
                    <div className="form-group mar-bottom">
                        <label for="comment">评论：</label>
                        <textarea className="form-control max-width-none" rows="3"
                                  value={this.state.commentText} onChange={this.handleChange}></textarea>

                        <div className="pull-right">
                            <button type="button" className="btn btn-default btn-sm ladda-button" data-style="zoom-out"
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

var Comment = React.createClass({
    render: function () {
        var {dataContext, ...other} = this.props;

        return (
            <div>
                <div className="info mar-zero">
                    <a href="#">{dataContext.UserName}</a>

                    <p className="date pull-right">{dataContext.CommentTime}</p>
                </div>
                <p>{dataContext.Text}</p>
            </div>
        );
    }
});

var CommentList = React.createClass({
    render: function () {
        var {dataContext, ...other} = this.props;

        var commentNodes = dataContext.map(function (comment) {
            return (
                <Comment dataContext={comment}/>
            );
        });
        return (
            <div>
                {commentNodes}
            </div>
        );
    }
});

var LoadingMore = React.createClass({
    render: function () {
        var {eventHandlers, ...other} = this.props;

        return (
            <button type="button" className="btn btn-default btn-lg btn-block btn-rectangle ladda-button"
                    data-style="zoom-out" data-spinner-color="#333"
                    onClick={eventHandlers.loadTumblrs}><span class="ladda-label">加载更多</span></button>
        );
    }
});

var TumblrContainer = React.createClass({
    getInitialState: function () {
        return {
            comments: []
        };
    },
    componentDidMount: function () {
        var {dataContext, eventHandlers, ...other} = this.props;
        var that = this;

        $('#' + dataContext.Base64Id).on('show.bs.collapse', function () {
            if (that.state.comments.length <= 0) {
                eventHandlers.loadComments(dataContext, function (loadedComments) {
                    that.state.comments = loadedComments;

                    lv.refreshState(that);
                });
            }
        });
    },
    postComment: function (parameters) {
        var dataContext = this.props.dataContext;
        var that = this;

        lv.comment.postComment({
            tumblr: dataContext,
            commentText: parameters.commentText,
            button: parameters.e.target,
            callback: function (postedComment) {
                that.state.comments.unshift(lv.factory.createComment(postedComment));

                lv.refreshState(that);
                parameters.finishing();
            }
        });
    },
    render: function () {
        var {dataContext, ...other} = this.props;
        var comments = this.state.comments;

        return (
            <div className="box">
                <div className="m-post photo">
                    <Tumblr {...other} dataContext={dataContext}/>
                    <Functions {...other} dataContext={dataContext}/>

                    <div className="collapse" id={dataContext.Base64Id}>
                        <CommentForm {...other} dataContext={dataContext} postCommentEventHandler={this.postComment}/>
                        <CommentList dataContext={comments}/>

                        <div className="info2">
                            <a href="#">全文链接</a>
                        </div>
                    </div>

                </div>
            </div>
        );
    }
});

var TumblrContainerList = React.createClass({
    render: function () {
        var {dataContext, ...other} = this.props;

        var TumblrContainerNodes = dataContext.map(function (tumblr) {
            return (
                <TumblrContainer {...other} dataContext={tumblr}/>
            );
        });
        return (
            <div>
                {TumblrContainerNodes}
            </div>
        );
    }
});