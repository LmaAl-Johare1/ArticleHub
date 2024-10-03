import React, { useState } from "react";
import Select from 'react-select';
import 'bootstrap/dist/css/bootstrap.min.css';

const ArticleModal = ({ show, handleClose }) => {
  const availableTags = [
    { value: 'Technology', label: 'Technology' },
    { value: 'Science', label: 'Science' },
    { value: 'Health', label: 'Health' },
   
  ];

  const [selectedTags, setSelectedTags] = useState([]);

  if (!show) return null;

  return (
    <>
      <div className="modal fade show d-block" tabIndex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div className="modal-dialog" role="document">
          <div className="modal-content">
            <div className="modal-header d-flex justify-content-between align-items-center">
              <h5 className="modal-title" id="exampleModalLabel">Create New Article</h5>
              <button type="button" className="btn-close" onClick={handleClose} aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <form>
                <div className="form-group">
                  <label htmlFor="formArticleTitle">Article Title</label>
                  <input type="text" className="form-control" id="formArticleTitle" placeholder="Enter article title" />
                </div>

                <div className="form-group mt-3">
                  <label htmlFor="formArticleFile">Upload File</label>
                  <input type="file" className="form-control" id="formArticleFile" />
                </div>

                <div className="form-group mt-3">
                  <label htmlFor="formArticleTags">Tags</label>
                  <Select
                    isMulti
                    name="tags"
                    options={availableTags}
                    className="basic-multi-select"
                    classNamePrefix="select"
                    onChange={(selected) => setSelectedTags(selected.map(tag => tag.value))}
                  />
                </div>

                <div className="form-group mt-3">
                  <label htmlFor="formArticleBody">Article Body</label>
                  <textarea className="form-control" id="formArticleBody" rows="5" placeholder="Enter article content"></textarea>
                </div>
              </form>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={handleClose}>Cancel</button>
              <button type="button" className="btn btn-primary">Submit</button>
            </div>
          </div>
        </div>
      </div>

      <div className="modal-backdrop fade show" onClick={handleClose}></div>
    </>
  );
};

export default ArticleModal;
