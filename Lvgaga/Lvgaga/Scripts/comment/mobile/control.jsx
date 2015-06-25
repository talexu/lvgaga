import * as Core from '../core.js';
import * as comment from '../../business/comment.js';
import Tumblr from '../../common/mobile/tumblr.instant.jsx';
import Loading from '../../common/mobile/loading.jsx';

class Functions extends React.Component {
    constructor() {
        super();
        this.setFavorite = this.setFavorite.bind(this);
    }

    setFavorite(e) {
        let {dataContext} = this.props;

        return Core.setFavorite({
            buttonK: e.target,
            tumblrK: dataContext,
            doneK: ()=> {
                Core.refreshState(Core.reactRoot);
            }
        });
    }

    render() {
        let {dataContext} = this.props;

        let classNameOfFavorite = "btn btn-default btn-favorite ladda-button";
        classNameOfFavorite += dataContext.IsFavorited ? " btn-selected" : "";

        return (
            <div>
                <button className={classNameOfFavorite} type="button" data-style="zoom-out"
                        data-spinner-color="#333" onClick={this.setFavorite}>
                    <span className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span>
                </button>
                <a className="btn btn-default btn-share" href={dataContext.sharingUri} target="_blank">
                    <span className="glyphicon glyphicon-share-alt" aria-hidden="true"></span>
                </a>
            </div>
        );
    }
}

class CommentForm extends React.Component {
    constructor() {
        super();
        this.state = {text: ""};

        this.handleChange = this.handleChange.bind(this);
        this.postComment = this.postComment.bind(this);
    }

    handleChange(e) {
        this.setState({text: e.target.value});
    }

    postComment(e) {
        let {dataContext, postSuccess} = this.props;

        return comment.postComment({
            tumblrK: dataContext,
            commentTextK: this.state.text,
            buttonK: e.target
        }).done(() => {
            this.setState({text: ""});
        }).done((data)=> {
            postSuccess(data);
        });
    }

    render() {
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
}

class Comment extends React.Component {
    render() {
        let {dataContext} = this.props;

        return (
            <div>
                <hr className="hr-comment"/>
                <a>{dataContext.UserName}</a>

                <p className="date-comment pull-right">{dataContext.CommentTime}</p>

                <p className="text-comment">{dataContext.Text}</p>
            </div>
        );
    }
}

class CommentList extends React.Component {
    render() {
        let {dataContext} = this.props;

        let commentNodes = dataContext.map((comment) => {
            return (
                <Comment key={comment.Base64Id} dataContext={comment}/>
            );
        });
        return (
            <div>
                {commentNodes}
            </div>
        );
    }
}

class TumblrContainer extends React.Component {
    constructor() {
        super();
        this.postSuccess = this.postSuccess.bind(this);
    }

    postSuccess(comment) {
        let {dataContext} = this.props;
        dataContext.comments.unshift(Core.createComment(comment));
        Core.refreshState(Core.reactRoot);
    }

    render() {
        let {dataContext} = this.props;

        return (
            <div>
                <div>
                    <Tumblr dataContext={dataContext}/>

                    <div className="content">
                        <Functions dataContext={dataContext}/>
                    </div>

                    <div className="content">
                        <CommentForm dataContext={dataContext} postSuccess={this.postSuccess}/>
                        <CommentList dataContext={dataContext.comments}/>
                    </div>

                </div>
            </div>
        );
    }
}

class TumblrContainerBox extends React.Component {
    constructor(props) {
        super(props);
        this.componentDidMount = this.componentDidMount.bind(this);

        Core.initialize({
            reactRootK: this,
            comSasK: props.dataContext.Sas,
            continuationTokenK: props.dataContext.ContinuationToken,
            tableNameOfFavoriteK: props.tableNameOfFavorite,
            tableNameOfCommentK: props.tableNameOfComment,
            hostK: props.host
        });
        this.state = {dataContext: Core.createTumblr(props.dataContext)};
    }

    componentDidMount() {
        Core.loadComments();
        Core.loadFavorite(this.state.dataContext);
    }

    render() {
        return (
            <div className="g-mn">
                <TumblrContainer dataContext={this.state.dataContext}/>
                <Loading onClickHandler={Core.loadComments} style={Core.getLoadingButtonStyle()}/>
            </div>
        );
    }
}

export default TumblrContainerBox;