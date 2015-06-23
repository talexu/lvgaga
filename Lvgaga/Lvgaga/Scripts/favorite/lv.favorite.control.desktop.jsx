import * as Core from './lv.favorite.core.js';
import Loading from '../common/lv.control.desktop.loading.jsx';

class Thumbnail extends React.Component {
    render() {
        let {dataContext} = this.props;

        return (
            <a href={dataContext.MediaLargeUri} title={dataContext.Text} data-gallery data-link={dataContext.Uri}>
                <img src={dataContext.MediaMediumUri} alt={dataContext.Text} width={Core.cellWidth}/>
            </a>
        );
    }
}

class LightBox extends React.Component {
    constructor(props) {
        super(props);
        this.componentDidMount = this.componentDidMount.bind(this);

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
        Core.setSize(links.scrollWidth, 300);

        links.onclick = function (event) {
            event = event || window.event;
            let target = event.target || event.srcElement,
                link = target.src ? target.parentNode : target,
                options = {index: link, event: event},
                links = this.getElementsByTagName('a');

            options.onslide = function (index, slide) {
                // Callback function executed on slide change.
                let text = this.list[index].getAttribute('data-link'),
                    node = this.container.find('.link');
                node[0].href = text;
            };
            blueimp.Gallery(links, options);
        };

        Core.loadFavorites();
    }

    render() {
        let Thumbnails = this.state.tumblrs.map((tumblr) => {
            return (
                <Thumbnail key={tumblr.Base64Id} dataContext={tumblr}/>
            );
        });

        return (
            <div>
                <div id="links" ref="links">
                    {Thumbnails}
                </div>
                <div className="mar-top">
                    <Loading onClickHandler={Core.loadFavorites} style={Core.getLoadingButtonStyle()}/>
                </div>
                <div id="blueimp-gallery" className="blueimp-gallery blueimp-gallery-controls">
                    <div className="slides"></div>
                    <a className="title link">
                        <h3 className="title"></h3>
                    </a>
                    <a className="prev">‹</a>
                    <a className="next">›</a>
                    <a className="close">×</a>
                    <a className="play-pause"></a>
                    <ol className="indicator"></ol>
                </div>
            </div>
        );
    }
}

export default LightBox;