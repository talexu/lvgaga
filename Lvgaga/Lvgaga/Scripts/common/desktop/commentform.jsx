import * as comment from '../../business/comment.js';

export default class CommentForm extends React.Component {
    constructor() {
        super();
        this.state = {text: ""};

        this.handleChange = this.handleChange.bind(this);
        this.postComment = this.postComment.bind(this);
    }

    handleChange(e) {
        this.setState({text: e.target.value});
    }

    postComment(e) {
        let {dataContext, postSuccess} = this.props;

        return comment.postComment({
            tumblrK: dataContext,
            commentTextK: this.state.text,
            buttonK: e.target
        }).done(() => {
            this.setState({text: ""});
        }).done((data)=> {
            postSuccess(data);
        });
    }

    render() {
        return (
            <div>
                <form role="form">
                    <div className="form-group mar-bottom">
                        <label>评论：</label>
                            <textarea className="form-control max-width-none" rows="3" value={this.state.text}
                                      onChange={this.handleChange}></textarea>

                        <div className="pull-right">
                            <button type="button" className="btn btn-default btn-sm ladda-button"
                                    data-style="zoom-out"
                                    data-spinner-color="#333" onClick={this.postComment}>
                                <span className="ladda-label">发表评论</span>
                            </button>
                        </div>
                    </div>
                </form>
            </div>
        );
    }
}