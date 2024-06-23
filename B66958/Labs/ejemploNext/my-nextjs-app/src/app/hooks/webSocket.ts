'use client'
import { useEffect, useRef, useState } from 'react';
import WebSocketMessage from '../navbar/WebSocketMessage';

const useWebSocket = (url: string, setNewMessages: (quantity: number) => void, 
    setMessagesList: (messages: WebSocketMessage[]) => void ) => {
    const [messages, setMessages] = useState<WebSocketMessage[]>([]);

    useEffect(() => {
        const socket = new WebSocket(url);

        socket.onmessage = (event) => {
            const message: WebSocketMessage = JSON.parse(event.data);
            setMessages((prevMessages) => [...prevMessages, message]);
            setMessagesList((prevMessages) => [...prevMessages, message]);
            setNewMessages((prevCount) => prevCount + 1);
        };

        return () => {
            socket.close();
        };
    }, [url, setMessagesList, setNewMessages]);

    return messages;
};

export default useWebSocket;
