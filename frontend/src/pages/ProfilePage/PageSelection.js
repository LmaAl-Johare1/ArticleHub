import React from 'react';
import { Pagination,Row} from 'react-bootstrap';

function PageSelcetion() {
    return (
      <div style={{marginTop:40,marginLeft:500}}>
      <nav aria-label="Page navigation example">
      <ul class="pagination">
        <li class="page-item">
          <a class="page-link" href="#" aria-label="Previous" style={{color:"#2c3b4c"}}>
            <span aria-hidden="true">&laquo;</span>
            
          </a>
        </li>
        <li class="page-item"><a class="page-link" href="#" style={{color:"#2c3b4c"}}>1</a></li>
        <li class="page-item"><a class="page-link" href="#"style={{color:"#2c3b4c"}} >2</a></li>
        <li class="page-item"><a class="page-link" href="#" style={{color:"#2c3b4c"}}>3</a></li>
        <li class="page-item">
          <a class="page-link" href="#" aria-label="Next" style={{color:"#2c3b4c"}}>
            <span aria-hidden="true">&raquo;</span>
            
          </a>
        </li>
      </ul>
    </nav>
      </div>
    );
  }
  
  export default PageSelcetion;