var Tumblr = React.createClass({
    render: function () {
        var ctx = this.props.dataContext;
        return (
            <div className="cont">
                <div className="pic">
                    <div className="img">
                        <img src={ctx.MediaLargeUri}></img>
                    </div>
                </div>


                <div>
                    <div className="text text-1">
                        <p>{ctx.Text}</p>
                    </div>
                    <div className="info2">
                        <p className="date">{ctx.CreateTime}</p>
                    </div>
                </div>
            </div>
        );
    }
});

var Functions = React.createClass({
    render: function () {
        var ctx = this.props.dataContext;
        return (
            <div>
                <button type="button" className="btn btn-default btn-sm">
                    <span className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span>
                </button>
                <button type="button" className="btn btn-default btn-sm mar-left">
                    <span className="glyphicon glyphicon-share-alt" aria-hidden="true"></span>
                </button>
                <button type="button" className="btn btn-default btn-sm pull-right" data-toggle="collapse"
                        data-target={"#"+ctx.Base64Id} aria-expanded="false" aria-controls={ctx.Base64Id}>
                    <span className="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                </button>
            </div>
        );
    }
});

var CommentForm = React.createClass({
    render: function () {
        return (
            <div>
                Hello, world! I am a CommentForm.
            </div>
        );
    }
});

var Comment = React.createClass({
    render: function () {
        var ctx = this.props.dataContext;
        return (
            <div>
                <p>{ctx.UserName}</p>

                <p>{ctx.CommentTime}</p>

                <p>{ctx.Text}</p>
            </div>
        );
    }
});

var CommentList = React.createClass({
    render: function () {
        var commentNodes = this.props.dataContext.map(function (comment) {
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
        return (
            /*<div className="box">
             <div className="info2">
             <button type="button" className="btn btn-link">加载更多</button>
             </div>
             </div>*/
            <button type="button" className="btn btn-default btn-lg btn-block btn-rectangle"
                    onClick={this.props.eventHandler}>加载更多</button>
        );
    }
});

var TumblrContainer = React.createClass({
    getInitialState: function () {
        return {
            comments: [
                {
                    UserName: "bjutales@hotmail.com",
                    CommentTime: "2015-06-02 16:33:41",
                    Text: "一些评论1"
                },
                {
                    UserName: "bjutales@hotmail.com",
                    CommentTime: "2015-06-02 16:33:41",
                    Text: "一些评论2"
                }
            ]
        };
    },
    render: function () {
        var ctx = this.props.dataContext;
        return (
            <div className="box">
                <div className="m-post photo">
                    <Tumblr {...this.props}/>
                    <Functions {...this.props}/>

                    <div className="collapse" id={ctx.Base64Id}>
                        <CommentForm {...this.props}/>
                        <CommentList dataContext={this.state.comments}/>
                    </div>

                </div>
            </div>
        );
    }
});

var TumblrContainerList = React.createClass({
    render: function () {
        var TumblrContainerNodes = this.props.dataContext.map(function (tumblr) {
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
        return {dataContext: this.props.initialState};
    },
    loadMoreTumblrs: function (e) {
        var that = this;
        this.props.eventHandlers.loadMoreTumblrs(function (data) {
            that.state.dataContext = that.state.dataContext.concat(data.value);
            that.setState(that.state);
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