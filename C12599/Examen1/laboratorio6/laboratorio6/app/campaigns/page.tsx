'use client';
import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import '../ui/globals.css';
const URL = process.env.NEXT_PUBLIC_API;

function Chat() {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchCampaigns = async () => {
            const response = await fetch(`${URL}/api/Campannas`);
            if (response.ok) {
                const data = await response.json();
                const campaigns = data.map((campanna) => `Nueva Campaña: ${campanna.contenidoHtml}`);
                setMessages(campaigns.slice(-3)); // Solo mantenemos los últimos 3 mensajes
            } else {
                setError('Error al obtener las campañas.');
            }
        };

        fetchCampaigns();

        const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:7043/chatHub", {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();

        const startConnection = async () => {
            try {
                await newConnection.start();
                setConnection(newConnection);
                setError(null);
            } catch (err) {
                setError('Error al conectar con el servidor. Intentando reconectar...');
                setTimeout(() => startConnection(), 5000);
            }
        };

        newConnection.onclose(() => {
            setError('Conexión cerrada. Intentando reconectar...');
            setTimeout(() => startConnection(), 5000);
        });

        startConnection();

        return () => {
            if (newConnection) {
                newConnection.stop();
            }
        };
    }, []);

    useEffect(() => {
        if (connection) {
            const handleReceiveMessage = (user, message) => {
                setMessages((prevMessages) => {
                    const newMessages = [...prevMessages, `${user}: ${message}`];
                    return newMessages.slice(-3); // Solo mantenemos los últimos 3 mensajes
                });
            };

            const handleUpdateCampaigns = (contenidoHtml) => {
                setMessages((prevMessages) => {
                    const newMessages = [...prevMessages, `Nueva Campaña: ${contenidoHtml}`];
                    return newMessages.slice(-3); // Solo mantenemos los últimos 3 mensajes
                });
            };

            connection.on('ReceiveMessage', handleReceiveMessage);
            connection.on('UpdateCampaigns', handleUpdateCampaigns);

            // Cleanup function to remove event listeners when the component unmounts or connection changes
            return () => {
                connection.off('ReceiveMessage', handleReceiveMessage);
                connection.off('UpdateCampaigns', handleUpdateCampaigns);
            };
        }
    }, [connection]);

    return (
        <div className="chat-container">
            <h1 className="chat-title">Mensajes Recibidos</h1>
            {error && <div className="error-message">{error}</div>}
            <ul className="messages-list">
                {messages.map((message, index) => (
                    <li key={index} className="message-item" dangerouslySetInnerHTML={{ __html: message }}></li>
                ))}
            </ul>
        </div>
    );
}

export default Chat;
