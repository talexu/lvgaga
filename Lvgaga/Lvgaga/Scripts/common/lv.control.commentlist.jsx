export default class CommentList extends React.Component {
    render() {
        var {dataContext} = this.props;
        
        var commentNodes = dataContext.map(function (comment) {
            return (
                <Comment dataContext={comment}/>
            );
        });
        return (
            <div>
                {commentNodes}
            </div>
        );
    }
}