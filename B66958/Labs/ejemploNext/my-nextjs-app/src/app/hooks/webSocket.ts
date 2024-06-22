'use client'
import { useEffect, useRef } from 'react';

type WebSocketMessage = {
    id: text;
    text: string;
    sentAt: string;
};

const useWebSocket = (url: string) => {
    const socket = useRef<WebSocket | null>(null);

    useEffect(() => {
        socket.current = new WebSocket(url);

        socket.current.onopen = () => {
            console.log('WebSocket connection established');
        };

        socket.current.onmessage = (event: MessageEvent) => {
            try {
                const message: WebSocketMessage = JSON.parse(event.data);
                // Handle the incoming notification
                console.log('New message:', message);
            } catch (error) {
                console.error('Error parsing WebSocket message:', error);
            }
        };

        return () => {
            if (socket.current) {
                socket.current.close();
            }
        };
    }, [url]);

    return socket.current;
};

export default useWebSocket;
