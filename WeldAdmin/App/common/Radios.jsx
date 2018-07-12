import React, { Component } from 'react';
import Radio from './Radio.jsx';
import { Row} from 'react-bootstrap';


export default class Radios extends Component {
    constructor(props) {
		super(props);
		this.state = {
            radioChecked: this.props.radioChecked,
			radioData : []		
        };

	    this.props.getRadios.map(item =>  this.state.radioData.push(item));

	};
	
	componentWillReceiveProps(newProps){
		this.setState(
			{radioChecked : newProps.radioChecked}
		)
	}
	render() {
		return <div>
			<Row>
                 {this.state.radioData.map(item => 
                     <Radio {...item} key={item.radioNumber} checked={this.state.radioChecked===item.radioNumber} onClick={item.onClick.bind(this)}  text = {item.radioText}  name={this.props.name}/> )}              
           </Row>
		 </div>
	}
}


