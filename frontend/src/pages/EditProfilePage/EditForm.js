import React, { useState } from 'react';
import { Button, Container, Row, Col, Form, Card } from 'react-bootstrap';
import styles from './EditForm.module.css';
import saveBtnClicked from '../../services/editprofileService';

function EditForm(){
    // State for form fields
    const [formData, setFormData] = useState({
        first_Name: '',
        last_Name: '',
        user_Name: '',
        email: '',
        bio: ''
    });

    // Handle form input changes
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    // Handle form submission
    const handleSubmit = (e) => {
        e.preventDefault();
        saveBtnClicked(formData)
    };

    // Handle form reset
    const handleReset = () => {
        setFormData({
            first_Name: '',
            last_Name: '',
            user_Name: '',
            email: '',
            bio: ''
        });
    };

    return (
        <Container className={styles.EditForm}>
            

            {/* Edit Profile Form */}
            <Row className="justify-content-center" >
                <Col md={6}>
                    <Card className="p-4 shadow-sm" style={{backgroundColor:"#f7f7f6" , border: "5 solid 2c3b4c"}}>
                        <Form onSubmit={handleSubmit}>
                            <Row>
                                <Col md={6}>
                                    <Form.Group className="mb-3" controlId="formFirstName">
                                        <Form.Control 
                                            type="text" 
                                            name="first_Name"
                                            value={formData.first_Name}
                                            onChange={handleChange}
                                            placeholder="Edit First Name" 
                                        />
                                    </Form.Group>
                                </Col>
                                <Col md={6}>
                                    <Form.Group className="mb-3" controlId="formLastName">
                                        <Form.Control 
                                            type="text" 
                                            name="last_Name"
                                            value={formData.last_Name}
                                            onChange={handleChange}
                                            placeholder="Edit Last Name" 
                                        />
                                    </Form.Group>
                                </Col>
                            </Row>
                            <Form.Group className="mb-3" controlId="formUserName">
                                <Form.Control 
                                    type="text" 
                                    name="user_Name"
                                    value={formData.user_Name}
                                    onChange={handleChange}
                                    placeholder="Edit User Name" 
                                />
                            </Form.Group>
                            <Form.Group className="mb-3" controlId="formEmail">
                                <Form.Control 
                                    type="email" 
                                    name="email"
                                    value={formData.email}
                                    onChange={handleChange}
                                    placeholder="Edit Email" 
                                />
                            </Form.Group>
                            <Form.Group className="mb-3" controlId="formBio">
                                <Form.Control 
                                    as="textarea" 
                                    name="bio"
                                    rows={3} 
                                    value={formData.bio}
                                    onChange={handleChange}
                                    placeholder="Write your bio here..." 
                                />
                            </Form.Group>

                            <div className="d-flex justify-content-between">
                                <Button variant="primary" type="submit">Save</Button>
                                <Button variant="secondary" onClick={handleReset}>Cancel</Button>
                            </div>
                        </Form>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
};

export default  EditForm;