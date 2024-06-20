'use client';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import Link from 'next/link';
import '/app/ui/global.css';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';
import { useRouter } from 'next/navigation';

type Message = {
    id: string;
    content: string;
    timestamp: string;
};

export default function Campaigns() {
    const [messages, setMessages] = useState<Message[]>([]);
    const [newMessage, setNewMessage] = useState('');
    const router = useRouter();

    useEffect(() => {
        const fetchMessages = async () => {
            try {
                const response = await axios.get('/api/messages');
                setMessages(response.data);
            } catch (error) {
                console.error("There was an error fetching the messages!", error);
            }
        };

        fetchMessages();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://your-websocket-server-url/hub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on("newMessage", (message: Message) => {
            setMessages(prevMessages => [message, ...prevMessages].slice(0, 3));
        });

        connection.start().catch(err => console.error("Connection error:", err));

        return () => {
            connection.stop().catch(err => console.error("Disconnection error:", err));
        };
    }, []);

    const handleAddMessage = () => {
        axios.post('/api/messages', { content: newMessage })
            .then(response => {
                setNewMessage('');
            })
            .catch(error => {
                console.error("There was an error posting the message!", error);
            });
    };

    const handleDeleteMessage = (id: string) => {
        axios.delete(`/api/messages/${id}`)
            .then(response => {
                setMessages(messages.filter(message => message.id !== id));
            })
            .catch(error => {
                console.error("There was an error deleting the message!", error);
            });
    };

    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-12 d-flex justify-content-end align-items-center">
                        <Link href="/admin/init">
                            <button className="btn btn-dark">GO Back</button>
                        </Link>
                    </div>
                </div>
            </header>
            <div className='container'>
                <div className="col-md-12">
                    <div className="content">
                        <h2>Campaign Messages</h2>
                        <div className="mb-3">
                            <textarea
                                className="form-control"
                                value={newMessage}
                                onChange={e => setNewMessage(e.target.value)}
                                placeholder="Enter new message (max 5000 characters)"
                                maxLength={5000}
                            />
                            <button className="btn btn-success mt-2" onClick={handleAddMessage}>Add Message</button>
                        </div>
                        <ul className="list-group">
                            {messages.map(message => (
                                <li key={message.id} className="list-group-item d-flex justify-content-between align-items-center">
                                    <div dangerouslySetInnerHTML={{ __html: message.content }}></div>
                                    <button className="btn btn-danger" onClick={() => handleDeleteMessage(message.id)}>Delete</button>
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            </div>
            <footer className="footer" style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light">Biblioteca de Paula</h5>
                </div>
            </footer>
        </div>
    );
}
