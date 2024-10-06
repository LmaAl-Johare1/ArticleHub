import React from 'react';
import { Button, Container, Row, Col, Card, Pagination, Badge } from 'react-bootstrap';

function UserStats() {
    return (
        <div>
            <Row className="mb-5" >
             
             <Col md={4}style={{marginLeft:"auto" , marginTop:-255 ,width:355}}>
                 {/* Stats */}
                 <Card className="text-center p-3 mb-3 " style={{height:75,cursor:"pointer" , background:"#f7f6f6"}}>
                     <h5><Badge bg="secondary" >Followers</Badge></h5>
                     <h5 style={{marginTop:-5}}>120</h5>
                 </Card>
                 <Card className="text-center p-3 mb-3" style={{height:75,cursor:"pointer", background:"#f7f6f6"}}>
                     <h5 ><Badge bg="secondary">Following</Badge></h5>
                     <h5 style={{marginTop:-5}}>180</h5>
                 </Card>
                 <Card className="text-center p-3 mb-3" style={{height:75,cursor:"pointer", background:"#f7f6f6"}}>
                     <h5><Badge bg="secondary">Articles</Badge></h5>
                     <h5 style={{marginTop:-5}}>30</h5>
                 </Card>
             </Col>
         </Row>
        </div>
      
    );
  }
  
  export default UserStats;
