var Tumblr = React.createClass({
    render: function () {
        var ctx = this.props.dataContext;
        return (
            <div>
                <img src={ctx.MediaLargeUri}></img>

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
                Hello, world! I am a Functions.
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
    getInitialState: function() {
        return {comments: [
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
        ]};
    },
    render: function () {
        return (
            <div>
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
            <div>
                {TumblrContainerNodes}
            </div>
        );
    }
});

var TumblrContainerBox = React.createClass({
    getInitialState: function() {
        return {dataContext: this.props.initialState};
    },
    render: function (){
        return (
            <TumblrContainerList dataContext={this.state.dataContext}/>
        );
    }
});