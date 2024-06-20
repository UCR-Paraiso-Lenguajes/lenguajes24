'use client';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';
import { Badge, Dropdown, DropdownButton } from 'react-bootstrap';

export default function ClientMessages() {
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        const fetchMessages = async () => {
            try {
                const response = await axios.get('/api/messages');
                setMessages(response.data);
            } catch (error) {
                console.error("Error fetching messages:", error);
            }
        };

        fetchMessages();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://your-websocket-server-url/hub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on("newMessage", (message) => {
            setMessages(prevMessages => [message, ...prevMessages].slice(0, 3));
        });

        connection.start().catch(err => console.error("Connection error:", err));

        return () => {
            connection.stop().catch(err => console.error("Disconnection error:", err));
        };
    }, []);

    const handleDeleteMessage = async (id) => {
        try {
            await axios.delete(`/api/messages/${id}`);
            setMessages(messages.filter(message => message.id !== id));
        } catch (error) {
            console.error("Error deleting message:", error);
        }
    };

    return (
        <div className="container">
            <div className="position-relative">
                <div className="d-flex align-items-center">
                    <h1>Messages</h1>
                    <div style={{ color: 'gray' }}>
                        <ul>
                            {messages.map(message => (
                                <li key={message.id}>
                                    {message.content}
                                    <button onClick={() => handleDeleteMessage(message.id)}>Eliminar</button>
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    );
}
