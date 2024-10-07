import axios from 'axios';

const baseUrl = 'http://localhost:50001/api'; 

// Function to fetch articles by tag
export default async function tagClickOn(tag) {
  try {
    const token = localStorage.getItem('token');
    
    if (!token) {
      alert('Please log in to create an article.');
      return;
    }
    const response = await axios.get(`${baseUrl}/articles`, {
      params: { tag: tag === 'All' ? '' : tag }, 
      headers: {
        'Authorization': `Bearer ${token}`, 
        'Content-Type': 'multipart/form-data',
      },
    });

    // Log response structure to understand it
    console.log('Response data:', response.data);

    // Assuming articles are returned under `data.articles`
    if (response.data.articles) {
      return response.data.articles;
    }

    // If the structure is different, adjust accordingly
    return response.data; // Or however your API sends the data
  } catch (error) {
    console.error('Error fetching articles by tag:', error);
    throw error;
  }
}