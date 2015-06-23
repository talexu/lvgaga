import * as Core from './lv.tumblr.core.js';
import Tumblr from '../common/lv.control.mobile.tumblr.jsx';
import Loading from '../common/lv.control.mobile.loading.jsx';

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
            <div className="content">
                <button className={classNameOfFavorite} type="button" data-style="zoom-out"
                        data-spinner-color="#333" onClick={this.setFavorite}><span
                    className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span></button>
                <a className="btn btn-default btn-share" href={dataContext.sharingUri} target="_blank"><span
                    className="glyphicon glyphicon-share-alt" aria-hidden="true"></span></a>
                <a className="btn btn-default pull-right btn-comment" href={dataContext.Uri}><span
                    className="glyphicon glyphicon-th-list" aria-hidden="true"></span></a>
            </div>
        );
    }
}

class TumblrContainer extends React.Component {
    render() {
        let {dataContext} = this.props;

        return (
            <div className="container-tumblr">
                <Tumblr dataContext={dataContext}/>
                <Functions dataContext={dataContext}/>
                <hr className="hr-tumblr"/>
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

class TumblrBox extends React.Component {
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
            <div>
                <TumblrContainerList dataContext={this.state.dataContext}/>
                <Loading onClickHandler={Core.loadTumblrs} style={Core.getLoadingButtonStyle()}/>
            </div>
        );
    }
}

export default TumblrBox;