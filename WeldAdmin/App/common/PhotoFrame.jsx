import React, { Component } from 'react';
import PropTypes from 'prop-types';
import '../../Content/Frame.css';

export default class PhotoFrame extends Component {
    constructor(props) {
        super(props);
        this.state = {
            children: this.props.children    
        }
    };
	
    componentWillReceiveProps(newProps){
        this.setState({
            children: newProps.children        
        })
    }

    componentDidMount(){
        this.setState({
            children: this.props.children       
        })
    }
    render(){
            return  <table className="photo-frame" >
             <tr>
                 <td className="topx--"></td>
                    <td className="top-x-"></td>
                    <td className="top--x"></td>
                </tr>
                <tr>
                    <td className="midx--"></td>
                    <td>
                        {this.state.children}
                    </td>
                    <td className="mid--x"></td>
            </tr>
                <tr>
                    <td className="botx--"></td>
                    <td className="bot-x-"></td>
                    <td className="bot--x"></td>
                </tr>
            </table>
        };
}

    
PhotoFrame.propTypes= 
{
    children: PropTypes.element.isRequired
}

