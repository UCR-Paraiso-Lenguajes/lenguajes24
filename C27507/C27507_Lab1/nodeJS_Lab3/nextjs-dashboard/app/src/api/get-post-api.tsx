//Interfaces
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
