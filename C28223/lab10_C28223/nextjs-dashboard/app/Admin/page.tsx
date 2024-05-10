"use client"
import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import "@/app/ui/styles.css";

const LoginAdmin = () => {
    const handleSubmit = (event:any) => {
        if(event==undefined){throw new Error('Error: es nulo o indefinido');}
        event.preventDefault();
        window.location.href = '/Admin/init';
    };
    return (
        <div className="log">
            <form onSubmit={handleSubmit}>
                <div className="form-row">
                    <h1>Ingreso al Sistema</h1>
                    <div className="form-group">
                        <label htmlFor="Usuario">Usuario:</label>
                        <input type="text" className="form-control" id="Usuario" placeholder="Ingrese su usuario" minLength={4} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="Contrasena">Contraseña</label>
                        <input type="password" className="form-control" id="Contrasena" placeholder="Ingrese su contraseña" minLength={8} required />
                    </div>
                    <button type="submit" className="btn btn-primary">
                        Login
                    </button>
                </div>
            </form>
        </div>
    );
};

export default LoginAdmin;
