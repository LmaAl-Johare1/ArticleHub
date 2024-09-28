import React from 'react';
import './TagList.css'; // Import your custom CSS
import CreateArticleButton from '../CreateArticleButton/CreateArticleButton';

const TagList = () => {
  return (
    <div className="taglist-container d-flex justify-content-between align-items-center">
      {/* Tag Buttons */}
      <div className="tags">
        <button className="btn tag-btn active-tag">All</button>
        <button className="btn tag-btn">tag1</button>
        <button className="btn tag-btn">tag2</button>
        <button className="btn tag-btn">tag3</button>
        <CreateArticleButton/>
      </div>
      
    
    </div>
  );
};

export default TagList;
