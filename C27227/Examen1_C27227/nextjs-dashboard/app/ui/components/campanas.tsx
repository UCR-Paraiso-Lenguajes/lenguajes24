"use client";

import React, { useState, useEffect } from 'react';

export interface Message {
  id: number;
  title: string;
  content: string;
  timestamp: string;
  isDeleted: boolean;
  read: boolean;
}

interface CampanasProps {
  messages: Message[];
  error: string | null;
  onCreateMessage: (message: { title: string, content: string }) => void;
  onDeleteMessage: (id: number) => void;
  onRefresh: () => void;
}

const Campanas: React.FC<CampanasProps> = ({ messages, error, onCreateMessage, onDeleteMessage, onRefresh }) => {
  const [content, setContent] = useState('');
  const [title, setTitle] = useState('');

  useEffect(() => {
    onRefresh();
  }, [onRefresh]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const trimmedTitle = title.trim();
    const trimmedContent = content.trim();

    if (trimmedTitle && trimmedContent) {
      const exists = messages.some(msg => msg.title === trimmedTitle && msg.content === trimmedContent);
      if (!exists) {
        await onCreateMessage({ title: trimmedTitle, content: trimmedContent });
        setTitle('');
        setContent('');
      } else {
        alert('El mensaje ya existe.');
      }
    } else {
      alert('El título y el contenido no pueden estar vacíos.');
    }
  };

  return (
    <div>
      <h1>Campañas</h1>
      {error && <div className="text-danger mb-3">{error}</div>}
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="Título del mensaje"
          className="form-control mb-3"
        />
        <textarea
          value={content}
          onChange={(e) => setContent(e.target.value)}
          maxLength={5000}
          placeholder="Escribe tu mensaje aquí"
          className="form-control"
          rows={5}
        />
        <button type="submit" className="btn btn-primary mt-3">Enviar</button>
      </form>
      <h2 className="mt-5">Campañas en existencia</h2>
      <button onClick={onRefresh} className="btn btn-secondary mb-3">Refrescar</button>
      <ul className="list-group mt-3">
        {messages && messages.length > 0 ? (
          messages.map((msg) => (
            <li key={`${msg.id}-${msg.timestamp}`} className={`list-group-item ${msg.read ? 'list-group-item-secondary' : 'list-group-item-primary'}`}>
              <div className="message-title"><strong>{msg.title}</strong></div>
              <div dangerouslySetInnerHTML={{ __html: msg.content }} />
              <button onClick={() => onDeleteMessage(msg.id)} className="btn btn-danger btn-sm mt-2">Eliminar</button>
            </li>
          ))
        ) : (
          <li className="list-group-item">No hay campañas existentes.</li>
        )}
      </ul>
    </div>
  );
};

export default Campanas;
