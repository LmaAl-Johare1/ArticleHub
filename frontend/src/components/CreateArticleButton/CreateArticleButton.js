// CreateArticleButton.js
import React from "react";

const CreateArticleButton = ({ onClick }) => {
  return (
    <button type="button" className="btn btn-primary" onClick={onClick}>
      Create Article
    </button>
  );
};

export default CreateArticleButton;
