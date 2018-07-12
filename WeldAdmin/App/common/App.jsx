import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import Dialog from './Dialog.jsx';

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = {
            taco: null,
            showForm:false
        };
        this.handleTacoSubmission = this.handleTacoSubmission.bind(this);
        this.showForm = this.showForm.bind(this);
        this.handleDialogClose = this.handleDialogClose.bind(this);
    };

    handleTacoSubmission(event) {
        event.preventDefault();
        //var taco = this.refs.favoriteTaco.getDOMNode().value;
        var taco = ReactDOM.findDOMNode(this.refs.favoriteTaco).value;
        this.setState({
            taco: taco,
            showForm: false
        });
    };

    renderTaco() {
        return this.state.taco ?
          "Vilken är din favorittacos: " + this.state.taco:
          "Har du ingen favorittacos ännu?";
    };

    showForm() {
        this.setState({showForm: true});
    };

    handleDialogClose() {
        if (!this.state.taco)
            alert("Har du ingen favorittacos?");
    };

    render() {
        return (
          <div>
            <h1>jQuery UI dialog i ReactJS</h1>
            <button onClick={this.showForm}>Berätta vilken din favorittacos är</button>
            <p>{this.renderTaco()}</p>
            <Dialog title="Favorit Taco" open={this.state.showForm} onClose={this.handleDialogClose}>
                <form onSubmit={this.handleTacoSubmission}>
                    <p>Tacos är läckra. Vilken är din favorit?</p>
                    <p>
                        <input type="text" ref="favoriteTaco"/> 
                        <button type="submit">Submit</button>
                    </p>
                </form>
            </Dialog>
        </div>
      );
    }
};
