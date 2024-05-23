'use client';
import React, { useState, useEffect } from 'react';
import { Carousel } from 'react-bootstrap';
import Link from 'next/link';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

const Page = () => {
  const [state, setState] = useState({
    cart: {
      productos: [],
      count: 0,
    },
    productList: [],
    categories: [],
    selectedCategory: '',
  });

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('https://localhost:7043/api/Store');
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      const { products, categories } = json;

      setState({
        ...state,
        productList: products || [],
        categories: categories || [],
      });
    };

    fetchData();
  }, []);

  const handleAddToCart = (product) => {
    if (!product || typeof product !== 'object' || !product.hasOwnProperty('id') || !product.hasOwnProperty('name') || !product.hasOwnProperty('price')) {
      throw new Error('Invalid product object');
    }

    const { cart } = state;
    const { productos, count } = cart;
    const isProductInCart = productos.some(item => item.id === product.id);

    if (!isProductInCart) {
      const updatedCart = {
        ...cart,
        productos: [...productos, product],
        count: count + 1,
      };

      setState({
        ...state,
        cart: updatedCart,
      });

      localStorage.setItem('cartData', JSON.stringify(updatedCart));
    } else {
      throw new Error('Product is already in the cart');
    }
  };

  const handleCategoryChange = async (event) => {
    if (!event || typeof event !== 'object' || !event.hasOwnProperty('target') || typeof event.target !== 'object') {
      throw new Error('Invalid event object');
    }

    const selectedCategoryId = event.target.value;

    if (selectedCategoryId === '') {
      const response = await fetch('https://localhost:7043/api/store');
      if (!response.ok) {
        throw new Error('Failed to fetch products');
      }
      const json = await response.json();
      const { products } = json;

      setState({
        ...state,
        productList: products || [],
        selectedCategory: '',
      });
    } else {
     
      const response = await fetch(`https://localhost:7043/api/products?categoryId=${selectedCategoryId}`);
      if (!response.ok) {
        throw new Error('Failed to fetch products');
      }
      const json = await response.json();
      const productList = json || [];

      setState({
        ...state,
        productList,
        selectedCategory: selectedCategoryId,
      });
    }
  };

  const renderCategories = () => {
    const { categories } = state;

    return (
      <select value={state.selectedCategory} onChange={handleCategoryChange} className="form-select mb-3">
        <option value="">Todas las categorías</option>
        {categories.map(category => (
          <option key={category.id} value={category.id}>
            {category.name}
          </option>
        ))}
      </select>
    );
  };

  const renderProducts = () => {
    const { productList } = state;

    return productList.map(product => (
      <div key={product.id} className="col-sm-3 mb-4">
        <div className="card">
          <img src={product.imageUrl} className="card-img-top" alt={product.name} />
          <div className="card-body">
            <h5 className="card-title">{product.name}</h5>
            <p className="card-text">{product.description}</p>
            <p className="card-text">Precio: ${product.price}</p>
            <p className="card-text"> Categoria: {product.category.name} </p>
            <button className="btn btn-primary" onClick={() => handleAddToCart(product)}>
              Comprar
            </button>
          </div>
        </div>
      </div>
    ));
  };

  
  const renderCarouselItems = () => {
  
  
    if (!state.productList || state.productList.length === 0) {
      return null; 
    }
  
    const featuredProducts = state.productList.slice(0, 5); 
  
    return (
      <Carousel>
        {featuredProducts.map((product) => (
          <Carousel.Item key={product.id}>
            <div className="d-flex flex-column align-items-center">
              <img
                src={product.imageUrl}
                alt={product.name}
                style={{ maxHeight: '300px', maxWidth: '100%', objectFit: 'contain' }}
              />
              <div className="mt-3 text-center">
                <h3>{product.name}</h3>
                <p>{product.description}</p>
                <p>Precio: ${product.price}</p>
                <p>Categoria: {product.category.name} </p>
                <button className="btn btn-primary" onClick={() => handleAddToCart(product)}>
                  Comprar
                </button>
              </div>
            </div>
          </Carousel.Item>
        ))}
      </Carousel>
    );
  };
  
  return (
    <div className="container">
      <div className="row">
        <div className="col-sm-8 text-center">
          <form action="/buscar" method="GET" className="mt-2">
            <input type="text" name="q" placeholder="Buscar..." />
            <button type="submit" className="btn btn-primary">
              Buscar
            </button>
          </form>
        </div>
        <div className="col-sm-2">
          <div className="my-3">
            <Link href="/cart">
              <button className="btn btn-primary">
                <i className="bi bi-cart-fill"></i>
              </button>
            </Link>
            <span className="badge rounded-pill bg-danger">{state.cart.count}</span>
          </div>
        </div>
        <div className="col-sm-2">
          <div className="my-3">
            <Link href="/admin">
              <button className="btn btn-primary">
                <i className="bi bi-person"></i>
              </button>
            </Link>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col-sm-4">
          {renderCategories()}
        </div>
      </div>
      <div className="row">
        <h1 className="mb-0">Lista de Productos</h1>
      </div>
      <div className="row row-cols-4 g-4">
        {renderProducts()}
      </div>
      <div className="row mt-4">
        <h2 className="mb-3">Productos Destacados</h2>
        {renderCarouselItems()}
      </div>
      <footer className="footer mt-auto py-3" style={{ backgroundColor: '#ADD8E6' }}>
        <div className="container">
          <span className="text-muted">Mariano Duran Artavia</span>
        </div>
      </footer>
    </div>
  );
};

export default Page;
