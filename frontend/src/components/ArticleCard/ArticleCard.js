import React from 'react';
import './ArticleCard.css'; 

const ArticleCard = ({ image, title, content }) => {
  return (
    <div className="article-card">
      <img className="article-image" src={image} alt="Article" />
      <div className="article-info">
        <h3 className="article-title">{title}</h3>
        <p className="article-content">{content}</p>
        <button className="btn btn-link">Read More</button>

      </div>
    </div>
  );
};

export default ArticleCard;
