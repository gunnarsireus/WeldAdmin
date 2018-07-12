import React, { Component } from 'react';
import PropTypes from 'prop-types';
export default class Input extends Component {
    displayName: 'Input';
    constructor(props) {
        super(props);
        this.state = {
            value: this.props.value,
            disabled: this.props.disabled,
            checked: this.props.checked,
            className:this.props.className,
            maxLength:this.props.maxLength,
            placeholder:this.props.placeholder,
            id:this.props.id,
            name:this.props.name,
            type:this.props.name,
            oldValue:this.props.value,
            backgroundColor:''
        };
        this.handleBlur = this.handleBlur.bind(this);
        this.handleChange = this.handleChange.bind(this);
    };
    componentWillReceiveProps(nextProps) {
        if (this.state.value !== nextProps.value) {
            this.setState({ value: nextProps.value});
        };
        if (this.state.disabled !== nextProps.disabled) {
            this.setState({ disabled: nextProps.disabled});
        };
        if (this.state.checked !== nextProps.checked) {
            this.setState({ checked: nextProps.checked});
        };
        if (this.state.className !== nextProps.className) {
            this.setState({ className: nextProps.className});
        };
        if (this.state.maxLength !== nextProps.maxLength) {
            this.setState({ maxLength: nextProps.maxLength});
        };
        if (this.state.placeholder !== nextProps.placeholder) {
            this.setState({ placeholder: nextProps.placeholder});
        };
    };
    componentDidMount() {
        this.setState({ value: this.props.value,
            disabled: this.props.disabled,
            checked: this.props.checked,
            className:this.props.className,
            maxLength:this.props.maxLength,
            placeholder:this.props.placeholder
        });                                        
    };
    handleBlur(event) {
        if ((this.props.checkError===null)||(this.props.checkError(event,false) === true)) {
            this.setState({ value: event.target.value,
                oldValue: event.target.value
            })
        }
        else
        {
            this.setState({ value: this.state.oldValue })
        }
        this.setState({ backgroundColor: ''})
    };

    handleChange(event) {
        if (this.state.value !== event.target.value) {
            this.setState({ value: event.target.value })
            if ((this.props.checkError!==null)&&(this.props.checkError(event,true) === false)) {
                this.setState({ backgroundColor: 'red'})
            }
            else
            {
                this.setState({ backgroundColor: ''})
            }
        }
        if (this.props.onClick!==null) {
            this.props.onClick();
        }
    };
    render() {  
        return <input value={this.state.value} 
                    maxLength={this.state.maxLength} 
                    placeholder={this.state.placeholder} 
                    className={this.state.className}
                    id={this.props.id}
                    name={this.props.name}
                    type={this.props.type}
                    disabled={this.state.disabled} 
                    checked={this.state.checked} 
                    onBlur={this.handleBlur}
                    onChange={this.handleChange}
                    style={{background:this.state.backgroundColor}}/>
}
};
Input.propTypes= 
{
    value:PropTypes.string,
    placeholder:PropTypes.string,
    maxLength: PropTypes.number,
    tabIndex: PropTypes.number,
    disabled:PropTypes.bool,
    checked:PropTypes.bool,
    className:PropTypes.string,
    id:PropTypes.string,
    name:PropTypes.string,
    type:PropTypes.string,
    readOnly:PropTypes.bool,
    checkError: PropTypes.func,
    onClick: PropTypes.func
}
Input.defaultProps =
{
    placeholder:'',
    maxLength:100,
    tabIndex:0,
    disabled:false,
    checked:false,
    value:'',
    className:'',
    id:'',
    name:'',
    type:'text',
    checkError:null,
    onClick:null
}