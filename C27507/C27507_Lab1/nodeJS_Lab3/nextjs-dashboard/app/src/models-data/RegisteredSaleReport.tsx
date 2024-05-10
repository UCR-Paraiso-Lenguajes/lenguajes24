import { getRegisteredSalesFromAPI } from "../api/get-post-api"
import { RegisteredSaleAPI } from "./RegisteredSale";
import { RegisteredSaleWeek } from "./RegisteredSaleWeek";

export interface RegisteredSaleReport{
    salesByDay: RegisteredSaleAPI[];
    salesByWeek: RegisteredSaleWeek[];    
}