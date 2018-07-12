import React, { Component } from 'react';
import Swirl from './Swirl.jsx';

export default class Portal extends Component {
    constructor(props) {
        super(props);
        this.state = {
            showForm:true
        };
    };

    componentDidMount() {
        this.setState({showForm: true});
    };

    componentWillReceiveProps(newProps) {
        this.setState({showForm: true});
    };

    render() {
        return  <Swirl open={this.state.showForm}>
          <div className="panel panel-primary  center-block">
                <div className="panel-heading text-center">
                    <h3 className="panel-title">Dagens bild</h3>
                </div>
                <div className="panel-body">
                    <div className="row text-center">
                        <div className="span4"></div>
                            <div className="span4">
                                <a href="/Details/Index/0/0"><img src="/Handler/Index/PhotoID=0/Size=M" alt="" /></a>
                            </div>
                        <div className="span4"></div>
                    </div>
                </div>
          </div>
        </Swirl>
    }
};
