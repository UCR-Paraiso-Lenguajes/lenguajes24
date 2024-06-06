import { CartShopAPI } from "./CartShopAPI";
import { PaymentMethod } from "./PaymentMethodAPI";

//Para serializar lo que venga de SaleController
export interface RegisteredSaleWeek{ 
    dayOfWeek: string;
    total: number;    
  }