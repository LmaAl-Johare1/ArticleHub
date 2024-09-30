import React from 'react';
import { Pagination,Row} from 'react-bootstrap';

function PageSelcetion() {
    return (
      <div style={{marginTop:40}}>
        <Row className="justify-content-center mt-4" style={{marginLeft:530}}>
        <Pagination>
            <Pagination.Item active>1</Pagination.Item>
            <Pagination.Item>2</Pagination.Item>
            <Pagination.Item>3</Pagination.Item>
        </Pagination>
        </Row>
      </div>
    );
  }
  
  export default PageSelcetion;