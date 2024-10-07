import axios from 'axios';

const baseUrl = 'http://localhost:50001/api'; 

// Function to get articles by tag, page, and keyword
export default async function getArticles(tag = 'All', page = 1, keyword = '') {
  const token = localStorage.getItem('token');
  try {
    // Set up query parameters dynamically based on the inputs
    const params = {
      offset: page,  // Assuming 'offset' is used for pagination, otherwise adjust as needed
    };
    
    if (tag !== 'All') {
      params.tag = tag;  // Only add tag if it's not the default "All"
    }
    
    if (keyword) {
      params.keyword = keyword;  // Add keyword only if provided
    }

    // Perform the API request with parameters and token
    const response = await axios.get(`${baseUrl}/articles`, {
      params,  // Automatically converts the params object into query parameters
      headers: {
        'Authorization': `Bearer ${token}`,  // Include the token in the Authorization header
      },
    });

    return response.data;  // Assuming response contains { articles, totalPages }
  } catch (error) {
    console.error('Error fetching articles:', error);
    throw error;
  }
}
