import axios from 'axios';

const baseUrl = 'http://localhost:5000/api'; 

// Function to get the featured article based on most likes
export default async function getFeaturedArticle() {
  try {
    // Fetch all articles from the backend
    const response = await axios.get(`${baseUrl}/articles`);
    
    if (response.status === 200) {
      const articles = response.data.articles; // Assuming the response contains an array of articles
      if (articles.length === 0) {
        throw new Error('No articles found');
      }

      // Find the article with the most likes
      const featuredArticle = articles.reduce((max, article) => (article.likes > max.likes ? article : max), articles[0]);
      
      return featuredArticle; // Return the article with the most likes
    } else {
      throw new Error('Failed to fetch articles');
    }
  } catch (error) {
    console.error('Error fetching featured article:', error);
    throw error;
  }
}
