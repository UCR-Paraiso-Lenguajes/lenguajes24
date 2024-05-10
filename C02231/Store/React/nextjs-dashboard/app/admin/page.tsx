'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

export default function LoginPage() {

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [isFormValid, setIsFormValid] = useState(false);


    const handleLogin = async () => {
        if (email.trim() === '' || password.trim() === '') {
            setError('Please, enter a valid email and password.');
            return;
        }

    };

    const updateFormValidity = () => {
        setIsFormValid(email.trim() !== '' && password.trim() !== '');
    };


    useEffect(() => {
        updateFormValidity();
    }, [email, password]);


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
                            <div className="card-body">
                                <h5 className="card-title text-center">Iniciar Sesión</h5>
                                <form>
                                    <div>
                                        <div className="mb-3">
                                            <label htmlFor="emailInputField" className="form-label">Email address</label>
                                            <input type="email" className="form-control" id="emailInputField" aria-describedby="emailHelp" value={email} onChange={(e) => setEmail(e.target.value)} />
                                            <div id="emailHelp" className="form-text">We'll never share your email with anyone else.</div>
                                        </div>
                                        <div className="mb-3">
                                            <label htmlFor="passwordInputField" className="form-label">Password</label>
                                            <input type="password" className="form-control" id="passwordInputField" value={password} onChange={(e) => setPassword(e.target.value)} />
                                        </div>
                                        {error && <div className="alert alert-danger" role="alert">{error}</div>}
                                        <div className="d-grid gap-2" >
                                            {isFormValid ? (
                                                <Link href="/admin/init">
                                                    <button
                                                        type="button"
                                                        className="btn btn-success"
                                                        onClick={handleLogin}
                                                        style={{ width: '100%' }}>
                                                        Iniciar sesión
                                                    </button>
                                                </Link>
                                            ) : (
                                                <button
                                                    type="button"
                                                    className="btn btn-success"
                                                    onClick={handleLogin}
                                                    style={{ width: '100%' }}>
                                                    Iniciar sesión
                                                </button>
                                            )}

                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <footer className='footer' style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light"> Paula's Library</h5>
                </div>
            </footer>


        </div >
    );
}
