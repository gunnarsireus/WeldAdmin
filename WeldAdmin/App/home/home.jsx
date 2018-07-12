import React, { Component } from 'react';
import '../../Content/Site.css';
import '../../Content/ColumnsOfSameHeight.css';
import '../../Content/custom.css';
import Swirl from '../common/Swirl.jsx';
import { Grid, Row, Col, Glyphicon, Button } from 'react-bootstrap';
import IIFE from 'react-iife';
import PhotoFrame from '../common/PhotoFrame.jsx';
import epostJpg from '../../Content/images/epost.jpg';

export default class Home extends Component {
    constructor(props) {
        super(props);
        this.getRandom = this.getRandom.bind(this);
        this.state = {
            rand:this.getRandom()
        };
    };

    getRandom(){
        return Math.floor((Math.random() * 10000)); 
    };

    componentWillUnmount() {
    };

    componentDidMount() {
        this.state = {
            rand:this.getRandom()
        };
    };

    componentWillReceiveProps(newProps) {
        this.state = {
            rand:this.getRandom()
        };
    };

    widget() {
        $.getScript("http://www.klart.se/widget/widget_loader/420cee0ee12535cd2a922993c70ebd2e", function (data, textStatus, jqxhr) {
            console.log('Widget was loaded.');
        });
    };

    render() {       
        return <div className="container">
            <Row>
                <div className="row-height">
                    <Col md={3} className="hidden-md hidden-sm hidden-xs col-md-height col-md-top custom-vertical-left-border custom-vertical-right-border grey-background">
                        <Row>
                            <Col md={12}>
                                <h4></h4>
                            </Col>
                        </Row>
                        <Row>
                            <Col md={12}>
                                <div className="panel panel-primary center-block">
                                    <div className="panel-heading text-center">
                                        <h3 className="panel-title">Dagens bild</h3>
                                    </div>
                                    <div className="panel-body">
                                        <Row className="text-center">
                                            <Col md="12" id="divSwirl">
                                                <Swirl open={true}> 
                                                    <PhotoFrame>
                                                        <a href={'/#/details/0/0/-?' + this.state.rand}>
                                                            <img src={'/Handler/Index/PhotoID=0/Size=M?' + this.state.rand} alt="" style={{ border: "4px solid white", maxHeight: "100%", maxWidth: "100%", verticalAlign:"middle"}} />
                                                       </a>
                                                    </PhotoFrame>
                                                </Swirl>
                                            </Col>
                                        </Row>
                                        <Row className="text-center">
                                            <hr />
                                            <div id="divText" style={{ visibility:'hidden', minHeight:'60px' }}>Bilder tagna av medarbetare p&#229; Sir&#233;us Consulting AB.</div>
                                            <div id="divDownload" style={{ visibility:'hidden', minHeight:'50px' }}>
                                                <p>
                                                    <a href="/Handler/Download/0/M">
                                                        <img src="./Content/images/button-download.gif" alt="download photo" />
                                                    </a>
                                                </p>
                                                <hr />
                                                <p>
                                                    Se <a href="/#/albums">fler bilder</a>
                                                </p>
                                            </div>
                                        </Row>
                                    </div>
                                </div>
                                <div id="divContact" style={{ visibility:'hidden', minHeight:'90px' }}>
                                    <h4>
                                        Kontaktinformation
                                    </h4>
                                    <p>
                                        &nbsp;&nbsp;<span className="glyphicon glyphicon-phone-alt" style={{ color:"gray" }}></span>&nbsp;0708-940 567
                                    </p>
                                    <p>
                                        <img src={epostJpg} alt="epost" className="img-responsive" />
                                    </p>
                                </div> 
                                <Row >
                                    <Col md={12}>
                                        <div className="panel panel-primary" >
                                            <div className="panel-heading">
                                                <h3 className="panel-title text-center">Dagens väder</h3>
                                            </div>
                                            <div className="panel-body panel-height_small">
                                                <div id="divWeather" style={{visibility:"hidden"}}>
                                                    <IIFE fn={this.widget}>
                                                        <Row>
                                                            <div id="c_420cee0ee12535cd2a922993c70ebd2e" className="center-block widget"></div>
                                                        </Row>
                                                    </IIFE>
                                                </div>
                                            </div>
                                        </div>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>
                    </Col>
                    <Col md={9} className="col-md-height">
                        <Row >
                            <Col md={12}>
                                <h4></h4>
                            </Col>
                        </Row>
                        <Row>
                            <Col md={12}>
                                <div className="panel panel-primary">
                                    <div className="panel-heading">
                                        <h3 className="panel-title">Välkommen till Gunnar Siréus ReactJS Single Page Application</h3>
                                    </div>
                                    <div className="panel-body">
                                        Jag arbetar numera på <a href="http://www.itancan.com/">Itancan Consulting AB</a> i Upplands Väsby och tar gärna konsultuppdrag inom ASP.NET och SQL Server-utveckling, backend-programmering i C#, PHP, Visual Basic och frontendutveckling med ReactJS.
                                        <br />
                                        <br />
                                        <img src="./Content/images/MCTS.jpg" alt="Logo" className="center-block" />
                                        <br />
                                        Tveka inte att kontakta mig på <a href="mailto:gunnarsireus@telia.com">gunnarsireus@telia.com</a>
                                    </div>
                                </div>
                            </Col>
                            <Col md={12}>
                                <div className="panel panel-primary">
                                    <div className="panel-heading">
                                        <h3 className="panel-title">Dimensionering av Life Time Buy lagernivå.</h3>
                                    </div>
                                    <div className="panel-body">
                                        En sökning i internet visar att det finns många papper publicerade som beskriver LTB-problemet. Som referens, och som exempel bland många andra hänvisas till följande papper:
                                       <ul>
                                            <li>
                                                <a href="http://www.enme.umd.edu/ESCML/Papers/AgingAircraft07-LTB.pdf">
                                                    Lifetime Buy Optimization to Minimize Lifecycle Cost
                                                </a>
                                            </li>
                                            <li>
                                                <a href="../../Content/images/Final_Order_Problem.pdf">The Final Order Problem for Repairable Spare Parts under condemnation</a>
                                            </li>
                                        </ul>
                                        <p>
                                        I korthet innebär problemet att beräkna hur många reservdelar som behöver lagras för att kunna fullgöra garantiåtaganden. Detta problem
            uppstår för t.ex. flygplanstillverkare och leverantörer av telekomutrustning.
            Tänk er en flygplanstillverkare som har sålt tiotusentals flygplan av en viss
            modell med låt oss säga 30 års garanti. Så länge som de utbytbara komponenter
            som används i flygplanen fortfarande tillverkas kan reservdelar beställas direkt från fabrik.
            Men vid någon tidpunkt kommer produktionen av en viss komponent att upphöra. På grund av
            den stora mängd flygplan i drift kommer det att finnas ett behov av komponenten under hela
            garantiperioden. I många fall är det möjligt att reparera trasiga delar och de reparerade delarna kan återanvändas som reservdelar. Men inte alla delar är möjliga
            att reparera, man talar då om reparationsförluster på exempelvis 30%. Det innebär att endast 70% av
            de delar som skickas för reparation kan återanvändas som reservdel efter reparation. Så länge som
            reparationsverkstäderna har den kunskap och utrustning som krävs för reparation är behovet
            av att lagra delen som reservdel mindre. Vid någon tidpunkt är det inte längre lönsamt hålla
            reparationsverksamheten igång och då måste återstående behov av komponenten täckas genom lagring i ett reservdelslager.
            På sidan <a href="http://www.sireus.se/LTB/Index">LTB</a> har ni möjligheten att själva prova dimensionering av ett LTB-lager.
                                    </p>
                                   </div>
                                </div>
                            </Col>
                        </Row>
                    </Col>
                </div>
            </Row>
        </div>;
    }
};