import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class Select extends Component {
    displayName: 'Select';
    constructor(props) {
        super(props);
        var selected = this.getSelectedFromProps(this.props);
        this.state = {
            selected:selected
        };
        this.handleChange = this.handleChange.bind(this);
    };
    getSelectedFromProps(props) {
        var selected;
        if (props.value === null && props.options.length !== 0) {
            selected = props.options[0][props.valueField];
        } else {
            selected = props.value;
        }
        return selected;
    };

    componentWillReceiveProps(nextProps) {
        var selected = this.getSelectedFromProps(nextProps);
        this.setState({
            selected: selected
        });
    };

    handleChange(event) {
        if (this.props.onChange) {
            var change = {
                oldValue: this.state.selected,
                newValue: event.target.value
            }
            this.props.onChange(change);
        }
        this.setState({selected: event.target.value});
    };

    render() {
        var self = this;
        var options = self.props.options.map(function(option) {
            return <option key={option[self.props.valueField]} value={option[self.props.valueField]}> {option[self.props.labelField]} </option>
        });
        return <select id={this.props.id} 
                        className={this.props.className}
                        value={this.state.selected} 
                        onChange={this.handleChange}>
                        {options}
                </select>
    };

};
    
Select.propTypes= 
{
    id: PropTypes.string.isRequired,
    options: PropTypes.array.isRequired,
    value: PropTypes.oneOfType(
        [
            PropTypes.number,
            PropTypes.string
        ]
    ),
    valueField: PropTypes.string,
    labelField: PropTypes.string,
    onChange: PropTypes.func,
    className: PropTypes.string
}
Select.defaultProps =
{
    value: 95,
    valueField: 'value',
    labelField: 'label',
    onChange: null,
    className:'form-control'
}