"use client" 
import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; 
import Navbar from '../components/Navbar';
import Producs from '../components/Producs';
import Carousel from 'react-bootstrap/Carousel';


export default function Home() {

  const initialState = {
    productosCarrusel: [],
    impVentas: 13,
    cart: { productos: [], subtotal: 0, total: 0, direccionEntrega: '', metodosPago : 0, ordenCompra : 0},
    necesitaVerificacion: false,
  };
  
  const [tienda, setTienda] = useState(() => {
    const storedTienda = localStorage.getItem('tienda');
    return storedTienda ? JSON.parse(storedTienda) : initialState;
  });

  useEffect(() => {
    localStorage.setItem('tienda', JSON.stringify(tienda));
  }, [tienda]);

  function agregarProducto(item){ 

    const isPresent = tienda.cart.productos.some(producto => producto.id === item.id);

    if (!isPresent) {

    const nuevosProductos = [...tienda.cart.productos, item];

    let subtotalCalc = 0;
    nuevosProductos.forEach((item) => {
        subtotalCalc += item.price;
    });
  
    const nuevoSubtotal = subtotalCalc;
    const nuevoTotal = nuevoSubtotal * (1 + tienda.impVentas / 100);

    setTienda({
      ...tienda,
      cart: {
        ...tienda.cart,
        productos: nuevosProductos,
        subtotal: nuevoSubtotal,
        total: nuevoTotal,
      }
    });
   }
  };

const Footer = () => (
  <footer className="bg-body-tertiary text-center text-lg-start">
    <div className="text-center p-3" style={{ backgroundColor: 'black' }}>
      <a className="text-white">© 2024: Condiciones de uso</a>
    </div>
  </footer>
);


const [productList, setProductList] = useState([]);
  useEffect(() => {
  const loadData = async () => {
        const response = await fetch('https://localhost:7013/api/Store');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const json = await response.json();
        setProductList(json);
    };

    loadData();
  }, []); 


const ControlledCarousel = ({ productListApi, agregarProducto }) => {
    const [index, setIndex] = useState(0);
  
    const handleSelect = (selectedIndex) => {
      setIndex(selectedIndex);
    };
  
    const handleClick = (item) => {
      agregarProducto(item);
    };
  
    return (
      <Carousel activeIndex={index} onSelect={handleSelect}>
        {productListApi && productListApi.products && productListApi.products.map((item) => (
          <Carousel.Item key={item.id}>
            <img
              className="d-block w-100"
              src={item.imageUrl}
              alt={item.name}
              style={{ height: '300px', objectFit: 'cover' }}
            />
            <Carousel.Caption>
              <button className="btn btn-outline-info" onClick={() => handleClick(item)}>Agregar al carrito</button>
            </Carousel.Caption>
          </Carousel.Item>
        ))}
      </Carousel>
    );
  };


return ( 
    <div>
      <div>
        <Navbar cantidad_Productos={tienda.cart.productos.length}/>
      </div>
      <ControlledCarousel productListApi={productList} agregarProducto={agregarProducto} />
      <main>
      <div className="container" >
        <div>
        <section className="container-fluid">
      <div className="row product-container">
      {productList && productList.products && productList.products.map((item) => ( 
        <div key={item.id} className="col-3 col-md-3 col-lg-3 mt-2 product-item">
             <Producs item={item} agregarProducto={agregarProducto} />
        </div>
      ))}
      </div>
    </section>
      </div>
      </div>
    </main>
    <div>
        <Footer></Footer>
    </div>
    </div>
  );
}
