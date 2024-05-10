'use client'
import React, { useState } from 'react';
import '../ui/globals.css';
import 'bootstrap/dist/css/bootstrap.css';
import {useRouter} from 'next/router';

const Admin: React.FC = () => {
  const router = useRouter();
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

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const { username, password } = formData;

    const validationResult = validateForm(username, password);

    if (validationResult.isValid) {
      setFormData({
        ...formData,
        errorMessage: ''
      });
        router.push('/admin/init'); 

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
        errorMessage: 'Por favor, complete todos los campos.'
      };
    }
    return {
      isValid: true,
      errorMessage: ''
    };
  };

  return (
    <div>
      <h1>Iniciar Sesión</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label className="col-form-label" htmlFor="username">
            Nombre de Usuario:
          </label>
          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleInputChange}
          />
        </div>
        <div>
          <label className="col-form-label" htmlFor="password">
            Contraseña:
          </label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleInputChange}
          />
        </div>
        <div className="center-button">
          <button className="btn btn-primary my-4" type="submit">
            Iniciar Sesión
          </button>
        </div>
      </form>
      {formData.errorMessage && (
        <p style={{ color: 'red' }}>{formData.errorMessage}</p>
      )}
    </div>
  );
};

export default Admin;
