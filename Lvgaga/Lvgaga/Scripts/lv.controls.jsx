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

var TumblrBox = React.createClass({
    render: function () {
        return (
            <div>
                <Tumblr dataContext={this.props.dataContext}/>
                <Functions />
                <CommentForm />
                <CommentList dataContext={this.props.dataContext.comments}/>
            </div>
        );
    }
});

var TumblrBoxList = React.createClass({
    render: function () {
        var tumblrBoxNodes = this.props.dataContext.map(function (tumblr) {
            return (
                <TumblrBox dataContext={tumblr}/>
            );
        });
        return (
            <div>
                {tumblrBoxNodes}
            </div>
        );
    }
});