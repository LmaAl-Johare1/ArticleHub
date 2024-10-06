import React, { useState, useEffect } from 'react';
import './ArticleList.css'; 
import ArticleCard from '../ArticleCard/ArticleCard';
import getArticles from '../../services/articleService';

const ArticleList = ({ selectedTag }) => {
  // State to store articles fetched from the backend
  const [articles, setArticles] = useState([]);
  const [loading, setLoading] = useState(true); // Loading state

  // Fetch data from the backend using useEffect
  useEffect(() => {
    const fetchArticles = async () => {
      try {
        // Replace the mock data with actual API call to the backend
        const data = await getArticles(selectedTag);
        setArticles(data.articles); 
      } catch (error) {
        console.error("Error fetching articles:", error);
      } finally {
        setLoading(false); // Stop loading after the data is fetched
      }
    };

    fetchArticles();
  }, [selectedTag]);

  // Filter articles based on the selected tag
  const filteredArticles = selectedTag === 'All'
    ? articles
    : articles.filter(article => article.tag === selectedTag);

  // Loading state handling
  if (loading) {
    return <div>Loading articles...</div>; // Display a loading message while fetching
  }

  return (
    <div className="container">
      <div className="row">
        {/* Render filtered articles */}
        {filteredArticles.map((article) => (
          <div className="col-md-6 col-lg-4 mb-4" key={article.id}>
            {/* Pass the article data as props to ArticleCard */}
            <ArticleCard
              id={article.id}
              image={article.image}
              title={article.title}
              content={article.content}
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default ArticleList;
