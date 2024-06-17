import { CategoryAPI } from "./CategoryAPI";

//Interfaces para serializar las objetos JSON de la API
export interface ProductAPI {
    id: number | null,  
    name: string;
    imageUrl: string;
    price: number;
    quantity: number;  
    description: string;    
    category?: CategoryAPI;
  }