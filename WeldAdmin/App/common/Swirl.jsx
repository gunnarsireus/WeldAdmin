import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import '../../Scripts/jquery-ui.js';
import '../../Scripts/ui/jquery.ui.core.js';
import '../../Scripts/ui/jquery.ui.effect-swirl.js';
import '../../Content/custom.css';
import '../../Content/jquery-ui.css';

import { Grid, Row, Col, Glyphicon, Button } from 'react-bootstrap';

export default class Swirl extends Component {
    constructor(props) {
        super(props);
        this.state = {
            hasMounted:false
        };
    };

    render() {
        // don't render anything, this is where we open the portal
        return <div/>;
    };

    runAnimate() {
        $("#divWeather").css("visibility", "visible").hide().animate({ height: "show" }, 2000, 'easeOutBounce');
    };
    ShowText(selector, callback) {
        $(selector).css("visibility", "visible").hide().fadeIn(1000, callback);
    };
    DoSwirl(){
            this.node = ReactDOM.findDOMNode(this);
            this.swirl = $("#divSwirl");
            this.swirl.css("visibility", "visible");
            this.swirl.css("display", "none");
            this.txt = $("#divText");
            this.txt.css("visibility", "hidden");
            this.txt.css("display", "block");
            this.download = $("#divDownload");
            this.download.css("visibility", "hidden");
            this.download.css("display", "block");
            this.contact = $("#divContact");
            this.contact.css("visibility", "hidden");
            this.contact.css("display", "block");
            this.weather = $("#divWeather");
            this.weather.css("visibility", "hidden");
            this.weather.css("display", "block");
            var that=this;
            this.swirl.toggle("swirl", { spins: 6 }, 1500, function() {
                that.ShowText("#divText", 
                    function () { that.ShowText("#divDownload", 
                        function () { that.ShowText("#divContact", 
                            function () { that.runAnimate() }) }) })
            });
    }

    componentDidMount() {
        this.DoSwirl();           
        this.renderDialogContent();
    };

    componentWillReceiveProps(newProps) {
        if (this.state.hasMounted) {
            this.DoSwirl();        
        }
        this.setState({hasMounted: true});
        this.renderDialogContent(newProps);
    };

    renderDialogContent(props) {
        // decide to use newProps from `componentWillReceiveProps` or to use
        // existing props from `componentDidMount`
        props = props || this.props;
        ReactDOM.render(<div>{props.children}</div>, this.node);
    };
};

