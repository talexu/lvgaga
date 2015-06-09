(function () {
    var TumblrContainerBox = React.createClass({
        getInitialState: function () {
            return {dataContext: lv.factory.createTumblrs(this.props.initialState)};
        },
        componentDidMount: function () {
            lv.tumblr.loadFavorites(this.state.dataContext);
        },
        render: function () {
            var {initialState, ...other} = this.props;

            lv.tumblr.initialize({
                that: this
            });

            return (
                <div className="g-mn">
                    <TumblrContainerList {...other} dataContext={this.state.dataContext}/>
                    <LoadingMore {...other}/>
                </div>
            );
        }
    });

    var eventHandlers = {
        loadTumblrs: lv.tumblr.loadTumblrs,
        setFavorite: lv.tumblr.setFavorite
    };

    React.render(
        <TumblrContainerBox initialState={lv.dataContext.Tumblrs} eventHandlers={eventHandlers}/>,
        document.getElementById('div_tumblrs')
    );
})();