import React from 'react';
import UserArticle from './UserArticle';

function ArticlesList(){
  const articles = [
    { id: 1, title: "Article 1", content: "This is the first article.", image: "https://via.placeholder.com/150" },
    { id: 2, title: "Article 2", content: "This is the second article.", image: "https://via.placeholder.com/150" },
    { id: 3, title: "Article 3", content: "This is the third article.", image: "https://via.placeholder.com/150" }

  ];

  return (
    <div className="container">
      <div className="row">
        {articles.map((article) => (
          <div className="col-md-6 col-lg-4 mb4 "  key={article.id}>
            {/* Pass the article data as props to ArticleCard */}
            <UserArticle
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

export default ArticlesList;