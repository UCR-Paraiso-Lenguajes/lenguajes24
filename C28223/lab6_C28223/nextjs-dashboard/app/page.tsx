"use client";
import "bootstrap/dist/css/bootstrap.min.css";
import React, { useState, useEffect } from 'react';
import "@/app/ui/styles.css";
import Header from "@/app/navStarBuyStore/page";
import Carrito from "@/app/Cart/page";
import Carousel from 'react-bootstrap/Carousel';


// cards
const Product = ({ product, handleClick }) => {
  const { name, description, imageURL, price } = product;
  return (
    <div>
      <div className="row">
        <div className="productos">
          <div style={{ justifyContent: "center", textAlign: "center" }}>
            <h3>{name}</h3>
            <img src={imageURL} />
            <h5>{description}</h5>
            <h5>{price}</h5>
            <button onClick={() => handleClick(product)} className="btn btn-primary"> Añadir al carrito</button>
          </div>
        </div>
      </div>
    </div>
  );
};

const CarruselProductos=({productos, handleClick}) =>{
  return (
    <Carousel data-bs-theme="dark">
      {productos && productos.products && productos.products.map(product => (
         <Carousel.Item interval={1000}>
        <div className="carrusel-item">
        <Product key={product.id} product={product} handleClick={handleClick} />
        </div>
        </Carousel.Item>
      ))}
    </Carousel>
  );
};

const MostrarProductos = ({ handleClick }) => {
  const [Productos, setProductos] = useState([]);
  const loadData = async () => {
      const response = await fetch('https://localhost:7223/api/Store');// 7223
      if (!response.ok) {
        // throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      setProductos(json);
  };
  loadData();
  return (
    <div>
      <CarruselProductos productos={Productos} handleClick={handleClick}/>
      <h3> Lista de productos</h3>
      <div className="productos">
        {Productos && Productos.products && Productos.products.map(product => (
          <Product key={product.id} product={product} handleClick={handleClick} />
        ))}
      </div>
    </div>
  );
};


const ModalProductoYaAgregado = ({ closeModal }) => {
  return (
    <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Producto ya agregado</h5>
            <button type="button" className="close" onClick={closeModal} aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div className="modal-body">
            <p>Este producto ya ha sido añadido al carrito.</p>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" onClick={closeModal}>Cerrar</button>
          </div>
        </div>
      </div>
    </div>
  );
};


export default function Page() {
  const [show, setShow] = useState(true);
  const [showModal, setShowModal] = useState(false);

  const initialCart = {
    carrito: {
      subtotal: 0,
      porcentajeImpuesto: 13,
      total: 0,
      direccionEntrega: '',
      metodoDePago: 0
    },
    necesitaVerificacion: false,
    productos: [],
    idCompra: ''
  };
  const [store, setStore] = useState(() => {
    const storedStore = localStorage.getItem("tienda");
    return storedStore ? JSON.parse(storedStore) : initialCart;
  });

  useEffect(() => {
    handlePrice();
  }, [store]);

  const handleClick = (item) => {
    const isPresent = store.productos.some(producto => producto.id === item.id);
    if (isPresent) {
      setShowModal(true);
    } else {
      const newProd = [...(store.productos), item];
      setStore(({
        ...store,
        carrito: {
          ...store.carrito
        },
        productos: newProd
      }));
    }
  };

  const closeModal = () => {
    setShowModal(false);
  };
  const handlePrice = () => {
    let subtotalCalc = 0;
    store.productos.forEach((item) => {
      subtotalCalc += item.price;
    });

    let subtotalImpuestoCalc = subtotalCalc * (store.carrito.porcentajeImpuesto / 100);
    let totalCompraCalc = subtotalCalc + subtotalImpuestoCalc;

    const updatedCarrito = {
      ...store.carrito,
      subtotal: subtotalCalc,
      total: totalCompraCalc,
    };

    const updatedStore = {
      ...store,
      carrito: updatedCarrito,
    };

    localStorage.setItem("tienda", JSON.stringify(updatedStore));
  }; 
 

  return (
    <div>
      <Header size={store.productos.length} setShow={setShow} />
      {
        show ? <MostrarProductos handleClick={handleClick} /> : <Carrito />
      }
      {showModal && <ModalProductoYaAgregado closeModal={closeModal} />}

      <footer>
        <p>Derechos de autor © 2024. Para mi primer sitio</p>
      </footer>
    </div>
  );
};