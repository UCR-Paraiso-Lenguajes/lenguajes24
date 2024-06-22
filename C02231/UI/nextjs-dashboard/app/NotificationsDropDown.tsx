import React, { useState, useEffect } from 'react';
import { Dropdown, Badge } from 'react-bootstrap';
import * as signalR from '@microsoft/signalr';

const NotificationsDropdown = () => {
  const [messages, setMessages] = useState([]);
  const [unreadCount, setUnreadCount] = useState(0);
  const [isOpen, setIsOpen] = useState(false);
  const [connection, setConnection] = useState(null);
  const [error, setError] = useState(null);
  const [reconnectInterval, setReconnectInterval] = useState(1000); // Retry reconnect every 1 second

  const URL = process.env.NEXT_PUBLIC_API_URL;
  if (!URL) {
    throw new Error('NEXT_PUBLIC_API_URL is not defined');
  }

  // Effect to handle initial fetch and SignalR connection
  useEffect(() => {
    const fetchMessages = async () => {
      try {
        const response = await fetch(`${URL}/api/Campaign`);
        if (response.ok) {
          const data = await response.json();
          console.log('Datos recibidos de la API:', data);

          if (Array.isArray(data)) {
            const campaigns = data.map((campaign, index) => {
              if (Array.isArray(campaign) && campaign.length > 1) {
                return `New campaign: ${campaign[1]}`;
              } else {
                console.error(`Campaña sin detalles en el índice ${index}`, campaign);
                return 'Campaña sin detalles';
              }
            });
            setMessages(campaigns.slice(-3)); // Solo mantenemos los últimos 3 mensajes
          } else {
            setError('Respuesta de la API en un formato inesperado.');
          }
        } else {
          setError('Error al obtener las campañas.');
        }
      } catch (err) {
        console.error('Error al obtener las campañas:', err);
        setError('Error al obtener las campañas.');
      }
    };

    fetchMessages();

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${URL}/campaignHub`, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    const startConnection = async () => {
      try {
        await newConnection.start();
        console.log("Connected to SignalR Hub.");
        setConnection(newConnection);
      } catch (err) {
        console.error("SignalR Connection Error: ", err);
        console.log("Retrying connection in", reconnectInterval, "ms...");
        setTimeout(startConnection, reconnectInterval);
      }
    };

    newConnection.onclose(() => {
      setError('Conexión cerrada. Intentando reconectar...');
      setTimeout(startConnection, reconnectInterval);
    });

    startConnection();

    return () => {
      if (newConnection.state === signalR.HubConnectionState.Connected) {
        newConnection.stop();
      }
    };
  }, [URL, reconnectInterval]);

  // Effect to handle message updates from SignalR
  useEffect(() => {
    if (connection) {
      debugger;
      const handleReceiveMessage = (user, message) => {
        setMessages((prevMessages) => {
          const newMessages = [...prevMessages, `${user}: ${message}`];
          return newMessages.slice(-3); // Solo mantenemos los últimos 3 mensajes
        });
        setUnreadCount((prevCount) => prevCount + 1);
      };

      const handleUpdateCampaigns = (contenidoHtml) => {
        setMessages((prevMessages) => {
          const newMessages = [...prevMessages, `Nueva Campaña: ${contenidoHtml}`];
          return newMessages.slice(-3); // Solo mantenemos los últimos 3 mensajes
        });
        setUnreadCount((prevCount) => prevCount + 1);
      };

      connection.on('ReceiveMessage', handleReceiveMessage);
      connection.on('UpdateCampaigns', handleUpdateCampaigns);

      // Cleanup function to remove event listeners when the component unmounts or connection changes
      return () => {
        connection.off('ReceiveMessage', handleReceiveMessage);
        connection.off('UpdateCampaigns', handleUpdateCampaigns);
      };
    }
  }, [connection]);

  const handleMarkAsRead = () => {
    setUnreadCount(0);
  };

  return (
    <Dropdown show={isOpen} onToggle={() => setIsOpen(!isOpen)} onClick={handleMarkAsRead}>
      <Dropdown.Toggle variant="secondary" id="dropdown-basic">
        <img src="https://png.pngtree.com/png-vector/20240521/ourmid/pngtree-free-golden-3d-bell-on-white-background-png-image_12501298.png"
          style={{ height: '40px', width: '40px' }} className="img-fluid" />
        <i className="fa fa-bell" aria-hidden="true"></i>
        {unreadCount > 0 && (
          <Badge bg="danger">{unreadCount}</Badge>
        )}
      </Dropdown.Toggle>
      <Dropdown.Menu>
        {messages.map((message, index) => (
          <Dropdown.Item key={index}>
            <div>
              {message}
              <small className="text-muted d-block">
                {new Date().toLocaleString()}
              </small>
            </div>
          </Dropdown.Item>
        ))}
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default NotificationsDropdown;
