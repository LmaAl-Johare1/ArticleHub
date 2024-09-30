import React from 'react';
import { Button, Container, Row, Col, Card, Pagination, Badge } from 'react-bootstrap';
import styles from "./UserInfo.module.css"
function UserInfo() {
    return (
      <div className={styles.UserInfo}>
        <Col md={8} >
        <Card className="p-5" style={{height:254 ,marginLeft:14 , background:"#f7f6f6"}}>
            <div className="d-flex justify-content-between align-items-center mb-3">
                <div>
                    <h5 className="mb-1">Full Name</h5>
                    <p className="mb-1 text-muted">user@example.com</p>
                </div>
                <Button variant="outline-primary">Follow</Button>
            </div>
            <br></br>
            <p className="text-muted">
                Bio: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus
                imperdiet, nulla et dictum interdum, nisi lorem egestas odio.
            </p>
        </Card>
        </Col>
      </div>
    );
  }
  
  export default UserInfo;





