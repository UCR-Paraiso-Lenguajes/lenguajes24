"use client"
import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';


const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const isValidInput = (input: string): boolean => {
    return /^[a-zA-Z0-9]{4,}$/.test(input);
  };

  const handleLogin = async () => {
    if (username.trim() === '') {
      setError('Por favor ingrese su nombre de usuario');
      return;
    }
    if (password.trim() === '') {
      setError('Por favor ingrese su contraseña');
      return;
    }
    if (!isValidInput(username)) {
      setError('El nombre de usuario debe tener al menos 8 caracteres');
      return;
    }
    if (!isValidInput(password)) {
      setError('La contraseña debe tener al menos 8 caracteres');
      return;
    }

    try {
      const response = await fetch('https://localhost:7165/api/UserAuth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ UserName: username, Password: password })
      });

      if (response.ok) {
        const data = await response.json();
        sessionStorage.setItem('loginToken', data.token);  
        setError('');
        window.location.href = '/admin/init';
      } else {
        const errorData = await response.json();
        setError(errorData.message || 'Error en la autenticación');
      }
    } catch (error) {
      setError('Error en la conexión al servidor');
    }
  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-6">
          <div className="card">
            <div className="card-header">Iniciar Sesión</div>
            <div className="card-body">
              {error && <div className="alert alert-danger" role="alert">{error}</div>}
              <form onSubmit={(e) => e.preventDefault()}>
                <div className="mb-3">
                  <label htmlFor="username" className="form-label">Usuario</label>
                  <input 
                    type="text" 
                    className="form-control" 
                    id="username" 
                    value={username} 
                    onChange={(e) => setUsername(e.target.value)} 
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="password" className="form-label">Contraseña</label>
                  <input 
                    type="password" 
                    className="form-control" 
                    id="password" 
                    value={password} 
                    onChange={(e) => setPassword(e.target.value)} 
                  />
                </div>
                <button type="button" className="btn btn-primary" onClick={handleLogin}>Iniciar Sesión</button>
              </form>
            </div>
            <div className="card-footer">
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;