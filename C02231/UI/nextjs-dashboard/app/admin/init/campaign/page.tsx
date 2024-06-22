'use client';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import Link from 'next/link';
import '/app/ui/global.css';
import * as signalR from '@microsoft/signalr';

type Message = {
    id: number;
    update: string;
    timestamp: Date;
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

    const transformData = (data: any[]): Message[] => {
        return data.map(item => ({
            id: Number(item[0]),
            update: item[1],
            timestamp: new Date(item[2]) // Convertir a objeto Date
        }));
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
                console.log('Datos recibidos de la API:', data);
                setMessages(transformData(data));
            } else {
                setErrorMessage('Error al obtener las campañas.');
            }
        } catch (error) {
            setErrorMessage('Error al obtener las campañas.');
            console.error('Error fetching messages:', error);
        }
    };

    useEffect(() => {
        fetchMessages();

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${URL}/campaignHub`)
            .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveCampaignUpdate", (updateString: string) => {
            try {
                let update: Message;
                // Verificar si es una cadena y no un JSON válido
                if (typeof updateString === 'string') {
                    const parts = updateString.split(',');
                    if (parts.length === 3) {
                        update = {
                            id: Number(parts[0]),
                            update: parts[1],
                            timestamp: new Date(parts[2])
                        };
                    } else {
                        throw new Error('Invalid string format');
                    }
                } else {
                    // Intentar convertir la cadena en un objeto JSON
                    update = JSON.parse(updateString);

                    // Verificar si el objeto tiene la estructura esperada
                    if ('id' in update && 'update' in update && 'timestamp' in update) {
                        update.timestamp = new Date(update.timestamp); // Convertir a objeto Date
                    } else {
                        throw new Error('Received update does not have the expected structure');
                    }
                }
                setMessages(prevMessages => [update, ...prevMessages.slice(0, 2)]);
            } catch (error) {
                console.error('Error processing update:', error);
            }
        });

        connection.start().catch(err => console.error("Connection error:", err));

        return () => {
            connection.stop().catch(err => console.error("Disconnection error:", err));
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
            console.error('There was an error posting the message!', error);
        }
    };

    const handleDeleteMessage = async (id: number) => {
        try {
            const response = await fetch(`${URL}/api/Campaign/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            setMessages(messages.filter(message => message.id !== id));
        } catch (error) {
            console.error("There was an error deleting the message!", error);
        }
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
                                        <strong>{message.timestamp.toLocaleString()}:</strong> {message.update}
                                    </div>
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
};

export default Campaigns;
