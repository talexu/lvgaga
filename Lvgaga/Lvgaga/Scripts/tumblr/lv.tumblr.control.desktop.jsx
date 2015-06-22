import * as Core from './lv.tumblr.core.js';
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

                <button type="button display-block mar-right" className="btn btn-default btn-sm pull-right"
                        data-toggle="collapse"
                        data-target={"#"+dataContext.Base64Id} aria-expanded="false"
                        aria-controls={dataContext.Base64Id}>
                    <span className="glyphicon glyphicon-th-list" aria-hidden="true"></span>
                </button>
            </div>
        );
    }
}


class TumblrContainer extends React.Component {
    constructor() {
        super();
        this.postSuccess = this.postSuccess.bind(this);
        this.componentDidMount = this.componentDidMount.bind(this);
    }

    componentDidMount() {
        var {dataContext} = this.props;

        $('#' + dataContext.Base64Id).on('show.bs.collapse', () => {
            if (dataContext.comments.length <= 0) {
                Core.loadComments(dataContext);
            }
        });
    }

    postSuccess(comment) {
        var {dataContext} = this.props;
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

                    <div className="collapse" id={dataContext.Base64Id}>
                        <CommentForm dataContext={dataContext} postSuccess={this.postSuccess}/>
                        <CommentList dataContext={dataContext.comments}/>

                        <div className="info2">
                            <a href={dataContext.Uri} target="_blank">全文链接</a>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

class TumblrContainerList extends React.Component {
    render() {
        let {dataContext} = this.props;

        let TumblrContainerNodes = dataContext.map((tumblr) => {
            return (
                <TumblrContainer key={tumblr.Base64Id} dataContext={tumblr}/>
            );
        });
        return (
            <div>
                {TumblrContainerNodes}
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
            tumSasK: props.dataContext.Sas,
            continuationTokenK: props.dataContext.ContinuationToken,
            mediaTypeK: props.dataContext.MediaType,
            tumblrCategoryK: props.dataContext.TumblrCategory,
            tableNameOfTumblrK: props.tableNameOfTumblr,
            tableNameOfFavoriteK: props.tableNameOfFavorite,
            tableNameOfCommentK: props.tableNameOfComment
        });
        this.state = {dataContext: Core.createTumblrs(props.dataContext.Tumblrs)};
    }

    componentDidMount() {
        Core.loadFavorites(this.state.dataContext);
    }

    render() {


        return (
            <div className="g-mn">
                <TumblrContainerList dataContext={this.state.dataContext}/>
                <Loading onClickHandler={Core.loadTumblrs} style={Core.getLoadingButtonStyle()}/>
            </div>
        );
    }
}

export default TumblrContainerBox;