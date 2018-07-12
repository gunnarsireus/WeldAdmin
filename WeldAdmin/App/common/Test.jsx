import React, { Component } from 'react';
import Parent from './Parent.jsx';

export default class Test extends Component {
    constructor(props) {
        super(props);
    };
    render() {
        return <div><h1>Welcome to Reaction Tracker (from a component)</h1>
				<Parent/>
				</div>;
    }
};