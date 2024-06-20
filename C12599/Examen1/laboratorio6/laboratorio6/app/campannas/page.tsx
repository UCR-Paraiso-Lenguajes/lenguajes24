'use client';
import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/globals.css';
const URL = process.env.NEXT_PUBLIC_API;

interface Campanna {
  id: number;
  contenidoHtml: string;
  createdAt: string;
}

const Campannas: React.FC = () => {
  const [htmlContent, setHtmlContent] = useState<string>('');
  const [errorMessage, setErrorMessage] = useState<string>('');
  const [campannas, setCampannas] = useState<Campanna[]>([]);
  const maxChars = 5000;

  const fetchCampannas = async () => {
    const token = sessionStorage.getItem('authToken');
    const response = await fetch(URL+'/api/Campannas', {
      method: 'GET',
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    if (response.ok) {
      const data = await response.json();
      setCampannas(data);
    } else {
      setErrorMessage('Error al obtener las campañas.');
    }
  };

  useEffect(() => {
    fetchCampannas();
  }, []);

  const handleContentChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    const value = e.target.value;
    if (value.length <= maxChars) {
      setHtmlContent(value);
      setErrorMessage(''); // Clear error message if within limit
    } else {
      setErrorMessage(`Se ha excedido el límite de caracteres de ${maxChars}.`);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (htmlContent.length > maxChars) {
      setErrorMessage(`Se ha excedido el límite de caracteres de ${maxChars}.`);
      return;
    }

    const token = sessionStorage.getItem('authToken');
    const response = await fetch(URL+'/api/Campannas', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({ contenidoHtml: htmlContent }), // Make sure the property name matches the model
    });

    if (response.ok) {
      setHtmlContent(''); // Clear the text area
      await fetchCampannas(); // Refresh the list after submission
    } else {
      setErrorMessage('Error al enviar el mensaje.');
    }
  };

  return (
    <div>
      <h1>Servicio de campañas</h1>
      {errorMessage && <div className="text-danger mb-3">{errorMessage}</div>}
      <h2>Campañas</h2>
      <table className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Contenido HTML</th>
            <th>Fecha de creación</th>
          </tr>
        </thead>
        <tbody>
          {campannas.map((campanna) => (
            <tr key={campanna.id}>
              <td>{campanna.id}</td>
              <td>{campanna.contenidoHtml}</td>
              <td>{new Date(campanna.createdAt).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <h2>Crear nueva campaña</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group mb-3">
          <label htmlFor="exampleFormControlTextarea1">Contenido HTML</label>
          <textarea
            className="form-control"
            id="exampleFormControlTextarea1"
            rows={10}
            style={{ width: '100%', height: '300px' }}
            value={htmlContent}
            onChange={handleContentChange}
          />
          <small>{htmlContent.length}/{maxChars} caracteres</small>
        </div>
        <button type="submit" className="btn btn-primary mb-3">Enviar</button>
      </form>
    </div>
  );
};

export default Campannas;
