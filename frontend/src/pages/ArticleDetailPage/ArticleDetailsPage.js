import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import Header from '../../components/Header/Header';
import './ArticleDetailPage.css';

const ArticlePage = () => {
  const { id } = useParams(); // Get the article ID from the URL

  // State to store article details and comments fetched from the backend
  const [article, setArticle] = useState(null); // To store article data (title, body, author, etc.)
  const [comments, setComments] = useState([]); // To store comments fetched from the backend
  const [newComment, setNewComment] = useState(''); // To handle new comment input
  const [loading, setLoading] = useState(true); // Loading state for API call

  // State to track if the article has been liked
  const [liked, setLiked] = useState(false); // By default, the article is not liked

  // Fetch the article and comments when the page loads
  useEffect(() => {
    // TODO: Replace with actual API call when backend is ready
    const fetchArticleDetails = async () => {
      try {
        setLoading(true); // Set loading to true while data is being fetched
        
        // Simulate a backend call to get the article details
        const articleData = {
          id: id,
          title: `Article ${id}`,
          author: 'name of user',
          publishedDate: 'M dd, yyyy',
          tags: ['tag1', 'tag2'],
          body: `The Article body of Article ${id}.`
        };
        
        // Simulate a backend call to get comments
        const commentData = [
          { user: 'user 2', date: 'dd month, yyyy', text: 'comment' },
          { user: 'user 3', date: 'dd month, yyyy', text: 'comment' }
        ];

        // Update state with the fetched data
        setArticle(articleData);
        setComments(commentData);
        setLoading(false); // Data has been fetched, stop the loading state
      } catch (error) {
        console.error('Error fetching article details:', error);
        setLoading(false); // Stop loading on error
      }
    };

    fetchArticleDetails();
  }, [id]); // Run this effect when the component mounts or when the article ID changes

  // Handle new comment submission
  const handleCommentSubmit = (e) => {
    e.preventDefault();
    if (newComment.trim() !== '') {
      // TODO: Send this comment to the backend when the backend is ready
      setComments([...comments, { user: 'new user', date: 'dd month, yyyy', text: newComment }]);
      setNewComment(''); // Clear the comment box after submission
    }
  };

  // Handle "Like" button click
  const handleLikeClick = () => {
    setLiked(!liked); // Toggle the liked state when button is clicked
  };

  // Show loading state if still fetching data
  if (loading) {
    return <div>Loading...</div>; // Show loading message while data is being fetched
  }

  return (
    <div>
      <Header />

      {/* Article Section */}
      <div className="article">
        {article && (
          <>
            <h1>{article.title}</h1> {/* Display article title */}
            <p>by <strong>{article.author}</strong></p> {/* Display article author */}
            <p>Published on {article.publishedDate}</p> {/* Display published date */}

            {/* Tags */}
            <div className="mb-3">
              {article.tags.map((tag, index) => (
                <span key={index} className="badge bg-secondary me-2">{tag}</span>
              ))}
            </div>

            {/* Article Body */}
            <div className="article-body mb-4">
              {article.body}
            </div>

            {/* Specific Like Button */}
            <button 
              className={`btn-like mb-4 ${liked ? 'liked' : ''}`} 
              onClick={handleLikeClick}
            >
              {liked ? 'Liked' : 'Like'}
            </button> {/* Like button updates based on state */}

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
          </>
        )}
      </div>
    </div>
  );
};

export default ArticlePage;
