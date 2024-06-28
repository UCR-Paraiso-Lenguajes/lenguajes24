//Interfaces
import { CategoryAPI } from "../models-data/CategoryAPI";
import { ProductAPI } from "../models-data/ProductAPI";
import { RegisteredSaleAPI } from "../models-data/RegisteredSale";
import { RegisteredSaleReport } from "../models-data/RegisteredSaleReport";
import { RegisteredSaleWeek } from "../models-data/RegisteredSaleWeek";
import { UserAccountAPI } from "../models-data/UserAccountAPI";
import { useRouter } from 'next/navigation';

import { jwtDecode } from 'jwt-decode';
import { CartShopAPI } from "../models-data/CartShopAPI";
import { stringify } from "querystring";
import { NotificationAPI } from "../models-data/NotificationAPI";


const { default: jwt_decode } = require("jwt-decode");


    export async function getAllProductsFromAPI():Promise<string | { productsFromStore: ProductAPI[], categoriesFromStore: CategoryAPI[] } | null> {
        //let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV;
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
        let directionAPI = `${urlByReactEnviroment}/api/Store`;
                
        try {

            const response = await fetch(directionAPI)
            if (!response.ok){                
                //Obtenemos el mensaje de error de CartController
                const errorMessage = await response.text();
                return errorMessage;
            }
            const dataStore = await response.json();            
            return {
                productsFromStore: dataStore.products,
                categoriesFromStore: dataStore.allProductCategories
            };            
            
        } catch (error) {
            throw new Error('Failed to fetch data');                
        }

    }

    //POST Sale
    export async function sendCartDataToAPI(data:any): Promise<string | null> {
                
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';

        let directionAPI = `${urlByReactEnviroment}/api/Cart`;
        //Especificacion POST
        let postConfig = {
            method: "POST",
            //pasamos un objeto como atributo de otro
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        }
    
        try {
    
                //A las peticiones POST se les debe agregar parametro de configuracion para diferenciarlas de las
            //GET            
            let responsePost = await fetch(directionAPI,postConfig);
            //await solo se puede usar dentro de funciones asincronas
    
            if(!responsePost.ok){
                //Obtenemos el mensaje de error de CartController
                const errorMessage = await responsePost.text();
                return errorMessage;
            }
            // Obtener los datos de la respuesta en formato JSON                        
            const responseData = await responsePost.json();        
            const purchaseNum = responseData.purchaseNum;        
            return purchaseNum;
            
        } catch (error) {
            throw new Error('Failed to POST data: '+ error);
        }        
    }    

    export async function getRegisteredSalesFromAPI(data: any): Promise<string | RegisteredSaleReport | null> {
        //const router = useRouter();       
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';

        let directionAPI = `${urlByReactEnviroment}/api/Sale`;
        
        //Validamos si el token ha expirado
        let loginToken = sessionStorage.getItem("loginToken");
        if (!loginToken) {            
            window.location.reload();
            return "Default Error";
        }
        let tokenFormat = jwtDecode(loginToken);

        let todayDate = Date.now() / 1000;
        let tokenLifeTime = tokenFormat.exp;
        if (tokenLifeTime && tokenLifeTime < todayDate) window.location.reload();

        let postConfig = {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                'Authorization': `Bearer ${loginToken}`
            },
            body: JSON.stringify(data)
        };
    
        try {         
            let responsePost = await fetch(directionAPI,postConfig);
            if(!responsePost.ok){                
                const errorMessage = await responsePost.text();                                
                return errorMessage;
            }        
            let jsonRegisteredSales = await responsePost.json();            

            return jsonRegisteredSales.specificListOfRegisteredSales;
            
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }

    export async function getProductsByCategory(idCategory: number): Promise<string | ProductAPI[] | null> {        
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
        
        let directionAPI = `${urlByReactEnviroment}/api/products/store/product?category=${encodeURIComponent(idCategory)}`;

        //Especificacion POST
        let getConfig = {
            method: "GET",
            //pasamos un objeto como atributo de otro
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            }
        }
    
        try {         
            let responsePost = await fetch(directionAPI,getConfig);
            if(!responsePost.ok){                
                const errorMessage = await responsePost.text();                
                return errorMessage;
            }        
            const productsFilteredFromAPI = await responsePost.json();                      
            return productsFilteredFromAPI;
            
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }

    export async function validateUserAndGetToken(userData: UserAccountAPI): Promise<string | null> {
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
        
        let directionAPI = `${urlByReactEnviroment}/api/auth`;
        
        //Especificacion POST
        let postConfig = {
            method: "POST",
            //pasamos un objeto como atributo de otro
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"
            },
            body: JSON.stringify(userData)
        }        
    
        try {         
            let responsePost = await fetch(directionAPI,postConfig);
            if(!responsePost.ok){                
                const errorMessage = await responsePost.text();                
                return errorMessage;
            }        
            const userToken = await responsePost.json();
            const tokenValue = userToken.token;            
            return tokenValue;
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }
    
    export async function getProductsBySearchTextAndCategory(searchText:string,categoryIds: number[]): Promise<string | ProductAPI[] | null> {
        
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
        const searchTextToUrl = encodeURIComponent(searchText);        
        //se construye manualmente, ya que es por GET
        const categoryIdsToUrl = categoryIds.map(id => `categoryIds=${encodeURIComponent(id.toString())}`).join("&");        
        const directionAPI = `${urlByReactEnviroment}/store/products/product/search?searchText=${searchTextToUrl}&${categoryIdsToUrl}`;
                        
        //Especificacion POST
        let getConfig = {
            method: "GET"            
        }
    
        try {         
            let responsePost = await fetch(directionAPI,getConfig);            
            if(!responsePost.ok){                                
                const errorMessage = await responsePost.text();                                
                return errorMessage;
            }        
            const productsFilteredFromAPI = await responsePost.json();                      
            return productsFilteredFromAPI;          
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }



    //CRUD
    export async function insertNewProductInDBAsync(newProduct: ProductAPI): Promise<string | boolean | null> {
                
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
        let directionAPI = `${urlByReactEnviroment}/api/ProductManagement/product/insert`;


        //Validamos si el token ha expirado
        let loginToken = sessionStorage.getItem("loginToken");
        if (!loginToken) {            
            window.location.reload();
            return "Default Error";
        }
        let tokenFormat = jwtDecode(loginToken);

        let todayDate = Date.now() / 1000;
        let tokenLifeTime = tokenFormat.exp;
        if (tokenLifeTime && tokenLifeTime < todayDate) window.location.reload();

        //Especificacion POST
        let postConfig = {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
                'Authorization': `Bearer ${loginToken}`
            },
            body: JSON.stringify(newProduct)
        }            
    
        try {                 
            let responsePost = await fetch(directionAPI,postConfig);            
            if(!responsePost.ok){                                
                const errorMessage = await responsePost.text();                                
                return errorMessage;
            }        
            const insertedWithSuccess = await responsePost.json();                      
            return insertedWithSuccess;
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }


    export async function getAllNotificationsAdmin(): Promise<string | NotificationAPI[] | null> {

        //let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV;
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
        let directionAPI = `${urlByReactEnviroment}/api/CampaignManagement/campaign/select`;
                
        try {            
            const response = await fetch(directionAPI)
            if (!response.ok){                
                //Obtenemos el mensaje de error de CartController
                const errorMessage = await response.text();
                return errorMessage;
            }
            const notificationList = await response.json();            
            return notificationList;
            
        } catch (error) {
            throw new Error('Failed to fetch data');                
        }

    }


    export async function insertNewNotificationInDBAsync(newNotification: NotificationAPI): Promise<string | boolean | null> {
                
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';
        let directionAPI = `${urlByReactEnviroment}/api/CampaignManagement/campaign/insert`;


        //Validamos si el token ha expirado
        // let loginToken = sessionStorage.getItem("loginToken");
        // if (!loginToken) {            
        //     window.location.reload();
        //     return "Default Error";
        // }
        // let tokenFormat = jwtDecode(loginToken);

        // let todayDate = Date.now() / 1000;
        // let tokenLifeTime = tokenFormat.exp;
        // if (tokenLifeTime && tokenLifeTime < todayDate) window.location.reload();
        //'Authorization': `Bearer ${loginToken}`
        //Especificacion POST
        let postConfig = {
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json"          
            },
            body: JSON.stringify(newNotification)
        }
        console.log(newNotification);
    
        try {                 
            let responsePost = await fetch(directionAPI,postConfig);            
            if(!responsePost.ok){                                
                const errorMessage = await responsePost.text();                                
                return errorMessage;
            }        
            const insertedWithSuccess = await responsePost.json();                      
            return insertedWithSuccess;
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }

    export async function deleteNotificationInDBAsync(idNotification: number): Promise<string | boolean | null> {
                
        let urlByReactEnviroment = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7161';        
        let directionAPI = `${urlByReactEnviroment}/api/CampaignManagement/campaign/delete/${idNotification}`;

        //Validamos si el token ha expirado
        // let loginToken = sessionStorage.getItem("loginToken");
        // if (!loginToken) {            
        //     window.location.reload();
        //     return "Default Error";
        // }
        // let tokenFormat = jwtDecode(loginToken);

        // let todayDate = Date.now() / 1000;
        // let tokenLifeTime = tokenFormat.exp;
        // if (tokenLifeTime && tokenLifeTime < todayDate) window.location.reload();

        //Especificacion POST
        let deleteConfig  = {
            method: "DELETE",
            headers: {
                "Accept": "application/json",
                "Content-Type": "application/json",
            }            
        }            
    
        try {                 
            let responsePost = await fetch(directionAPI,deleteConfig );            
            if(!responsePost.ok){                                
                const errorMessage = await responsePost.text();                                
                return errorMessage;
            }        
            const deletedWithSuccess = await responsePost.json();                      
            return deletedWithSuccess;
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }



