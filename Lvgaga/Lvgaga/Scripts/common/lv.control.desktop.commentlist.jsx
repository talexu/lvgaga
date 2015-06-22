import Comment from './lv.control.desktop.comment.jsx';

export default class CommentList extends React.Component {
    render() {
        let {dataContext} = this.props;

        let commentNodes = dataContext.map((comment) => {
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