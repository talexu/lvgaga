export default class Tumblr extends React.Component {
    constructor() {
        super();
        this.componentDidMount = this.componentDidMount.bind(this);
    }

    componentDidMount() {
        let image = $(React.findDOMNode(this.refs.lazyImage));
        image.lazyload();
    }

    render() {
        let {dataContext} = this.props;

        return (
            <div>
                <img className="img-tumblr lazy" ref="lazyImage" data-original={dataContext.MediaLargeUri} alt=""/>

                <div className="content">
                    <p className="text-tumblr">{dataContext.Text}</p>

                    <p className="date-tumblr">{dataContext.CreateTime}</p>
                </div>
            </div>
        );
    }
}