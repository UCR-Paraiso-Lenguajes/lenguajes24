import React, { useEffect, useState } from 'react';
import * as signalR from "@microsoft/signalr";

function Chat() {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [reconnectInterval, setReconnectInterval] = useState(1000); // Retry reconnect every 1 seconds
    
    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5123/chatHub")
            .build();

        newConnection.onclose(() => {
            console.log("SignalR Connection closed. Retrying connection in", reconnectInterval, "ms...");
            setTimeout(startConnection, reconnectInterval);
        });

        const startConnection = () => {
            newConnection.start()
                .then(() => {
                    console.log("Connected to SignalR Hub.");
                    setConnection(newConnection);
                })
                .catch(err => {
                    console.error("SignalR Connection Error: ", err);
                    console.log("Retrying connection in", reconnectInterval, "ms...");
                    setTimeout(startConnection, reconnectInterval);
                });
        };

        startConnection();

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, [reconnectInterval]);

    useEffect(() => {
        if (connection) {
            connection.on("ReceiveMessage", (user, message) => {
                console.log("ReceiveMessage", user, message, "ends");
                setMessages([...messages, `${user}: ${message}`]);
            });
        }
    }, [connection, messages]);

    const sendMessage = () => {
        const user = "YourUser"; // Change this to the user's name or fetch dynamically
        const message = "Hello from React!"; // Change this to the message input from the user
        connection.invoke("SendMessage", user, message)
            .catch(err => console.error("Error sending message: ", err));
    };

    return (
        <div>
            <h1>Chat App</h1>
            <ul>
                {messages.map((message, index) => (
                    <li key={index}>{message}</li>
                ))}
            </ul>
            <button onClick={sendMessage}>Send Message</button>
        </div>
    );
}

export default Chat;
