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
import Dropdown from 'react-bootstrap/Dropdown';


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
import { getAllProductsFromAPI, getProductsByCategory } from './src/api/get-post-api';
import { CategoryAPI } from './src/models-data/CategoryAPI';


function Page() {     
    //cargamos los datos desde la API (StoreController por Metodo Get)    
    const [myCartInStorage, setMyCartInStorage] = useState<CartShopAPI | null>(getCartShopStorage("A"));    
    const [products, setProducts] = useState<ProductAPI[]>([]);       
    const [productCategory, setproductCategory] = useState(0);
    const [categoryList, setCategoryList] = useState<CategoryAPI[]>([]);
    
    //Se llama por defecto (este trae el Tax para los productos y la lista de Categorias)
    useEffect(() => {

        const loadDataProductAPI = async ()=>{
            try{            
                let dataFromStore = await getAllProductsFromAPI();

                if (typeof dataFromStore  === "object" && dataFromStore !== null){
                    setProducts(dataFromStore.productsFromStore);
                    setCategoryList(dataFromStore.categoriesFromStore);
                }   
            } catch (error) {                
                throw new Error('Failed to fetch data:' + error);
            }
        }  
        loadDataProductAPI();
    }, []);

    
    //Se ejecuta cada que seleccionamos una categoria
    const selectCategory = (eventKey: string | null) => {        
        if(eventKey !== null) setproductCategory(parseInt(eventKey));        
    };
    
    useEffect(() => {
        const fetchProductsByCategory = async () => {
            try {
                const filteredProducts = await getProductsByCategory(productCategory);

                //Validar el tipo de informacion recibida es null, obj o string = error 504/501...etc
                if (typeof filteredProducts  === "object" && filteredProducts !== null) {                    
                    setProducts(filteredProducts)
                }else{
                    

                }                
                
            } catch (error) {
                //callAlertShop("Error","Error al obtener datos","Al parecer los datos no pueden ser mostrados. Por favor intentalo de nuevo");
            }            
        };    
        if (productCategory) fetchProductsByCategory();
    }, [productCategory]);

  return (
    <main className="flex min-h-screen flex-col p-6">
        
      <div className="main_banner row">    

                <div className="logo-container col-sm-3">
                    <img src="./img/logo.png"/>
                </div>
                        
                <div className="search_container col-sm-6 align-items-center justify-content-center">
                    <input type="search" name="name" placeholder="Busca cualquier cosa..."/>                    
                    <button type="submit">
                        <i className="fas fa-search"></i>
                    </button>
                </div>
                
                <div className="category-container col-sm-3">
                    <Dropdown onSelect={selectCategory}>
                        <Dropdown.Toggle variant="secondary" id="dropdown-basic" className="dropdown-style">
                            Productos por Categoría
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                            <Dropdown.Item eventKey="0">Todos los productos:</Dropdown.Item>                                
                                {categoryList.map((category, index) => (
                                    <Dropdown.Item key={index} eventKey={category.id.toString()}>
                                        {category.name}
                                    </Dropdown.Item>
                                ))}            
                        </Dropdown.Menu>
                    </Dropdown>
                </div>
                                   
      </div>  

      {/* Galeria de Productos */}
      <div>
            {/* El uso de las Keys es importante ya que le hacen saber a React cuando hay cambios en los elementos del proyecto
            Ademas, todas los componentes deben llevar una Key, es una buena practica
            */}
                    
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
      <CartShop
            myCartInStorage={myCartInStorage}  
            setMyCartInStorage={setMyCartInStorage}                             
        />                
    </main>
  );
}
export default Page;