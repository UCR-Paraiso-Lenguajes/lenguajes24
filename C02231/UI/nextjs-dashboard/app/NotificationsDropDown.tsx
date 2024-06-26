'use client';
import React, { useState, useEffect, useCallback } from 'react';
import { Dropdown, Badge } from 'react-bootstrap';
import * as signalR from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr';

const NotificationsDropdown = () => {
  const [apiMessages, setApiMessages] = useState<string[]>([]);
  const [campaignMessages, setCampaignMessages] = useState<string[]>([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [isOpen, setIsOpen] = useState(false);
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  const URL = process.env.NEXT_PUBLIC_API_URL;
  if (!URL) {
    throw new Error('NEXT_PUBLIC_API_URL is not defined');
  }

  // Fetch initial messages from the API
  useEffect(() => {
    const fetchMessages = async () => {
      try {
        const response = await fetch(`${URL}/api/Campaign`);
        if (response.ok) {
          const data = await response.json();
          if (Array.isArray(data)) {
            const campaigns = data.map((campaign) => {
              return `Campaign: ${campaign[1]}`;
            });
            setApiMessages(campaigns.slice(-3)); // Keep the last 3 messages from the API
          } else {
            throw new Error('Unexpected API response format:', data);
          }
        } else {
          throw new Error('Failed to fetch campaigns from API:', response.statusText);
        }
      } catch (err) {
        throw new Error('Error fetching campaigns from API:', err);
      }
    };

    fetchMessages();
  }, [URL]);

  // Setup SignalR connection and event listeners
  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${URL}/campaignHub`, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    const startConnection = async () => {
      try {
        await newConnection.start();
        setConnection(newConnection);
      } catch (err) {
        throw new Error('SignalR Connection Error: ', err);
      }
    };

    newConnection.onclose(() => {
      setTimeout(startConnection, 1000); // Retry connection every 1 second
    });

    startConnection();

    return () => {
      if (newConnection.state === signalR.HubConnectionState.Connected) {
        newConnection.stop();
      }
    };
  }, [URL]);

  useEffect(() => {
    if (!connection) return;

    const handleReceiveCampaignUpdate = (message) => {
      const updateMessage = `Campaign: ${message}`;
      setCampaignMessages((prevMessages) => [updateMessage, ...prevMessages.slice(0, 2)]);
      setUnreadCount((prevCount) => prevCount + 1);
    };

    connection.on('ReceiveCampaignUpdate', handleReceiveCampaignUpdate);

    return () => {
      connection.off('ReceiveCampaignUpdate', handleReceiveCampaignUpdate);
    };
  }, [connection]);

  const handleDeleteMessage = useCallback(async (id: number) => {
    try {
      const response = await fetch(`${URL}/api/Campaign/Delete/${id}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }

      setCampaignMessages((prevMessages) => prevMessages.filter(message => message.id !== id));
    } catch (error) {
      throw new Error("There was an error deleting the message!", error);
    }
  }, [URL]);

  const handleMarkAsRead = () => {
    setUnreadCount(0);
  };

  const renderMessage = (message) => {
    const youtubeRegex = /(https?:\/\/(www\.)?youtube\.com\/watch\?v=|https?:\/\/youtu\.be\/)([a-zA-Z0-9_-]+)/;
    const match = message.match(youtubeRegex);

    if (match) {
      const videoId = match[3];
      const embedUrl = `https://www.youtube.com/embed/${videoId}`;
      return (
        <div key={message}>
          <iframe
            width="100%"
            height="315"
            src={embedUrl}
            title="YouTube video player"
            frameBorder="0"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
            allowFullScreen
          ></iframe>
        </div>
      );
    }

    return (
      <div dangerouslySetInnerHTML={{ __html: message }} />
    );
  };

  return (
    <Dropdown show={isOpen} onToggle={() => setIsOpen(!isOpen)}>
      <Dropdown.Toggle variant="secondary" id="dropdown-basic">
        <img
          src="https://png.pngtree.com/png-vector/20240521/ourmid/pngtree-free-golden-3d-bell-on-white-background-png-image_12501298.png"
          style={{ height: '40px', width: '40px' }}
          className="img-fluid"
          alt="Notification bell"
        />
        <i className="fa fa-bell" aria-hidden="true"></i>
        {unreadCount > 0 && (
          <Badge bg="danger">{unreadCount}</Badge>
        )}
      </Dropdown.Toggle>
      <Dropdown.Menu>
        {[...apiMessages].reverse().slice(-3).map((message, index) => ( 
          <Dropdown.Item key={`api-${index}`}>
            {renderMessage(message)}
            <small className="text-muted d-block">
              {new Date().toLocaleString()}
            </small>
          </Dropdown.Item>
        ))}
        {[...campaignMessages].reverse().slice(-3).map((message, index) => (
          <Dropdown.Item key={`campaign-${index}`}>
            {renderMessage(message)}
            <small className="text-muted d-block">
              {new Date().toLocaleString()}
            </small>
          </Dropdown.Item>
        ))}
        <Dropdown.Item onClick={handleMarkAsRead}>
          Marcar como leído
        </Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default NotificationsDropdown;
