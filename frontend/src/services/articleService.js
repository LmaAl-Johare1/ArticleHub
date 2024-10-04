import axios from 'axios';

// Define your base URL for the API
const baseUrl = 'http://localhost:50001/api'; // Replace with your actual API URL

// Function to create an article
export const createArticle = async (title, body, image, token) => {
  try {
    const formData = new FormData();
    formData.append('title', title);
    formData.append('body', body);
    formData.append('image', image);

    const response = await axios.post(`${baseUrl}/article`, formData, {
      headers: {
        //'Authorization': `Token ${token}`, // Send the token in the Authorization header
        'Authorization': `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImxtYWpvaGFyZSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImxtYS5hcmVmLmpvQGdtYWlsLmNvbSIsImV4cCI6MTcyODA3NjcxNSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwLyJ9.j5Kpo7JH_Xp39pKA61gltWHPczdzfL6lkCUcjsgNSJs`, 
        'Content-Type': 'multipart/form-data',
      },
    });

    return response.data; // Returns {msg: "Article created successfully"}
  } catch (error) {
    console.error('Error creating article:', error);
    throw error;
  }
};

// Function to get an article by ID
export const getArticle = async (articleId, token) => {
  try {
    const response = await axios.get(`${baseUrl}/articles/${articleId}`, {
      headers: {
        'Authorization': `Token ${token}`,
      },
    });

    return response.data; // Returns the article details including comments and like count
  } catch (error) {
    console.error('Error fetching article:', error);
    throw error;
  }
};

// Function to update an article by ID
export const updateArticle = async (articleId, title, body, image, token) => {
  try {
    const formData = new FormData();
    formData.append('title', title);
    formData.append('body', body);
    formData.append('image', image);

    const response = await axios.put(`${baseUrl}/articles/${articleId}`, formData, {
      headers: {
        'Authorization': `Token ${token}`,
        'Content-Type': 'multipart/form-data',
      },
    });

    return response.data; // Returns {msg: "The article updated successfully"}
  } catch (error) {
    console.error('Error updating article:', error);
    throw error;
  }
};
