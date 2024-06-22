'use client';
import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
//para renderizar html en React
import { Parser } from 'html-to-react';




import { useRouter } from 'next/router';
import Dropdown from 'react-bootstrap/Dropdown';


//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './../../../src/css/demoCSS.css'
import './../../../src/css/products_info.css'
import './../../../src/css/fonts_awesome/css/all.min.css'
import { getAllProductsFromAPI } from '@/app/src/api/get-post-api';
import { ProductAPI } from '@/app/src/models-data/ProductAPI';
import { CategoryAPI } from '@/app/src/models-data/CategoryAPI';
import { CartShopAPI } from '@/app/src/models-data/CartShopAPI';
import { getCartShopStorage } from '@/app/src/storage/cart-storage';
import { ModalInsert } from '../../admin-components/modal-insert';

//Funciones

// @ts-ignore
const htmlToReactParser = new Parser();


export default function ProductsInfo(){

    //cargamos los datos desde la API (StoreController)    
    const [products, setProducts] = useState<ProductAPI[]>([]);
    const [categoryListFromStore, setCategoryListFromStore] = useState<CategoryAPI[]>([]);




    //States del ModalInsert
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    

    //Llamamos los productos desde Store    
    useEffect(() => {
        const loadDataProductAPI = async ()=>{
            try{            
                let dataFromStore = await getAllProductsFromAPI();

                if (typeof dataFromStore  === "object" && dataFromStore !== null){
                    setProducts(dataFromStore.productsFromStore);
                    setCategoryListFromStore(dataFromStore.categoriesFromStore)
                }
                

            } catch (error) {                
                throw new Error('Failed to fetch data:' + error);
            }
        }  
        loadDataProductAPI();
    }, []);


    return (
        <>
            <table>
                <tbody>
                    <tr>
                        <th colSpan={6}><h1>List of Products</h1></th>
                    </tr>
                    <tr className="crud-header">
                
                        <th><button>Home</button></th>
                        <th><button onClick={handleShow}>Add New</button></th>
                    </tr>
                    <tr>
                        <th>Code</th>
                        <th>Producto Name</th>
                        <th>Description</th>
                        <th>Category</th>
                        <th>Price</th>
                        <th>Action</th>
                    </tr>

                    {products.map((product,index) => (
                        <tr key={product.id} className="crud-product-info">
                            <td>{product.id}</td>
                            <td>{product.name}</td>
                            <td>
                                {htmlToReactParser.parse(product.description)}
                            </td>                            
                            <td>{product.category && product.category.name}</td>
                            <td>{product.price}</td>
                            <td className="crud-actions-container">
                                <button>Edit</button> 
                                <button>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody> 
            </table>
            <ModalInsert
                show={show}
                handleClose={handleClose}
                categoryListFromStore={categoryListFromStore}
            />
        </>
        //https://vaidrollteam.blogspot.com/2023/03/crud-basico-de-productos-en-php-mysql.html
    );

}