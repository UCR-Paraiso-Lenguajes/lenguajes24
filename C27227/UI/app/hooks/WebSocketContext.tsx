"use client";
import React, { createContext, useContext, useEffect, useState, ReactNode } from 'react';
import * as signalR from '@microsoft/signalr';

interface CampaignMessage {
  id: number;
  title: string;
  content: string;
}

interface WebSocketContextProps {
  connection: signalR.HubConnection | null;
  messages: CampaignMessage[];
}

const WebSocketContext = createContext<WebSocketContextProps | undefined>(undefined);
const URLConection = process.env.NEXT_PUBLIC_API;

export const useWebSocket = (): WebSocketContextProps => {
  const context = useContext(WebSocketContext);
  if (!context) {
    throw new Error('useWebSocket must be used within a WebSocketProvider');
  }
  return context;
};

interface WebSocketProviderProps {
  children: ReactNode;
}

export const WebSocketProvider: React.FC<WebSocketProviderProps> = ({ children }) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [messages, setMessages] = useState<CampaignMessage[]>([]);

  useEffect(() => {
    const fetchLatestCampaigns = async () => {
      try {
        const token = sessionStorage.getItem('sessionToken');
        const response = await fetch(`${URLConection}/api/campaign/top3`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
          },
        });
        if (!response.ok) {
          throw new Error('Error fetching latest campaigns');
        }
        const data = await response.json();
        setMessages(data);
      } catch (error) {
        console.error('Error fetching latest campaigns:', error);
      }
    };

    const connect = new signalR.HubConnectionBuilder()
      .withUrl(`${URLConection}/hub/campaignsHub`)
      .withAutomaticReconnect()
      .build();

    connect.start()
      .then(() => {
        console.log("Connected to SignalR hub");

        connect.on("ReceiveNewCampaign", (content: string, title: string, id: number) => {
          setMessages(prevMessages => [...prevMessages, { id, title, content }]);
        });

        connect.on("ReceiveDeletedCampaign", (id: number) => {
          setMessages(prevMessages => prevMessages.filter(message => message.id !== id));
        });

        fetchLatestCampaigns();
      })
      .catch(err => console.error("Connection to SignalR hub failed: ", err));

    setConnection(connect);

    return () => {
      connect.stop();
    };
  }, []);

  return (
    <WebSocketContext.Provider value={{ connection, messages }}>
      {children}
    </WebSocketContext.Provider>
  );
};
