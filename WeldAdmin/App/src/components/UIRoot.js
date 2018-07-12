import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Header from './Header';
import ReactionContainer from './ReactionContainer';
import Footer from './Footer';

export default class UIRoot extends Component {
    constructor(props) {
        super(props);
    };

    componentDidMount() {
    };

    componentWillReceiveProps(newProps) {
    };

    render() {
        return <div>
            {/*<Header /> */}

            <h1>Welcome to Reaction Tracker!</h1>
            <ReactionContainer />
            {/*<Footer /> */}
            <div id="divReactIndex"></div>
        </div>;
            }

};