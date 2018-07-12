import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';
import Alert from '../common/Alert.jsx';
import Input from '../common/Input.jsx';
import { Panel, Grid, Row, Col, Glyphicon, Button, ButtonGroup } from 'react-bootstrap';

export default class WeldAdmin extends Component {
    constructor(props) {
        super(props);
        this.state = {
            nameText: props.nameText
        };
        this.checkInput = this.checkInput.bind(this);
        this.getAjax = this.getAjax.bind(this);
        this.doAlert = this.doAlert.bind(this);
    };

    componentDidMount() {
        console.log('componentDidMount')
        this.getAjax();
    }

    componentWillReceiveProps(newProps) {
        console.log('newProps.nameText ' + newProps.nameText);
        this.setState({
            nameText: newProps.nameText
        });
    };

    doAlert(text) {
        ReactDOM.render(<Alert value={text} />, document.getElementById("divAdminIndex"));
    };

    checkInput(event, cancelAlert) {
        return true;
    };

    getAjax() {
        var that = this;
        $.ajax({
            url: 'https://simple-rest-weld.herokuapp.com/user/' + this.state.nameText,
            type: 'GET',
            contentType: 'application/json',
            success: function (result) {
                that.setState({
                    nameText: result.name
                });
            },
            error: function (result) {
                that.doAlert('Kan ej kontakta server ' + result.name);
                that.setState({
                    nameText: 'Error'
                });
            }.bind(this)
        });
    }

    render() {
        return (
            <div>
                <h1>Weld Your own App administration:</h1>

                <Row>
                    <Input checkError={this.checkInput} className="col-xs-3 col-md-3 col-sm-3 BorderInset" type={'text'} value={this.state.nameText} maxLength={50} id={'nameText'} />
                    <Col md={3} sm={3} xs={3} />
                </Row>
                {/* <button onClick={this.getAjax()}>Spara</button>*/}
                <div id="divAdminIndex"></div>
            </div>
        );
    }
};
WeldAdmin.propTypes = {
    nameText: PropTypes.string
}
WeldAdmin.defaultProps = {
    nameText: '---'
}
