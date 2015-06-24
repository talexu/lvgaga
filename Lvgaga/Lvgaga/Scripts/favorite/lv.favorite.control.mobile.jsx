import * as Core from './lv.favorite.core.js';
import Loading from '../common/lv.control.mobile.loading.jsx';

class Thumbnail extends React.Component {
    render() {
        let {dataContext} = this.props;

        return (
            <a href={dataContext.MediaLargeUri} title={dataContext.Text} data-gallery>
                <img src={dataContext.MediaMediumUri} alt={dataContext.Text} width={Core.cellWidth}/>
            </a>
        );
    }
}

class LightBox extends React.Component {
    constructor(props) {
        super(props);
        this.componentDidMount = this.componentDidMount.bind(this);
        this.setFavorite = this.setFavorite.bind(this);

        this.state = {
            tumblrs: [],
            selectedTumblr: undefined
        };

        Core.initialize({
            reactRootK: this,
            sasFavK: props.dataContext.Sas,
            mediaTypeK: props.dataContext.MediaType,
            tableNameOfFavoriteK: props.tableNameOfFavorite
        });
    }

    componentDidMount() {
        let links = React.findDOMNode(this.refs.links);
        Core.setSize(links.scrollWidth, 100);

        let self = this;
        links.onclick = function (event) {
            event = event || window.event;
            let target = event.target || event.srcElement,
                link = target.src ? target.parentNode : target,
                options = {index: link, event: event},
                links = this.getElementsByTagName('a');

            options.onslide = function (index, slide) {
                // Callback function executed on slide change.
                self.state.selectedTumblr = self.state.tumblrs[index];
                Core.refreshState(self);
            };
            blueimp.Gallery(links, options);
        };

        Core.loadFavorites();
    }

    setFavorite(e) {
        let {selectedTumblr} = this.state;

        return Core.setFavorite({
            buttonK: e.target,
            tumblrK: selectedTumblr,
            doneK: ()=> {
                Core.refreshState(Core.reactRoot);
            }
        });
    }

    render() {
        let Thumbnails = this.state.tumblrs.map((tumblr) => {
            return (
                <Thumbnail key={tumblr.Base64Id} dataContext={tumblr}/>
            );
        });

        let selectedTumblr = this.state.selectedTumblr;
        let Text = selectedTumblr && selectedTumblr.Text;
        let Uri = selectedTumblr && selectedTumblr.Uri;
        let SharingUri = selectedTumblr && selectedTumblr.sharingUri;
        let IsFavorited = selectedTumblr && selectedTumblr.IsFavorited;

        let classNameOfFavorite = "btn btn-default btn-outline navbar-btn ladda-button";
        classNameOfFavorite += IsFavorited ? " btn-selected" : " btn-white";

        return (
            <div>
                <div id="links" ref="links">
                    {Thumbnails}
                </div>
                <Loading onClickHandler={Core.loadFavorites} style={Core.getLoadingButtonStyle()}/>
                <div id="blueimp-gallery" className="blueimp-gallery blueimp-gallery-controls">
                    <div className="slides"></div>
                    <a className="prev">‹</a>
                    <a className="next">›</a>
                    <a className="close">×</a>
                    <nav className="navbar navbar-inverse navbar-fixed-bottom fav-nav">
                        <div className="container">
                            <p className="text-fav">{Text}</p>
                            <button type="button"
                                    className={classNameOfFavorite}
                                    data-style="zoom-out" data-spinner-color="#333" onClick={this.setFavorite}>
                                <span className="ladda-label glyphicon glyphicon-heart" aria-hidden="true"></span>
                            </button>
                            <a className="btn btn-default btn-white btn-outline navbar-btn btn-share"
                               href={SharingUri}
                               target="_blank"><span className="glyphicon glyphicon-share-alt"
                                                     aria-hidden="true"></span></a>
                            <a className="btn btn-default btn-white btn-outline navbar-btn pull-right" href={Uri}><span
                                className="glyphicon glyphicon-th-list" aria-hidden="true"></span></a>
                        </div>
                    </nav>
                </div>
            </div>
        );
    }
}

export default LightBox;