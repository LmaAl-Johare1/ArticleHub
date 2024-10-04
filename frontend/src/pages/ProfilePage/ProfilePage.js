import React from 'react';
import { Container} from 'react-bootstrap';
import ArticlesList from './AriclesList';
import Header from '../../components/Header/Header';
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
    </div>
    );
};

export default ProfilePage;
