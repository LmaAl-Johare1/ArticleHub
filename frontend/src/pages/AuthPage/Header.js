import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import styles from './Header.module.css';
import logo from '../../images/logo-no-background.png';
import loginBtnClicked from '../../services/loginService';
import { useNavigate } from 'react-router-dom';

function Header() {
  const navigate = useNavigate();  
  const handleloginClick = () => {
    loginBtnClicked(navigate);
  };
  return (
    <div className={styles.Header}>
      <div className={`${styles['logo-box']} col-12 col-md-6 text-center text-md-start`}>
        <img src={logo} className={styles.Logo} alt="Logo" />
      </div>
      <div className="col-12 col-md-6 d-flex justify-content-end align-items-center">
        <input
          type="text"
          id="Email-input"
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
          style={{ width: '200px' }}
        />
        <button className="btn btn-primary" onClick={handleloginClick}>
          Login
        </button>
      </div>
    </div>
  );
}

export default Header;
