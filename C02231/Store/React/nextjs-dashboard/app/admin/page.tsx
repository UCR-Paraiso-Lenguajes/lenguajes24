'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';
import router, { useRouter } from 'next/router';

export default function LoginPage() {
    const URL = process.env.NEXT_PUBLIC_NODE_ENV;
    //const URL = "http://localhost:5207";
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        errorMessage: ''
    });

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const { username, password } = formData;

        const validationResult = validateForm(username, password);

        if (validationResult.isValid) {
            setFormData({
                ...formData,
                errorMessage: ''
            });

            try {
                const response = await fetch(URL + '/api/Auth/login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ userName: username, userPassword: password })
                });

                if (response.ok) {
                    const data = await response.json();
                    // Guarda el token en sessionStorage
                    sessionStorage.setItem('authToken', data.token);
                    console.log('Login successful:', data);
                    // Redirigir al usuario a la página de administración
                    window.location.href = '/admin/init';  //ver si funciona con el router
                    //router.push('/admin/init');
                } else {
                    const errorData = await response.json();
                    setFormData({
                        ...formData,
                        errorMessage: errorData.message || 'Invalid password or user data'
                    });
                }
            } catch (error) {
                setFormData({
                    ...formData,
                    errorMessage: 'Error en la conexión con el servidor'
                });
            }
        } else {
            setFormData({
                ...formData,
                errorMessage: validationResult.errorMessage
            });
        }
    };

    const validateForm = (username: string, password: string) => {
        if (!username.trim() || !password.trim()) {
            return {
                isValid: false,
                errorMessage: 'Please, fill all fields.'
            };
        }
        return {
            isValid: true,
            errorMessage: ''
        };
    };

    return (
        <div >
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-9">
                        <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" alt="Company Logo" />
                    </div>
                    <div className="col-sm-3 d-flex justify-content-end align-items-center">
                        <Link href="/">
                            <button className="btn btn-dark"> Go Home</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div className="container-fluid">
                <div className="row justify-content-center align-items-center" style={{ height: '80vh' }}>
                    <div className="col-sm-4">
                        <div className="card">
                            <h5 className="card-title text-center">Iniciar Sesión</h5>
                            <form onSubmit={handleSubmit}>
                                <div>
                                    <div className="mb-3">
                                        <label htmlFor="username" className="form-label">User name</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            id="username"
                                            name="username"
                                            value={formData.username}
                                            onChange={handleInputChange}
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label htmlFor="password" className="form-label">Password</label>
                                        <input
                                            type="password"
                                            className="form-control"
                                            id="password"
                                            name="password"
                                            value={formData.password}
                                            onChange={handleInputChange} />
                                    </div>
                                    <div className="center-button">
                                        <button className="btn btn-succesfuly my-4" type="submit">
                                            Iniciar Sesión
                                        </button>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
                {formData.errorMessage && (
                    <p style={{ color: 'red' }}>{formData.errorMessage}</p>
                )}
            </div >

            <footer className='footer' style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light"> Paula's Library</h5>
                </div>
            </footer>


        </div >
    );
}
