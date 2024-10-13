import axios from 'axios';

const baseUrl = 'http://localhost:50001/api';

// Function to fetch articles by tag
export default async function tagClickOn(tag, page = 1) {
    try {
        const token = localStorage.getItem('token');
        if (!token) {
            alert('Please log in to view articles.');
            return { articles: [], totalCount: 0 };
        }

        const params = {
            offset: page,
        };

        if (tag && tag !== 'All') {
            params.tag = tag;
        }

        const response = await axios.get(`${baseUrl}/articles`, {
            headers: { Authorization: `Bearer ${token}` },
            params,
        });

        return response.data; // Return both articles and totalCount
    } catch (error) {
        console.error('Error fetching articles by tag:', error);
        return { articles: [], totalCount: 0 };
    } 
}
