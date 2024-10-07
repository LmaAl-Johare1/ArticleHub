import React, { useState } from "react";
import ArticleModal from "./ArticleModal/ArticleModal";
import TagList from "./TagList/TagList";

const ParentComponent = () => {
  const [showModal, setShowModal] = useState(false);

  const handleShowModal = () => setShowModal(true);
  const handleCloseModal = () => setShowModal(false);

  return (
    <div>
      {/* Pass handleShowModal to the TagList so clicking the button opens the modal */}
      <TagList onCreateClick={handleShowModal} />

      {/* Show the modal when showModal is true */}
      {showModal && <ArticleModal show={showModal} handleClose={handleCloseModal} />}
    </div>
  );
};

export default ParentComponent;
