import * as Core from './lv.tumblr.core.js'
import Tumblr from '../common/lv.control.tumblr.jsx'

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
                <TumblrContainer dataContext={tumblr}/>
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
        this.state = {dataContext: Core.createTumblrs(props.dataContext.Tumblrs)};
    }

    render() {
        return (
            <div className="g-mn">
                <TumblrContainerList dataContext={this.state.dataContext}/>
            </div>
        );
    }
}

export {TumblrContainerBox};