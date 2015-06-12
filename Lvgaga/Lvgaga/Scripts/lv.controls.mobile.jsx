var Tumblr = React.createClass({
    componentDidMount: function () {
        var image = $(React.findDOMNode(this.refs.lazyImage));
        image.lazyload();
    },
    render: function () {
        var {dataContext} = this.props;

        return (
            <div>
                <img className="img-tumblr lazy" ref="lazyImage" data-original={dataContext.MediaLargeUri} alt=""/>

                <div className="content">
                    <p className="text-tumblr">{dataContext.Text}</p>

                    <p className="date-tumblr">{dataContext.CreateTime}</p>
                </div>
            </div>
        );
    }
});

var TumblrInstant = React.createClass({
    render: function () {
        var {dataContext} = this.props;

        return (
            <div>
                <img className="img-tumblr" src={dataContext.MediaLargeUri} alt=""/>

                <div className="content">
                    <p className="text-tumblr">{dataContext.Text}</p>

                    <p className="date-tumblr">{dataContext.CreateTime}</p>
                </div>
            </div>
        );
    }
});

var Comment = React.createClass({
    render: function () {
        var {dataContext} = this.props;

        return (
            <div>
                <hr className="hr-comment"/>
                <a>{dataContext.UserName}</a>

                <p className="date-comment pull-right">{dataContext.CommentTime}</p>

                <p className="text-comment">{dataContext.Text}</p>
            </div>
        );
    }
});

var CommentList = React.createClass({
    render: function () {
        var {dataContext} = this.props;

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