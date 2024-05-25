import { ProductAPI } from "./ProductAPI";
//Interfaces para serializar las objetos JSON de la API
export interface StoreAPI{
    products: ProductAPI[],
    taxPorcentage: number
   }