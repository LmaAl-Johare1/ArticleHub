import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import './AuthProfilePage.css';
import ArticleCard from '../../components/ArticleCard/ArticleCard';
import Header from '../../components/Header/Header'; // Import Header component

const AuthProfile = () => {
  const { author } = useParams(); // Get author name from URL params
  const [articles, setArticles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [isFollowing, setIsFollowing] = useState(false); // State to track if the user is following the author

  // Fetch articles by the author
  useEffect(() => {
    const fetchAuthorArticles = async () => {
      // Simulating fetching articles by the author
      const authorArticles = [
        { id: 1, title: "Author's Article 1", content: "This is the first article by the author.", image: "https://via.placeholder.com/150" },
        { id: 2, title: "Author's Article 2", content: "This is the second article by the author.", image: "https://via.placeholder.com/150" },
      ];

      setArticles(authorArticles);
      setLoading(false);
    };

    fetchAuthorArticles();
  }, [author]);

  // Toggle follow/unfollow state
  const handleFollowClick = () => {
    setIsFollowing(!isFollowing); // Toggle the follow state
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <Header /> {/* Add Header component at the top */}
      
      <div className="container auth-profile">
        <h1>{author}'s Profile</h1>

        {/* Follow Button */}
        <button className={`btn ${isFollowing ? 'btn-secondary' : 'btn-primary'} mb-4`} onClick={handleFollowClick}>
          {isFollowing ? 'Unfollow' : 'Follow'}
        </button>

        <h3>Articles by {author}</h3>

        <div className="row">
          {articles.map(article => (
            <div className="col-12 col-md-6 col-lg-4 mb-4" key={article.id}>
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
    </>
  );
};

export default AuthProfile;
