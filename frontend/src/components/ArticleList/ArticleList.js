import React, { useState, useEffect } from 'react';
import './ArticleList.css'; 
import ArticleCard from '../ArticleCard/ArticleCard';
import getArticles from '../../services/getArticles';

const ArticleList = ({ selectedTag, keyword }) => {
  const [articles, setArticles] = useState([]);  // Initialized as an empty array
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    const fetchArticles = async () => {
      setLoading(true);
      try {
        const data = await getArticles(selectedTag, currentPage, keyword);

        // Ensure that the articles field exists in the response
        if (data && data.articles) {
          setArticles(data.articles);  // Set articles array
          setTotalPages(data.totalPages || 1);  // Set totalPages if available, otherwise 1
        } else {
          setArticles([]);  // Set to empty array if no articles field
        }
      } catch (error) {
        console.error("Error fetching articles:", error);
        setArticles([]);  // Fallback to an empty array in case of error
      } finally {
        setLoading(false);
      }
    };

    fetchArticles();
  }, [selectedTag, currentPage, keyword]);

  // Page change handler
  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  // Loading state handling
  if (loading) {
    return <div>Loading articles...</div>;
  }

  return (
    <div className="container">
      <div className="row">
        {articles && articles.length > 0 ? (
          articles.map((article) => (
            <div className="col-md-6 col-lg-4 mb-4" key={article.article_id}>
              <ArticleCard
                id={article.article_id}
                image={article.image}
                title={article.title}
                content={article.body.slice(0, 50) + '...'}
              />
            </div>
          ))
        ) : (
          <div>No articles found.</div>
        )}
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="row">
          <div className="col-12 d-flex justify-content-center">
            <nav aria-label="Page navigation">
              <ul className="pagination">
                {Array.from({ length: totalPages }, (_, index) => (
                  <li
                    key={index + 1}
                    className={`page-item ${index + 1 === currentPage ? 'active' : ''}`}
                    onClick={() => handlePageChange(index + 1)}
                  >
                    <button className="page-link">{index + 1}</button>
                  </li>
                ))}
              </ul>
            </nav>
          </div>
        </div>
      )}
    </div>
  );
};

export default ArticleList;
