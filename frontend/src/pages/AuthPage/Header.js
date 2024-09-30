import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import styles from "./Header.module.css";
import Logo from "./Logo.png"
<script src="../services/authService.js"></script>

function Header() {
  
// const [loginInfo, setLoginInfo] = useState({ email: '', password: '' });

// const handleLoginChange = (e) => {
//     setLoginInfo({ ...loginInfo, [e.target.name]: e.target.value });
//   };

  

// const handleLoginSubmit = () => {
//     console.log('Login Info:', loginInfo);
//   };

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
                value={loginInfo.email}
                onChange={handleLoginChange}
                className="form-control mx-2"
                style={{ width: '200px' }}
              />
              <input
                type="password"
                id="password-input"
                placeholder="Password"
                name="password"
                value={loginInfo.password}
                onChange={handleLoginChange}
                className="form-control mx-2"
                style={{ width: '200px' } }
              />
              <button className="btn btn-primary" onClick={loginBtnClicked()} >
                Login
              </button>
            </div>
          </div>
    
       
        
      );
  

  
  }
  
  export default Header;