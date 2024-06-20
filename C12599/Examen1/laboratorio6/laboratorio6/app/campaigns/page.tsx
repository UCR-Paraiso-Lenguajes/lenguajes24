'use client';
import React, { useEffect, useState } from 'react';
import * as signalR from "@microsoft/signalr";
import '../ui/globals.css';
const URL = process.env.NEXT_PUBLIC_API;

function Chat() {
    const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
    const [messages, setMessages] = useState<string[]>([]);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCampaigns = async () => {
            const response = await fetch(URL+'Campannas');
            if (response.ok) {
                const data = await response.json();
                const campaigns = data.map((campanna: any) => `Nueva Campaña: ${campanna.contenidoHtml}`);
                setMessages(campaigns);
            } else {
                setError('Error al obtener las campañas.');
            }
        };

        fetchCampaigns();

        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:7043/chatHub", {
                withCredentials: true
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
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
            if (connection) {
                connection.stop();
            }
        };
    }, []);

    useEffect(() => {
        if (connection) {
            connection.on("ReceiveMessage", (user: string, message: string) => {
                setMessages(prevMessages => [...prevMessages, `${user}: ${message}`]);
            });

            connection.on("UpdateCampaigns", (contenidoHtml: string) => {
                setMessages(prevMessages => [...prevMessages, `Nueva Campaña: ${contenidoHtml}`]);
            });
        }
    }, [connection]);

    return (
        <div className="chat-container">
            <h1 className="chat-title">Mensajes Recibidos</h1>
            {error && <div className="error-message">{error}</div>}
            <ul className="messages-list">
                {messages.map((message, index) => (
                    <li key={index} className="message-item">{message}</li>
                ))}
            </ul>
        </div>
    );
}

export default Chat;
