import React from 'react';
import './UserArticle.module.css'; 
import Img from "./Img.jpg"

function UserArticle({ image, title, content }){
  return (
    <div >
    <div class="card" style={{width: "22rem", marginTop:-15 ,background:"#f7f6f6"}}>
    <img src={Img} class="card-img-top" alt="..." style={{width:"95%" ,height:"18rem",margin:10}} />
    <div class="card-body" style={{width:"95%" ,marginLeft:10 ,marginBottom:10}}>
        <h5 class="card-title">Article title</h5>
        <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
        <hr></hr>
        <div style={{disply:"felx" ,width:"100%"}}>
        <a href="#" class="btn btn-primary" style={{marginLeft:42,marginRight:50 ,width:90}}>Edit</a>
        <a href="#" class="btn btn-primary" style={{width:90}}>Delete</a>
        </div>

    </div>
    </div>
    </div>
  );
};

export default UserArticle;