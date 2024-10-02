import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import styles from "./Header.module.css";
import Logo from "./Logo.png"
import loginBtnClicked from '../../services/loginService';


function Header() {
  
return (
        <div  className={styles.Header}>
          
            <div className="col-12 col-md-6 Logo">
              <img src={Logo} className={styles.Logo}></img>
            </div>
            <div className="col-12 col-md-6 d-flex justify-content-end align-items-center">
              <input
                type="text"
                id="username-input"
                placeholder="Email"
                name="email"
                className="form-control mx-2"
                style={{ width: '200px' }}
              />
              <input
                type="password"
                id="password-input"
                placeholder="Password"
                name="password"
                className="form-control mx-2"
                style={{ width: '200px' } }
              />
              <button className="btn btn-primary" onClick={loginBtnClicked}>
                Login
              </button>
            </div>
          </div>
    
       
        
      );
  

  
  }
  
  export default Header;