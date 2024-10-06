import axios from 'axios';

const baseUrl = 'http://localhost:5000/api'; 

// Function to fetch articles by tag
export default async function tagClickOn(tag) {
  try {
    const response = await axios.get(`${baseUrl}/articles`, {
      params: { tag: tag === 'All' ? '' : tag }, // If 'All' is selected, fetch all articles
    });

    return response.data; 
  } catch (error) {
    console.error('Error fetching articles by tag:', error);
    throw error;
  }
}
