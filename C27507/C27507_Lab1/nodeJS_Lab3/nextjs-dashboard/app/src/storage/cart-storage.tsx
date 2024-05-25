//Interfaces
import { CartShopAPI } from "../models-data/CartShopAPI";
import { PaymentMethod, PaymentMethodNumber } from "../models-data/PaymentMethodAPI";
import { ProductAPI } from "../models-data/ProductAPI";
//Funciones
import { totalPriceNoTax, totalPriceTax } from "../util-utility/tax-functions";

    //Crear un nuevo carrito
    export const setCartShopStorage = (key: string, mockup: CartShopAPI | null) => {
        if(mockup !== null){
            //como no es nulo, se guarda en el localStorage
            const cartShopData = JSON.stringify(mockup);
            localStorage.setItem(key,cartShopData);
        }
    }
    
        //Leemos lo que esta dentro del carrito o sea cartShopItem    
    export const getCartShopStorage = (key: string): CartShopAPI | null => {
            
        const cartShopData = localStorage.getItem(key);
        if(cartShopData !== null){
            return JSON.parse(cartShopData) as CartShopAPI;        
        }
    
        const defaultPaymentMethod: PaymentMethod = {
            payment: PaymentMethodNumber.CASH,
            verify: false
        };
    
        let cart: CartShopAPI = {  
            allProduct: [],
            subtotal: 0,
            tax: 0.13,
            total: 0,
            direction: "",            
            paymentMethod: defaultPaymentMethod 
    
        };
        //guardamos el carrito en el storage y luego se lo retornamos al state myCartInStorage
        setCartShopStorage("A",cart);
        return cart;    
    }
    
        //verficar si un producto ha sido agregado o no
    export const verifyProductInCart = (id:number, allProductsInCart: ProductAPI[]) => {
    
        for (let i = 0; i < allProductsInCart.length; i++) {
            let elementID = allProductsInCart[i].id;
            let isSameID = elementID === id
            if( isSameID ){
                return i;
            }        
        }
        //si no lo encuentra
        return -1;
    }
    
        //agregar un producto al carrito (dependiendo si ya ha sido agregado antes)
    export const addProductInCart = (index: number, product: ProductAPI, myCartInStorage: CartShopAPI, setMyCartInStorage: React.Dispatch<React.SetStateAction<CartShopAPI | null>>, setCartShopStorage: (key: string, value: any) => void) => {
        
        //Una clonacion del carrito para sobreescribir de golpe en el antiguo y evitar
        //malas actualziaciones por la asincronia                    
        const cloneCart = { ...myCartInStorage };
        if(index === -1){        
            //Spread operator, aqui creamos una copia del producto y el resto del parametro son modificaciones a ese mismo producto
            cloneCart.allProduct.push({...product,quantity:1});                
    
        }else{
            //se aumenta en 1 la cantidad de ese producto
            cloneCart.allProduct[index].quantity += 1;        
        }
        //Se calculan los totales
        cloneCart.subtotal = totalPriceNoTax(cloneCart.allProduct);
        cloneCart.total = totalPriceTax(cloneCart.allProduct);        
    
        // actualizamos el estado del carrito
        setMyCartInStorage(cloneCart);
        //sobbreescrimos el carrito clonado sobre el original
        setCartShopStorage("A", cloneCart);          
    }
    
     //Vaciar lista de productos - Local y la del Carrito
     //export const deleteAllProduct = (myCartInStorage: CartShopAPI | null, setMyCartInStorage: React.Dispatch<React.SetStateAction<CartShopAPI | null>>, setCartShopStorage: (key: string, value: any) => void) => {        
    export const deleteAllProduct = (myCartInStorage: CartShopAPI | null, setMyCartInStorage: React.Dispatch<React.SetStateAction<CartShopAPI | null>>) => {        
        if(myCartInStorage !== null){        
            
            //Setteamos un metodo de pago por defecto
            const defaultPaymentMethod: PaymentMethod = {
                payment: PaymentMethodNumber.CASH, // Establecer el método de pago predeterminado
                verify: false // Establecer la verificación a falso o verdadero según corresponda
            };
    
            //setteamos todo el carrito
            const newMockup: CartShopAPI = {
                allProduct: [],
                subtotal: 0,
                tax: 0.13,
                total: 0,
                direction: '',           
                paymentMethod: defaultPaymentMethod 
            };   
            //Limpiamos el storage y el estado actual
            setCartShopStorage("A",newMockup)         
            setMyCartInStorage(newMockup);
        }            
    }