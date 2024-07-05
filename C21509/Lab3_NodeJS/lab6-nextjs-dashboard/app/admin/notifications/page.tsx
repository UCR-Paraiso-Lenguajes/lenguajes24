"use client";
import React, { useState, useEffect, ChangeEvent } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Button, Modal, Form, Table } from 'react-bootstrap';

interface Notification {
  notificationId: number;
  notificationName: string;
  notificationMessage: string;
  notificationCreatedDate: string;
  notificationStatus: number;
}

const NotificationPage = () => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [show, setShow] = useState(false);
  const [newNotification, setNewNotification] = useState({
    notificationName: '',
    notificationMessage: '',
    notificationCreatedDate: new Date().toISOString(),
    notificationStatus: 1,
  });

  useEffect(() => {
    const fetchNotifications = async () => {
      try {
        const response = await fetch('https://localhost:7165/api/Notifications/Select');
        if (!response.ok) {
          throw new Error('Failed to fetch notifications');
        }
        const data = await response.json();
        console.log('Fetched notifications:', data);
        setNotifications(data);
      } catch (error) {
        console.error('Error fetching notifications:', error);
      }
    };

    fetchNotifications();
  }, []);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setNewNotification((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    const urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7165';
    const directionAPI = `${urlByReactEnviroment}/api/Notifications/Insert`;

    const postConfig = {
      method: "POST",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify(newNotification)
    }

    try {
      const responsePost = await fetch(directionAPI, postConfig);
      if (!responsePost.ok) {
        const errorMessage = await responsePost.text();
        console.error('Error inserting notification:', errorMessage);
      } else {
        const insertedWithSuccess = await responsePost.json();
        console.log('Notification inserted:', insertedWithSuccess);
        setNotifications((prev) => [insertedWithSuccess, ...prev]);
        handleClose();
      }
    } catch (error) {
      console.error('Failed to POST data:', error);
    }
  };

  const handleDelete = async (notificationId: number) => {
    const urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7165';
    const directionAPI = `${urlByReactEnviroment}/api/Notifications/Delete/${notificationId}`;

    const deleteConfig = {
      method: "DELETE",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json"
      }
    }

    try {
      const responsePost = await fetch(directionAPI, deleteConfig);
      if (!responsePost.ok) {
        const errorMessage = await responsePost.text();
        console.error('Error deleting notification:', errorMessage);
      } else {
        const deletedWithSuccess = await responsePost.json();
        console.log('Notification deleted:', deletedWithSuccess);
        setNotifications((prev) => prev.filter((n) => n.notificationId !== notificationId));
      }
    } catch (error) {
      console.error('Failed to DELETE data:', error);
    }
  };

  const renderDescription = (description: string) => {
    return { __html: description };
  };

  return (
    <main className="container">
      <h1 className="my-4">Gestion de notificaciones</h1>
      <Button variant="primary" onClick={handleShow}>Crear notificacion</Button>

      <Modal show={show} onHide={handleClose}>
        <Modal.Header closeButton>
          <Modal.Title>Crear Notificacion</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="formNotificationName">
              <Form.Label>Nombre</Form.Label>
              <Form.Control
                type="text"
                name="notificationName"
                value={newNotification.notificationName}
                onChange={handleInputChange}
              />
            </Form.Group>
            <Form.Group controlId="formNotificationMessage">
              <Form.Label>Mensaje</Form.Label>
              <Form.Control
                as="textarea"
                rows={3}
                name="notificationMessage"
                value={newNotification.notificationMessage}
                onChange={handleInputChange}
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleClose}>Cerrar</Button>
          <Button variant="primary" onClick={handleSubmit}>Guardar</Button>
        </Modal.Footer>
      </Modal>

      <Table striped bordered hover className="mt-4">
        <thead>
          <tr>
            <th>ID</th>
            <th>Nombre</th>
            <th>Mensaje</th>
            <th>Fecha de Creación</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {notifications.map((notification) => (
            <tr key={notification.notificationId}>
              <td>{notification.notificationId}</td>
              <td>{notification.notificationName}</td>
              <td dangerouslySetInnerHTML={renderDescription(notification.notificationMessage)}></td>
              <td>{new Date(notification.notificationCreatedDate).toLocaleString()}</td>
              <td>{notification.notificationStatus === 1 ? 'Active' : 'Inactive'}</td>
              <td>
                <Button variant="danger" onClick={() => handleDelete(notification.notificationId)}>Eliminar</Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </main>
  );
}

export default NotificationPage;