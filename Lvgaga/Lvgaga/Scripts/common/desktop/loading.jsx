﻿export default class Loading extends React.Component {
    render() {
        let {onClickHandler, style} = this.props;

        return (
            <button type="button" className="btn btn-default btn-lg btn-block btn-rectangle ladda-button"
                    data-style="zoom-out" data-spinner-color="#333" style={style}
                    onClick={onClickHandler}><span className="ladda-label">加载更多</span></button>
        );
    }
}