"use client";

import { useEffect, useState, useCallback } from 'react';
import * as signalR from '@microsoft/signalr';

const URL = process.env.NEXT_PUBLIC_API || 'http://localhost:5072';

export interface Message {
  id: number;
  title: string;
  content: string;
  timestamp: string;
  read: boolean;
}

export const useSignalR = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [error, setError] = useState<string | null>(null);

  const handleReceiveMessage = useCallback((message: Message) => {
    setMessages((prevMessages) => {
      const existingMessageIndex = prevMessages.findIndex(msg => msg.id === message.id);
      if (existingMessageIndex !== -1) {
        const updatedMessages = [...prevMessages];
        updatedMessages[existingMessageIndex] = { ...updatedMessages[existingMessageIndex], ...message };
        return updatedMessages;
      } else {
        return [message, ...prevMessages];
      }
    });
  }, []);

  const handleRemoveMessage = useCallback((id: number) => {
    setMessages((prevMessages) => prevMessages.filter((msg) => msg.id !== id));
  }, []);

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${URL}/hubs/marketing`, { withCredentials: true })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    const startConnection = async () => {
      try {
        await newConnection.start();
        setConnection(newConnection);
        setError(null);
      } catch (err) {
        setError('Error connecting to the server. Attempting to reconnect...');
        setTimeout(() => startConnection(), 5000);
      }
    };

    newConnection.onclose(() => {
      setError('Connection closed. Attempting to reconnect...');
      setTimeout(() => startConnection(), 5000);
    });

    startConnection();

    return () => {
      if (newConnection) {
        newConnection.stop();
      }
    };
  }, []);

  useEffect(() => {
    if (connection) {
      connection.on('ReceiveMessage', handleReceiveMessage);
      connection.on('RemoveMessage', handleRemoveMessage);

      return () => {
        connection.off('ReceiveMessage', handleReceiveMessage);
        connection.off('RemoveMessage', handleRemoveMessage);
      };
    }
  }, [connection, handleReceiveMessage, handleRemoveMessage]);

  return { connection, messages, setMessages, error, setError };
};
