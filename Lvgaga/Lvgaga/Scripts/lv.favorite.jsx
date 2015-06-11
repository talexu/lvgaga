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
        if (e) {
            button = e.target;
        }

        lv.retryExecute(function () {
            return lv.ajaxLadda(function () {
                return lv.queryAzureTable(sasFav, {
                    continuationToken: continuationToken,
                    filter: sprintf("RowKey ge '%s' and RowKey lt '%s'", mediaType, mediaType + 1),
                    top: takingCount
                }).done(function (data, textStatus, jqXhr) {
                    continuationToken = continuationToken || {};
                    continuationToken.NextPartitionKey = jqXhr.getResponseHeader("x-ms-continuation-NextPartitionKey");
                    continuationToken.NextRowKey = jqXhr.getResponseHeader("x-ms-continuation-NextRowKey");
                }).done(function (data) {
                    that.state.tumblrs = that.state.tumblrs.concat(lv.factory.createTumblrs(data.value));

                    lv.refreshState(that);
                });
            }, button);
        }, function () {
            return lv.ajaxLadda(function () {
                return lv.token.getToken([tableNameOfFavorite]).done(function (data) {
                    sasFav = data;
                });
            }, button);
        });
    };

    var Thumbnail = React.createClass({
        render: function () {
            var {dataContext} = this.props;

            return (
                <a href={dataContext.MediaLargeUri} title={dataContext.Text} data-gallery data-link={dataContext.Uri}>
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
                    var text = this.list[index].getAttribute('data-link'),
                        node = this.container.find('.link');
                    node[0].href = text;
                };
                blueimp.Gallery(links, options);
            };

            loadFavorites();
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

            return (
                <div>
                    <div id="links" ref="links">
                        {Thumbnails}
                    </div>
                    <div id="blueimp-gallery" className="blueimp-gallery">
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
                    <div className="mar-top">
                        <button type="button" className="btn btn-default btn-lg btn-block btn-rectangle ladda-button"
                                data-style="zoom-out" data-spinner-color="#333" style={btnStyle}
                                onClick={loadFavorites}><span class="ladda-label">加载更多</span></button>
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
        expectedCellWidth = 300;
        loadingRow = 5;

        React.render(
            <LightBox/>,
            document.getElementById('div_content')
        );
    };

    lv.favorite.initialize = initialize;
})();