import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DialogAlert from './DialogAlert.jsx';

export default class Alert extends Component {
    constructor(props) {
        super(props);
    };

    render() {
        return (
            <DialogAlert >
                <p>{this.props.value}</p>
            </DialogAlert>
      );
    }
};
Alert.propTypes= 
{ 
    value:PropTypes.string.isRequired
}

