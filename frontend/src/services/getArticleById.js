import axios from 'axios';

const baseUrl = 'http://localhost:5000/api'; 

export default async function getArticleById(articleId) {
  try {
    const token = localStorage.getItem('token'); 
    if (!token) {
      throw new Error('User is not authenticated');
    }

    const response = await axios.get(`${baseUrl}/articles/${articleId}`, {
      headers: {
        'Authorization': `Bearer ${token}`, // Include the token in the Authorization header
      },
    });

    return response.data; // Return the article data
  } catch (error) {
    console.error('Error fetching article details:', error);
    throw error;
  }
}
