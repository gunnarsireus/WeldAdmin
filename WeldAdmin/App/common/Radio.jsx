import React, { Component } from 'react';
import Input from './Input.jsx';

import { Grid, Row, Col, Glyphicon, Button, ButtonGroup } from 'react-bootstrap';
export default class Radio extends Component {
    constructor(props) {
        super(props);
    };
    render() {
        return <label className="radio-inline">
                    <Input type='radio' className='warning' name={this.props.name} checked={this.props.checked} onClick={this.props.onClick}/>
					 {this.props.text}
                </label>;
    }
};