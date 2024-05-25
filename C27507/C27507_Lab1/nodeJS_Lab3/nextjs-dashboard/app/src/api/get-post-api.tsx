//Interfaces
import { CategoryAPI } from "../models-data/CategoryAPI";
import { ProductAPI } from "../models-data/ProductAPI";
import { RegisteredSaleAPI } from "../models-data/RegisteredSale";
import { RegisteredSaleReport } from "../models-data/RegisteredSaleReport";
import { RegisteredSaleWeek } from "../models-data/RegisteredSaleWeek";


    export async function getAllProductsFromAPI():Promise<string | { productsFromStore: ProductAPI[], categoriesFromStore: CategoryAPI[] } | null> {
        console.log(process.env.NODE_ENV);
        let urlByReactEnviroment = process.env.NODE_ENV === 'production'
        ? process.env.NEXT_PUBLIC_API        
        : process.env.NEXT_PUBLIC_LOCAL_API;

        let directionAPI = `${urlByReactEnviroment}/api/Store`;
        
        try {

            const response = await fetch(directionAPI)
            if (!response.ok){                
                //Obtenemos el mensaje de error de CartController
                const errorMessage = await response.text();
                return errorMessage;
            }
            const dataStore = await response.json();
            console.log(dataStore.products);
            console.log(dataStore.allProductCategories);
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

        //Seleccion la url de la API dependiendo del ambiente de la APP                
        console.log(process.env.NODE_ENV);
        let urlByReactEnviroment = process.env.NODE_ENV === 'production'
        ? process.env.NEXT_PUBLIC_API        
        : process.env.NEXT_PUBLIC_LOCAL_API;

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
        
        console.log(process.env.NODE_ENV);
        let urlByReactEnviroment = process.env.NODE_ENV === 'production'
        ? process.env.NEXT_PUBLIC_API        
        : process.env.NEXT_PUBLIC_LOCAL_API;

        let directionAPI = `${urlByReactEnviroment}/api/Sale`;

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
            let responsePost = await fetch(directionAPI,postConfig);
            if(!responsePost.ok){                
                const errorMessage = await responsePost.text();                
                return errorMessage;
            }        
            const jsonRegisteredSales = await responsePost.json();            

            return jsonRegisteredSales.specificListOfRegisteredSales;
            
        } catch (error) {            
            throw new Error('Failed to POST data: '+ error);
        }        
    }


    export async function getProductsByCategory(idCategory: number): Promise<string | ProductAPI[] | null> {        
        console.log(process.env.NODE_ENV);
        let urlByReactEnviroment = process.env.NODE_ENV === 'production'
        ? process.env.NEXT_PUBLIC_API        
        : process.env.NEXT_PUBLIC_LOCAL_API;

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
