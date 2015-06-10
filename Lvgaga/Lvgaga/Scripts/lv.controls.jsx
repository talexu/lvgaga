var Tumblr = React.createClass({
    render: function () {
        var {dataContext} = this.props;

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

var Comment = React.createClass({
    render: function () {
        var {dataContext} = this.props;

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