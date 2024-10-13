import React, { useState } from 'react';
import './TagList.css'; 
import CreateArticleButton from '../CreateArticleButton/CreateArticleButton';

const TagList = ({ onTagClick, onCreateClick }) => {
    const [activeTag, setActiveTag] = useState('All');

    const handleTagClick = (tag) => {
        setActiveTag(tag);
        onTagClick(tag);
    };

    return (
        <div className="container mt-4">
            <div className="taglist-container row d-flex justify-content-between align-items-center">
                <div className="tags col-12 col-md-8 d-flex flex-wrap mb-3 mb-md-0">
                    {['All', 'Technology', 'Science', 'Health', 'Following'].map((tag) => (
                        <button
                            key={tag}
                            className={`btn tag-btn ${activeTag === tag ? 'active-tag' : ''}`}
                            onClick={() => handleTagClick(tag)}
                        >
                            {tag}
                        </button>
                    ))}
                </div>
                <div className="col-12 col-md-4 d-flex justify-content-md-end">
                    <CreateArticleButton onClick={onCreateClick} />
                </div>
            </div>
        </div>
    );
};

export default TagList;
