'use client'
import { useEffect, useRef, useState } from 'react';

type WebSocketMessage = {
    id: string;
    text: string;
    sentAt: string;
};

const useWebSocket = (url: string, setNewMessages: (quantity: number) => void) => {
    const [messages, setMessages] = useState<WebSocketMessage[]>([]);

    useEffect(() => {
        const socket = new WebSocket(url);

        socket.onmessage = (event) => {
            const message: WebSocketMessage = JSON.parse(event.data);
            setMessages((prevMessages) => [...prevMessages, message]);
            setNewMessages((prevCount) => prevCount + 1);
        };

        return () => {
            socket.close();
        };
    }, [url, setNewMessages]);

    return messages;
};

export default useWebSocket;
