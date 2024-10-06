import React, { useState, useEffect } from 'react';
import './FeaturedArticle.css';
import { useNavigate } from 'react-router-dom';
import getFeaturedArticle from '../../services/articleService'; // Import the getFeaturedArticle function

const FeaturedArticle = () => {
  const [featuredArticle, setFeaturedArticle] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  // Fetch featured article on component mount
  useEffect(() => {
    const fetchFeatured = async () => {
      try {
        const article = await getFeaturedArticle(); // Fetch the most liked article
        setFeaturedArticle(article);
      } catch (error) {
        console.error('Error fetching featured article:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchFeatured();
  }, []);

  // Function to handle "Read More" click
  const handleReadMore = () => {
    if (featuredArticle) {
      navigate(`/article/${featuredArticle.id}`); // Navigate to the ArticlePage with the featured article ID
    }
  };

  // Placeholder image URL
  const defaultImage = "https://via.placeholder.com/600x400?text=No+Image+Available"; // Placeholder image URL

  if (loading) {
    return <div>Loading featured article...</div>;
  }

  if (!featuredArticle) {
    return <div>No featured article available.</div>;
  }

  return (
    <div className="featured-article card">
      <div className="row no-gutters align-items-center">
        {/* Image Section */}
        <div className="col-md-5">
          <img 
            src={featuredArticle.image || defaultImage} // Use default placeholder image if no image is provided
            alt="Featured Article" 
            className="featured-article-image img-fluid rounded-left" 
          />
        </div>

        {/* Text Section */}
        <div className="col-md-7 p-3">
          <h3 className="featured-article-title">{featuredArticle.title}</h3>
          <p className="featured-article-content">
            {featuredArticle.content.split(' ').slice(0, 20).join(' ')}... {/* Display the first 20 words */}
          </p>
          <button className="btn btn-primary" onClick={handleReadMore}>
            Read more
          </button>
        </div>
      </div>
    </div>
  );
};

export default FeaturedArticle;
