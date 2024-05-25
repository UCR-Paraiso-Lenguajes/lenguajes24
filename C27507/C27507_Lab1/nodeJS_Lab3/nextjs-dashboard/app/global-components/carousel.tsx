import {useState} from 'react';
import { number } from 'zod';

//Componentes
// import {Product,CartShop} from './layout';
// import {AlertShop} from './generic_overlay';
//Interfaces
import { ProductAPI } from '../src/models-data/ProductAPI';
import { CartShopAPI } from '../src/models-data/CartShopAPI';
//Funciones
import { verifyProductInCart, addProductInCart, setCartShopStorage } from '../src/storage/cart-storage';

interface CarouselProps {
    products: ProductAPI[];  
    myCartInStorage: CartShopAPI | null;
    setMyCartInStorage: React.Dispatch<React.SetStateAction<CartShopAPI | null>>;
  }
  
export const Carousel: React.FC<CarouselProps> = ({products,myCartInStorage,setMyCartInStorage}) => { 
    const [activeIndex, setActiveIndex] = useState(0);

    const handlePrevSlide = () => {
        const newIndex = activeIndex === 0 ? products.length - 1 : activeIndex - 1;
        setActiveIndex(newIndex);
    };

    const handleNextSlide = () => {
        const newIndex = activeIndex === products.length - 1 ? 0 : activeIndex + 1;
        setActiveIndex(newIndex);
    };

    const buyItem = (productInCarrusel:ProductAPI) => {    
    
        //como el objeto del carrito puede ser nulo, creamos una condicion para evitar estar haciendo
        //condiciones    
        if (myCartInStorage) {
    
          let indexInCart = verifyProductInCart(productInCarrusel.id,myCartInStorage.allProduct);            
          addProductInCart(indexInCart,productInCarrusel,myCartInStorage,setMyCartInStorage,setCartShopStorage);      
          
        } else {
          console.log("El carro no existe");
        }
      };  

    return (        
        <div id="carouselExampleCaptions" className="carousel slide">
            <div className="cover"></div>
            <div className="carousel-indicators">  
                {products.map((_, index) => (
                    <button
                        key={index}
                        type="button"
                        data-bs-target="#carouselExampleCaptions"
                        data-bs-slide-to={index}
                        className={activeIndex === index ? "active" : ""}
                        aria-current="true"
                        aria-label={`Slide ${index + 1}`}
                    ></button>
                ))}                                              
            </div>            
            <div className="carousel-inner">
                {products.map((product,index) => {
                    return(
                        <div key={product.id} className={`carousel-item ${activeIndex === index ? "active" : ""}`}>
                            <div className="cover_img"></div>
                            <div className='info_carousel'>
                                <h5>{product.name}</h5>
                                <p>${product.price}</p>
                                <button onClick={ () => buyItem(product) }>Agregar a carrito</button>
                            </div>
                            <img src={product.imageUrl} className="d-block w-100" alt="Mouse" />
                        </div>
                    )
                })}                
            </div>
            <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="prev" onClick={handlePrevSlide}>
                <span className="carousel-control-prev-icon" aria-hidden="true"></span>
                <span className="visually-hidden">Previous</span>
            </button>
            <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="next" onClick={handleNextSlide}>
                <span className="carousel-control-next-icon" aria-hidden="true"></span>
                <span className="visually-hidden">Next</span>
            </button>
        </div>
    );
}


