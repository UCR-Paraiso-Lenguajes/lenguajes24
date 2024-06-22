//Interfaces
import { CategoryAPI } from "../models-data/CategoryAPI";
import { ProductAPI } from "../models-data/ProductAPI";
import { RegisteredSaleAPI } from "../models-data/RegisteredSale";
import { RegisteredSaleReport } from "../models-data/RegisteredSaleReport";
import { RegisteredSaleWeek } from "../models-data/RegisteredSaleWeek";
import { UserAccountAPI } from "../models-data/UserAccountAPI";
import { NotificationsAPI } from '../models-data/NotificationsAPI';
import { useRouter } from 'next/navigation';

import { jwtDecode } from 'jwt-decode';
import { CartShopAPI } from "../models-data/CartShopAPI";
import { ProductWithoutCategoryAPI } from "../models-data/ProductWithoutCategoryAPI";
import { CartShopWithoutCategoryAPI } from "../models-data/CartShopWithoutCategoryAPI";
import { stringify } from "querystring";
//WebSockets
import * as signalR from '@microsoft/signalr';
import { useEffect, useState } from "react";




const { default: jwt_decode } = require("jwt-decode");


export async function getANotificationsFromDB():Promise<NotificationsAPI | null> {

    var connection = new signalR.HubConnectionBuilder().withUrl("beerHub").build();

    connection.start().then(function ()){
        console.log("Conexion exitosa");
    }

    connection.on("Receive", function (name,brand){                
    })
}






const NotificationComponent: React.FC = () => {
    const [notifications, setNotifications] = useState<Notifications[]>([]);

    useEffect(() => {
        
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7161/notificationHub")
            .build();
        
        connection.start().then(() => {
                console.log("Conexión exitosa");
                // Configurar el manejador para recibir notificaciones
                connection.on("Receive", (listOfnotifications: NotificationsAPI[]) => {
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
                    <li key={index}>{notification.message}</li> // Ajusta según las propiedades de tu objeto Notifications
                ))}
            </ul>
        </div>
    );
};

export default NotificationComponent;