import React, { Component } from 'react';
import '../Content/Site.css';
import '../Content/custom.css';
import { Router, Route, Link, browserHistory, IndexRoute, IndexLink  } from 'react-router';
import NavLink from './NavLink.jsx'

export default class Frame extends Component {
    constructor(props) {
        super(props);
        this.getClass = this.getClass.bind(this);
        this.getCss = this.getCss.bind(this);
    };
 
    getClass(dayNumber) {
        switch (dayNumber) {
            case 0:
                return "black";
            case 1:
                return "blue";
            case 2:
                return "bluegreen";
            case 3:
                return "bronze";
            case 4:
                return "gold";
            case 5:
                return "green";
            case 6:
                return "red";
            case 7:
                return "redgreen";
            case 8:
                return "redblue";
            case 9:
                return "silver";
            case 10:
                return "violet";
            case 11:
                return "white";
            default:
                return "white";
        }
    };

    getCss(spriteType, dayNumber) {

        switch (dayNumber) {
            case 0:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 0";
            case 1:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -81px";
            case 2:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -162px";
            case 3:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -243px";
            case 4:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -324px";
            case 5:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -405px";
            case 6:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -486px";
            case 7:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -567px";
            case 8:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -648px";
            case 9:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -729px";
            case 10:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -810px";
            case 11:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -891px";
            default:
                return "transparent url('./Content/images/Bootstrap" + spriteType + "Composed.jpg') 0 -891px";
        }
    };


    render() {    
        var headerClasses = []; 
        var footerClasses = []; 
        var titleClasses = [];
        var d = new Date();
        var dayNumber = d.getDay() % 12;
        headerClasses.push("spriteHeader navbar navbar-default navbar-static-top "); 
        headerClasses.push(this.getClass(dayNumber));
        footerClasses.push("spriteFooter navbar navbar-default navbar-static-top ");
        footerClasses.push(this.getClass(dayNumber));
        titleClasses.push("title ");
        titleClasses.push(this.getClass(dayNumber));
        
        return (<div>
	        <div className={this.headerClasses} style={{ minHeight: '81px', background: this.getCss("Header", dayNumber) }}>
            <div className="container">
                <div className="navbar-header">
                   <img src="./Content/images/Logo.png" style={{maxHeight:"50px", paddingLeft:"100px", paddingRight:"50px", paddingTop:"7px"}} alt=""/>
                    <button type="button" className="navbar-toggle" data-toggle="collapse" data-target="#navbar-collapse">
                        <span className="sr-only">Toggle navigation</span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                        <span className="icon-bar"></span>
                    </button>
                    <a className="navbar-brand" href="/">
					    <span className={this.titleClasses}>Home Page - SireusRR</span>
                    </a>
                </div>
                <div className="collapse navbar-collapse" id="navbar-collapse">
                    <ul id="spriteTabs" role="nav">
                        <NavLink to="/"  onlyActiveOnIndex><span/>&nbsp;&nbsp;&nbsp;Admin&nbsp;&nbsp;&nbsp;</NavLink>
                        <NavLink to="/sql"                ><span/>&nbsp;&nbsp;&nbsp;SQL&nbsp;&nbsp;&nbsp;</NavLink>
                        <NavLink to="/admin"              ><span/>&nbsp;&nbsp;&nbsp;Admin&nbsp;&nbsp;&nbsp;</NavLink>
                    </ul>
                </div>
            </div>
        </div>

        {this.props.children}

        <div className={this.footerClasses} style={{ minHeight: '81px', background: this.getCss("Footer", dayNumber) }}>
            <br />
            <footer>
                <p>Copyright &copy; 2017 - Siréus Consulting AB</p>
            </footer>
        </div>
      </div>
    )
    }
};