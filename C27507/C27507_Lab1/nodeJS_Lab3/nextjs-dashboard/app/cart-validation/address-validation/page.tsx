'use client';
import React, {useEffect} from 'react';
import { useState } from 'react';

//Interfaces
import { CartShopAPI } from '@/app/src/models-data/CartShopAPI';
import { PaymentMethod, PaymentMethodNumber, PaymentMethods } from '@/app/src/models-data/PaymentMethodAPI';
//Componentes
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { AlertShop } from '@/app/global-components/generic_overlay';
import { getAllPaymentMethodsToAdmin, sendCartDataToAPI } from '@/app/src/api/get-post-api';

//Funciones
import {getCartShopStorage, setCartShopStorage, deleteAllProduct } from '@/app/src/storage/cart-storage';


//Modelos para la API
type Province = [string, string];
type Canton = [string, string];
type District = [string, string];

export default function ModalDirection() {  

    //manejar carrito
    const [myCartInStorage, setMyCartInStorage] = useState<CartShopAPI | null>(getCartShopStorage("A"));        
    //cargamos los datos de pago desde la API (StoreController)    
    const [listOfPaymentMethods, setListOfPaymentMethods] = useState<PaymentMethod[]>([]);

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


    // Estados y manejaodres para provincias, cantones y distritos    
    const [provinces, setProvinces] = useState<Province[]>([]);
    const [selectedProvince, setSelectedProvince] = useState<string>('');

    const handleProvinceChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedProvince(event.target.value);
    };
    const [cantons, setCantons] = useState<Canton[]>([]);
    const [selectedCanton, setSelectedCanton] = useState<string>('');
    const handleCantonChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedCanton(event.target.value);
    };        
    const [districts, setDistricts] = useState<District[]>([]);
    const [selectedDistrict, setSelectedDistrict] = useState<string>('');
    const handleDistrictChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setSelectedDistrict(event.target.value);
    };





    //Obtener los metodos de pago de Store
    useEffect(() => {
        const loadPaymentMethodsAPI = async ()=>{
            try{            
                let dataFromStore = await getAllPaymentMethodsToAdmin();                
                if (Array.isArray(dataFromStore)) {
                    let verifiedPaymentMethods = dataFromStore.filter((method: { verify: any; }) => method.verify);
                    setListOfPaymentMethods(verifiedPaymentMethods);
                } else {
                    callAlertShop("danger","Éxito al","VACIO");
                }
                 
            } catch (error) {
                callAlertShop("danger","Error al obtener datos","Hubo un error al intentar obtener los mensajes de la campana  de notificaciones")
            }                    
            
        }  
        loadPaymentMethodsAPI();
    }, []);    


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
    
    //Seleccionar Lugares Completos
    useEffect(() => {
        fetch('https://ubicaciones.paginasweb.cr/provincias.json')
            .then(response => response.json())
            .then(data => {
                const provincesArray: Province[] = Object.entries(data) as Province[];
                setProvinces(provincesArray);
            });
    }, []);

    // Cargar cantones cuando se seleccione una provincia
    useEffect(() => {
        if (selectedProvince) {
            fetch(`https://ubicaciones.paginasweb.cr/provincia/${selectedProvince}/cantones.json`)
                .then(response => response.json())
                .then(data => {
                    const cantonsArray: Canton[] = Object.entries(data) as Canton[];
                    setCantons(cantonsArray);
                });
        } else {
            setCantons([]);
        }
    }, [selectedProvince]);

    // Cargar distritos cuando se seleccione un cantón
    useEffect(() => {
        if (selectedCanton) {
            fetch(`https://ubicaciones.paginasweb.cr/provincia/${selectedProvince}/canton/${selectedCanton}/distritos.json`)
                .then(response => response.json())
                .then(data => {
                    const districtsArray: District[] = Object.entries(data) as District[];
                    setDistricts(districtsArray);
                });
        } else {
            setDistricts([]);
        }
    }, [selectedCanton]);

    
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
                

         // Validación de selección de provincia
        if (!selectedProvince) {
            newErrors.selectedProvince = 'Seleccione una provincia';
        }

        // Validación de selección de cantón
        if (!selectedCanton) {
            newErrors.selectedCanton = 'Seleccione un cantón';
        }

        // Validación de selección de distrito
        if (!selectedDistrict) {
            newErrors.selectedDistrict = 'Seleccione un distrito';
        }
                
        let isSelectPaymentValid = !payment || payment === '';
        if(isSelectPaymentValid){            
            newErrors.payment = 'Seleccione un tipo de pago';                        
        }

        //Agregamos y concatenamos las direcciones:        
        let completeDirection = `${selectedProvince}, ${selectedCanton}, ${selectedDistrict}`;
        if (myCartInStorage) {
            myCartInStorage.direction = completeDirection;
            setCartShopStorage("A", myCartInStorage);            
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
            const purchaseNum = await sendCartDataToAPI(myCartInStorage);            
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
                    <Form.Label style={{ fontWeight: 'bolder' }}>Dirección de entrega:</Form.Label>
                    <Form.Group className="mb-3" controlId='province'>
                        <Form.Label style={{ fontWeight: 'bolder' }}>Provincia:</Form.Label>
                        <Form.Select value={selectedProvince} onChange={handleProvinceChange}>
                            <option value="">Seleccione una provincia:</option>
                            {provinces.map(([id, name]) => (
                                <option key={id} value={id}>{name}</option>
                            ))}
                        </Form.Select>
                        {submitAttempted && errors.selectedProvince && (
                            <Form.Control.Feedback type='invalid'>{errors.selectedProvince}</Form.Control.Feedback>
                        )}
                    </Form.Group>

                    <Form.Group className="mb-3" controlId='canton'>
                        <Form.Label style={{ fontWeight: 'bolder' }}>Cantón:</Form.Label>
                        <Form.Select value={selectedCanton} onChange={handleCantonChange}>
                            <option value="">Seleccione un cantón:</option>
                            {cantons.map(([id, name]) => (
                                <option key={id} value={id}>{name}</option>
                            ))}
                        </Form.Select>
                        {submitAttempted && errors.selectedCanton && (
                            <Form.Control.Feedback type='invalid'>{errors.selectedCanton}</Form.Control.Feedback>
                        )}
                    </Form.Group>

                    <Form.Group className="mb-3" controlId='district'>
                        <Form.Label style={{ fontWeight: 'bolder' }}>Distrito:</Form.Label>
                        <Form.Select value={selectedDistrict} onChange={handleDistrictChange}>
                            <option value="">Seleccione un distrito:</option>
                            {districts.map(([id, name]) => (
                                <option key={id} value={id}>{name}</option>
                            ))}
                        </Form.Select>
                        {submitAttempted && errors.selectedDistrict && (
                            <Form.Control.Feedback type='invalid'>{errors.selectedDistrict}</Form.Control.Feedback>
                        )}
                    </Form.Group>
                                                
                    <Form.Group className="mb-3">
                        <Form.Label htmlFor="disabledSelect" style={{ fontWeight: 'bolder' }}>Forma de pago:</Form.Label>                                        
                        <Form.Select id="disabledSelect" 
                            value={selectPayment} 
                            onChange={(e)=>{setField("payment",e.target.value);getSelectPayment(e);}}
                            isInvalid={submitAttempted && !!errors.payment}
                        >             
                            <option value="">Seleccione un tipo de pago:</option>                                        
                            {listOfPaymentMethods.map((method) => (
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
