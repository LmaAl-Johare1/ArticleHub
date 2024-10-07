import React, { useState } from 'react';
import './TagList.css'; 
import CreateArticleButton from '../CreateArticleButton/CreateArticleButton';
import tagClickOn from '../../services/tagClickOn';

const TagList = ({ onTagClick, onCreateClick }) => {
  const [activeTag, setActiveTag] = useState('All'); // Default active tag

  // Function to handle tag selection
  const handleTagClick = async (tag) => {
    setActiveTag(tag); // Update the active tag
  
    try {
      // Fetch articles based on the clicked tag
      const articlesData = await tagClickOn(tag);
  
      if (articlesData) {
        onTagClick(articlesData); // Notify parent component of the selected tag and articles data
        console.log('Articles Data:', articlesData);
      } else {
        console.log('No articles found for the selected tag.');
      }
    } catch (error) {
      console.error('Error fetching articles for selected tag:', error);
    }
  };


  return (
    <div className="container mt-4">
      <div className="taglist-container row d-flex justify-content-between align-items-center">
        {/* Tag Buttons */}
        <div className="tags col-12 col-md-8 d-flex flex-wrap mb-3 mb-md-0">
          {['All', 'Technology', 'Science', 'Health', 'Following'].map((tag) => (
            <button
              key={tag}
              className={`btn tag-btn ${activeTag === tag ? 'active-tag' : ''}`}
              onClick={() => handleTagClick(tag)} // Call handleTagClick when a tag is clicked
            >
              {tag}
            </button>
          ))}
        </div>

        {/* Create Article Button */}
        <div className="col-12 col-md-4 d-flex justify-content-md-end">
          <CreateArticleButton onClick={onCreateClick} /> {/* Call onCreateClick to open the modal */}
        </div>
      </div>
    </div>
  );
};

export default TagList;
