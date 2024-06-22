'use client'
import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import 'bootstrap/dist/css/bootstrap.min.css';

export const Page = () => {
    const [messages, setMessages] = useState([]);
    const [newMessage, setNewMessage] = useState({ content: '' });
    const [connection, setConnection] = useState(null);

    
  const URL = process.env.NEXT_PUBLIC_API_URL;
  if (!URL) {
      throw new Error('NEXT_PUBLIC_API_URL is not defined');
  }
  
    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(`${URL}/Campannas`)
            .withAutomaticReconnect()
            .build();

        newConnection.start()
            .then(() => console.log('Connected to the campaign hub'))
            .catch(err => console.error('Error connecting to campaign hub:', err));

        // Escuchar actualizaciones de mensajes
        newConnection.on("UpdateMessages", (receivedMessages) => {
            setMessages(receivedMessages);
        });

        setConnection(newConnection);

        return () => {
            newConnection.stop();
        };
    }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewMessage({ ...newMessage, [name]: value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (connection && newMessage.content.trim()) {
            try {
                await connection.invoke('SendCampaignMessage', newMessage.content);
                setNewMessage({ content: '' });  // Clear the form upon submission
            } catch (error) {
                console.error('Error sending message:', error);
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
                <button type="submit" className="btn btn-primary">Create Campaign</button>
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

export default Page;
