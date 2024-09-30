import React from 'react';
import './FeaturedArticle.css'; // Import the CSS file
import { useNavigate } from 'react-router-dom';
import sampleImage from '../../images/sampleImage.jpg'; // Replace with actual image path

const FeaturedArticle = () => {
  const navigate = useNavigate();

  // Function to handle "Read More" click
  const handleReadMore = () => {
    navigate('/article/featured'); // Navigate to the featured article details page (replace 'featured' with the actual ID if needed)
  };

  return (
    <div className="featured-article card">
      <div className="row no-gutters align-items-center">
        {/* Image Section */}
        <div className="col-md-5">
          <img src={sampleImage} alt="Featured Article" className="featured-article-image img-fluid rounded-left" />
        </div>

        {/* Text Section */}
        <div className="col-md-7 p-3">
          <h3 className="featured-article-title">This is a featured article</h3>
          <p className="featured-article-content">
            Very short description of what’s actually being discussed in this article, 
            maybe the first sentences to provide a preview.
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
