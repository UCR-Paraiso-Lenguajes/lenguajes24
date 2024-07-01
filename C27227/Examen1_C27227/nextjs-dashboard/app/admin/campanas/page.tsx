"use client";

import React, { useState, useEffect, useCallback } from 'react';
import Campanas, { Message } from '@/app/ui/components/campanas';
import { useSignalR } from '../../hooks/useSignalR';
import "bootstrap/dist/css/bootstrap.min.css";
const URL = process.env.NEXT_PUBLIC_API || 'http://localhost:5072';

const CampanasPage: React.FC = () => {
  const { connection, messages, setMessages, error, setError } = useSignalR();

  const onRefresh = useCallback(async () => {
    try {
      const response = await fetch(`${URL}/api/messages`);
      if (!response.ok) {
        throw new Error('Failed to fetch messages');
      }
      const data: Message[] = await response.json();
      setMessages(data);
    } catch (error) {
      setError(error.message);
    }
  }, [setMessages, setError]);

  const onCreateMessage = async ({ title, content }: { title: string, content: string }) => {
    try {
      const response = await fetch(`${URL}/api/messages`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ title, content }),
      });
      if (!response.ok) {
        throw new Error('Failed to create message');
      }
      const newMessage: Message = await response.json();

      // Checking for duplication before adding
      if (messages.some(msg => msg.id === newMessage.id)) {
        alert('Se ha encontrado un duplicado y no se añadirá.');
        return;
      }

      setMessages((prevMessages) => [...prevMessages, newMessage]);
      connection?.invoke('SendMessage', newMessage);
    } catch (error) {
      setError(error.message);
    }
  };

  const onDeleteMessage = async (id: number) => {
    try {
      const response = await fetch(`${URL}/api/messages/${id}`, {
        method: 'DELETE',
      });
      if (!response.ok) {
        throw new Error('Failed to delete message');
      }
      setMessages((prevMessages) => prevMessages.filter((msg) => msg.id !== id));
      connection?.invoke('RemoveMessage', id);
    } catch (error) {
      setError(error.message);
    }
  };

  return (
    <div>
      <Campanas
        messages={messages}
        error={error}
        onCreateMessage={onCreateMessage}
        onDeleteMessage={onDeleteMessage}
        onRefresh={onRefresh}
      />
    </div>
  );
};

export default CampanasPage;
