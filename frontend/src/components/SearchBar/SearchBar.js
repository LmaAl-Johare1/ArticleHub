import React, { useState } from 'react';
import './SearchBar.css';
import searchBtnClick from '../../services/searchBtnClick';

const SearchBar = ({ onSearchResults }) => {
  const [searchQuery, setSearchQuery] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const searchResults = await searchBtnClick(searchQuery); // Call the search function
      onSearchResults(searchResults); // Send results to the parent component
    } catch (error) {
      console.error('Error during search:', error);
      alert('Failed to perform the search. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="search-bar-container">
      <input
        type="text"
        className="search-input"
        placeholder="Search by author, tag, or article title..."
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />
      <button className="search-button" onClick={handleSearch} disabled={loading}>
        {loading ? 'Searching...' : 'Search'}
      </button>
    </div>
  );
};

export default SearchBar;
