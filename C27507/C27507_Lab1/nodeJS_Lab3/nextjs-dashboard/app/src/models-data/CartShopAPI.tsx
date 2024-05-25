import { ProductAPI } from "./ProductAPI";
import { PaymentMethod,PaymentMethods,PaymentMethodNumber } from "./PaymentMethodAPI";
//Interfaces para serializar las objetos JSON de la API
export interface CartShopAPI {  
    allProduct: ProductAPI[];
    subtotal: number;
    tax: number;
    total: number;
    direction: string; 
    paymentMethod: PaymentMethod
  }