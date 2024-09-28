import React from 'react';
import './FeaturedArticle.css'; // Import the CSS file
import sampleImage from '../../images/sampleImage.jpg'; // Replace with actual image path

const FeaturedArticle = () => {
  return (
    <div className="featured-article-container">
      <div className="row no-gutters d-flex flex-row-reverse justify-content-between align-items-center">
        {/* Text Section */}
        <div className="col-md-7 p-2 featured-text">
          <h3 className="article-title">This is a featured article</h3>
          <p>
            Very short description of what’s actually being discussed in this article, 
            maybe the first sentences to provide a preview.
          </p>
          <button className="btn btn-dark read-more-btn">Read more</button>
        </div>

        {/* Image Section */}
        <div className="col-md-3 featured-image">
          <img src={sampleImage} alt="Featured Article" className="img-fluid rounded" />
        </div>
      </div>
    </div>
  );
};

export default FeaturedArticle;
