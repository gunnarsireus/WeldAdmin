import React, {Component} from 'react';
import PropTypes from 'prop-types';
import ReactDatePicker from 'react-datepicker';
import moment from 'moment';
import 'react-datepicker/dist/react-datepicker.css';
import 'bootstrap/dist/css/bootstrap.css';
import '../../Content/Site.css';
import '../../Content/custom.css';

export default class Datepicker extends Component {
    constructor(props) {
        super(props);   
        this.state = {
            startDate: this.props.startDate
        };  
        this.handleChange = this.handleChange.bind(this);
        this.handleDefaultChange = this.handleDefaultChange.bind(this);
    };

    handleDefaultChange(){
        alert('Default Change');
    };
    handleChange(date) {
      this.setState({
        startDate: date
      });
      this.props.onDateChanged(date);
    };

    componentWillReceiveProps(newProps) {
        this.state = {
            startDate: newProps.startDate
        }; 
    }
 
    render() {
    return <ReactDatePicker
        fixedHeight
        selected={this.state.startDate}
        onChange={this.handleChange} 
        className='col-xs-8 col-md-8 col-sm-8  text-danger input-sm BorderInset'
        dateFormat="YYYY-MM-DD" 
        dateFormatCalendar="MMMM"
        locale="sv" 
        todayButton="Idag"
        showWeekNumbers 
        showYearDropdown 
        scrollableYearDropdown
        dropdownMode="select" />;
  }
}
Datepicker.propTypes= 
{
    startDate:PropTypes.object, 
    className:PropTypes.string,
    onDateChanged: PropTypes.func
};
Datepicker.defaultProps =
{
    startDate:moment(),
    className:'col-xs-8 col-md-8 col-sm-8  text-danger input-sm BorderInset',
    onDateChanged:Datepicker.prototype.handleDefaultChange
};
