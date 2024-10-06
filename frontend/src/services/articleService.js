// src/services/articleService.js
import axios from 'axios';

const baseUrl = 'http://localhost:5000/api'; // Replace with your actual backend URL

export default function createArticleClicked(formData) {
  // Get the token from localStorage
  const token = localStorage.getItem('token');
  if (!token) {
    alert('Please log in to create an article.');
    return;
  }

  // API endpoint
  const url = `${baseUrl}/article`;

  // Make the POST request to create the article
  axios.post(url, formData, {
    headers: {
      'Authorization': `Bearer ${token}`, // Include the token in the header
      'Content-Type': 'multipart/form-data',
    },
  })
  .then((response) => {
    console.log('Article created successfully:', response.data);
    alert('Article created successfully!');
  })
  .catch((error) => {
    console.error('Error creating article:', error.response?.data?.message || error.message);
    alert('Failed to create the article.');
  });
};


