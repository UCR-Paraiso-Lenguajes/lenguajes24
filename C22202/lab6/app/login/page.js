'use client';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Container, Form, Button, Card, FloatingLabel } from 'react-bootstrap';
import { useState, useEffect } from 'react';
import AddEditProduct from '../ui/addProduct';

export default function Page() {
    const [isClient, setIsClient] = useState(false);
    const [user, setUser] = useState('');
    const [password, setPassword] = useState('');
    const [showError, setShowError] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    
    const handleError = (error) => {
        setErrorMessage(error);
        setShowError(true);
    };

    useEffect(() => {
        setIsClient(true);
    }, []);

    const handleSubmit = async (event) => {
        event.preventDefault();

        const data = { user, password };
        console.log(data.user, data.password)

        try {
            const response = await fetch('https://localhost:7194/api/auth', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            if (!response.ok) {
                handleError(`${response.status} ${response.statusText}`)
            }

            const result = await response.json();
        } catch (error) {
            handleError('Error del servidor, por favor, intenta más tarde')
        }
    };

    if (!isClient) {
        return null;
    }

    return (
        <Container className="d-flex justify-content-center align-items-center vh-100 bg-light">
            <AddEditProduct show={showModal} onClose={handleCloseModal} onSave={handleSaveProduct} />
            <Card className="shadow-sm" style={{ width: '400px', minHeight: '400px', backgroundColor: '#f8f9fa' }}>
                <Card.Body className="d-flex flex-column justify-content-center">
                    <Form onSubmit={handleSubmit}>
                        <FloatingLabel controlId="floatingUser" label="User" className="mb-4">
                            <Form.Control
                                type="text"
                                placeholder="User"
                                value={user}
                                onChange={(e) => setUser(e.target.value)}
                                aria-describedby="userHelpBlock"
                            />
                        </FloatingLabel>
                        <FloatingLabel controlId="floatingPassword" label="Password" className="mb-4">
                            <Form.Control
                                type="password"
                                placeholder="Password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                aria-describedby="passwordHelpBlock"
                            />
                        </FloatingLabel>
                        <div className="d-flex justify-content-center">
                            <Button variant="primary" type="submit" style={{ width: '100px' }}>
                                Submit
                            </Button>
                        </div>
                    </Form>
                </Card.Body>
            </Card>
        </Container>
    );
}
