//Componentes

import { CartShopAPI } from "../src/models-data/CartShopAPI";
import { ProductAPI } from "../src/models-data/ProductAPI";
import { Parser } from 'html-to-react';
//Funciones
import { verifyProductInCart, addProductInCart, setCartShopStorage } from "../src/storage/cart-storage";

// @ts-ignore
const htmlToReactParser = new Parser();

interface ProductGalleryProps {
    product: ProductAPI;  
    myCartInStorage: CartShopAPI | null;
    setMyCartInStorage: React.Dispatch<React.SetStateAction<CartShopAPI | null>>;
  }
  
  //Galeria de Productos
  export const ProductGallery: React.FC<ProductGalleryProps> = ({product,myCartInStorage,setMyCartInStorage}) => { 
    let idActualProduct = product.id;
  
    const buyItem = () => {    
      
      //como el objeto del carrito puede ser nulo, creamos una condicion para evitar estar haciendo
      //condiciones    
      if (myCartInStorage) {
  
        let indexInCart = verifyProductInCart(idActualProduct,myCartInStorage.allProduct);            
        addProductInCart(indexInCart,product,myCartInStorage,setMyCartInStorage,setCartShopStorage);      
        
      }
    };  
      
    return (
        <div className="product col-sm-4 row">
            <div hidden>{product.id}</div>
            <div className="row-sm-3"><img src={product.imageUrl}/></div>
            <p className="row-sm-3">{product.name}</p>
            <p className="row-sm-3">${product.price}</p>
            <p className="row-sm-3">{htmlToReactParser.parse(product.description)}</p>
            <button className="row-sm-3" onClick={ () => buyItem() }>Comprar</button>          
        </div>
    );
  }