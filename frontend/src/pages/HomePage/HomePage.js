import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from '../../components/Header/Header';
import SearchBar from '../../components/SearchBar/SearchBar';
import FeaturedArticle from '../../components/FeaturedArticle/FeaturedArtcle';
import TagList from '../../components/TagList/TagList';
import ArticleList from '../../components/ArticleList/ArticleList';
import Pagination from '../../components/Pagination/Pagination';
import ArticleModal from '../CreateArticlePage/CreateArticlePage(Popup)';
import { useNavigate } from 'react-router-dom';


function HomePage() {
  // Modal state and handlers
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleOpenModal = () => setIsModalOpen(true);
  const handleCloseModal = () => setIsModalOpen(false);

  // State to store the selected tag for filtering articles
  const [selectedTag, setSelectedTag] = useState('All');

   // State to store search results
   const [searchResults, setSearchResults] = useState([]);

   // State to store articles (if fetched without search)
  const [articles, setArticles] = useState([]); // The default articles list

  // Function to handle tag selection
  const handleTagChange = (tag) => {
    setSelectedTag(tag);
    setSearchResults([]); // Clear search results when a tag is selected
  };

  
  // Determine the articles to be shown (either search results or based on selected tag)
  const displayedArticles = searchResults.length > 0 ? searchResults : articles;

  return (
    <div>
      <Header />

      <div className="container mt-4">
      <div>
      <SearchBar onSearchResults={(results) => setSearchResults(results)} />
      <ArticleList articles={searchResults} />
    </div>

        <div className="row mb-4">
          <div className="col-12">
            <FeaturedArticle />
          </div>
        </div>

        <div className="row mb-4">
          <div className="col-12 d-flex justify-content-between align-items-center">
            <TagList onTagClick={handleTagChange} onCreateClick={handleOpenModal} />
          </div>
        </div>

        <div className="row mb-4">
          <div className="col-12">
            <ArticleList articles={displayedArticles} />
          </div>
        </div>

        <div className="row mb-4">
          <div className="col-12 d-flex justify-content-center">
            <Pagination />
          </div>
        </div>

        {/* Only show modal when isModalOpen is true */}
        {isModalOpen && <ArticleModal show={isModalOpen} handleClose={handleCloseModal} />}
        
      
        
      </div>
    </div>
    
  );
}

export default HomePage;
