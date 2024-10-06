import React, { useState } from "react";
import Select from 'react-select';
import 'bootstrap/dist/css/bootstrap.min.css';
<<<<<<< HEAD
import createArticleClicked from '../../services/articleService'; 

const ArticleModal = ({ show, handleClose }) => {
  const availableTags = [
    { value: 'Technology', label: 'Technology' },
    { value: 'Science', label: 'Science' },
    { value: 'Health', label: 'Health' },
  ];

  // Input states for the article
  const [articleTitle, setArticleTitle] = useState('');
  const [articleBody, setArticleBody] = useState('');
  const [articleFile, setArticleFile] = useState(null);
  const [selectedTags, setSelectedTags] = useState([]);
  const [loading, setLoading] = useState(false);

  // Handle file selection
  const handleFileChange = (e) => {
    setArticleFile(e.target.files[0]); 
  };

  // Handle form submission using createArticleClicked from the service
  const handleSubmit = async () => {
    setLoading(true);
    try {
      // Prepare form data for submission
      const formData = new FormData();
      formData.append('title', articleTitle);
      formData.append('body', articleBody);
      formData.append('image', articleFile);
      formData.append('tags', JSON.stringify(selectedTags));

      // Use createArticleClicked function to submit data
      createArticleClicked(formData);
=======
import createArticleClicked from '../../services/articleService'; // Import createArticleClicked

const ArticleModal = ({ show, handleClose }) => {
  const availableTags = [
    { value: 'Technology', label: 'Technology' },
    { value: 'Science', label: 'Science' },
    { value: 'Health', label: 'Health' },
  ];

  // Input states for the article
  const [articleTitle, setArticleTitle] = useState('');
  const [articleBody, setArticleBody] = useState('');
  const [articleFile, setArticleFile] = useState(null);
  const [selectedTags, setSelectedTags] = useState([]);
  const [loading, setLoading] = useState(false);

  // Handle file selection
  const handleFileChange = (e) => {
    setArticleFile(e.target.files[0]); // Only selecting the first file
  };

  // Handle form submission using createArticleClicked from the service
  const handleSubmit = async () => {
    setLoading(true);
    try {
      // Prepare form data for submission
      const formData = new FormData();
      formData.append('title', articleTitle);
      formData.append('body', articleBody);
      formData.append('image', articleFile);
      formData.append('tags', JSON.stringify(selectedTags));

      // Use createArticleClicked function to submit data
      createArticleClicked(formData);
>>>>>>> ca400d9910c93a9f70248b89ff91a23b4c49c417

      alert("Article created successfully");
      handleClose(); // Close the modal on success
    } catch (error) {
      console.error("Error submitting article:", error);
      alert("Failed to create article. Please try again.");
    } finally {
      setLoading(false);
    }
  };

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
                  <input
                    type="text"
                    className="form-control"
                    id="formArticleTitle"
                    placeholder="Enter article title"
                    value={articleTitle}
                    onChange={(e) => setArticleTitle(e.target.value)}
                    required
                  />
                </div>

<<<<<<< HEAD
                <div className="form-group mt-3">
                  <label htmlFor="formArticleFile">Upload image File</label>
                  <input
                    type="file"
                    className="form-control"
                    id="formArticleFile"
                    onChange={handleFileChange}
                    required
                  />
                </div>
=======
                <div className="form-group mt-3">
                  <label htmlFor="formArticleFile">Upload image File</label>
                  <input
                    type="file"
                    className="form-control"
                    id="formArticleFile"
                    onChange={handleFileChange}
                    required
                  />
                </div>
>>>>>>> ca400d9910c93a9f70248b89ff91a23b4c49c417

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
                  <textarea
                    className="form-control"
                    id="formArticleBody"
                    rows="5"
                    placeholder="Enter article content"
                    value={articleBody}
                    onChange={(e) => setArticleBody(e.target.value)}
                    required
                  ></textarea>
                </div>
              </form>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={handleClose}>Cancel</button>
              <button type="button" className="btn btn-primary" onClick={handleSubmit} disabled={loading}>
                {loading ? 'Submitting...' : 'Submit'}
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="modal-backdrop fade show" onClick={handleClose}></div>
    </>
  );
};

export default ArticleModal;
<<<<<<< HEAD
=======

>>>>>>> ca400d9910c93a9f70248b89ff91a23b4c49c417
