import React, { useEffect, useState, useCallback } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Header from '../../components/Header/Header';
import SearchBar from '../../components/SearchBar/SearchBar';
import FeaturedArticle from '../../components/FeaturedArticle/FeaturedArtcle';
import TagList from '../../components/TagList/TagList';
import ArticleList from '../../components/ArticleList/ArticleList';
import Pagination from '../../components/Pagination/Pagination';
import ArticleModal from '../CreateArticlePage/CreateArticlePage(Popup)';
import tagClickOn from '../../services/tagClickOn';

function HomePage() {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedTag, setSelectedTag] = useState('All');
    const [articles, setArticles] = useState([]);
    const [loading, setLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const [articlesPerPage] = useState(6);
    const [totalPages, setTotalPages] = useState(1);

    const fetchArticles = useCallback(async (tag = 'All', page = 1) => {
        setLoading(true);
        const offset = page;

        console.log(`Fetching articles for tag: '${tag}' on page: ${page}`);
        
        try {
            const articlesData = await tagClickOn(tag, offset);
            console.log(`Fetched articles:`, articlesData);
            
            setArticles(articlesData.articles);
            setTotalPages(Math.ceil(articlesData.totalCount / articlesPerPage));
        } catch (error) {
            console.error('Error fetching articles:', error);
        } finally {
            setLoading(false);
        }
    }, [articlesPerPage]);

    // Fetch articles whenever the tag or current page changes
    useEffect(() => {
        fetchArticles(selectedTag, currentPage);
    }, [currentPage, selectedTag, fetchArticles]);

    // Handle tag change and reset page to 1
    const handleTagChange = async (tag) => {
        setSelectedTag(tag);
        setCurrentPage(1); // Reset to first page when tag changes
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <div>
            <Header />
            <div className="container mt-4">
                <SearchBar />
                <div className="row mb-4">
                    <div className="col-12">
                        <FeaturedArticle />
                    </div>
                </div>
                <div className="row mb-4">
                    <div className="col-12 d-flex justify-content-between align-items-center">
                        <TagList 
                            selectedTag={selectedTag} 
                            onTagClick={handleTagChange} 
                            onCreateClick={() => setIsModalOpen(true)} 
                        />
                    </div>
                </div>
                <div className="row mb-4">
                    <div className="col-12">
                        <ArticleList articles={articles} loading={loading} />
                    </div>
                </div>
                <div className="row mb-4">
                    <div className="col-12 d-flex justify-content-center">
                        <Pagination 
                            currentPage={currentPage}
                            onPageChange={setCurrentPage} 
                            totalPages={totalPages}
                        />
                    </div>
                </div>
                {isModalOpen && <ArticleModal show={isModalOpen} handleClose={() => setIsModalOpen(false)} />}
            </div>
        </div>
    );
}

export default HomePage;
