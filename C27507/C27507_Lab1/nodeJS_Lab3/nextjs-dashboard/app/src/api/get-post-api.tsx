//Interfaces
import { ProductAPI } from "../models-data/ProductAPI";
import { RegisteredSaleAPI } from "../models-data/RegisteredSale";
import { RegisteredSaleReport } from "../models-data/RegisteredSaleReport";
import { RegisteredSaleWeek } from "../models-data/RegisteredSaleWeek";

    //POST Sale
    export async function sendDataAPI(directionAPI:string, data:any): Promise<string | null> {

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


    export async function getRegisteredSalesFromAPI(directionAPI: string, data: any): Promise<string | RegisteredSaleReport | null> {


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

        const directionAPI = `https://localhost:7161/store/products/product/category?category=${encodeURIComponent(idCategory)}`;
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

    export async function getProductsBySearchTextAndCategory(searchText:string,categoryIds: number[]): Promise<string | ProductAPI[] | null> {
        
        const searchTextToUrl = encodeURIComponent(searchText);        
        //se construye manualmente, ya que es por GET
        const categoryIdsToUrl = categoryIds.map(id => `categoryIds=${encodeURIComponent(id.toString())}`).join("&");        
        const directionAPI = `https://localhost:7161/store/products/product/search?searchText=${searchTextToUrl}&${categoryIdsToUrl}`;
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
