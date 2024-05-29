"use client"
import React, { useState } from 'react';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css";
import '@/app/ui/styles/login.css';



const LoginForm = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [warningUser, setWarningUser] = useState(false);
  const [warningPassword, setWarningPassword] = useState(false);
  const [redirectToDashboard, setRedirectToDashboard] = useState(false);

  const handleSubmit = (event) => {
    if(!event || !event.target){
      throw new Error('Evento no válido.');
      return;
    }
    event.preventDefault();
    if (username.trim() === '') {
      setWarningUser(true);
      setTimeout(() => {
        setWarningUser(false);
      }, 2000);
      return;
    }
    if (password.trim() === '') {c
      setWarningPassword(true);
      setTimeout(() => {
        setWarningPassword(false);
      }, 2000);
      return;
    }
    if (username === 'admin' && password === 'admin') {
      setRedirectToDashboard(true);
    }
  };

  if (redirectToDashboard) {
    window.location.href = '/admin/init';
  }

  return (
    <div className="login">
      <h4>Login</h4>
      <form onSubmit={handleSubmit}>
        <div className="text_area">
          <input
            type="text"
            id="username"
            name="username"
            className="text_input"
            onChange={(e) => setUsername(e.target.value)}
          />
          {warningUser && <div className='alert'>Usuario Incorrecto, por favor ingrese su usuario.</div>}
        </div>
        <div className="text_area">
          <input
            type="password"
            id="password"
            name="password"
            className="text_input"
            onChange={(e) => setPassword(e.target.value)}
          />
          {warningPassword && <div className='alert'>Contraseña incorrecta, por favor ingrese su contraseña.</div>}
        </div>
        <input
          type="submit"
          value="LOGIN"
          className="btn"
        />
      </form>
    </div>
  );
};

export default LoginForm;
