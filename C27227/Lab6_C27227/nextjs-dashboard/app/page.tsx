"use client";
import "bootstrap/dist/css/bootstrap.min.css";
import 'bootstrap/dist/js/bootstrap.bundle.min.js';;
import React, { useState, useId, useEffect} from 'react';
import Navbar from '@/app/ui/components/header';
import Products_Store from'./ui/components/products';
import Cart_Store from './ui/components/cart';
import '@/app/ui//styles/app.css';
import Alert from './Alert'; 

const Page = () => {
  const initialState = {
    products: [],
    cart: { 
      subtotal: 0,
      subtotalImpuesto: 0,
      total: 0,
      impVentas: 13,
      direccionEntrega: [],
      metodoPago: ''
    },
    necesitaVerifica: false,
    idCompra: ''
  };

  const [tienda, setTienda] = useState(() => {
    const storedTienda = localStorage.getItem('tienda');
    return storedTienda ? JSON.parse(storedTienda) : initialState;
  });

  const [show, set_show] = useState(true);
  const [warning, setWarning] = useState(false);

  useEffect(() => {
    localStorage.setItem("tienda", JSON.stringify(tienda));
  }, [tienda]);

  const handleClick = (newProduct) => {
    const isPresent = tienda.products.some(product => product.id === newProduct.id);
    if (isPresent) {
      setWarning(true);
			setTimeout(()=>{
				setWarning(false);
			}, 2000);
			return ;
    } else {
      const newProd = [...tienda.products, newProduct];
      setTienda({
        ...tienda,
        products: newProd
      });
    }
  };

  return (
    
    <div>
      
      <Navbar size={tienda.products.length} setShow={set_show} />
      {
			warning && <div className='alert'>El producto ya se encuentra en el carrito</div>
		  }
      {show ? <Products_Store handleClick={handleClick} /> : <Cart_Store />}
      <footer className="bg-dark text-white text-center text-lg-start d-flex justify-content-center align-items-center">
        <div className="row row-cols-4">
            © 2024: Derechos reservados para Kendall Sánchez
        </div>
    </footer>   
    </div>
  );
};

export default Page;


