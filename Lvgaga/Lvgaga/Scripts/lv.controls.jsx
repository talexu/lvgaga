var Tumblr = React.createClass({
    render: function () {
        var ctx = this.props.dataContext;
        return (
            <div className="cont">
                <div className="img">
                    <img src={ctx.MediaLargeUri}></img>
                </div>


                <div>
                    <p>{ctx.Text}</p>

                    <p>{ctx.CreateTime}</p>
                </div>
            </div>
        );
    }
});

var Functions = React.createClass({
    render: function () {
        return (
            <div>
                <button className="btn btn-default btn-favorite ladda-button" type="button" data-style="zoom-out"
                        data-spinner-color="#333"><span className="ladda-label glyphicon glyphicon-heart"
                                                        aria-hidden="true"></span></button>
                <a className="btn btn-default btn-share" href="#" target="_blank"><span
                    className="glyphicon glyphicon-share-alt" aria-hidden="true"></span></a>
                <button className="btn btn-default pull-right btn-comment" type="button"><span
                    className="glyphicon glyphicon-th-list" aria-hidden="true"></span></button>
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
        return (
            <div className="box">
                <Tumblr dataContext={this.props.dataContext}/>
                <Functions />
                <CommentForm />
                <CommentList dataContext={this.state.comments}/>
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
            <div className="g-mn">
                {TumblrContainerNodes}
            </div>
        );
    }
});

var TumblrContainerBox = React.createClass({
    getInitialState: function () {
        return {dataContext: this.props.initialState};
    },
    render: function () {
        return (
            <div className="g-doc">
                <TumblrContainerList dataContext={this.state.dataContext}/>
            </div>
        );
    }
});