import axios from 'axios';

const baseUrl = 'http://localhost:50001/api'; 

// Function to get the featured article based on most likes
export default async function getFeaturedArticle() {
  try {
    const token = localStorage.getItem('token');

    if (!token) {
      alert('Please log in to view articles.');
      return null;
    }

    const response = await axios.get(`${baseUrl}/articles`, {
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json',
      },
    });

    const articles = response.data.articles || response.data; // Adjust according to your API response

    if (!Array.isArray(articles) || articles.length === 0) {
      throw new Error('No articles found or invalid articles data');
    }

    const featuredArticle = articles.reduce((max, article) => 
      (article.likes > max.likes ? article : max), articles[0]);

    return featuredArticle;
  } catch (error) {
    console.error('Error fetching featured article:', error);
    return null; // Return null in case of an error
  }
}
