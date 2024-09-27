import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import styles from "./Registration.module.css";

function Registration() {
const [registerInfo, setRegisterInfo] = useState({
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    password: '',
  });

const handleRegisterChange = (e) => {
    setRegisterInfo({ ...registerInfo, [e.target.name]: e.target.value });
  };


const handleRegisterSubmit = () => {
    console.log('Register Info:', registerInfo);
  };

return(

    <div  className={styles.Registration} >
        <div className="col-12 col-md-6 p-4 bg-light rounded shadow">
          <h2 className="text-center mb-4">Create an account</h2>
          <div className="row">
            <div className="col">
              <input
                type="text"
                placeholder="First Name"
                name="firstName"
                value={registerInfo.firstName}
                onChange={handleRegisterChange}
                className="form-control mb-3"
              />
            </div>
            <div className="col">
              <input
                type="text"
                placeholder="Last Name"
                name="lastName"
                value={registerInfo.lastName}
                onChange={handleRegisterChange}
                className="form-control mb-3"
              />
            </div>
          </div>
          <input
            type="text"
            placeholder="User Name"
            name="userName"
            value={registerInfo.userName}
            onChange={handleRegisterChange}
            className="form-control mb-3"
          />
          <input
            type="email"
            placeholder="Email"
            name="email"
            value={registerInfo.email}
            onChange={handleRegisterChange}
            className="form-control mb-3"
          />
          <input
            type="password"
            placeholder="Password"
            name="password"
            pattern="(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}"
            value={registerInfo.password}
            onChange={handleRegisterChange}
            className="form-control mb-3"
          />
          <button className="btn btn-success w-100" onClick={handleRegisterSubmit}>
            Sign Up
          </button>
        </div>
     </div>
);
  

};

export default Registration;
