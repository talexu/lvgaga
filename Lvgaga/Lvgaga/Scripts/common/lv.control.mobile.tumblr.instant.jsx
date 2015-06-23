export default class Tumblr extends React.Component {
    render() {
        let {dataContext} = this.props;

        return (
            <div>
                <img className="img-tumblr lazy" ref="lazyImage" src={dataContext.MediaLargeUri} alt=""/>

                <div className="content">
                    <p className="text-tumblr">{dataContext.Text}</p>

                    <p className="date-tumblr">{dataContext.CreateTime}</p>
                </div>
            </div>
        );
    }
}