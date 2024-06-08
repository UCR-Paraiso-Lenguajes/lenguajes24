"use client";
import "bootstrap/dist/css/bootstrap.min.css";
import React, { useState, useEffect, useCallback } from 'react';
import "@/app/ui/styles.css";
import Header from "@/app/navStarBuyStore/page";
import Carrito from "@/app/Cart/page";
import Carousel from 'react-bootstrap/Carousel';

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
  if (handleClick == undefined || Productos == undefined) { throw new Error('Error: Los argumentos de MostrarProductos no pueden ser nulos.'); }
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
  const [showNoResultsModal, setShowNoResultsModal] = useState(false);
  const URLConection = process.env.NEXT_PUBLIC_API;

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(URLConection+'/api/store');//hola
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

  const fetchDataByCategory = async (searchText, categoryIDs) => {
    if (categoryIDs == undefined) throw new Error("Error: Los argumentos fetchDataByCategory  no pueden ser indefinidos.");// esta lista si puede ser 0
    if (searchText==undefined) throw new Error("Error: Los argumentos fetchDataByCategory  no pueden ser indefinidos.");
    try {
      const url = new URL(URLConection+'/api/store/products');
      if (searchText) {
        url.searchParams.append('searchText', searchText);
      }
      if (categoryIDs && categoryIDs.length > 0) {
        categoryIDs.forEach(categoryID => {
          url.searchParams.append('categoryIDs', categoryID.toString());
        });
      }
      window.history.pushState(null, '','?'+url.searchParams);
      const response = await fetch(url.toString());
      if (!response.ok) {
        throw new Error('Failed to fetch filtered products');
      }
      const data = await response.json();
      const filteredProducts = { products: data.filteredProducts };
      if (data.filteredProducts.length === 0) {
        setShowNoResultsModal(true);
        setProductos([]);
      } else {
        setProductos(filteredProducts);
        setShowNoResultsModal(false);
      }
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
  const closeNoResultsModal = () => {
    setShowNoResultsModal(false);
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
      <Header
        size={store.productos.length}
        setShow={setShow}
        fetchData={fetchDataByCategory}
        category={category}
      />
      {
        show ? <MostrarProductos handleClick={handleClick} Productos={productos} /> : <Carrito />
      }
      {showModal && <ModalProductoYaAgregado closeModal={closeModal} />}
      {showNoResultsModal && (
        <Modal
          title="No se encontraron resultados"
          content="No se encontraron productos según los criterios de búsqueda seleccionados."
          onClose={closeNoResultsModal}
        />
      )}
      <footer>
        <p>Derechos de autor © 2024. Para mi primer sitio</p>
      </footer>
    </div>
  );
};


const Modal = ({ title, content, onClose, closeButtonText = 'Cerrar', showCloseButton = true }) => {
  if (!title || !content || !onClose || typeof onClose !== 'function') {
    throw new Error('Error: Los argumentos title, content y onClose son obligatorios y onClose debe ser una función.');
  }
  if (title === undefined || content === undefined || onClose === undefined) {
    throw new Error('Error: Los argumentos title, content no pueden ser indefinidos');
  }
  const handleCerrarModal = () => {
    onClose();
    window.location.reload();
  
}; 
 return (
    <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{title}</h5>
            {showCloseButton && (
              <button type="button" className="close" onClick={handleCerrarModal} aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            )}
          </div>
          <div className="modal-body">
            <p>{content}</p>
          </div>
          {showCloseButton && (
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={handleCerrarModal}>
                {closeButtonText}
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};