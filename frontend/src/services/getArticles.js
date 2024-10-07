import axios from 'axios';

const baseUrl = 'http://localhost:5000/api'; 

// Function to get articles by tag and/or page
export default async function getArticles(tag = 'All', page = 1) {
  const token = localStorage.getItem('token');
  try {
    
    const params = {};
    if (tag !== 'All') params.tag = tag; // If a specific tag is selected, filter articles by that tag
    params.page = page;

    const response = await axios.get(`${baseUrl}/articles?tag=${params.tag}&offset=${params.page}`,{
        headers: {
            'Authorization': `Bearer ${token}`, // Include the token in the Authorization header
          },
    });
    
    return response.data; // Assuming response contains { articles, totalPages }
  } catch (error) {
    console.error('Error fetching articles:', error);
    throw error;
  }
}
