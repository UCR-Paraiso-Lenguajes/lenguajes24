"use client";
import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import 'bootstrap/dist/css/bootstrap.min.css';

interface Notification {
  notificationId: number;
  notificationName: string;
  notificationMessage: string;
  notificationCreatedDate: string;
  notificationStatus: number;
}

export const NotificationComponent: React.FC = () => {
  const loadNotificationsFromLocalStorage = (): Notification[] => {
    const storedNotifications = localStorage.getItem("notify");
    return storedNotifications ? JSON.parse(storedNotifications) : [];
  };

  const [notifications, setNotifications] = useState<Notification[]>(loadNotificationsFromLocalStorage);
  const [notifyModalOpen, setNotifyModal] = useState(false);

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl('https://localhost:7165/notificationHub')
      .withAutomaticReconnect()
      .build();

    connection.start();

    connection.on("Receive", (notification: Notification) => {
      const updatedNotifications = [notification, ...notifications].slice(0, 3);
      setNotifications(updatedNotifications);
      localStorage.setItem('notify', JSON.stringify(updatedNotifications));
    });

    connection.on("Delete", (idNotification: number) => {
      const updatedNotifications = notifications.filter(notification => notification.notificationId !== idNotification);
      setNotifications(updatedNotifications);
      localStorage.setItem('notify', JSON.stringify(updatedNotifications));
    });

    return () => {
      connection.stop();
    };
  }, [notifications]);

  const toggleModal = () => {
    setNotifyModal(!notifyModalOpen);
  };

  const clearNotifications = () => {
    setNotifications([]);
    localStorage.removeItem("notify");
  };

  const renderMessage = (message: string) => {
    return { __html: message };
  };

  return (
    <div className="position-fixed top-0 end-0 p-3">
      <button className="btn btn-secondary position-relative" onClick={toggleModal}>
        <img src="/img/campana.png" alt="Notification Bell" style={{ width: '24px', height: '24px' }} />
        {notifications.length > 0 && (
          <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
            {notifications.length}
          </span>
        )}
      </button>

      {notifyModalOpen && (
        <div className="modal show d-block" tabIndex={-1} style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog modal-dialog-centered modal-lg">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Notificaciones Recibidas</h5>
                <button type="button" className="btn-close" onClick={toggleModal}></button>
              </div>
              <div className="modal-body">
                {notifications.length === 0 ? (
                  <p>No hay notificaciones.</p>
                ) : (
                  notifications.map(notification => (
                    <div key={notification.notificationId} className="mb-3">
                      <h5>{notification.notificationName}</h5>
                      <div className="mb-2" dangerouslySetInnerHTML={renderMessage(notification.notificationMessage)}></div>
                      <small className="text-muted">{new Date(notification.notificationCreatedDate).toLocaleString()}</small>
                      <hr />
                    </div>
                  ))
                )}
              </div>
              <div className="modal-footer">
                <button type="button" className="btn btn-secondary" onClick={toggleModal}>Cerrar</button>
                <button type="button" className="btn btn-danger" onClick={clearNotifications}>Limpiar buzón</button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};