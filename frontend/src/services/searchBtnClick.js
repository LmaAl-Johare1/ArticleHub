import axios from 'axios';

const baseUrl = 'http://localhost:5000/api';

// Function to perform a search
export default async function searchBtnClick(query) {
  try {
    const response = await axios.get(`${baseUrl}/articles/search`, {
      params: { q: query },
    });

    return response.data; // Return the search results
  } catch (error) {
    console.error('Error performing search:', error);
    throw error;
  }
}
