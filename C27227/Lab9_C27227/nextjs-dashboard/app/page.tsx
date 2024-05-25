"use client"
import React, { useState, useEffect } from 'react';
import Navbar from '@/app/ui/components/header';
import Products_Store from './ui/components/products';
import Cart_Store from './ui/components/cart';
import CarouselBanner from '@/app/ui/components/carrusel';
import "bootstrap/dist/css/bootstrap.min.css";
import '@/app/ui//styles/app.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
const Page = () => {
  const initialState = {
    products: [],
    cart: { 
      subtotal: 0,
      subtotalImpuesto: 0,
      total: 0,
      impVentas: 13,
      direccionEntrega: '',
      metodoPago: ''
    },
    necesitaVerifica: false,
    idCompra: ''
  };

  const [tienda, setTienda] = useState(() => {
    const storedTienda = localStorage.getItem('tienda');
    return storedTienda ? JSON.parse(storedTienda) : initialState;
  });

  const [isMainPage, setIsMainPage] = useState(true);
  const [warning, setWarning] = useState(false);
  const [productList, setProductList] = useState([]);
  const [currentProductIndex, setCurrentProductIndex] = useState(0);
  
  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await fetch('http://localhost:5072/api/Store');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const json = await response.json();
        setProductList(json);
      } catch (error) {
         throw new Error('Failed to fetch data:');
      }
    };
    
    loadData();
  }, []);


  const handleAddToCart = (newProduct) => {
    const currentProduct = productList.products[currentProductIndex];
    const isPresent = tienda.products.some(product => product.id === newProduct.id);

    if (isPresent) {
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
      }, 2000);
      return;
    }
    const updatedProducts = [...tienda.products, newProduct];
    const updatedDataObject = { ...tienda, products: updatedProducts };
  
    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
    setTienda(updatedDataObject);
  };

  return (
    <div>
      <Navbar size={tienda.products.length} setShow={setIsMainPage} />
      {warning && <div className='alert'>El producto ya se encuentra en el carrito</div>}
      {isMainPage && productList && productList.products && productList.products.length > 0 && (
        <div className='container'>
          <div id="carouselExampleIndicators" className="carousel slide" data-bs-ride="carousel">
            <div className="carousel-indicators">
              {productList && productList.products && productList.products.map((product, i) => (
                <button key={product.id} type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to={i} className={i === currentProductIndex ? ' active' : ''} aria-current={i === currentProductIndex ? 'true' : 'false'} aria-label={`Slide ${i + 1}`} />                                    
              ))}
            </div>
            <div className="carousel-inner">
              {productList && productList.products && productList.products.map((product, index) => (
                <CarouselBanner key={product.id} banner={product} />
              ))}
            </div>
            <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev" onClick={() => setCurrentProductIndex((currentProductIndex + productList.products.length - 1) % productList.products.length)}>
              <span className="carousel-control-prev-icon" aria-hidden="true"></span>
              <span className="visually-hidden">Previous</span>
            </button>
            <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next" onClick={() => setCurrentProductIndex((currentProductIndex + 1) % productList.products.length)}>
              <span className="carousel-control-next-icon" aria-hidden="true"></span>
              <span className="visually-hidden">Next</span>
            </button>
          </div>
        </div>
      )}
       <div className='AddFromCarousel'>
        {isMainPage && <button className="btn btn-primary" onClick={handleAddToCart}>Add to cart</button>}
      </div>
      {isMainPage ? <Products_Store handleClick={handleAddToCart} /> : <Cart_Store />}
      <footer className="bg-dark text-white text-center text-lg-start d-flex justify-content-center align-items-center">
        <div className="row row-cols-4">
          © 2024: Derechos reservados para Kendall Sánchez
        </div>
      </footer>   
    </div>
  );
};
export default Page;