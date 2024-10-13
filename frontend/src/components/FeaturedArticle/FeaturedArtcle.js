import React from 'react';
import './FeaturedArticle.css';

const FeaturedArticle = ({ article }) => {
  if (!article) {
    return <div>No featured article available.</div>;
  }

  return (
    <div className="featured-article">
      <h3>{article.title}</h3>
      <p>{article.content}</p>
      <img src={article.image || "https://via.placeholder.com/600x400?text=No+Image+Available"} alt="Featured" />
      <button onClick={() => window.location.href = `/articles/${article.id}`}>Read More</button>
    </div>
  );
};

export default FeaturedArticle;
