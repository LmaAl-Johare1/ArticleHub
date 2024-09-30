import React from 'react';
import './ArticleCard.css';
import { useNavigate } from 'react-router-dom';

const ArticleCard = ({ id, image, title, content }) => {
  const navigate = useNavigate();

  // Function to handle "Read More" click
  const handleReadMore = () => {
    navigate(`/article/${id}`); // Navigate to the ArticlePage with the article ID
  };

  return (
    <div className="article-card">
      <img className="article-image" src={image} alt="Article" />
      <div className="article-info">
        <h3 className="article-title">{title}</h3>
        <p className="article-content">{content}</p>
        <button className="btn btn-link" onClick={handleReadMore}>
          Read More
        </button>
      </div>
    </div>
  );
};

export default ArticleCard;
