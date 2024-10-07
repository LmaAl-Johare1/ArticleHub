import axios from 'axios';

const baseUrl = 'http://localhost:50001/api'; 

export default async function submitClicked(formData) {
  try {
 
    const token = localStorage.getItem('token');
    
    if (!token) {
      alert('Please log in to create an article.');
      return;
    }

  
    const url = `${baseUrl}/articles`;

    // Make the POST request to create the article
    const response = await axios.post(url, formData, {
      headers: {
        'Authorization': `Bearer ${token}`, 
        'Content-Type': 'multipart/form-data',
      },
    });

    console.log('Article created successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error creating article:', error.response?.data?.message || error.message);
    throw new Error('Failed to create the article.');
  }
}
