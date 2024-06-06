'use client'
import React, { useState, useEffect } from 'react';
import Modal from 'react-modal';
import { jwtDecode } from 'jwt-decode'; 

function Page() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);

  useEffect(() => {
    const token = sessionStorage.getItem('token');
    if (token) {
      const decodedToken = jwtDecode(token);
      const tokenExpiration = decodedToken.exp * 1000;  
      const timeout = tokenExpiration - Date.now(); 

      if (timeout < 0) {
        sessionStorage.removeItem('token');
        throw new Error('La sesión ha expirado, por favor inicie sesión nuevamente.');
        window.location.href = '/Admin';
      } else {
        setTimeout(() => {
          sessionStorage.removeItem('token');
          window.location.href = '/Admin';
        }, timeout);
      }
    }
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const loginData = {
      UserName: username,
      Password: password
    };

    try {
      const response = await fetch('https://localhost:7280/api/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData)
      });

      if (response.ok) {
        const data = await response.json();
        const token = data.token;
        
        const decodedToken = jwtDecode(token); 
        const roleKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

        if (decodedToken[roleKey] && decodedToken[roleKey] === 'Admin') {
          sessionStorage.setItem('token', data.token);
          window.location.href = '/Admin/init';
        } else {
          throw new Error('Solo los administradores pueden acceder a esta página');
        }

      } else {
        throw new Error('Nombre de usuario o contraseña incorrectos');
      }
    } catch (error) {
      throw new Error('Error al intentar iniciar sesión');
    }
  };

  
  return (
    <div>
      <button onClick={() => setIsModalOpen(true)}>Iniciar Sesión</button>
      <Modal isOpen={isModalOpen} onRequestClose={() => setIsModalOpen(false)}>
        <div className="login-container">
          {error && <p className="error-message">{error}</p>}
          <form onSubmit={handleSubmit}>
            <div>
              <label htmlFor="username">Usuario:</label>
              <input
                type="text"
                id="username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            <div>
              <label htmlFor="password">Contraseña:</label>
              <input
                type="password"
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>
            <button type="submit">Iniciar Sesión</button>
          </form>
        </div>
      </Modal>
    </div>
  );
}

export default Page;
