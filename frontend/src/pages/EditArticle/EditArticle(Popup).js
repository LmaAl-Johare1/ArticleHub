import React, { useState, useEffect } from "react";
import Select from 'react-select';
import 'bootstrap/dist/css/bootstrap.min.css';
import './EditArticle.css';
import updateArticleClicked from '../../services/updateArticleClicked';

const EditArticleModal = ({ show, handleClose, articleData }) => {
  const availableTags = [
    { value: 'Technology', label: 'Technology' },
    { value: 'Science', label: 'Science' },
    { value: 'Health', label: 'Health' },
  ];

  const [formData, setFormData] = useState({
    title: '',
    file: null,
    tags: [],
    body: ''
  });

  const [loading, setLoading] = useState(false); 

  // Pre-populate form data if articleData is passed
  useEffect(() => {
    if (articleData) {
      setFormData({
        title: articleData.title || '',
        file: articleData.file || null,
        tags: articleData.tags || [],
        body: articleData.body || ''
      });
    }
  }, [articleData]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleFileChange = (e) => {
    setFormData({ ...formData, file: e.target.files[0] });
  };

  const handleTagChange = (selected) => {
    setFormData({ ...formData, tags: selected ? selected.map(tag => tag.value) : [] });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true); // Set loading state to true while submitting
    try {
      // Prepare form data for the API request
      const updatedData = new FormData();
      updatedData.append('title', formData.title);
      updatedData.append('body', formData.body);
      if (formData.file) {
        updatedData.append('image', formData.file); // If the file is replaced
      }
      updatedData.append('tags', JSON.stringify(formData.tags));

      // Call the updateArticleClicked function to update the article
      await updateArticleClicked(articleData.id, updatedData);
      handleClose(); // Close modal after submitting
    } catch (error) {
      console.error('Error updating article:', error);
      alert('Failed to update the article. Please try again.');
    } finally {
      setLoading(false); // Set loading state back to false after completion
    }
  };

  if (!show) return null;

  return (
    <>
      <div className="modal fade show d-block" tabIndex="-1" role="dialog" aria-labelledby="editArticleLabel" aria-hidden="true">
        <div className="modal-dialog" role="document">
          <div className="modal-content">
            <div className="modal-header d-flex justify-content-between align-items-center">
              <h5 className="modal-title" id="editArticleLabel">Edit Article</h5>
              <button type="button" className="btn-close" onClick={handleClose} aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <form onSubmit={handleSubmit}>
                <div className="form-group">
                  <label htmlFor="formArticleTitle">Article Title</label>
                  <input
                    type="text"
                    className="form-control"
                    id="formArticleTitle"
                    name="title"
                    value={formData.title}
                    onChange={handleInputChange}
                    placeholder="Edit article title"
                    required
                  />
                </div>

                <div className="form-group mt-3">
                  <label htmlFor="formArticleFile">Replace File (Optional)</label>
                  <input
                    type="file"
                    className="form-control"
                    id="formArticleFile"
                    onChange={handleFileChange}
                  />
                  {formData.file && <p className="mt-2">Current File: {formData.file.name}</p>}
                </div>

                <div className="form-group mt-3">
                  <label htmlFor="formArticleTags">Tags</label>
                  <Select
                    isMulti
                    name="tags"
                    options={availableTags}
                    className="basic-multi-select"
                    classNamePrefix="select"
                    value={availableTags.filter(tag => formData.tags.includes(tag.value))}
                    onChange={handleTagChange}
                  />
                </div>

                <div className="form-group mt-3">
                  <label htmlFor="formArticleBody">Article Body</label>
                  <textarea
                    className="form-control"
                    id="formArticleBody"
                    name="body"
                    rows="5"
                    value={formData.body}
                    onChange={handleInputChange}
                    placeholder="Edit article content"
                    required
                  ></textarea>
                </div>
              </form>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={handleClose}>Cancel</button>
              <button type="button" className="btn btn-primary" onClick={handleSubmit} disabled={loading}>
                {loading ? 'Saving...' : 'Save Changes'}
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="modal-backdrop fade show" onClick={handleClose}></div>
    </>
  );
};

export default EditArticleModal;
