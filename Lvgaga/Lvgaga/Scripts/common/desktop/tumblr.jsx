export default class Tumblr extends React.Component {
    render() {
        let {dataContext} = this.props;

        return (
            <div className="cont">
                <div className="pic">
                    <div className="img">
                        <img src={dataContext.MediaLargeUri}></img>
                    </div>
                </div>

                <div>
                    <div className="text text-1">
                        <p>{dataContext.Text}</p>
                    </div>
                    <div className="info2">
                        <p className="date">{dataContext.CreateTime}</p>
                    </div>
                </div>
            </div>
        );
    }
}