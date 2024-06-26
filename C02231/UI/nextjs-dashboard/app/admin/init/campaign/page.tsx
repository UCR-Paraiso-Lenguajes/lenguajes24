'use client';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import Link from 'next/link';
import '/app/ui/global.css';
import * as signalR from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr';

type Message = {
    id: number;
    update: string;
    timestamp: string; // Cambiado a string para mantener el formato ISO
};

const URL = process.env.NEXT_PUBLIC_API_URL;
if (!URL) {
    throw new Error('NEXT_PUBLIC_API_URL is not defined');
}

const Campaigns: React.FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const [newMessage, setNewMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState<string>('');
    const maxChars = 5000;

    const isValidDate = (date: any): boolean => {
        return !isNaN(Date.parse(date));
    };

    const transformData = (data: any[]): Message[] => {
        return data.map(item => {
            const timestamp = isValidDate(item[2]) ? new Date(item[2]).toISOString() : new Date().toISOString();
            return {
                id: Number(item[0]),
                update: item[1],
                timestamp: timestamp
            };
        });
    };

    const fetchMessages = async () => {
        try {
            const response = await fetch(`${URL}/api/Campaign`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                },
            });

            if (response.ok) {
                const data = await response.json();
                setMessages(transformData(data));
            } else {
                setErrorMessage('Error al obtener las campañas.');
            }
        } catch (error) {
            setErrorMessage('Error al obtener las campañas.');
            throw new Error('Error fetching messages:', error);
        }
    };
    
    useEffect(() => {
        fetchMessages();

        const newConnection = new HubConnectionBuilder()
            .withUrl(`${URL}/campaignHub`)
            .withAutomaticReconnect()
            .build();

        newConnection.on("ReceiveCampaignUpdate", (updateString) => {
            try {
                let update;
                if (typeof updateString === 'string') {
                    const parts = updateString.split(',');
                    if (parts.length === 10) {
                        update = {
                            id: Number(parts[0]),
                            update: parts[1],
                            timestamp: isValidDate(parts[2]) ? new Date(parts[2]).toISOString() : new Date().toISOString()
                        };
                    } else {
                        throw new Error('Invalid string format');
                    }
                } else {
                    update = JSON.parse(updateString);
                    if ('id' in update && 'update' in update && 'timestamp' in update) {
                        update.timestamp = isValidDate(update.timestamp) ? new Date(update.timestamp).toISOString() : new Date().toISOString();
                    } else {
                        throw new Error('Received update does not have the expected structure');
                    }
                }
                setMessages(prevMessages => [update, ...prevMessages.slice(0, 2)]);
            } catch (error) {
                throw new Error('Error processing update:', error);
            }
        });

        newConnection.start()
            .then(() => console.log('Connected to the campaign hub'))
            .catch(err => console.error('Error connecting to campaign hub:', err));

        return () => {
            newConnection.stop();
        };
    }, []);

    const handleAddMessage = async () => {
        try {
            const timestamp = new Date().toISOString(); // Obtener timestamp en UTC
            const response = await fetch(`${URL}/api/Campaign`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: 0,
                    update: newMessage,
                    timestamp: timestamp // Enviar el timestamp
                })
            });

            if (response.ok) {
                setNewMessage('');
                await fetchMessages(); // Refrescar la lista después de enviar el mensaje
            } else {
                throw new Error('Network response was not ok');
            }
        } catch (error) {
            throw new Error('There was an error posting the message!', error);
        }
    };

    const handleDeleteMessage = async (id: number) => {
        try {
            const response = await fetch(`${URL}/api/Campaign/Delete/${id}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            setMessages(messages.filter(message => message.id !== id));
        } catch (error) {
            throw new Error("There was an error deleting the message!", error);
        }
    };

    const renderMessage = (message: Message) => {
        const youtubeRegex = /(https?:\/\/(www\.)?youtube\.com\/watch\?v=|https?:\/\/youtu\.be\/)([a-zA-Z0-9_-]+)/;
        const match = message.update.match(youtubeRegex);

        if (match) {
            const videoId = match[3];
            const embedUrl = `https://www.youtube.com/embed/${videoId}`;
            return (
                <div key={message.id}>
                    <iframe
                        width="100%"
                        height="315"
                        src={embedUrl}
                        title="YouTube video player"
                        frameBorder="0"
                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
                        allowFullScreen
                    ></iframe>
                </div>
            );
        }

        return (
            <div key={message.id}>
                <strong>{new Date(message.timestamp).toLocaleString()}:</strong>
                <span dangerouslySetInnerHTML={{ __html: message.update }} />
            </div>
        );
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
                        {errorMessage && <div className="text-danger mb-3">{errorMessage}</div>}
                        <div className="mb-3">
                            <textarea
                                className="form-control"
                                value={newMessage}
                                onChange={e => setNewMessage(e.target.value)}
                                placeholder="Enter new message (max 5000 characters)"
                                maxLength={maxChars}
                            />
                            <button className="btn btn-success mt-2" onClick={handleAddMessage}>Add Message</button>
                        </div>
                        <ul className="list-group">
                            {messages.map((message) => (
                                <li key={message.id} className="list-group-item d-flex justify-content-between align-items-center">
                                    <div>
                                        {renderMessage(message)}
                                    </div>
                                    <button className="btn btn-danger" onClick={() => handleDeleteMessage(message.id)}>Delete</button>
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            </div>
            <footer className="footer">
                <div className="text-center p-3">
                    <h5 className="text-light">Biblioteca de Paula</h5>
                </div>
            </footer>
        </div>
    );
};

export default Campaigns;
