import React, { useState, useEffect } from 'react';
import './ArticleList.css'; // Import custom CSS for styling
import ArticleCard from '../ArticleCard/ArticleCard';

const ArticleList = ({ selectedTag }) => {
  // State to store articles fetched from the backend
  const [articles, setArticles] = useState([]);
  const [loading, setLoading] = useState(true); // Loading state

  // Fetch data from the backend using useEffect
  useEffect(() => {
    // Simulating a backend call
    // Replace this with actual API call to the backend when it's ready
    const fetchArticles = async () => {
      try {
        // Uncomment this block when backend is ready
        /*
        const response = await fetch('BACKEND_API_ENDPOINT'); // Replace with actual API endpoint
        const data = await response.json();
        setArticles(data.articles); // Assuming the response contains an "articles" array
        */

        // Mock data to simulate backend response for now
        const mockArticles = [
          { id: 1, title: "Article 1", content: "This is the first article.", image: "https://via.placeholder.com/150", tag: "Technology" },
          { id: 2, title: "Article 2", content: "This is the second article.", image: "https://via.placeholder.com/150", tag: "Science" },
          { id: 3, title: "Article 3", content: "This is the third article.", image: "https://via.placeholder.com/150", tag: "Health" },
          { id: 4, title: "Article 4", content: "This is the fourth article.", image: "https://via.placeholder.com/150", tag: "Technology" },
          { id: 5, title: "Article 5", content: "This is the fifth article.", image: "https://via.placeholder.com/150", tag: "Science" },
          { id: 6, title: "Article 6", content: "This is the sixth article.", image: "https://via.placeholder.com/150", tag: "Health" },
        ];
        setArticles(mockArticles); // Simulate the articles being fetched
        setLoading(false); // Stop loading after the data is fetched
      } catch (error) {
        console.error("Error fetching articles:", error);
        setLoading(false);
      }
    };

    fetchArticles();
  }, []);

  // Step 3: Filter articles based on the selected tag
  const filteredArticles = selectedTag === 'All'
    ? articles
    : articles.filter(article => article.tag === selectedTag);

  // Step 4: Loading state handling
  if (loading) {
    return <div>Loading articles...</div>; // Display a loading message while fetching
  }

  return (
    <div className="container">
      <div className="row">
        {/* Step 5: Render filtered articles */}
        {filteredArticles.slice(0, 6).map((article) => (
          <div className="col-md-6 col-lg-4 mb-4" key={article.id}>
            {/* Pass the article data as props to ArticleCard */}
            <ArticleCard
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
