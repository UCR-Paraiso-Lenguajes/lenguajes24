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
            if(message.Enabled){
                setMessages((prevMessages) => [...prevMessages, message]);
                setMessagesList((prevMessages) => [...prevMessages, message]);
                setNewMessages((prevCount) => prevCount + 1);
            }else{
                setNewMessages((prevCount) => prevCount - 1);
                setMessages((prevMessages) => prevMessages.filter(msg => msg.Id !== message.Id));
                setMessagesList((prevMessages) => prevMessages.filter(msg => msg.Id !== message.Id));
            }
        };

        return () => {
            socket.close();
        };
    }, [url, setMessagesList, setNewMessages]);

    return messages;
};

export default useWebSocket;
