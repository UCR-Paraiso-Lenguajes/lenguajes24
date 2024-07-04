'use client'
import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import 'bootstrap/dist/css/bootstrap.min.css';

const Campannas = () => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState({ content: '' });
    const [connection, setConnection] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const URL = process.env.NEXT_PUBLIC_API_URL;
    if (!URL) {
        throw new Error('NEXT_PUBLIC_API_URL is not defined');
    }

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(`${URL}/Campannas`)
            .withAutomaticReconnect()
            .build();

       newConnection.on("UpdateMessages", (receivedMessages) => {
        setMessages(receivedMessages);
       });

        newConnection.start()
            .then(() => console.log('Connected to the campaign hub'))
            .catch(err => console.error('Error connecting to campaign hub:', err));

        setConnection(newConnection);

        return () => {
            newConnection.off("UpdateMessages");
            newConnection.stop();
        };
    }, [URL]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewMessage({ ...newMessage, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (isSubmitting) return;

        if (newMessage.content.trim()) {
            setIsSubmitting(true);

            try {
                const response = await fetch(`${URL}/api/messages`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(newMessage.content)
                });

                if (response.status === 200) {
                    if (connection) {
                        await connection.send('SendCampaignMessage', newMessage.id, newMessage.content);
                        setNewMessage({ content: '' }); 
                    }
                } else {
                    throw new Error('Failed to add message');
                }
            } catch (error) {
                console.error('Error sending message:', error);
            } finally {
                setIsSubmitting(false);
            }
        }
    };

    const handleDeleteMessage = async (id) => {
        if (connection) {
            try {
                await connection.invoke('DeleteMessage', id);
            } catch (error) {
                console.error('Error deleting message:', error);
            }
        }
    };

    return (
        <div className="container mt-5">
            <h2 className="mb-4">Campaigns</h2>
            <form onSubmit={handleSubmit} className="mb-4">
                <div className="mb-3">
                    <label htmlFor="content" className="form-label">Content (HTML allowed)</label>
                    <textarea
                        className="form-control"
                        id="content"
                        name="content"
                        value={newMessage.content}
                        onChange={handleInputChange}
                        placeholder="Enter campaign content"
                    />
                </div>
                <button type="submit" className="btn btn-primary" disabled={isSubmitting}>Create Campaign</button>
            </form>
            <div className="list-group">
                {messages.map((message) => (
                    <div key={message.id} className="list-group-item">
                        <div dangerouslySetInnerHTML={{ __html: message.content }}></div>
                        <button className="btn btn-danger" onClick={() => handleDeleteMessage(message.id)}>Delete</button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Campannas;
