import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Header.css';
import logo from '../../images/logo-no-background.png';

const Header = () => {
  return (
    <header className="header">
      <nav className="container-fluid d-flex justify-content-between align-items-center flex-wrap">
        {/* Logo */}
        <div className="logo-box">
          <img src={logo} alt="Logo" className="img-fluid" />
        </div>

        {/* Profile and Logout Buttons */}
        <div className="nav-buttons d-flex justify-content-end flex-wrap">
          <button className="btn custom-profile-btn rounded-pill mx-2">profile</button>
          <button className="btn custom-logout-btn rounded-pill mx-2">logout</button>
        </div>
      </nav>
    </header>
  );
};

export default Header;
