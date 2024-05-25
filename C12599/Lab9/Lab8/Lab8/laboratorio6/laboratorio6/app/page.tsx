'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import { Carousel } from 'react-bootstrap';
import Link from 'next/link';

const Page = () => {
  const [state, setState] = useState({
    cart: {
      productos: [],
      count: 0,
    },
    productList: [],
  });

  useEffect(() => {
    // Función para cargar datos iniciales de productos desde la API
    const fetchData = async () => {
        const response = await fetch('https://localhost:7043/api/Store');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const json = await response.json();
        setState((prevState) => ({
          ...prevState,
          productList: json.products || [],
        }));
    
    };

    fetchData();
  }, []);

  useEffect(() => {
    // Cargar datos del carrito desde localStorage al montar el componente
    const cartData = localStorage.getItem('cartData');
    if (cartData) {
      const parsedCartData = JSON.parse(cartData);
      setState((prevState) => ({
        ...prevState,
        cart: parsedCartData,
      }));
    }
  }, []);

  const handleAddToCart = (product) => {
    const { cart } = state;
    const { productos, count } = cart;

    const isProductInCart = productos.some((item) => item.id === product.id);

    if (isProductInCart) {
      return;
    }

    const updatedCart = {
      ...cart,
      productos: [...productos, product],
      count: count + 1,
    };

    setState((prevState) => ({
      ...prevState,
      cart: updatedCart,
    }));

    localStorage.setItem('cartData', JSON.stringify(updatedCart));
  };

  const renderGridItems = (start, end) => {
    const { productList } = state;
    if (!productList || productList.length === 0) {
      return null;
    }

    const slicedProducts = productList.slice(start, end);
    return slicedProducts.map((product) => (
      <div key={product.id} className="col-sm-3 mb-4">
        <div className="card">
          <img src={product.imageUrl} className="card-img-top" alt={product.name} />
          <div className="card-body">
            <div className="text-center">
              <h5 className="card-title my-3">{product.name}</h5>
              <p className="card-text my-3">{product.description}</p>
              <p className="card-text my-3">Precio: ${product.price}</p>
              <button className="btn btn-primary" onClick={() => handleAddToCart(product)}>
                Comprar
              </button>
            </div>
          </div>
        </div>
      </div>
    ));
  };

  const renderCarouselItems = (start, end) => {
    const { productList } = state;
    if (!productList || productList.length === 0) {
      return null;
    }

    const slicedProducts = productList.slice(start, end);
    return slicedProducts.map((product) => (
      <Carousel.Item key={product.id}>
        <div className="text-center">
          <img src={product.imageUrl} alt={product.name} style={{ maxHeight: '300px' }} />
          <h3>{product.name}</h3>
          <p>{product.description}</p>
          <p>Precio: ${product.price}</p>
          <button className="btn btn-primary" onClick={() => handleAddToCart(product)}>
            Comprar
          </button>
        </div>
      </Carousel.Item>
    ));
  };

  return (
    <div className="container">
      <div className="row">
        <div className="col-sm-10 text-center">
          <form action="/buscar" method="GET" className="mt-2">
            <input type="text" name="q" placeholder="Buscar..." />
            <button type="submit" className="btn btn-primary">
              Buscar
            </button>
          </form>
        </div>
        <div className="col-sm-2">
          <div className="my-3" style={{ position: 'relative', display: 'inline-block' }}>
            <Link href="/cart">
              <button className="btn btn-primary">
                <i className="bi bi-cart-fill"></i>
              </button>
            </Link>
            <span
              className="badge rounded-pill bg-danger"
              style={{
                position: 'absolute',
                top: '-10px',
                right: '-10px',
                fontSize: '0.8rem',
                minWidth: '20px',
                padding: '5px',
              }}
            >
              {state.cart.count}
            </span>
          </div>
        </div>
      </div>
      <div className="row">
        <h1 className="mb-0">Lista de Productos</h1>
      </div>
      <div className="row">{renderGridItems(0, 4)}</div>
      <div className="row mt-4">
        <h2 className="mb-3">Productos Destacados</h2>
        <Carousel>{renderCarouselItems(4, 8)}</Carousel>
      </div>
      <div className="row mt-4">{renderGridItems(8, 12)}</div>
      <footer className="footer mt-auto py-3" style={{ backgroundColor: '#ADD8E6' }}>
        <div className="container">
          <span className="text-muted">Mariano Duran Artavia</span>
        </div>
      </footer>
    </div>
  );
};

export default Page;
