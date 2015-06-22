import * as Core from './lv.tumblr.core.js';
import Tumblr from '../common/lv.control.desktop.tumblr.jsx';
import Loading from '../common/lv.control.desktop.loading.jsx';

class Functions extends React.Component {
    constructor() {
        super();
        this.setFavorite = this.setFavorite.bind(this);
    }

    setFavorite(e) {
        var {dataContext} = this.props;

        return Core.setFavorite({
            button: e.target,
            tumblr: dataContext,
            done: function () {
                Core.refreshState(Core.reactRoot);
            }
        });
    }

    render() {
        var {dataContext} = this.props;

        var classNameOfFavorite = "btn btn-default btn-sm ladda-button";
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
    render() {
        var {dataContext} = this.props;

        return (
            <div className="box">
                <div className="m-post photo">
                    <Tumblr dataContext={dataContext}/>
                    <Functions dataContext={dataContext}/>
                </div>
            </div>
        );
    }
}

class TumblrContainerList extends React.Component {
    render() {
        var {dataContext} = this.props;

        var TumblrContainerNodes = dataContext.map(function (tumblr) {
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

        Core.initialize(
            this,
            props.dataContext.Sas,
            props.dataContext.ContinuationToken,
            props.dataContext.MediaType,
            props.dataContext.TumblrCategory,
            props.tableNameOfTumblr,
            props.tableNameOfFavorite,
            props.tableNameOfComment
        );
        this.state = {dataContext: Core.createTumblrs(props.dataContext.Tumblrs)};
    }

    render() {
        return (
            <div className="g-mn">
                <TumblrContainerList dataContext={this.state.dataContext}/>
                <Loading onClickHandler={Core.loadTumblrs}/>
            </div>
        );
    }
}

export default TumblrContainerBox;