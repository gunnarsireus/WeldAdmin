import React, { Component } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap';
import { Router, Route, IndexLink, Link,  hashHistory, IndexRoute  } from 'react-router';
import Frame from './Frame.js';
import Sql from './sql/Sql.jsx';
import WeldAdmin from './weldAdmin/WeldAdmin.jsx';
const NotFound = () => (<h1>404.. This page is not found!</h1>);

export default class MainApp extends Component {
    constructor(props) {
        super(props);
    };
    render() { return  <Router history = {hashHistory}>
            <Route path="/" component={Frame}>
                <IndexRoute component={WeldAdmin} />
                <Route path="sql" component={Sql}/>
                <Route path='*' component={NotFound} />
            </Route>
        </Router>
    }
}