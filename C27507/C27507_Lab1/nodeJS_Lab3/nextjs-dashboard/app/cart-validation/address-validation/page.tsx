'use client';
import React, {useEffect} from 'react';
import { useState } from 'react';

//Interfaces
import { CartShopAPI } from '@/app/src/models-data/CartShopAPI';
import { PaymentMethodNumber, PaymentMethods } from '@/app/src/models-data/PaymentMethodAPI';
//Componentes
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { AlertShop } from '@/app/global-components/generic_overlay';
import { sendDataAPI } from '@/app/src/api/get-post-api';

//Funciones
import {getCartShopStorage, setCartShopStorage, deleteAllProduct } from '@/app/src/storage/cart-storage';
import { useRouter } from 'next/router';

export default function ModalDirection() {  

    //manejar carrito
    const [myCartInStorage, setMyCartInStorage] = useState<CartShopAPI | null>(getCartShopStorage("A"));        

    //Estados  para los alert de Boostrap
    const [showAlert, setShowAlert] = useState(false);
    const [alertInfo,setAlertInfo] = useState("");
    const [alertTitle,setAlertTitle] = useState("");
    const [alertType,setAlertType] = useState("");

    //Estados de los campos del formulario        
    const [textAreaDataDirection, setTextAreaDataDirection] = useState("");
    const [selectPayment, setPayment] = useState<PaymentMethodNumber>(myCartInStorage?.paymentMethod.payment ?? PaymentMethodNumber.CASH);
    const [textAreaSinpe, setTextAreaSinpe] = useState("");//campo del codigo del sinpe    

    //Estados del formulario:
        //objeto que guarda los valores de los campos como propiedades sin definir
    const [form,setForm] = useState<{[key:string]: string | null}>({});    
        //objeto que guarda los errores de los campos como propiedades sin definir
    const [errors, setErrors] = useState<{[key:string]: string | null}>({});
    const [submitAttempted, setSubmitAttempted] = useState(false);


    //Cada vez que se produzca un cambio en los campos de los formularios, guardamos su estado
    const setField = (field: string,valueFromForm: any)=>{
        setForm({...form,[field]:valueFromForm});
        
        //Si no existe un atributo con ese nombre de campo, entonces lo creamos
        //pero con error nulo, ya que no sabemos si el dato es correcto o erroneo
        if(!!errors[field]){
            setErrors({...errors,[field]:null})
        }        
    }
    

    //funciones para gestionar los alert
    function closeAlertShop(): void {
        setShowAlert(false);     
    }
    function callAlertShop (alertType:string,alertTitle:string,alertInfo:string): void {
        setAlertTitle(alertTitle);
        setAlertInfo(alertInfo);
        setAlertType(alertType)
        setShowAlert(true);
    }
    
    const getSelectPayment = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const actualValue = parseInt(event.target.value);        

        if (myCartInStorage) {
            // Verificamos si existe paymentMethod antes de modificarlo
            if (myCartInStorage.paymentMethod) {
                myCartInStorage.paymentMethod.payment = actualValue;
            } else {
                // Si no existe paymentMethod lo creamos
                myCartInStorage.paymentMethod = {
                    payment: actualValue,
                    verify: false
                };
            }
            setCartShopStorage("A", myCartInStorage);                        
            setPayment(actualValue);
        }        
    };


    //Para guardar la informacion en el carrito local storage
    const getTextAreaDirection = (event: React.ChangeEvent<any>) =>{        
        const actualValue = event.target.value;          
        setTextAreaDataDirection(actualValue);           
        if (myCartInStorage) {
            myCartInStorage.direction = textAreaDataDirection;
            setCartShopStorage("A", myCartInStorage);            
        }            
    }
    const getTextAreaSinpe = (event: React.ChangeEvent<any>) =>{        
        const actualValue = event.target.value;        
        setTextAreaSinpe(actualValue);                  
    }    
          
    //Reiniciar los datos al cerrar el modal
    const resetModal = () =>{
        setTextAreaDataDirection("");
        setTextAreaSinpe("");      
        deleteAllProduct(myCartInStorage,setMyCartInStorage);       
    }

    //Validcacion de inputs
    const purchaseProcess = async () =>{

        setSubmitAttempted(true);
        const {address,payment,paymentSINPE} = form;
        const newErrors: { [key: string]: string | null } = {};
        
        let isAddressValid = !address || address === '';
        if(isAddressValid){
            newErrors.address = 'El campo de direccion debe contener informacion';
        }
                
        let isSelectPaymentValid = !payment || payment === '';
        if(isSelectPaymentValid){            
            newErrors.payment = 'Seleccione un tipo de pago';                        
        }

        //Verificar si payment es del tipo SINPE, ya que se podria activar el textArea del codigo SINPE
        //estando en otro tipo de pago por algun error  
        var isPaymentIntValue = payment && parseInt(payment) === PaymentMethodNumber.SINPE
        if (isPaymentIntValue) {
            if (!paymentSINPE || paymentSINPE === "") {
                newErrors.paymentSINPE = 'Introduzca el código obtenido del SINPE';
            }
        }        

        setErrors(newErrors);
        
        if(Object.keys(newErrors).length === 0){                        
            console.log(form);            
            const purchaseNum = await sendDataAPI("https://localhost:7161/api/Cart", myCartInStorage);        
            resetModal();//setteamos el modal o mandamos el resumen a la pagina            
            callAlertShop("success","Compra finalizada","El codigo de su compra es: " + purchaseNum);   
        }else{
            callAlertShop("danger","Campos de formulario incompletos","Por favor, verifique que los campos esten llenos y con la informacion solicitada")
            
        }
    }    

    return (
        <>
                                        
            <Form>
                <fieldset>
                    <Form.Group className="mb-3" controlId='address'>
                        <Form.Label style={{ fontWeight: 'bolder' }}>Dirección de entrega:</Form.Label>
                        <Form.Control 
                            rows={5} 
                            as="textarea" 
                            placeholder="Ingresa tu dirección para la entrega" 
                            value={textAreaDataDirection} 
                            onChange={(e) => {setField("address",e.target.value);getTextAreaDirection(e);}} 
                            isInvalid={submitAttempted && !!errors.address} 
                        />
                        {submitAttempted && errors.address && (
                            <Form.Control.Feedback type='invalid'>{errors.address}</Form.Control.Feedback>
                        )}
                        <Form.Text className="text-muted">
                            Tu información es confidencial con nosotros
                        </Form.Text>

                    </Form.Group>    
                                                
                    <Form.Group className="mb-3">
                        <Form.Label htmlFor="disabledSelect" style={{ fontWeight: 'bolder' }}>Forma de pago:</Form.Label>                                        
                        <Form.Select id="disabledSelect" 
                            value={selectPayment} 
                            onChange={(e)=>{setField("payment",e.target.value);getSelectPayment(e);}}
                            isInvalid={submitAttempted && !!errors.payment}
                        >                                        
                                <option value="">Seleccione un tipo de pago:</option>                                        
                                {PaymentMethods.map((method) => (
                                    <option key={method.payment} value={method.payment}>{PaymentMethodNumber[method.payment]}</option>
                                ))}
                        </Form.Select>
                        {submitAttempted && errors.payment && (
                            <Form.Control.Feedback type='invalid'>{errors.payment}</Form.Control.Feedback>
                        )}
                    </Form.Group>
                    
                    {selectPayment == PaymentMethodNumber.SINPE && (

                    <>
                        <Form.Group className="mb-3">
                            <Form.Label><span style={{ fontWeight: 'bolder' }}>Nuestro número de teléfono:</span> 62889872</Form.Label>
                            <br />                                                                      
                            <br />                                  
                            <Form.Label style={{ fontWeight: 'bolder' }}>Comprobante del SINPE:</Form.Label>
                            <Form.Control 
                                rows={5} 
                                as="textarea" 
                                placeholder="Ingrese su comprobante" 
                                onChange={(e) => {setField("paymentSINPE",e.target.value);getTextAreaSinpe(e);}}/>
                            {submitAttempted && errors.paymentSINPE && (
                                <Form.Control.Feedback type="invalid">{errors.paymentSINPE}</Form.Control.Feedback>
                            )}

                        </Form.Group>
                    </>

                    )}                    
                    <Button type="button" onClick={purchaseProcess}>Finalizar Compra</Button>                                                                                                    
                </fieldset>
            </Form>                
            <AlertShop alertTitle={alertTitle} alertInfo={alertInfo} alertType={alertType} showAlert={showAlert} onClose={closeAlertShop}/>                    
        </>
    );
}
