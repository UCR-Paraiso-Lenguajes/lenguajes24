'use client';
import {useState} from 'react';
import {useEffect} from 'react';

//Componentes
import {Carousel} from './global-components/carousel';
import {AlertShop} from './global-components/generic_overlay';
import { CartShop } from './global-components/cart-shop';
import { ProductGallery } from './global-components/product-gallery';
import Link from 'next/link';
import Button from 'react-bootstrap/Button';

//Interfaces
import { PaymentMethod, PaymentMethodNumber } from './src/models-data/PaymentMethodAPI';
import { ProductAPI } from './src/models-data/ProductAPI';
import { CartShopAPI } from './src/models-data/CartShopAPI';
//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './src/css/demoCSS.css'
import './src/css/fonts_awesome/css/all.min.css'
import { mock } from 'node:test';
//Funciones
import { getCartShopStorage } from './src/storage/cart-storage';


function Page() {     
    //cargamos los datos desde la API (StoreController por Metodo Get)    
    const [myCartInStorage, setMyCartInStorage] = useState<CartShopAPI | null>(getCartShopStorage("A"));    
    const [products, setProducts] = useState<ProductAPI[]>([]);               
    
    useEffect(() => {

        const loadDataProductAPI = async ()=>{
            try{            
                const response = await fetch('https://localhost:7161/api/Store')
                if (!response.ok){
                    throw new Error('Failed to fetch data');                
                }
                const json = await response.json();            

                //Como ahora Store desde la API se devuelve dentro de un objeto Action
                //hacemos una validacion para saber si trae datos dentro de su metodo
                if(json.hasOwnProperty('value')){
                    setProducts(json.valeu.products);                            
                }else{
                    //si el dato no viene dentro de un ActionResult se guarda normal
                    setProducts(json.products);                        
                }                                
                return json;
            } catch (error) {                
                throw new Error('Failed to fetch data:' + error);
            }
        }  
        loadDataProductAPI();
    }, []);
   
  return (
    <main className="flex min-h-screen flex-col p-6">
        <Link href='/login'>
            <Button variant="secondary">Iniciar compra</Button>
        </Link>

        <Link href='/admin/init'>
            <Button variant="secondary">Dashboard</Button>
        </Link>
      <div className="main_banner">    
            
            <div className="row">
                <div className="search_container col-sm-6">
                    <input type="search" name="name" placeholder="Busca cualquier cosa..."/>
                    <i className="fas fa-search"></i>                                  
                </div>
                                
                <CartShop                     
                    myCartInStorage={myCartInStorage}  
                    setMyCartInStorage={setMyCartInStorage}                             
                />
            </div>            
      </div>  

      {/* Galeria de Productos */}
      <div>
            {/* El uso de las Keys es importante ya que le hacen saber a React cuando hay cambios en los elementos del proyecto
            Ademas, todas los componentes deben llevar una Key, es una buena practica
            */}
          <h1>Lista de Productos</h1>             
          <div id='div_gallery' className="row">
              {products && products.length >= 0 && products.map(product => {
                  if (product.description === "carousel") {
                      return(
                          <section className="container_carousel col-sm-4" key={product.id}>
                              <Carousel                                     
                                    products={products}
                                    myCartInStorage={myCartInStorage}                                    
                                    setMyCartInStorage={setMyCartInStorage}           
                              />;
                          </section>
                      ) 
                  } else {
                      return (
                        <ProductGallery 
                            key={product.id} 
                            product={product}
                            myCartInStorage={myCartInStorage}                                    
                            setMyCartInStorage={setMyCartInStorage}           
                        />                        
                      );
                  }
              })}
            </div>
      </div>      
      <footer>@ Derechos Reservados 2024</footer>         
    </main>
  );
}
export default Page;