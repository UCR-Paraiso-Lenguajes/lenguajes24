'use client';
import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
//para renderizar html en React
import { Parser } from 'html-to-react';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './../../../src/css/demoCSS.css'
import './../../../src/css/products_info.css'
import './../../../src/css/fonts_awesome/css/all.min.css'

//Interfaces
import { deleteNotificationInDBAsync, getAllNotificationsAdmin, getAllProductsFromAPI } from '@/app/src/api/get-post-api';
import { ModalInsertNotification } from '../../admin-components/modal-insert-notification';
import { NotificationAPI } from '@/app/src/models-data/NotificationAPI';
import { AlertShop } from '@/app/global-components/generic_overlay';

//Funciones

// @ts-ignore
const htmlToReactParser = new Parser();


export default function NotificationsInfo(){

    //cargamos los datos desde la API (StoreController)    
    const [listOfNotifications, setListOfNotifications] = useState<NotificationAPI[]>([]);    

    //States del ModalInsert
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

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
    

    //Llamamos los productos desde Store    
    useEffect(() => {
        const loadNotificationsAPI = async ()=>{
            try{            
                let dataFromStore = await getAllNotificationsAdmin();

                if (typeof dataFromStore  === "object" && dataFromStore !== null){
                    setListOfNotifications(dataFromStore);
                }
            } catch (error) {
                callAlertShop("danger","Error al obtener datos","Hubo un error al intentar obtener los mensajes de la campana  de notificaciones")
            }                    
            
        }  
        loadNotificationsAPI();
    }, []);

    const deleteNotification = async (notifyId: number) => {
        try {
            console.log("ID Notification: " + notifyId);
            console.log("Tipo: " + typeof + notifyId);
          
            if (!notifyId || notifyId <= 0) {
                callAlertShop("danger", "Datos incorrectos", "Los datos de la notificación que intentas borrar no son válidos. Inténtelo más tarde");
                return;
            }
        
            const response = await deleteNotificationInDBAsync(notifyId);
    
            if (typeof response === "string") {
    
                callAlertShop("danger", "Eliminación Fallida", response);
            } else if (response === true) {                    

                const updatedNotifications = listOfNotifications.filter(notification => notification.notifyId !== notifyId);
                setListOfNotifications(updatedNotifications);
                callAlertShop("success", "Eliminación Exitosa", `La notificación con ID ${notifyId} fue desactivada correctamente`);

            } else {                
                callAlertShop("danger", "Error Inesperado", "Ocurrió un error inesperado al intentar desactivar la notificación");
            }
        } catch (error) {            
            callAlertShop("danger", "Error al borrar notificación", "La notificación no pudo ser desactivada. Por favor, inténtelo más tarde.");
        }
    };


    return (
        <>
            <table>
                <tbody>
                    <tr>
                        <th colSpan={6}><h1>Notifications History</h1></th>
                    </tr>
                    <tr className="crud-header">
                
                        <th><button>Home</button></th>
                        <th><button onClick={handleShow}>Add New</button></th>
                    </tr>
                    <tr>
                        <th>Code</th>
                        <th>Title</th>                        
                        <th>Creation Date</th>
                        <th>Status</th>
                        <th>Actions</th>

                    </tr>

                    {listOfNotifications.map((notify,index) => (
                        <tr key={notify.notifyId} className="crud-notification-info">
                            <td>{notify.notifyId}</td>
                            <td>{notify.notifyTitle}</td>                            
                            <td>{notify.notifyCreationDate}</td>
                            <td>{notify.notifyStatus === 1 ? 'Avaliable' : 'Deleted'}</td>
                            <td className="crud-notification-container td-notify-html">
                                {notify.notifyStatus === 1 && (
                                    <button onClick={() => deleteNotification(notify.notifyId)}>Delete</button>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody> 
            </table>
            <ModalInsertNotification
                show={show}
                handleClose={handleClose}                
            />
            <AlertShop alertTitle={alertTitle} alertInfo={alertInfo} alertType={alertType} showAlert={showAlert} onClose={closeAlertShop}/>
        </>        
    );

}