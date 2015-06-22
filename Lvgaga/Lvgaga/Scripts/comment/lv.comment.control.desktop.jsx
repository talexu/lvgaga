import * as Core from './lv.comment.core.js';
import Tumblr from '../common/lv.control.desktop.tumblr.jsx';
import Loading from '../common/lv.control.desktop.loading.jsx';
import CommentForm from '../common/lv.control.desktop.commentform.jsx';
import CommentList from '../common/lv.control.desktop.commentlist.jsx';

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

        let classNameOfFavorite = "btn btn-default btn-sm ladda-button";
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
}

class TumblrContainer extends React.Component {
    constructor() {
        super();
        this.postSuccess = this.postSuccess.bind(this);
    }

    componentDidMount() {
        Core.loadComments();
    }

    postSuccess(comment) {
        let {dataContext} = this.state;
        dataContext.comments.unshift(Core.createComment(comment));
        Core.refreshState(Core.reactRoot);
    }

    render() {
        let {dataContext} = this.props;

        return (
            <div className="box">
                <div className="m-post photo">
                    <Tumblr dataContext={dataContext}/>
                    <Functions dataContext={dataContext}/>

                    <CommentForm dataContext={dataContext} postSuccess={this.postSuccess}/>
                    <CommentList dataContext={dataContext.comments}/>
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
            tableNameOfCommentK: props.tableNameOfComment
        });
        this.state = {dataContext: Core.createTumblr(props.dataContext)};
    }

    componentDidMount() {
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