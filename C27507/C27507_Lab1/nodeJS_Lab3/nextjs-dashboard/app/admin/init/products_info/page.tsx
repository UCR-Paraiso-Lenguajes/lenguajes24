'use client';
import {useState} from 'react';
import {useEffect} from 'react';


import { useRouter } from 'next/router';
import Dropdown from 'react-bootstrap/Dropdown';


//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './src/css/demoCSS.css'
import './src/css/fonts_awesome/css/all.min.css'
import { getAllProductsFromAPI } from '@/app/src/api/get-post-api';
import { ProductAPI } from '@/app/src/models-data/ProductAPI';
import { CategoryAPI } from '@/app/src/models-data/CategoryAPI';
import { CartShopAPI } from '@/app/src/models-data/CartShopAPI';
import { getCartShopStorage } from '@/app/src/storage/cart-storage';

//Funciones

export default function ProductsInfo(){

    //cargamos los datos desde la API (StoreController)
    const [myCartInStorage, setMyCartInStorage] = useState<CartShopAPI | null>(getCartShopStorage("A"));    
    const [products, setProducts] = useState<ProductAPI[]>([]);           

    //Llamamos los productos desde Store    
    useEffect(() => {
        const loadDataProductAPI = async ()=>{
            try{            
                let dataFromStore = await getAllProductsFromAPI();

                if (typeof dataFromStore  === "object" && dataFromStore !== null){
                    setProducts(dataFromStore.productsFromStore);                    
                }   
            } catch (error) {                
                throw new Error('Failed to fetch data:' + error);
            }
        }  
        loadDataProductAPI();
    }, []);


    return(
        <div>Tabla de Productos</div>
    );
}