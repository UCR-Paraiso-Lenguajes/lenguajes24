'use client';
import {useState} from 'react';
import {useEffect} from 'react';

//Componentes
import {Carousel} from './global-components/carousel';
import {AlertShop} from './global-components/generic_overlay';
import { CartShop } from './global-components/cart-shop';
import { ProductGallery } from './global-components/product-gallery';

import { useRouter } from 'next/router';
import Dropdown from 'react-bootstrap/Dropdown';

//Interfaces
import { PaymentMethod, PaymentMethodNumber } from './src/models-data/PaymentMethodAPI';
import { ProductAPI } from './src/models-data/ProductAPI';
import { CartShopAPI } from './src/models-data/CartShopAPI';
//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './src/css/demoCSS.css'
import './src/css/fonts_awesome/css/all.min.css'
//Funciones
import { getCartShopStorage } from './src/storage/cart-storage';
import { getProductsByCategory, getProductsBySearchTextAndCategory } from './src/api/get-post-api';
import { CategoryAPI } from './src/models-data/CategoryAPI';


function Page() {     
    //cargamos los datos desde la API (StoreController)
    const [myCartInStorage, setMyCartInStorage] = useState<CartShopAPI | null>(getCartShopStorage("A"));    
    const [products, setProducts] = useState<ProductAPI[]>([]);       
    const [categoryListFromStore, setCategoryListFromStore] = useState<CategoryAPI[]>([]);
    //useState para el buscador y la búsqueda rapida (solo catalogo)
    const [productCategory, setproductCategory] = useState(0);    
    const [categoryListForSearch, setCategoryListForSearch] = useState<number[]>([]);
   
    //Se llama por defecto (este trae el Tax para la tienda, la lista de Categorias y los productos por defecto)
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
                    setCategoryListFromStore(json.allProductCategories);
                }else{
                    //si el dato no viene dentro de un ActionResult se guarda normal
                    setProducts(json.products);
                    setCategoryListFromStore(json.allProductCategories);
                }                  
                return json;
            } catch (error) {                
                throw new Error('Failed to fetch data:' + error);
            }
        }  
        loadDataProductAPI();
    }, []);

    function getSearchParamsFromUrl(){
        const params = new URLSearchParams(window.location.search);
        const searchText = params.get('searchText') || '';
        const categoryIds = params.getAll('categoryIds');        
        return { searchText, categoryIds };
    };

    async function fetchWithUrlParams(){
        const { searchText, categoryIds } = getSearchParamsFromUrl();
        const categoryIdsAsNumbers = categoryIds.map(id => parseInt(id, 10));
        try{
            const filteredProducts = await getProductsBySearchTextAndCategory(searchText,categoryIdsAsNumbers);

            //Validar el tipo de informacion recibida es null, obj o string = error 504/501...etc
            if (typeof filteredProducts  === "object" && filteredProducts !== null) {                    
                setProducts(filteredProducts);

                const searchTextToUrl = encodeURIComponent(searchText);
                const categoryIdsToUrl = categoryListForSearch.map(id => `categoryIds=${encodeURIComponent(id.toString())}`).join("&");
                const newUrl = `/search?searchText=${searchTextToUrl}&${categoryIdsToUrl}`;
                // Cambiar la URL sin recargar la página
                window.history.pushState({ filteredProducts }, '', newUrl);
            }
        } catch (error) {
            //callAlertShop("Error","Error al obtener datos","Al parecer los datos no pueden ser mostrados. Por favor intentalo de nuevo");
        }
    };

    window.onload = function() {    
        fetchWithUrlParams();            
        window.addEventListener('popstate', function() {
            fetchWithUrlParams();
        });
    };
    
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
                }
            } catch (error) {
                callAlertShop("Error","Error al obtener datos","Al parecer los datos no pueden ser mostrados. Por favor intentalo de nuevo");
            }            
        };    
        if (productCategory) fetchProductsByCategory();
    }, [productCategory]);


    const selectCategoryInCheckBox = (e: React.MouseEvent<HTMLUListElement>) => {

        var targetSelected = e.target as HTMLInputElement;
        var parsedValue = 0;
        if(targetSelected.tagName === "INPUT"  && targetSelected.type === 'checkbox'){            
            parsedValue = parseInt(targetSelected.value);
            if(targetSelected.checked){                
                setCategoryListForSearch(prevState => [...prevState, parsedValue]);
            }else{
                setCategoryListForSearch(prevState => prevState.filter(category => category !== parsedValue))
            }                        
        }        
    }

    function getInputSearchValue(): string {        
        var inputElement = document.getElementById('search-input') as HTMLInputElement;
        //que el campo no tenga espacios en blanco y que el input exista
        if(inputElement && inputElement.value.trim() !== "" ){
            return inputElement.value;
        }
        return "default";        
    }


    //Iniciar busqueda parametrizada
    const startSearch = async (event: React.MouseEvent<HTMLButtonElement>): Promise<void> =>{
        event.preventDefault();        
        var inputSearchValue = getInputSearchValue();
        try {
            const filteredProducts = await getProductsBySearchTextAndCategory(inputSearchValue,categoryListForSearch);

            //Validar el tipo de informacion recibida es null, obj o string = error 504/501...etc
            if (typeof filteredProducts  === "object" && filteredProducts !== null) {                    
                setProducts(filteredProducts);

                const searchTextToUrl = encodeURIComponent(inputSearchValue);
                const categoryIdsToUrl = categoryListForSearch.map(id => `categoryIds=${encodeURIComponent(id.toString())}`).join("&");
                const newUrl = `/search?searchText=${searchTextToUrl}&${categoryIdsToUrl}`;
                // Cambiar la URL sin recargar la página
                window.history.pushState({ filteredProducts }, '', newUrl);
            }
        } catch (error) {
            callAlertShop("Error","Error al obtener datos","Al parecer los datos no pueden ser mostrados. Por favor intentalo de nuevo");
        }                    
    }

  return (
    <main className="flex min-h-screen flex-col p-6">
        
      <div className="main_banner row">    

                <div className="logo-container col-sm-3">
                    <img src="./img/logo.png"/>
                </div>
                        
                <div className="search_container col-sm-6 align-items-center justify-content-center">
                    
                    <div className="category-list-searching">  
                        <span><img src="./img/sliders-solid.svg" alt="" /></span>
                        <ul onClick={selectCategoryInCheckBox}>
                            
                            {categoryListFromStore.map((category,index) => (

                                <li>
                                    <input value={category.id.toString()} type="checkbox" id={category.name}/>
                                    <label htmlFor={category.name}>{category.name}</label>
                                </li>
                            ))}
                                                        
                        </ul>            
                    </div>

                    <input id="search-input" type="search" name="name" placeholder="Busca cualquier cosa..."/>                    
                    <button type="submit" onClick={startSearch}>
                        <i className="fas fa-search"></i>
                    </button>
                </div>
                
                <div className="category-container col-sm-3">
                    <Dropdown onSelect={selectCategory}>
                        <Dropdown.Toggle variant="secondary" id="dropdown-basic" className="dropdown-style">
                            Búsqueda rápida:
                        </Dropdown.Toggle>

                        <Dropdown.Menu>
                            <Dropdown.Item eventKey="0">Todos los productos:</Dropdown.Item>                                
                                {categoryListFromStore.map((category, index) => (
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