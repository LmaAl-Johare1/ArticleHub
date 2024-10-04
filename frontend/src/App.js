import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import HomePage from './pages/HomePage/HomePage';
import ArticlePage from './pages/ArticleDetailPage/ArticleDetailsPage';
import ProfilePage from './pages/ProfilePage/ProfilePage';
import EditProfilePage from './pages/EditProfilePage/EditProfilePage'; 
import AuthPage from './pages/AuthPage/AuthPage';
import AuthProfile from './pages/AuthProfilePage/AuthProfilePage';
import Footer from './components/Footer/Footer';

function App() {
  return (
    <Router>
      <Routes>
        {/* Auth Route */}
        <Route path="/" element={<AuthPage />} />

        {/* Home Route */}
        <Route path="/homepage" element={<HomePage />} /> 

        {/* Profile Route */}
        <Route path="/profile" element={<ProfilePage />} />

        {/* Edit Profile Route */}
        <Route path="/profile/edit" element={<EditProfilePage />} />

        {/* Route for Featured Article Page */}
        <Route path="/article/featured" element={<ArticlePage />} />

        {/* Route for other articles */}
        <Route path="/article/:id" element={<ArticlePage />} />

        {/* Auth Profile Page */}
        <Route path="/authprofile/:author" element={<AuthProfile />} /> {/* New route for author's profile */}

        {/* Add more routes as necessary */}
      </Routes>
      <Footer />
    </Router>
  );
}

export default App;
