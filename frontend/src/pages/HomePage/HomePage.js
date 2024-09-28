import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from '../../components/Header/Header'
import SearchBar from '../../components/SearchBar/SearchBar';
import FeaturedArticle from '../../components/FeaturedArticle/FeaturedArtcle';
import TagList from '../../components/TagList/TagList';
import ArticleList from '../../components/ArticleList/ArticleList';
import Pagination from '../../components/Pagination/Pagination';
import ArticleModal from '../CreateArticlePage/CreateArticlePage(Popup)';

function HomePage() {
  
  return (
    <div className="HomePage">
    <Header />
    <SearchBar />
    <FeaturedArticle />
    <TagList />
    <ArticleList/>
    <Pagination />
    <ArticleModal />
  

  </div>

  );
}

export default HomePage;