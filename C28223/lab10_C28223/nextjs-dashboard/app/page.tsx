"use client";
import "bootstrap/dist/css/bootstrap.min.css";
import React, { useState, useEffect } from 'react';
import "@/app/ui/styles.css";
import Header from "@/app/navStarBuyStore/page";
import Carrito from "@/app/Cart/page";
import Carousel from 'react-bootstrap/Carousel';
import { DropdownButton, Dropdown } from 'react-bootstrap';

// cards
const Product = ({ product, handleClick }) => {
  if (product == undefined || handleClick == undefined) {
    throw new Error('Error: Los argumentos de producto no pueden ser nulos.');
  }
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

const CarruselProductos = ({ productos, handleClick }) => {
  if (productos == undefined || handleClick == undefined) {
    throw new Error('Error: Los argumentos de CarruselProductos no pueden ser nulos.');
  }
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

const MostrarProductos = ({ Productos, handleClick }) => {
  if (handleClick == undefined || Productos==undefined) { throw new Error('Error: Los argumentos de MostrarProductos no pueden ser nulos.'); }
  return (
    <div>
      <CarruselProductos productos={Productos} handleClick={handleClick} />
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
  if (closeModal == undefined) { throw new Error('Error: Los argumentos de ModalProductoYaAgregado no pueden ser indefinidos.'); }
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
            <p>Este producto ya ha sido añadido al carrito, puedes editar su cantidad al confirmar tu compra.</p>
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
  const [productos, setProductos] = useState([]);
  const [category, setCategory] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch('https://localhost:7223/api/Store');
        if (!response.ok) {
          throw new Error('Failed to fetch products');
        }
        const data = await response.json();
        setProductos(data);
        setCategory(data.categories);
      } catch (error) {
        throw new Error('Failed to fetch data');
      }
    };
    fetchData();
  }, []);

  const fetchDataByCategory = async (category) => {
    if (category == undefined) throw new Error("Error: Los argumentos fetchDataByCategory  no pueden ser indefinidos.");
    try {
      const response = await fetch(`https://localhost:7223/api/Store/Products?categoryID=${category.categoryID}`);
      if (!response.ok)throw new Error('Failed to fetch filtered products');
  
      const data = await response.json();
      const filteredProducts = { products: data.filteredProducts.result };
      setProductos(filteredProducts);
    } catch (error) {
      throw new Error('Failed to fetch filtered products');
    }
  };

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
    if (item == undefined) { throw new Error('Los argumentos para agregar un productom no pueden ser nulos.'); }
    const isPresent = store.productos.some(producto => producto.id === item.id);
    if (isPresent) {
      setShowModal(true);
    } else {
      const newItem = { ...item, cant: 1 };
      const newProd = [...(store.productos), newItem];
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
  const handleCategoryChange = (categoryID) => {
    if (categoryID == undefined) throw new Error("Error: Los argumentos handleCategoryChange  no pueden ser indefinidos.");
    fetchDataByCategory(categoryID);
  };

  return (
    <div>
      <Header size={store.productos.length} setShow={setShow} />
      <DropdownButton id="dropdown-basic-button" title="Categorías">
        {category && category.map((category) => (
          <Dropdown.Item
            key={category.categoryID}
            onClick={() => handleCategoryChange(category)}
          >
            {category.nameCategory}
          </Dropdown.Item>
        ))}
      </DropdownButton>
      {
        show ? <MostrarProductos handleClick={handleClick} Productos={productos} /> : <Carrito />
      }
      {showModal && <ModalProductoYaAgregado closeModal={closeModal} />}

      <footer>
        <p>Derechos de autor © 2024. Para mi primer sitio</p>
      </footer>
    </div>
  );
};