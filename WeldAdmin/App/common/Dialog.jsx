import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import '../../Scripts/jquery-ui.js';
import '../../Content/custom.css';
import '../../Content/jquery-ui.css';

export default class Dialog extends Component {
    constructor(props) {
        super(props);
    };
    render() {
        // don't render anything, this is where we open the portal
        return <div/>;
    };
    componentDidMount() {
        // 2) do DOM lib stuff
        this.node = ReactDOM.findDOMNode(this);
        this.dialog = $(this.node).dialog({
            autoOpen: false,
            // pass in lib options from props
            title: this.props.title,
            close: this.props.onClose
        }).data('ui-dialog');

        // 3) call method to reconnect React's rendering
        this.renderDialogContent();
    };

componentWillReceiveProps(newProps) {
        // 4) render reconnected tree when props change
        this.renderDialogContent(newProps);
    };

    renderDialogContent(props) {
        // decide to use newProps from `componentWillReceiveProps` or to use
        // existing props from `componentDidMount`
        props = props || this.props;

        // 5) make a new rendering tree, we've now hidden the DOM
        //    manipulation from jQuery UI dialog and then continued
        //    rendering with React
        ReactDOM.render(<div>{props.children}</div>, this.node);

        // 6) Call methods on the DOM lib via prop changes
        if (props.open)
            this.dialog.open();
        else
            this.dialog.close();
    };

    componentWillUnmount() {
        // clean up the mess
        this.dialog.destroy();
    };
};
Dialog.defaultProps =
{
    title: '',
    onClose(){}
}
