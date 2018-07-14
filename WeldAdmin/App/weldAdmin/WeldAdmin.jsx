import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';
import Alert from '../common/Alert.jsx';
import Input from '../common/Input.jsx';
import { Panel, Grid, Row, Col, Glyphicon, Button, ButtonGroup } from 'react-bootstrap';
import { fail } from 'assert';

class NameForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            inputName: '',
            oldName: ''
        };

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.getAjax = this.getAjax.bind(this);
        this.putAjax = this.putAjax.bind(this);
        this.doAlert = this.doAlert.bind(this);
    }

    componentDidMount() {
        this.getAjax();
    }

    handleChange(event) {
        if (event.target.value !== 'Donald Duck') {
            if (this.state.oldName !== event.target.value) {
                this.setState({ inputName: event.target.value });
                return true;
            }
            this.doAlert('Namn ej ändrat');
            return true;
        }
        return false;
    }

    handleSubmit(event) {
        if (this.state.oldName !== this.state.inputName) {
            this.putAjax();  
            return;
        }
        this.doAlert('Namn ej ändrat');
        event.preventDefault();
    }

    getAjax() {
        let that = this;
        $.ajax({
            url: 'https://simple-rest-weld.herokuapp.com/user/' + this.state.inputName,
            type: 'GET',
            contentType: 'application/json',
            success: function (result) {
                that.setState({ inputName: result.name });
            },
            error: function (result) {
                that.doAlert('Kan ej kontakta server ' + result.name);
                that.setState({ inputName: 'Error' });
            }.bind(this)
        });
    }

    putAjax() {
        if (this.state.inputName === 'Donald Duck') return;
        let that = this;
        $.ajax({
            url: 'https://simple-rest-weld.herokuapp.com/user/',
            type: 'PUT',
            data: JSON.stringify(this.state.inputName),
            contentType: 'application/json',
            statusCode: {
                204: function () {
                    alert('namn ändrat till: ' + that.state.inputName);
                    that.state.oldName = that.state.inputName;  
                }
            },
            error: function (result) {
                that.doAlert('Kan ej kontakta server ' + result.name);
                that.setState({ inputName: 'Error' });
            }.bind(this)
        });
    }

    doAlert(text) {
        ReactDOM.render(<Alert value={text} />, document.getElementById("divAdminIndex"));
    };


    render() {
        return (
            <form onSubmit={this.handleSubmit}>
                <label>
                    Name: <Input type={'text'} value={this.state.nameText} maxLength={50} id={'inputName'} value={this.state.inputName} checkError={this.handleChange} />
                </label>
                <input type="submit" value="Spara" />
            </form>
        );
    }
}


export default class WeldAdmin extends Component {
    constructor(props) {
        super(props);
        this.state = {
            nameText: props.nameText
        };
        this.checkInput = this.checkInput.bind(this);
        this.getAjax = this.getAjax.bind(this);
        this.putAjax = this.putAjax.bind(this);
        this.doAlert = this.doAlert.bind(this);
    };

    componentDidMount() {
        this.getAjax();
    }

    componentWillReceiveProps(newProps) {
        if (this.state.nameText !== newProps.nameText) {
            this.setState({ nameText: newProps.nameText });
            console.log('newProps.nameText ' + newProps.nameText);
        };
    };

    doAlert(text) {
        ReactDOM.render(<Alert value={text} />, document.getElementById("divAdminIndex"));
    };

    checkInput(event) {
        if (event.target.value !== 'Donald Duck') {
            this.setState({ nameText: event.target.value });
            return true;
        }
        return false;
    };

    getAjax() {
        let that = this;
        $.ajax({
            url: 'https://simple-rest-weld.herokuapp.com/user/' + this.state.nameText,
            type: 'GET',
            contentType: 'application/json',
            success: function (result) {
                that.setState({ nameText: result.name });
            },
            error: function (result) {
                that.doAlert('Kan ej kontakta server ' + result.name);
                that.setState({ nameText: 'Error' });
            }.bind(this)
        });
    }

    putAjax() {
        if (this.state.nameText === 'Donald Duck') return;
        let that = this;
        $.ajax({
            url: 'https://simple-rest-weld.herokuapp.com/user/',
            type: 'PUT',
            data: JSON.stringify(this.state.nameText),
            contentType: 'application/json',
            statusCode: {
                204: function () {
                    alert('namn ändrat till: ' + that.state.nameText);
                }
            },
            error: function (result) {
                that.doAlert('Kan ej kontakta server ' + result.name);
                that.setState({ nameText: 'Error' });
            }.bind(this)
        });
    }

    render() {
        return <div className="container">
            <Row>
                <Col className="row-height">
                    <Col md={3} className="hidden-md hidden-sm hidden-xs col-md-height col-md-top custom-vertical-left-border custom-vertical-right-border grey-background">
                        <Row>
                            <Col md={12}>
                                <h4></h4>
                            </Col>
                        </Row>
                        <Row>
                            <Col md={12}>
                                <h4>Admin</h4>
                            </Col>
                        </Row>
                    </Col>
                    <Col md={9} className="col-md-height">
                        <Row>
                            <Col md={2}></Col>
                            <Col md={10}>
                                <h1>Weld Your own App administration:</h1>
                                <NameForm />
                                <br />
                                <br />
                                <Row>
                                    <Input checkError={this.checkInput} className="col-xs-3 col-md-3 col-sm-3 BorderInset" type={'text'} value={this.state.nameText} maxLength={50} id={'nameText'} />
                                    <Col md={3} sm={3} xs={3} />
                                </Row>
                                <button onClick={() => { this.putAjax() }}>Spara</button>
                                <div id="divAdminIndex"></div>
                            </Col>
                        </Row>
                    </Col>
                </Col>
            </Row>
        </div>;
    }
};
WeldAdmin.propTypes = {
    nameText: PropTypes.string
}
WeldAdmin.defaultProps = {
    nameText: '---'
}
