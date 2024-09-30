import React from 'react';
import { Container} from 'react-bootstrap';
import ArticlesList from './AriclesList';
import Footer from './Footer';
import Header from './Header';
import PageSelcetion from './PageSelection';
import UserInfo from './UserInfo';
import UserStats from './UserStats';

const ProfilePage = () => {
    return (
    <div>
    <Header/>
        <Container className="mt-5">

            <UserInfo/>
            <UserStats/>
            <ArticlesList/>
            <PageSelcetion/>

        </Container>

    <Footer/>
    </div>
    );
};

export default ProfilePage;
