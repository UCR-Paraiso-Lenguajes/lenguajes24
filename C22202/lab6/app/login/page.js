'use client';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Container, Form, Button, Card, FloatingLabel } from 'react-bootstrap';
import { useState, useEffect } from 'react';

export default function Page() {
    const [isClient, setIsClient] = useState(false);
    const [user, setUser] = useState('');
    const [password, setPassword] = useState('');

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
                throw new Error('Network response was not ok');
            }

            const result = await response.json();
            console.log(result);
            // Handle the response data as needed
        } catch (error) {
            console.error('Error:', error);
            // Handle the error as needed
        }
    };

    if (!isClient) {
        return null; // Render nothing on the server-side
    }

    return (
        <Container className="d-flex justify-content-center align-items-center vh-100 bg-light">
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
