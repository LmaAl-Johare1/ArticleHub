import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import './Header.css';
import logo from '../../images/logo-no-background.png';

const Header = () => {
  const location = useLocation(); // Get the current location
  const navigate = useNavigate(); // To programmatically navigate

  // Function to handle Profile button click
  const handleProfileClick = () => {
    navigate('/profile'); 
  };

  const handleEditProfileClick = () => {
    navigate('/profile/edit'); 
  };

  const handleHomeClick = () => {
    navigate('/Homepage'); 
  };

  // Function to handle Logout click
  const handleLogoutClick = () => {
    // Clear session data (e.g., remove token from localStorage or sessionStorage)
    localStorage.removeItem('authToken'); // Assuming authToken is stored
    sessionStorage.removeItem('userSession'); // Remove session storage if used
    
    // Optionally clear all stored data
    // localStorage.clear();
    // sessionStorage.clear();

    // Redirect to AuthPage
    navigate('/'); // Redirect to authentication page after logout
  };

  return (
    <header className="header">
      <nav className="container-fluid d-flex justify-content-between align-items-center flex-wrap">
        
        {/* Logo */}
        <div className="logo-box">
          <Link to="/home"> 
            <img src={logo} alt="Logo" className="img-fluid" />
          </Link>
        </div>

        {/* Buttons */}
        <div className="nav-buttons d-flex justify-content-end flex-wrap">
          {/* Show Home button only when not on the home page */}
          {location.pathname !== '/Homepage' && (
            <button className="btn btn-secondary rounded-pill mx-2" onClick={handleHomeClick}>
              Home
            </button>
          )}

          {/* Show Edit profile button only on the ProfilePage */}
          {location.pathname === '/profile' && (
            <button className="btn btn-primary rounded-pill mx-2" onClick={handleEditProfileClick}>
              Edit Profile
            </button>
          )}

          {/* Show Profile button on all pages except the ProfilePage itself */}
          {location.pathname !== '/profile' && (
            <button className="btn custom-profile-btn rounded-pill mx-2" onClick={handleProfileClick}>
              Profile
            </button>
          )}

          {/* Logout Button - Always show */}
          <button className="btn custom-logout-btn rounded-pill mx-2" onClick={handleLogoutClick}>
            Logout
          </button>
        </div>
      </nav>
    </header>
  );
};

export default Header;
