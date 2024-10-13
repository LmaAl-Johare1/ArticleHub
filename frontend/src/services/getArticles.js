import axios from 'axios';

const baseUrl = 'http://localhost:50001/api';

export default async function getAllArticles(tag = 'All', offset = 1) {
  try {
      const token = localStorage.getItem('token');

      if (!token) {
          alert('Please log in to view articles.');
          return { articles: [], totalCount: 0 }; // Return an object
      }

      const params = { offset };

      if (tag !== 'All') params.tag = tag;
      
      const response = await axios.get(`${baseUrl}/articles`, {
          headers: {
              Authorization: `Bearer ${token}`,
              'Content-Type': 'application/json',
          },
          params,
      });

      console.log(`API Response:`, response.data);
      return response.data; // Return both articles and totalCount
  } catch (error) {
      console.error('Error fetching articles:', error.response ? error.response.data : error.message);
      return { articles: [], totalCount: 0 }; // Return an object
  }
}
