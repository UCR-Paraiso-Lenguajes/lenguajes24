'use client';
import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
//para renderizar html en React
import { Parser } from 'html-to-react';




import { useRouter } from 'next/router';
import Dropdown from 'react-bootstrap/Dropdown';


//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './../../../src/css/demoCSS.css'
import './../../../src/css/products_info.css'
import './../../../src/css/fonts_awesome/css/all.min.css'
import { getAllProductsFromAPI } from '@/app/src/api/get-post-api';
import { ProductAPI } from '@/app/src/models-data/ProductAPI';
import { CategoryAPI } from '@/app/src/models-data/CategoryAPI';
import { CartShopAPI } from '@/app/src/models-data/CartShopAPI';
import { getCartShopStorage } from '@/app/src/storage/cart-storage';
import { ModalInsert } from '../../admin-components/modal-insert';
import { ModalInsertNotification } from '../../admin-components/modal-insert-notification';
import { NotificationAPI } from '@/app/src/models-data/NotificationAPI';

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
    

    //Llamamos los productos desde Store    
    useEffect(() => {
        const loadDataProductAPI = async ()=>{
            try{            
                let dataFromStore = await getAllProductsFromAPI();

                if (typeof dataFromStore  === "object" && dataFromStore !== null){
                    setListOfNotifications(dataFromStore.productsFromStore);                    
                }
                

            } catch (error) {                
                throw new Error('Failed to fetch data:' + error);
            }
        }  
        loadDataProductAPI();
    }, []);


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
                        <th>Código</th>
                        <th>Título</th>
                        <th>Contenido</th>
                        <th>Fecha de Creación</th>                        
                    </tr>

                    {listOfNotifications.map((notify,index) => (
                        <tr key={notify.id} className="crud-notification-info">
                            <td>{notify.notifyTitle}</td>                            
                            <td>
                                {htmlToReactParser.parse(notify.notifyMessage)}
                            </td>                                                        
                            <td>{notify.notifyCreationDate}</td>
                            <td>sdsds</td>                
                            <td className="crud-notification-container">
                                <button>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody> 
            </table>
            <ModalInsertNotification
                show={show}
                handleClose={handleClose}
                listOfNotifications={listOfNotifications}
            />
        </>
        //https://vaidrollteam.blogspot.com/2023/03/crud-basico-de-productos-en-php-mysql.html
    );

}