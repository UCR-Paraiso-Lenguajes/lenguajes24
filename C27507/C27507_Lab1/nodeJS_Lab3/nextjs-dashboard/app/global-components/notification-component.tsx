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
  
  //Precargamos las notifiaciones que no se hayan limpiado desde la bandeja o que el admin haya borrado
  const loadNotificationsFromLocalStorage = (): NotificationAPI[] => {
    const storedNotifications = localStorage.getItem("notify");
    return storedNotifications ? JSON.parse(storedNotifications) : [];
  };
  const [notifications, setNotifications] = useState<NotificationAPI[]>(loadNotificationsFromLocalStorage);
  const [notifyModalOpen, setNotifyModal] = useState(false);
  
  process.env.NEXT_PUBLIC_NODE_ENV

  useEffect(() => {
    const baseUrl = process.env.NEXT_PUBLIC_NODE_ENV || "https://localhost:7161";
    const connection = new HubConnectionBuilder()
      .withUrl(`${baseUrl}/notificationHub`)
      .withAutomaticReconnect()
      .build();

    connection.start()      

    //Actualizamos en la lista
    connection.on("Receive", (notification: NotificationAPI) => {
      //Se actualiza la lista
      // Actualizar el estado local y localStorage con las últimas 3 notificaciones
      const updatedNotifications = [...notifications, notification].slice(-3);
      setNotifications(updatedNotifications);
      localStorage.setItem('notify', JSON.stringify(updatedNotifications));      
    });

    //Eliminamos de la lista
    connection.on("Delete", (idNotification: number) => {
      // Se eliminar la notificación con el ID especificado      
      const updatedNotifications = notifications.filter(notification => notification.notifyId !== idNotification);
      setNotifications(updatedNotifications);
      localStorage.setItem('notify', JSON.stringify(updatedNotifications));      
    });


    return () => {
      connection.stop();
    };
  }, [notifications]);

  //Manejar el modal
  const toggleModal = () => {
    setNotifyModal(!notifyModalOpen);
  };

  //Borrar todas las notifiaciones
  const clearNotifications = () => {
    setNotifications([]);
    localStorage.removeItem("notify");
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
            <div className="list-notify">
              {notifications.map(notification => (                
                <div key={notification.notifyId} className="notification-item">
                  <h5>Título: {notification.notifyTitle}</h5>
                  <p className="html-message">Mensaje: {htmlToReactParser.parse(notification.notifyMessage)}</p>
                  <p>Fecha: {notification.notifyCreationDate}</p>
                </div>
              ))}
            </div>
            <button onClick={clearNotifications}>Limpiar bandeja</button>
          </div>
        </div>
      )}
    </div>
  );  
}