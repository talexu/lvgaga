(function () {
    var TumblrContainerBox = React.createClass({
        getInitialState: function () {
            return {dataContext: this.props.initialState};
        },
        componentDidMount: function () {
            lv.tumblr.loadFavorites(this.state.dataContext);
        },
        render: function () {
            lv.tumblr.initialize({
                that: this
            });
            return (
                <div className="g-mn">
                    <TumblrContainerList dataContext={this.state.dataContext}/>
                    <LoadingMore eventHandler={lv.tumblr.loadTumblrs}/>
                </div>
            );
        }
    });

    React.render(
        <TumblrContainerBox initialState={lv.dataContext.Tumblrs}/>,
        document.getElementById('div_tumblrs')
    );
})();