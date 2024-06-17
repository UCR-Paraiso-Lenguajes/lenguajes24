"use client";
import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import "@/app/ui/styles.css";
import {jwtDecode} from 'jwt-decode';

const Modal = ({ title, content, onClose, closeButtonText = 'Cerrar', showCloseButton = true }) => {
    if (!title || !content || !onClose || typeof onClose !== 'function') {
        throw new Error('Error: Los argumentos title, content y onClose son obligatorios y onClose debe ser una función.');
    }
    return (
        <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
            <div className="modal-dialog" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{title}</h5>
                        {showCloseButton && (
                            <button type="button" className="close" onClick={onClose} aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        )}
                    </div>
                    <div className="modal-body">
                        <p>{content}</p>
                    </div>
                    {showCloseButton && (
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" onClick={onClose}>
                                {closeButtonText}
                            </button>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

const LoginAdmin = () => {
    const [usuario, setUsuario] = useState('');
    const [contrasena, setContrasena] = useState('');
    const [error, setError] = useState('');
    const [showModal, setShowModal] = useState(false);
    const URLConection = process.env.NEXT_PUBLIC_API;
  
    const handleCloseModal = () => {
        setShowModal(false);
        setError('');
    };

    const handleSubmit = async (event) => {
        if (event == undefined) {
            throw new Error('Los parametros de inicio de sesión, no pueden ser indefinidos.');
        }
        event.preventDefault();
        const loginData = {
            userLog: usuario,
            passwordLog: contrasena
        };
        try {
            const response = await fetch(URLConection + '/api/auth', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(loginData)
            });
            if (response.ok) {
                const data = await response.json();
                sessionStorage.setItem('token', data.token);
                const decodedToken = jwtDecode(data.token);
                const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                if (userRole === "Admin") {
                    window.location.href = '/Admin/init';
                } else {
                    setError('No tienes permisos de administrador.');
                    setShowModal(true);
                }
            } else {
                const errorData = await response.json();
                setError(errorData.message || 'Error de autenticación');
                setShowModal(true);
            }
        } catch (error) {
            setError('Error en la solicitud. Por favor, intente de nuevo más tarde.');
            setShowModal(true);
        }
    };

    return (
        <div className="log">
            <form onSubmit={handleSubmit}>
                <div className="form-row">
                    <h1>Ingreso al Sistema</h1>
                    <div className="form-group">
                        <label htmlFor="Usuario">Usuario:</label>
                        <input type="text" className="form-control" id="Usuario" placeholder="Ingrese su usuario"
                            minLength={4} value={usuario} onChange={(e) => setUsuario(e.target.value)} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Contrasena">Contraseña</label>
                        <input type="password" className="form-control" id="Contrasena" placeholder="Ingrese su contraseña"
                            minLength={5} value={contrasena} onChange={(e) => setContrasena(e.target.value)} required />
                    </div>
                    <button type="submit" className="btn btn-primary">Login</button>
                </div>
            </form>
            {showModal && (
                <Modal
                    title="Error de Autenticación"
                    content={error}
                    onClose={handleCloseModal}
                />
            )}
        </div>
    );
};

export default LoginAdmin;
