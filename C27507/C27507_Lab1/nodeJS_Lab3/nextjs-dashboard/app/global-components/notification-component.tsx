//Interfaces
import { NotificationAPI } from '../src/models-data/NotificationAPI';

import React, { useEffect, useState } from 'react';
import signalR, { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export const NotificationComponent = () => {

  const [notifications, setNotifications] = useState<NotificationAPI[]>([]);

    useEffect(() => {
        
      const connection = new HubConnectionBuilder()
          .withUrl("https://localhost:7161/notificationHub")
          .configureLogging(LogLevel.Information)
          .build();
      
      connection.start().then(() => {
        console.log("Conexión exitosa");
          // Configurar el manejador para recibir notificaciones
          connection.on("Receive", (listOfnotifications: NotificationAPI[]) => {
            console.log("Notificaciones recibidas", listOfnotifications);
            setNotifications(listOfnotifications);
          });
        })
        .catch(error => console.error("Error al iniciar la conexión", error));

      // Limpiar la conexión al desmontar el componente
      return () => {
          connection.stop().then(() => console.log("Conexión terminada"));
      };
    }, []);

    return (
        <div>
          <h1>Notificaciones</h1>
          <ul>
            {notifications.map((notification, index) => (
                <li key={index}>{notification.notifyTitle}</li>
            ))}
          </ul>
        </div>
    );

}