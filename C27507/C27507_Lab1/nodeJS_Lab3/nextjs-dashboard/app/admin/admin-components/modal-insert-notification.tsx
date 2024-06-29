import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

//Componentes
//import { BrowserRouter as Router, Route, Link } from 'react-router-dom';
import Link from 'next/link';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import '../../src/css/modal_insert.css'
import '../../src/css/fonts_awesome/css/all.min.css'
import { mock } from 'node:test';
import { ProductAPI } from '@/app/src/models-data/ProductAPI';
import { CategoryAPI } from '@/app/src/models-data/CategoryAPI';
import { insertNewNotificationInDBAsync, insertNewProductInDBAsync } from '@/app/src/api/get-post-api';
import { AlertShop } from '@/app/global-components/generic_overlay';
import { NotificationAPI } from '@/app/src/models-data/NotificationAPI';



//Creamos la interfaz que deben seguir los props (o parametros) para el componente Modal
interface ModalInsertProps {
    show: boolean;
    handleClose: () => void;    
}
  
export const ModalInsertNotification: React.FC<ModalInsertProps> = ({ 
    show,
    handleClose    
}) => {
    
    //Atributos del producto nuevo
    const [notificationTitle, setNotificationTitle] = useState('');
    const [notificationMessage, setNotificationMessage] = useState('');    
    const [error, setError] = useState('');

    //gestionamiento para los alert de boostrap
    const [showAlert, setShowAlert] = useState(false);
    const [alertInfo,setAlertInfo] = useState("");
    const [alertTitle,setAlertTitle] = useState("");
    const [alertType,setAlertType] = useState("");
    
    function closeAlertShop(): void {
        setShowAlert(false);     
    }
    function callAlertShop (alertType:string,alertTitle:string,alertInfo:string): void {
        setAlertTitle(alertTitle);
        setAlertInfo(alertInfo);
        setAlertType(alertType)
        setShowAlert(true);
    }
            
    const deleteNewNotificationInfo = () => {
        setNotificationTitle('');
        setNotificationMessage('');
        setError('');
    }
    
  const isValidInput = (input: string): boolean => {
    return input.trim() !== '';
  };

  const insertNewNotification = async () => {
    if (!isValidInput(notificationTitle)) {
      setError('Por favor ingrese un título de notificación válido');
      return;
    }    

    if (!isValidInput(notificationMessage)) {
      setError('Por favor ingrese un mensaje de notificación válido');
      return;
    }
    
    //La fecha cuando se envia la Notificacion
    const notificationCreationDate = new Date().toISOString();
                        
    const newNotification : NotificationAPI = {

        notifyId: 0,
        notifyTitle: notificationTitle,
        notifyMessage: notificationMessage,
        notifyCreationDate: notificationCreationDate,
        notifyStatus: 1
    }
   
    deleteNewNotificationInfo();
        
    try{        
        let response = await insertNewNotificationInDBAsync(newNotification);

        if (typeof response === "string") {
            // Si la respuesta es un mensaje de error
            callAlertShop("danger", "Inserción Fallida", response);
        } else if (response === true) {
            //Si es true desde la API
            callAlertShop("success", "Inserción Exitosa", `La nueva notificación "${newNotification.notifyTitle}" fue agregada con éxito a la base de datos`);
        } else {
            // Respuesta inesperada
            callAlertShop("danger", "Error Inesperado", "Ocurrió un error inesperado al insertar la notificación");
        }
    } catch (error) {                
        throw new Error('Failed to fetch data:' + error);
    }

    //Cerramos el modal y borramos todo
    handleClose();
  };
 

  return (
    <>
        <Modal show={show} onHide={handleClose} animation={false}>
            <Modal.Header closeButton>
            <Modal.Title>
                <div className="modal_title_btn">
                <h4>
                    <i className="fas fa-shopping-cart"></i>Insertar Nueva Notificación:
                </h4>
                </div>
            </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                {error && <div className="alert alert-danger">{error}</div>}
                <form>
                    <label>
                        Título de la Notificación:
                        <input
                            type="text"
                            value={notificationTitle}
                            onChange={(e) => setNotificationTitle(e.target.value)}
                        />
                    </label>                    

                    <label>
                        Mensaje de la Notificación:
                        <textarea
                            value={notificationMessage}
                            onChange={(e) => setNotificationMessage(e.target.value)}
                        />
                    </label>

                </form>
            </Modal.Body>
            <Modal.Footer>
            
            <Button variant="primary" onClick={deleteNewNotificationInfo}>
                Borrar Datos
            </Button>

            <Button variant="secondary" onClick={handleClose}>
                Cerrar
            </Button>
            <Button variant="primary" onClick={insertNewNotification}>
                Insertar
            </Button>
            </Modal.Footer>
        </Modal>

        <AlertShop alertTitle={alertTitle} alertInfo={alertInfo} alertType={alertType} showAlert={showAlert} onClose={closeAlertShop}/>
    </>
  );
};