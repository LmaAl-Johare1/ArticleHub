import axios from 'axios';

const baseUrl = 'http://localhost:5000/api'; 

export default function updateArticleClicked(articleId, updatedData) {
  const token = localStorage.getItem('token'); 
  if (!token) {
    alert('Please log in to update the article.');
    return;
  }

  const url = `${baseUrl}/articles/${articleId}`;

  axios.put(url, updatedData, {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'multipart/form-data',
    },
  })
  .then((response) => {
    console.log('Article updated successfully:', response.data);
    alert('Article updated successfully!');
  })
  .catch((error) => {
    console.error('Error updating article:', error.response?.data?.message || error.message);
    alert('Failed to update the article.');
  });
};
