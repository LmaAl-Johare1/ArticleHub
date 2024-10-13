import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import Header from '../../components/Header/Header';
import './ArticleDetailPage.css';
import { likeArticle, unlikeArticle, createComment } from '../../services/reaction'; 

const ArticlePage = () => {
  const { id } = useParams(); 
  const navigate = useNavigate(); 
  const location = useLocation(); // Use useLocation to access state
  
  const [article, setArticle] = useState(null);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [loading, setLoading] = useState(true);
  const [liked, setLiked] = useState(false);

  // Access article data from location state
  const { title, author, created, tags, body } = location.state || {};

  // Fetch article details and comments
  useEffect(() => {
    const fetchArticleDetails = () => {
      const articleData = {
        id: id,
        title: title,
        author: author,
        publishedDate: created,
        tags: tags,
        body: body,
      };

      const commentData = [
        { user: 'John Doe', date: 'March 15, 2023', text: 'Great article!' },
        { user: 'Jane Smith', date: 'March 16, 2023', text: 'Very informative, thanks!' },
      ];

      setArticle(articleData);
      setComments(commentData);
      setLoading(false);
    };

    fetchArticleDetails();
  }, [id, title, author, created, tags, body]);


  // Handle Like and Unlike functionality
  const handleLikeClick = async () => {
    try {
      if (!liked) {
        await likeArticle(id); // Like the article
      } else {
        await unlikeArticle(id); // Unlike the article
      }
      setLiked(!liked); // Toggle the liked state
    } catch (error) {
      console.error('Error liking/unliking the article:', error);
    }
  };

  // Handle Comment submission
  const handleCommentSubmit = async (e) => {
    e.preventDefault();
    try {
      const newCommentData = await createComment(id, newComment); // Add the new comment
      setComments([...comments, newCommentData]); // Append the new comment to the list
      setNewComment(''); // Clear the comment input
    } catch (error) {
      console.error('Error submitting comment:', error);
    }
  };

  // Navigate to the author's profile page
  const handleAuthorClick = () => {
    navigate(`/authprofile/${article.author}`); // Navigate to the author's profile page
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <div>
      <Header />

      <div className="article container">
        {article && (
          <>
            <h1>{article.title}</h1>
            <p>
              by <strong onClick={handleAuthorClick} className="author-link">{article.author}</strong>
            </p>
            <p>Published on {article.publishedDate}</p>

            <div className="mb-3">
              {article.tags.map((tag, index) => (
                <span key={index} className="badge bg-secondary me-2">{tag}</span>
              ))}
            </div>

            <div className="article-body mb-4">
              <p>{article.body}</p>
            </div>

            {/* Like Button */}
            <button 
              className={`btn btn-like mb-4 ${liked ? 'liked' : ''}`} 
              onClick={handleLikeClick}
            >
              {liked ? 'Liked' : 'Like'}
            </button>

            {/* Comments Section */}
            <div className="comments-section">
              <h5>Comments</h5>
              <form onSubmit={handleCommentSubmit} className="mb-4">
                <textarea
                  className="form-control mb-2"
                  placeholder="Leave a comment"
                  value={newComment}
                  onChange={(e) => setNewComment(e.target.value)}
                  rows="2"
                ></textarea>
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
