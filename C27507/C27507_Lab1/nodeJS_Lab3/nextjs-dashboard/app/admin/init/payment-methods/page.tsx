'use client';
import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './../../../src/css/demoCSS.css'
import './../../../src/css/products_info.css'
import './../../../src/css/fonts_awesome/css/all.min.css'

//Interfaces
import { deleteNotificationInDBAsync, getAllNotificationsAdmin, getAllPaymentMethodsToAdmin, getAllProductsFromAPI, updateStatusPaymentMethod } from '@/app/src/api/get-post-api';
import { AlertShop } from '@/app/global-components/generic_overlay';
import { PaymentMethod, PaymentMethodNumber } from '@/app/src/models-data/PaymentMethodAPI';


export default function PaymentMethodAdmin(){

    //cargamos los datos desde la API (StoreController)    
    const [listOfPaymentMethods, setListOfPaymentMethods] = useState<PaymentMethod[]>([]);    

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
        const loadPaymentMethodsAPI = async ()=>{
            try{            
                let dataFromStore = await getAllPaymentMethodsToAdmin();
                
                if (Array.isArray(dataFromStore)) {
                    setListOfPaymentMethods(dataFromStore);                    
                } else {
                    callAlertShop("danger","Vacio","VACIO");
                }
                 
            } catch (error) {
                callAlertShop("danger","Error al obtener datos","Hubo un error al intentar obtener los mensajes de la campana  de notificaciones")
            }                    
            
        }  
        loadPaymentMethodsAPI();
    }, []);


    const getDescriptionForPaymentMethod = (paymentMethodId: PaymentMethodNumber) => {
        switch (paymentMethodId) {
            case PaymentMethodNumber.CASH:
                return 'Cash';
            case PaymentMethodNumber.CREDIT_CARD:
                return 'Credit Card';
            case PaymentMethodNumber.DEBIT_CARD:
                return 'Debit Card';
            case PaymentMethodNumber.SINPE:
                return 'SINPE';
            default:
                return 'Error';
        }
    };


    //Desactivar/Actiar el metodo de pago
    const togglePaymentMethodStatus = async (paymentMethodId: number, currentStatus: boolean) => {
        try {
            console.log("Status:" + currentStatus);
            let newStatus = 0;   
            if(currentStatus === true){
                newStatus = 0;
            }else{
                newStatus = 1;
            }

            const updatedList = listOfPaymentMethods.map((pm) =>
                pm.payment === paymentMethodId ? { ...pm, verify: !currentStatus } : pm
            );            
            
            var response =await updateStatusPaymentMethod(paymentMethodId, newStatus);                
            //Siempre sera boolean true, pero si ocurre un error en la API entonces el resultado es un string con el error 
            if (typeof response !== "boolean") {
    
                callAlertShop("danger", "Desactiviación Fallida", `La desactivación del método ${paymentMethodId} no pudo ser realizada`);            
            } else {                
                callAlertShop("success", "Eliminación Exitosa", `La desactivación del método ${paymentMethodId} fue hecha correctamente`);                

                //Si se cambia en la BD entonces se cambia aqui, pero sin llamar a la BD otra vez:
                setListOfPaymentMethods(updatedList);
            }

            
        } catch (error) {
            
        }
    };
    
    return (
        <>
            <table>
                <tbody>
                    <tr>
                        <th colSpan={6}><h1>Payment Methods Management</h1></th>
                    </tr>
                    {/* <tr className="crud-header">
                
                        <th><button>Home</button></th>
                        <th><button onClick={handleShow}>Add New</button></th>
                    </tr> */}
                    <tr>
                        <th>Payment Code</th>
                        <th>Description</th>
                        <th>Status</th>
                        <th>Actions</th>

                    </tr>

                    {listOfPaymentMethods.map((paymentMethod) => (
                        <tr key={paymentMethod.payment}>
                        <td>{paymentMethod.payment}</td>
                        <td>{getDescriptionForPaymentMethod(paymentMethod.payment)}</td>
                        <td>{paymentMethod.verify ? 'Active' : 'Inactive'}</td>
                        <td>
                            <button onClick={() => togglePaymentMethodStatus(paymentMethod.payment, paymentMethod.verify)}>
                                {paymentMethod.verify ? 'Deactivate' : 'Activate'}
                            </button>
                        </td>
                    </tr>
                    ))}
                </tbody> 
            </table>            
            <AlertShop alertTitle={alertTitle} alertInfo={alertInfo} alertType={alertType} showAlert={showAlert} onClose={closeAlertShop}/>
        </>        
    );

}