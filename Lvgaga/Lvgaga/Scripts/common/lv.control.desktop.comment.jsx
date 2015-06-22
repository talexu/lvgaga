export default class Comment extends React.Component {
    render() {
        var {dataContext} = this.props;
        
        return (
            <div>
                <div className="info mar-zero">
                    <a href="#">{dataContext.UserName}</a>

                    <p className="date-comment pull-right">{dataContext.CommentTime}</p>
                </div>
                <p>{dataContext.Text}</p>
            </div>
        );
    }
}