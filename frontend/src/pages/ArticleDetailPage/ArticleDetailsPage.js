import React, { useState } from 'react';
import Header from '../../components/Header/Header';

const ArticlePage = () => {
  const [comments, setComments] = useState([
    { user: 'user 2', date: 'dd month, yyyy', text: 'comment' },
    { user: 'user 3', date: 'dd month, yyyy', text: 'comment' },
  ]);

  const [newComment, setNewComment] = useState('');

  const handleCommentSubmit = (e) => {
    e.preventDefault();
    if (newComment.trim() !== '') {
      setComments([...comments, { user: 'new user', date: 'dd month, yyyy', text: newComment }]);
      setNewComment('');
    }
  };

  return (
    <div>
      <Header/>

      {/* Article Section */}
      <div className="article">
        <h1>Name of article</h1>
        <p>
          by <strong>name of user</strong>
        </p>
        <p>Published on M dd, yyyy</p>

        {/* Tags */}
        <div className="mb-3">
          <span className="badge bg-secondary me-2">tag1</span>
          <span className="badge bg-dark">tag2</span>
        </div>

        {/* Article Body */}
        <div className="p-4 mb-4 bg-light border rounded">
          The Article body
        </div>

        {/* Like Button */}
        <button className="btn btn-outline-secondary mb-4">Like</button>

        {/* Comments Section */}
        <div className="comments-section">
          <h5>Comments</h5>

          {/* Comment Form */}
          <form onSubmit={handleCommentSubmit} className="mb-4">
            <div className="form-group mb-2">
              <textarea
                className="form-control"
                placeholder="Your Comment"
                value={newComment}
                onChange={(e) => setNewComment(e.target.value)}
                rows="2"
              ></textarea>
            </div>
            <button type="submit" className="btn btn-secondary">Submit</button>
          </form>

          {/* Display Comments */}
          {comments.map((comment, index) => (
            <div key={index} className="bg-light p-3 mb-3 border rounded">
              <p><strong>{comment.user}</strong> <small className="text-muted">{comment.date}</small></p>
              <p>{comment.text}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default ArticlePage;
