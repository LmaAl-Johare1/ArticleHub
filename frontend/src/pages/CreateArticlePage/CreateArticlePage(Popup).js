// ArticleModal.js
import React, { useState } from "react";
import 'bootstrap/dist/css/bootstrap.min.css';
import CreateArticleButton from '../../components/CreateArticleButton/CreateArticleButton';

const ArticleModal = () => {
  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  return (
    <>
      {/* Use the CreateArticleButton component */}
      <CreateArticleButton onClick={handleShow} />

      {/* Modal */}
      {show && (
        <div className="modal fade show d-block" tabIndex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header d-flex justify-content-between align-items-center">
                {/* Flexbox header: Title and Close Button */}
                <h5 className="modal-title" id="exampleModalLabel">Create New Article</h5>
                <button type="button" className="btn-close" onClick={handleClose}>
                  &times;
                </button>
              </div>
              <div className="modal-body">
                {/* Form inside the modal */}
                <form>
                  <div className="form-group">
                    <label htmlFor="formArticleTitle">Article Title</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formArticleTitle"
                      placeholder="Enter article title"
                    />
                  </div>

                  <div className="form-group mt-3">
                    <label htmlFor="formArticleFile">Upload File</label>
                    <input
                      type="file"
                      className="form-control"
                      id="formArticleFile"
                    />
                  </div>

                  <div className="form-group mt-3">
                    <label htmlFor="formArticleTags">Tags</label>
                    <input
                      type="text"
                      className="form-control"
                      id="formArticleTags"
                      placeholder="Enter tags"
                    />
                  </div>

                  <div className="form-group mt-3">
                    <label htmlFor="formArticleBody">Article Body</label>
                    <textarea
                      className="form-control"
                      id="formArticleBody"
                      rows="5"
                      placeholder="Enter article content"
                    ></textarea>
                  </div>
                </form>
              </div>
              <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={handleClose}>
                  Cancel
                </button>
                <button type="button" className="btn btn-primary">
                  Submit
                </button>
              </div>
            </div>
          </div>
        </div>
      )}

      {/* Modal backdrop to close modal when clicked outside */}
      {show && <div className="modal-backdrop fade show" onClick={handleClose}></div>}
    </>
  );
};

export default ArticleModal;
