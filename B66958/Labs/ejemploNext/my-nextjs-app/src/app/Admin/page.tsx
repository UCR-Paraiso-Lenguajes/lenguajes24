'use client'
import "bootstrap/dist/css/bootstrap.min.css";
import { useEffect, useState } from "react";
import { Card, CardBody, Container, Form, Row, Alert } from "react-bootstrap";
import { checkTokenDate, decodeToken } from "../hooks/jwtHooks";

export default function LogIn() {

    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');

    let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV;

    const handleUsernameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setUsername(e.target.value);
    };

    const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (!username || !password) {
            setError('Ingrese un nombre de usuario y contraseña válidos');
            return;
        }
        await authenticateUser();
    }

    async function authenticateUser() {
        try {
            const response = await fetch(`${environmentUrl}/api/auth/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    name: username,
                    password: password,
                }),
            })

            if (!response.ok) setError('Inicio de sesión incorrecto')
            else {
                var tokenString = await response.json();
                var token = tokenString.token
                sessionStorage.removeItem("sessionToken");
                sessionStorage.removeItem("expiracyToken");

                sessionStorage.setItem("sessionToken", token);
                var decodedToken = decodeToken(token);
                var expiracyDate = decodedToken?.exp;
                sessionStorage.setItem("expiracyToken", String(expiracyDate));

                window.location.href = "/Admin/init";
            }
        } catch (error) {
            setError(String(error));
        } finally {
        }
    }

    return <>
        <Container>
            <Row>
                <div className="col-md-6 offset-md-3">
                    <h2 className="text-center text-dark mt-5">Autenticación de Administradores</h2>
                    <Card className="my-5">
                        <CardBody className="p-lg-4" style={{ backgroundColor: "#ebf2fa" }}>
                            <Form onSubmit={handleSubmit}>
                                <div className="d-flex w-100 justify-content-center">
                                    <img src="https://cdn.pixabay.com/photo/2016/03/31/19/56/avatar-1295397__340.png"
                                        className="img-fluid img-thumbnail rounded-circle my-3"
                                        style={{
                                            height: "200px",
                                            width: "200px",
                                            objectFit: "cover"
                                        }}
                                        width="200px" alt="profile" />
                                </div>
                                <div className="mb-3">
                                    <input type="text" className="form-control" value={username} onChange={handleUsernameChange}
                                        id="Username" aria-describedby="emailHelp"
                                        placeholder="Nombre de usuario ..." />
                                </div>
                                <div className="mb-3">
                                    <input type="password" className="form-control" value={password} onChange={handlePasswordChange}
                                        id="password" placeholder="Contraseña ..." />
                                </div>
                                <div className="text-center">
                                    <button
                                        className="btn px-5 mb-5 w-100"
                                        style={{ backgroundColor: "#0e1c36", color: "white" }}>
                                        Iniciar Sesión
                                    </button>
                                </div>
                            </Form>
                        </CardBody>
                    </Card>
                </div>
            </Row>
        </Container>
        {error ? <div style={{
            position: 'fixed',
            bottom: 20,
            right: 20,
            zIndex: 9999,
        }}>
            <Alert variant="danger" onClose={() => setError('')} dismissible>
                <Alert.Heading>Datos inválidos</Alert.Heading>
                <p>{error}</p>
            </Alert>
        </div> : ''}
    </>
}