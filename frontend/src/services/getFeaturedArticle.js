import axios from 'axios';

const baseUrl = 'http://localhost:50001/api'; 

// Function to get the featured article based on most likes
export default async function getFeaturedArticle() {
  try {
    const token = localStorage.getItem('token');
    
    if (!token) {
      alert('Please log in to create an article.');
      return;
    }

    // Fetch all articles from the backend
    const response = await axios.get(`${baseUrl}/articles`, {
      headers: {
        'Authorization': `Bearer ${token}`, 
        'Content-Type': 'multipart/form-data',
      },
    });

    if (response.status === 200) {
      const articles = response.data.articles; // Assuming the response contains an array of articles

      // Safeguard: Ensure that `articles` exists and is an array
      if (!Array.isArray(articles) || articles.length === 0) {
        throw new Error('No articles found or invalid articles data');
      }

      // Find the article with the most likes
      const featuredArticle = articles.reduce((max, article) => 
        (article.likes > max.likes ? article : max), articles[0]);

      return featuredArticle; // Return the article with the most likes
    } else {
      throw new Error('Failed to fetch articles');
    }
  } catch (error) {
    console.error('Error fetching featured article:', error);
    throw error;
  }
}
