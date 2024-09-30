import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import styles from "./Header.module.css";
import Logo from "./Logo.png"
function Header() {

  const handleHomeButton = () => {

  };
  const handleEditButton = () => {

  };
const handleLogoutButton= () => {

  };

return (
        <div  className={styles.Header}>

            <div className="col-12 col-md-6 Logo">
              <img src={Logo} className={styles.Logo}></img>
            </div>
            <div className="col-12 col-md-6 d-flex justify-content-end align-items-center" >
            <button className="btn btn-primary" onClick={handleHomeButton}  >
                Home
              </button>
              <button className="btn btn-primary" onClick={handleEditButton} >
                Edit
              </button>
              <button className="btn btn-primary" onClick={handleLogoutButton} >
                Logout
              </button>
            </div>
          </div>



      );



  }

  export default Header;