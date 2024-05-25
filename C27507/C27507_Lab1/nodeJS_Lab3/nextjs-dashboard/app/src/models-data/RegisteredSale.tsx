import { CartShopAPI } from "./CartShopAPI";
import { PaymentMethod } from "./PaymentMethodAPI";

//Para serializar lo que venga de SaleController
export interface RegisteredSaleAPI{ 
    idSale: number;
    purchaseNum: string;
    subTotal: number;
    total: number;        
    tax: number;
    direction: string; 
    paymentMethod: PaymentMethod
    dateTimeSale: string;
  }