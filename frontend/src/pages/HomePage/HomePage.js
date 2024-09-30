import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from '../../components/Header/Header';
import SearchBar from '../../components/SearchBar/SearchBar';
import FeaturedArticle from '../../components/FeaturedArticle/FeaturedArtcle';
import TagList from '../../components/TagList/TagList';
import ArticleList from '../../components/ArticleList/ArticleList';
import Pagination from '../../components/Pagination/Pagination';
import ArticleModal from '../CreateArticlePage/CreateArticlePage(Popup)'; // Your modal component

function HomePage() {
  // Modal state and handlers
  const [isModalOpen, setIsModalOpen] = useState(false); // State to control modal visibility

  const handleOpenModal = () => {
    setIsModalOpen(true); // Open the modal
  };

  const handleCloseModal = () => {
    setIsModalOpen(false); // Close the modal
  };

  // Step 1: State to store the selected tag for filtering articles
  const [selectedTag, setSelectedTag] = useState('All');

  // Step 2: Function to handle tag selection
  const handleTagChange = (tag) => {
    setSelectedTag(tag); // Update the selected tag when a tag is clicked
  };

  return (
    <div>
      {/* Header with Logo, Profile, and Logout */}
      <Header />

      {/* Main Content Area */}
      <div className="container mt-4">
        {/* Search Bar */}
        <div className="row mb-4">
          <div className="col-12">
            <SearchBar />
          </div>
        </div>

        {/* Featured Article Section */}
        <div className="row mb-4">
          <div className="col-12">
            <FeaturedArticle />
          </div>
        </div>

        {/* Tag List and Create Article Button */}
        <div className="row mb-4">
          <div className="col-12 d-flex justify-content-between align-items-center">
            {/* Pass handleTagChange to TagList to handle tag click */}
            <TagList onTagClick={handleTagChange} onCreateClick={handleOpenModal} />
          </div>
        </div>

        {/* Article List */}
        <div className="row mb-4">
          <div className="col-12">
            {/* Pass selectedTag to ArticleList for filtering */}
            <ArticleList selectedTag={selectedTag} />
          </div>
        </div>

        {/* Pagination */}
        <div className="row mb-4">
          <div className="col-12 d-flex justify-content-center">
            <Pagination />
          </div>
        </div>

        {/* ArticleModal for Create Article */}
        <ArticleModal show={isModalOpen} handleClose={handleCloseModal} />
      </div>
    </div>
  );
}

export default HomePage;
