(function () {
    var that;
    var sasFav;
    var continuationToken;
    var mediaType;
    var takingCount;
    var tableNameOfFavorite;
    var expectedCellWidth;
    var loadingRow;
    var cellWidth;

    var loadFavorites = function (e) {
        var button;
        if (e) button = e.target;

        return lv.favorite.loadFavoritesWithContinuationToken({
            button: button,
            sasFav: sasFav,
            tableNameOfFavorite: tableNameOfFavorite,
            continuationToken: continuationToken,
            mediaType: mediaType,
            takingCount: takingCount,
            onReceiveNewToken: function (token) {
                sasFav = token;
            },
            onReceiveNewContinuationToken: function (partitionKey, rowKey) {
                continuationToken = continuationToken || {};
                continuationToken.NextPartitionKey = partitionKey;
                continuationToken.NextRowKey = rowKey;
            },
            done: function (tumblrs) {
                that.state.tumblrs = that.state.tumblrs.concat(tumblrs);

                lv.refreshState(that);
            }
        });
    };

    var Thumbnail = React.createClass({
        render: function () {
            var {dataContext} = this.props;

            return (
                <a href={dataContext.MediaLargeUri} title={dataContext.Text} data-gallery>
                    <img src={dataContext.MediaMediumUri} alt={dataContext.Text} width={cellWidth}/>
                </a>
            );
        }
    });

    var LightBox = React.createClass({
        getInitialState: function () {
            return {
                tumblrs: [],
                selectedTumblr: undefined
            };
        },
        componentDidMount: function () {
            var self = this;
            var links = React.findDOMNode(this.refs.links);
            var containerWidth = links.scrollWidth;
            var rowCapacity = Math.ceil(containerWidth / expectedCellWidth);
            cellWidth = containerWidth / rowCapacity;
            takingCount = rowCapacity * loadingRow || takingCount;

            links.onclick = function (event) {
                event = event || window.event;
                var target = event.target || event.srcElement,
                    link = target.src ? target.parentNode : target,
                    options = {index: link, event: event},
                    links = this.getElementsByTagName('a');

                options.onslide = function (index, slide) {
                    // Callback function executed on slide change.
                    self.state.selectedTumblr = self.state.tumblrs[index];
                    lv.refreshState(self);
                };
                blueimp.Gallery(links, options);
            };

            loadFavorites();
        },
        setFavorite: function (e) {
            var self = this;
            var tumblr = this.state.selectedTumblr;

            return lv.favorite.setFavorite({
                button: e.target,
                tumblr: tumblr,
                done: function () {
                    lv.refreshState(self);
                }
            });
        },
        render: function () {
            that = this;

            var Thumbnails = this.state.tumblrs.map(function (tumblr) {
                return (
                    <Thumbnail dataContext={tumblr}/>
                );
            });

            var btnStyle = {
                display: "inline"
            };
            if (continuationToken && (!continuationToken.NextPartitionKey || !continuationToken.NextRowKey)) {
                btnStyle.display = "none";
            }

            var selectedTumblr = this.state.selectedTumblr;
            var Text = selectedTumblr && selectedTumblr.Text;
            var Uri = selectedTumblr && selectedTumblr.Uri;
            var SharingUri = selectedTumblr && selectedTumblr.sharingUri;
            var IsFavorited = selectedTumblr && selectedTumblr.IsFavorited;

            var classNameOfFavorite = "btn btn-default btn-outline navbar-btn ladda-button";
            classNameOfFavorite += IsFavorited ? " btn-selected" : " btn-white";

            return (
                <div>
                    <div id="links" ref="links">
                        {Thumbnails}
                    </div>
                    <div className="container-block">
                        <button type="button" className="btn btn-default btn-lg btn-block btn-rectangle ladda-button"
                                data-style="zoom-out" data-spinner-color="#333" style={btnStyle}
                                onClick={loadFavorites}><span className="ladda-label">加载更多</span></button>
                    </div>
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
    });

    var initialize = function (parameters) {
        sasFav = parameters.Sas;
        mediaType = parameters.MediaType;
        takingCount = lv.defaultTakingCount;
        tableNameOfFavorite = parameters.tableNameOfFavorite;
        expectedCellWidth = 100;
        loadingRow = 5;

        React.render(
            <LightBox/>,
            document.getElementById('div_content')
        );
    };

    lv.favorite.initialize = initialize;
})();