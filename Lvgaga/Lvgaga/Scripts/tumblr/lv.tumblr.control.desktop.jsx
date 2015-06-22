import * as Core from './lv.tumblr.core.js';
import Tumblr from '../common/lv.control.desktop.tumblr.jsx';
import Loading from '../common/lv.control.desktop.loading.jsx';

class TumblrContainer extends React.Component {
    render() {
        var {dataContext} = this.props;

        return (
            <div className="box">
                <div className="m-post photo">
                    <Tumblr dataContext={dataContext}/>
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