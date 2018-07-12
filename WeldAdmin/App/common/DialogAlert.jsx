import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import '../../Scripts/jquery-ui.js';
import '../../Content/custom.css';
import '../../Content/jquery-ui.css';

export default class DialogAlert extends Component {
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
            modal: true,
            title:'Felmeddelande:',
            buttons: {
                Ok: function() {
                    $(this).dialog( "close" );
                }
            }
        }).data('ui-dialog');

        // 3) call method to reconnect React's rendering
        this.renderDialogAlertContent();
    };

    componentWillReceiveProps(newProps) {
        // 4) render reconnected tree when props change
        this.renderDialogAlertContent(newProps);
    };

    renderDialogAlertContent(props) {
        // decide to use newProps from `componentWillReceiveProps` or to use
        // existing props from `componentDidMount`
        props = props || this.props;

        // 5) make a new rendering tree, we've now hidden the DOM
        //    manipulation from jQuery UI dialog and then continued
        //    rendering with React
        ReactDOM.render(<div style={{backgroundColor:"ivory", color:"red", fontWeight:"bold"}}>{props.children}</div>, this.node);

        // 6) Call methods on the DOM lib via prop changes

        this.dialog.open();

    };

    componentWillUnmount() {
        // clean up the mess
        this.dialog.destroy();
    };
};