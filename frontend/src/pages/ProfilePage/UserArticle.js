import React, { useState } from 'react';
import './UserArticle.module.css';
import Img from "./Img.jpg";
import EditArticleModal from '../EditArticle/EditArticle(Popup)';
import "./DeletePopUp.css";
import deleteService from "../../services/deleteService"
function UserArticle({ image, title, content }) {
  const [showModal, setShowModal] = useState(false); // State to handle modal visibility

  const handleEditClick = () => {
    setShowModal(true); // Show the modal when Edit is clicked
  };

  const handleCloseModal = () => {
    setShowModal(false); // Close the modal when finished
  };

//delete button
  const [showPopup, setShowPopup] = useState(false);

  const handleDeleteClick = () => {
    setShowPopup(true);
  };

  const handleConfirmDelete = () => {
    deleteService()
    setShowPopup(false);
  };

  const handleCancelDelete = () => {
    setShowPopup(false);
  };

  return (
    <div>
      <div className="card" style={{ width: "22rem", marginTop: -15, background: "#f7f6f6", cursor: "pointer" }}>
        <img src={Img} className="card-img-top" alt="Article" style={{ width: "95%", height: "18rem", margin: 10 }} />
        <div className="card-body" style={{ width: "95%", marginLeft: 10, marginBottom: 10 }}>
          <h5 className="card-title">{title}</h5>
          <p className="card-text">{content}</p>
          <input type="hidden" id="article-id-input" value=""/>
          <hr />
          <div style={{ display: "flex", justifyContent: "space-between", width: "100%" }}>
            <a href="#" className="btn btn-primary" style={{ width: 90 }} onClick={handleEditClick}>
              Edit
            </a>
            <a href="#" className="btn btn-primary" style={{ width: 90 }} onClick={handleDeleteClick}>
              Delete
            </a>
          </div>
        </div>
      </div>

      {/* Edit Article Modal */}
      {showModal && (
        <EditArticleModal
          show={showModal}
          handleClose={handleCloseModal}
          articleData={{ title, content, image }}
        />
      )}

      {/* Delete Modal */}
      {showPopup && (
              <div className="popup">
                <div className="popup-content">
                  <p>Are you sure you want to delete this article?</p>
                  <button onClick={handleConfirmDelete}>Yes, Delete</button>
                  <button onClick={handleCancelDelete}>Cancel</button>
                </div>
              </div>
            )}


      
    </div>

    
  );
}

export default UserArticle;
