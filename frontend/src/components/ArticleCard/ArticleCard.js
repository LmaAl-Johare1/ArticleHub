import React from 'react';
import './ArticleCard.css';
import { useNavigate } from 'react-router-dom';
import getArticleById from '../../services/getArticleById';

const ArticleCard = ({ id, image, title, content }) => {
  const navigate = useNavigate();

  // Function to handle "Read More" click
  const handleReadMore = async () => {
    try {
      const articleData = await getArticleById(id); 
      console.log(articleData); 
      navigate(`/article/${id}`,{state:articleData }); 
    } catch (error) {
      console.error('Error while fetching the article:', error);
    }
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
