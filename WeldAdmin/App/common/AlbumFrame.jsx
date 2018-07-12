import React, { Component } from 'react';
import PropTypes from 'prop-types';
import '../../Content/Frame.css';

export default class AlbumFrame extends Component {
    constructor(props) {
        super(props);
        this.state = {
            AlbumID: this.props.AlbumID,
            Count: this.props.Count,
            Caption: this.props.Caption,
            IsPublic: this.props.IsPublic     
        }
    };
	
    componentWillReceiveProps(newProps){
        this.setState({
            AlbumID: newProps.AlbumID,
            Count: newProps.Count,
            Caption: newProps.Caption,
            IsPublic: newProps.IsPublic     
        })
    }

    componentDidMount(){
        this.setState({
            AlbumID: this.props.AlbumID,
            Count: this.props.Count,
            Caption: this.props.Caption,
            IsPublic: this.props.IsPublic     
        })
    }
    render() { 
        return <td className="item">
            <table border="0" cellPadding="0" cellSpacing="0" className="album-frame">
            <tr>
                <td className="topx----">
                    <img alt="" src="/Content/Images/album-l1.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}} />
                </td>
                <td className="top-x---">
                    <img alt="" src="/Content/Images/album-mtl.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}} />
                </td>
                <td className="top--x--"></td>
                <td className="top---x-">
                    <img alt="" src="/Content/Images/album-mtr.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}} />
                </td>
                <td className="top----x">
                    <img alt="" src="/Content/Images/album-r1.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}} />
                </td>
            </tr>
            <tr>
                <td className="mtpx----">
                    <img alt="" src="/Content/Images/album-l2.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}} />
                </td>
                <td colSpan="3" rowSpan="3">
                    <a href={'/#/photos/' + this.state.AlbumID + '/' + this.state.Caption}>
                        <img src={'/Handler/Index/AlbumID=' + this.state.AlbumID + '/Size=M'} className="photo_198" style={{border:'4px solid white', maxHeight:'100%',maxWidth:'100%'}} alt={'Sample Photo from Album Number ' + this.state.AlbumID} /> 
                    </a> 
                </td>
                <td className="mtp----x">
                    <img alt="" src="/Content/Images/album-r2.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}}  />
                </td>
            </tr>
            <tr>
                <td className="midx----"></td>
                <td className="mid----x"></td>
            </tr>
            <tr>
                <td className="mbtx----">
                    <img alt="" src="/Content/Images/album-l3.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}}  />
                </td>
                <td className="mbt----x">
                    <img alt="" src="/Content/Images/album-r3.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}}  />
                </td>
            </tr>
            <tr>
                <td className="botx----">
                    <img alt="" src="/Content/Images/album-l4.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}}  />
                </td>
                <td className="bot-x---" style={{textAlign:'left'}}>
                                                            <img alt="" src="/Content/Images/album-mbl.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}}  />
                </td>
                <td className="bot--x--"></td>
                <td className="bot---x-" style={{textAlign:'right'}}>
                                                            <img alt="" src="/Content/Images/album-mbr.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}}  />&nbsp;&nbsp;
                </td>
                <td className="bot----x">
                    <img alt="" src="/Content/Images/album-r4.gif" style={{maxHeight:'100%',maxWidth:'100%', verticalAlign:'top'}}  />
                </td>
            </tr>
        </table>
        <h4>
            <a href={'/Photos/Index/' + this.state.AlbumID + '/' + this.state.Caption}>{this.state.Caption}</a>
        </h4>
            <div>{this.state.Count} bilder</div> 
    </td>
};

}

    
AlbumFrame.propTypes= 
{
    AlbumID: PropTypes.string.isRequired,
    Count: PropTypes.string.isRequired,
    Caption: PropTypes.string.isRequired,
    IsPublic: PropTypes.bool.isRequired
}
AlbumFrame.defaultProps =
{
    AlbumID: '0',
    Count: '0',
    Caption: '',
    IsPublic: true
}
