import React, { Component } from 'react';
import '../../Content/Site.css';
import '../../Content/custom.css';
import '../../Content/ColumnsOfSameHeight.css';
import CrossJoin from '../../Content/Images/CrossJoin.png';
import InnerJoin from '../../Content/Images/InnerJoin.png';
import LeftOuterJoin from '../../Content/Images/LeftOuterJoin.png';
import GroupJoin from '../../Content/Images/GroupJoin.png';

import { Grid, Row, Col, Glyphicon, Button, Carousel} from 'react-bootstrap';


export default class Sql extends Component {
    constructor(props) {
        super(props);
        this.state = {
            hasMounted:false
        };
    };
   
    componentDidMount() {
    };

	componentWillReceiveProps(newProps) {
	};

	render() {
	    const carouselInstance = (
       <Carousel interval={6000} >
         <Carousel.Item >
		    <img src={GroupJoin} alt="Group Join" className="img-responsive" />
			<Carousel.Caption>
            <p>
			    Group Join med LINQ, Lambda och SQL
			    <a className="btn btn-default btn-default" href="https://www.dotnetperls.com/groupjoin">
                   Läs mer på Dot Net Pearls
			   </a>
			</p>
            </Carousel.Caption>
        </Carousel.Item>
        <Carousel.Item >
		  <img src={LeftOuterJoin} alt="Left Outer Join" className="img-responsive" />
          <Carousel.Caption>
             <p>
				Left Outer Join med LINQ, Lambda och SQL
		        <a className="btn btn-default btn-default" href="https://msdn.microsoft.com/en-us/library/bb397895.aspx">
                   Läs mer på MSDN
			   </a>
			 </p>
          </Carousel.Caption>
        </Carousel.Item>
        <Carousel.Item >
		  <img src={InnerJoin} alt="Inner Join" className="img-responsive" />
          <Carousel.Caption>
             <p>
			    Left Outer Join med LINQ, Lambda och SQL
			    <a className="btn btn-default btn-default" href="https://msdn.microsoft.com/en-us/library/bb397895.aspx">
                     Läs mer på MSDN
				</a>
			 </p>
          </Carousel.Caption>
        </Carousel.Item>
        <Carousel.Item >
		  <img src={CrossJoin} alt="Cross Join" className="img-responsive" />
          <Carousel.Caption>
             <p>
                Cross Join med LINQ, Lambda och SQL
				<a className="btn btn-default btn-default" href="https://www.dotnetperls.com/selectmany">
                    Läs mer på Dot Net Pearls
				</a>
			 </p>
          </Carousel.Caption>
        </Carousel.Item>
      </Carousel>
);

        return <div className="container">
	<Row>
		<Col className="row-height">
			<Col md={3} className="hidden-md hidden-sm hidden-xs col-md-height col-md-top custom-vertical-left-border custom-vertical-right-border grey-background">
				<Row>
					<Col md={12}>
						<h4></h4>
					</Col>
				</Row>
				<Row>
					<Col md={12}>
						<h4>LINQ och SQL</h4>
					</Col>

				</Row>
			</Col>
			<Col md={9} className="col-md-height">
				<Row>
                    {carouselInstance}
				</Row>
			</Col>
		</Col>
	</Row>
   </div>;
    }
};