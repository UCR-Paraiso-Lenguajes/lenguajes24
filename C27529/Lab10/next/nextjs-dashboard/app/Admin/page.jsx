"use client"
import React, { useState } from 'react';
import Modal from 'react-modal';


function Page() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);


  const handleSubmit = (e) => {
    if (username === 'usu' && password === '1234') {
      if (username != undefined && password != undefined) {
        e.preventDefault();
        setIsModalOpen(false);
        window.location.href = '/Admin/init'; //aqui se entra como admin        
      }
    } else {
      setError('Nombre de usuario o contraseña incorrectos');
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
