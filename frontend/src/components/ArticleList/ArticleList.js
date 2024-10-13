import React from 'react';
import './ArticleList.css';
import { Card, Button, Spinner } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const ArticleList = ({ articles, loading, error }) => {
  // Show a loading spinner when articles are being fetched
  if (loading) {
    return (
      <div className="d-flex justify-content-center my-5">
        <Spinner animation="border" variant="primary" />
      </div>
    );
  }

  // Handle the case where no articles are found
  if (error) {
    return <p className="text-danger text-center mt-5">An error occurred while fetching articles. Please try again later.</p>;
  }

  if (!articles || articles.length === 0) {
    return <p className="text-center mt-5">No articles found. Please try again later.</p>;
  }
  
  return (
    <div className="row">
      {articles.map((article) => (
        <div key={article.id} className="col-md-4 col-sm-6 mb-4">
          <Card className="shadow-sm h-100">
            <Card.Img 
              variant="top" 
              src={article.image || "https://via.placeholder.com/300"} 
              alt={article.title}
            />
            <Card.Body>
              <Card.Title className="text-truncate">{article.title}</Card.Title>
              <Card.Text className="article-body">
                {article.body.length > 100 
                  ? `${article.body.substring(0, 100)}...` 
                  : article.body}
              </Card.Text>
              <Link to={`/articles/${article.id}`}>
                <Button variant="primary">Read More</Button>
              </Link>
            </Card.Body>
          </Card>
        </div>
      ))}
    </div>
  );
};

export default ArticleList;
