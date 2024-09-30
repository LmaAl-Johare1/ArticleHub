import AuthPage from './pages/AuthPage/AuthPage';
import ProfilePage from './pages/ProfilePage/ProfilePage';
export default App;
import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage/HomePage';
import ArticlePage from './pages/ArticleDetailPage/ArticleDetailsPage'

function App() {
  return (
    
    <Router>
    <Routes>
         <AuthePage/>
      {/* Route for HomePage */}
      <Route path="/" element={<HomePage />} />

      {/* Route for Featured Article Page */}
      <Route path="/article/featured" element={<ArticlePage />} />

      {/* Route for other articles */}
      <Route path="/article/:id" element={<ArticlePage />} />

    </Routes>
  </Router>
  );
}

export default App;
