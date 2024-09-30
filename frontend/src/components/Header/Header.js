import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Header.css';
import logo from '../../images/logo-no-background.png';

const Header = () => {
  const location = useLocation(); // Get the current location

  return (
    <header className="header">
      <nav className="container-fluid d-flex justify-content-between align-items-center flex-wrap">
        
        {/* Logo */}
        <div className="logo-box">
          <Link to="/"> {/* Link the logo to the home page */}
            <img src={logo} alt="Logo" className="img-fluid" />
          </Link>
        </div>
        

        {/* Profile and Logout Buttons */}
        <div className="nav-buttons d-flex justify-content-end flex-wrap">
            {/* Show Home button only when not on the home page */}
            {location.pathname !== '/' && (
            <Link to="/" className="btn btn-secondary rounded-pill mx-2">
              Home
            </Link>
          )}
          <button className="btn custom-profile-btn rounded-pill mx-2">Profile</button>
          <button className="btn custom-logout-btn rounded-pill mx-2">Logout</button>
          
        
        </div>
      </nav>
    </header>
  );
};

export default Header;
