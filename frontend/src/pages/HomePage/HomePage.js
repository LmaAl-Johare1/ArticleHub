import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from '../../components/Header/Header';
import SearchBar from '../../components/SearchBar/SearchBar';
import FeaturedArticle from '../../components/FeaturedArticle/FeaturedArtcle';
import TagList from '../../components/TagList/TagList';
import ArticleList from '../../components/ArticleList/ArticleList';
import Pagination from '../../components/Pagination/Pagination';
import ArticleModal from '../CreateArticlePage/CreateArticlePage(Popup)';
import Footer from '../../components/Footer/Footer';

function HomePage() {
  // Modal state and handlers
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleOpenModal = () => setIsModalOpen(true);
  const handleCloseModal = () => setIsModalOpen(false);

  // State to store the selected tag for filtering articles
  const [selectedTag, setSelectedTag] = useState('All');

  // Function to handle tag selection
  const handleTagChange = (tag) => setSelectedTag(tag);

  return (
    <div>
      <Header />

      <div className="container mt-4">
        <div className="row mb-4">
          <div className="col-12">
            <SearchBar />
          </div>
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
            <ArticleList selectedTag={selectedTag} />
          </div>
        </div>

        <div className="row mb-4">
          <div className="col-12 d-flex justify-content-center">
            <Pagination />
          </div>
        </div>

        <ArticleModal show={isModalOpen} handleClose={handleCloseModal} />
      </div>
    </div>
    
  );
}

export default HomePage;
