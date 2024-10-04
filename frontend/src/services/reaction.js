import axios from 'axios';

const baseUrl = 'http://your-backend-url'; // Replace with your backend base URL

// Like an article
export const likeArticle = async (slug) => {
  const token = localStorage.getItem('token'); // Get the token from localStorage for authorization

  try {
    const response = await axios.post(`${baseUrl}/api/articles/${slug}/like`, {}, {
      headers: {
        'Authorization': `Token ${token}`, // Include the token in the header
      },
    });

    if (response.status === 201) {
      console.log('Article liked successfully.');
      return response.data;
    }
  } catch (error) {
    console.error('Error liking the article:', error.response ? error.response.data : error.message);
    throw error;
  }
};

// Unlike an article
export const unlikeArticle = async (slug) => {
  const token = localStorage.getItem('token'); // Get the token from localStorage for authorization

  try {
    const response = await axios.delete(`${baseUrl}/api/articles/${slug}/like`, {
      headers: {
        'Authorization': `Token ${token}`, // Include the token in the header
      },
    });

    if (response.status === 200) {
      console.log('Article unliked successfully.');
      return response.data;
    }
  } catch (error) {
    console.error('Error unliking the article:', error.response ? error.response.data : error.message);
    throw error;
  }
};

// Create a new comment on an article
export const createComment = async (slug, commentBody) => {
  const token = localStorage.getItem('token'); // Get the token from localStorage for authorization

  const params = {
    body: commentBody,
  };

  try {
    const response = await axios.post(`${baseUrl}/articles/${slug}/comment`, params, {
      headers: {
        'Authorization': `Token ${token}`, // Include the token in the header
      },
    });

    if (response.status === 201) {
      console.log('Comment created successfully.');
      return response.data; // Return the comment data so it can be displayed directly
    }
  } catch (error) {
    console.error('Error creating comment:', error.response ? error.response.data : error.message);
    throw error;
  }
};
