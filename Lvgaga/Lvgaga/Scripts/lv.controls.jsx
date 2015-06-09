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
    render: function () {
        var {dataContext, eventHandlers, ...other} = this.props;

        var classNameOfFavorite = "btn btn-default btn-sm ladda-button";
        classNameOfFavorite += dataContext.IsFavorited ? " btn-selected" : "";

        var setFavorite = function(e){
            eventHandlers.setFavorite(dataContext, e);
        };
        return (
            <div>
                <button type="button" className={classNameOfFavorite} data-style="zoom-out" data-spinner-color="#333" onClick={setFavorite}>
                    <span className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span>
                </button>
                <button type="button" className="btn btn-default btn-sm mar-left">
                    <span className="glyphicon glyphicon-share-alt" aria-hidden="true"></span>
                </button>
                <button type="button" className="btn btn-default btn-sm pull-right" data-toggle="collapse"
                        data-target={"#"+dataContext.Base64Id} aria-expanded="false" aria-controls={dataContext.Base64Id}>
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
        var {dataContext, ...other} = this.props;

        return (
            <div>
                <p>{dataContext.UserName}</p>

                <p>{dataContext.CommentTime}</p>

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
        var {dataContext, ...other} = this.props;

        return (
            <div className="box">
                <div className="m-post photo">
                    <Tumblr {...other} dataContext={dataContext}/>
                    <Functions {...other} dataContext={dataContext}/>

                    <div className="collapse" id={dataContext.Base64Id}>
                        <CommentForm {...other}/>
                        <CommentList dataContext={this.state.comments}/>
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