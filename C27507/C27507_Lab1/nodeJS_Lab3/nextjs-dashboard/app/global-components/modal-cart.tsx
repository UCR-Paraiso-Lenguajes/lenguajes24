import React from 'react';
import {useState} from 'react';
import {useEffect} from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

//Componentes
//import { BrowserRouter as Router, Route, Link } from 'react-router-dom';
import Link from 'next/link';


//Interfaces
import { CartShopAPI } from '../src/models-data/CartShopAPI';
//Funciones
import { deleteAllProduct } from '../src/storage/cart-storage';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import '../src/css/modal_cart.css'
import '../src/css/fonts_awesome/css/all.min.css'
import { mock } from 'node:test';



//Creamos la interfaz que deben seguir los props (o parametros) para el componente Modal
interface ModalCartProps {
    show: boolean;
    handleClose: () => void;    
    myCartInStorage: CartShopAPI | null;    
    setMyCartInStorage: React.Dispatch<React.SetStateAction<CartShopAPI | null>>;

}
  
export const ModalCart: React.FC<ModalCartProps> = ({ 
    show, 
    handleClose,    
    myCartInStorage,
    setMyCartInStorage
}) => {
    
    return (
        <>                 
            <Modal show={show} onHide={handleClose} animation={false}>
                <Modal.Header closeButton>
                    <Modal.Title>
                        <div className="cart_title_btn">
                            <h4><i className="fas fa-shopping-cart"></i>Tu Carrito:</h4>                    
                        </div>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>

                    <div className="product-menu-cart">

                        {/* asegurarnos de que no venga nulo el carrito */}
                        {myCartInStorage && myCartInStorage.allProduct.map((productItem) => (
                            
                            <div key={productItem.id}>
                                <img src={productItem.imageUrl} alt="" />
                                <p>{productItem.name}</p>
                                <p><span>Cantidad:</span> {productItem.quantity}</p>
                                <p><span>Precio:</span> ₡{productItem.price}</p>
                                <button>Eliminar</button>
                                <hr></hr>
                            </div>                                             
                        ))}                
                    </div>                    
                

                </Modal.Body>
                
                <div className="total-price-container">
                    <div className="tax-price-cart total-price-cart">Total: <span>₡{myCartInStorage && myCartInStorage.total}</span></div>    
                    <hr></hr>
                    <div className="notax-price-cart total-price-cart">Total sin impuestos: <span>₡{myCartInStorage && myCartInStorage.subtotal}</span></div>    
                </div>
                <Modal.Footer>
                    {
                        myCartInStorage && myCartInStorage.allProduct.length ? (
                            <>                                                                                      
                                <Link href='/cart-validation/address-validation'>
                                    <Button variant="secondary">Iniciar compra</Button>
                                </Link>
                            </>
                            
                        ) : (
                            <></>
                        )
                    }
                    <Button variant="secondary" onClick={() => deleteAllProduct(myCartInStorage,setMyCartInStorage)}>
                        Vaciar Carrito
                    </Button>          
                    <Button variant="secondary" onClick={handleClose}>
                        Cerrar
                    </Button>                    
                    
                </Modal.Footer>
            </Modal>                                                
        </>
    );
}