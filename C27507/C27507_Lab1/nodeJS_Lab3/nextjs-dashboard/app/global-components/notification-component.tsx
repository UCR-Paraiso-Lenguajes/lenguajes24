//Interfaces
import { NotificationAPI } from '../src/models-data/NotificationAPI';

import React, { useEffect, useState } from 'react';
import signalR, { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
//para renderizar html en React
import { Parser } from 'html-to-react';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import '../src/css/notification-component.css'
import '../src/css/fonts_awesome/css/all.min.css'


// @ts-ignore
const htmlToReactParser = new Parser();
export const NotificationComponent = () => {
  
  //Lista de todas las notificaciones que se han recibido
  const [notifications, setNotifications] = useState<NotificationAPI[]>([]);
  const [notifyModalOpen, setNotifyModal] = useState(false);


  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:7161/notificationHub")
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(() => {
        console.log('Conexión establecida con SignalR Hub');
      })
      .catch(err => console.error('Error al conectar con SignalR Hub:', err));

    connection.on("Receive", (notification: NotificationAPI) => {
      //Se actualiza la lista
      console.log('Notificación recibida:', notification);
      setNotifications(prevNotifications => [...prevNotifications, notification]);
    });

    return () => {
      connection.stop();
    };
  }, []);

  //Manejar el modal
  const toggleModal = () => {
    setNotifyModal(!notifyModalOpen);
  };

  //Borrar todas las notifiaciones
  const clearNotifications = () => {
    setNotifications([]);
  };


  return (
    <div>
      <div className="notify-container" title='Mis Notificaciones'>
        <a onClick={toggleModal}>
          <div className="notify-info">
            <i className="fas fa-bell"></i>
            <div className="notify-notify">{notifications.length}</div>
          </div>
        </a>
      </div>

      {notifyModalOpen && (
        <div className="notify-modal">
          <div className="notify-modal-content">
            <span className="close" onClick={toggleModal}>&times;</span>
            <h2>Notificaciones Recibidas</h2>
            <ul>            
              {notifications.map(notification => (
                <li key={notification.notifyId}>
                  <div className="notification-item">
                    <h5>Título: {notification.notifyTitle}</h5>
                    <p className="html-message">Mensaje: {htmlToReactParser.parse(notification.notifyMessage)}</p>
                    {/* https://www.youtube.com/watch?v=g-kQv8EUI88 */}
                    <p>Fecha: {notification.notifyCreationDate}</p>
                  </div>
                </li>
              ))}
            </ul>
            <button onClick={clearNotifications}>Limpiar bandeja</button>
          </div>
        </div>
      )}
    </div>
  );  
}