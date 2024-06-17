'use client';
import React, { useState, useEffect } from 'react';
import { Carousel } from 'react-bootstrap';
import Link from 'next/link';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
const URL = process.env.NEXT_PUBLIC_API;

const Page = () => {
  const [state, setState] = useState({
    cart: {
      productos: [],
      count: 0,
    },
    productList: [],
    categories: [],
    selectedCategories: [],
  });

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch('https://localhost:7043/api/store');
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

  const buildUrl = (selectedCategories) => {
    // Validamos si selectedCategories es un array
    if (!Array.isArray(selectedCategories)) {
      throw new Error('selectedCategories debe ser un array');
    }

    const baseUrl = 'https://localhost:7043/api/Products';
    const queryParams = selectedCategories.map(id => `categoryIDs=${id}`).join('&');
    return `${baseUrl}?${queryParams}&search=null`;
  };

  const handleCategoryChange = async (categoryId) => {
    const { selectedCategories } = state;

    let updatedCategories = [];

    // Verificamos si la categoría es la de "Todos Productos"
    if (categoryId === 'all') {
      // Si es "Todos Productos" y ya estaba seleccionada, la deseleccionamos
      if (selectedCategories.length === state.categories.length) {
        updatedCategories = [];
      } else {
        // Si no estaba seleccionada, la seleccionamos junto con todas las demás categorías
        updatedCategories = state.categories.map(category => category.id);
      }
    } else {
      // Si no es "Todos Productos", manejamos la selección de categorías normalmente
      const index = selectedCategories.indexOf(categoryId);
      if (index === -1) {
        updatedCategories = [...selectedCategories, categoryId];
      } else {
        updatedCategories = selectedCategories.filter(id => id !== categoryId);
      }
    }

    // Actualizamos el estado con las categorías seleccionadas
    setState({
      ...state,
      selectedCategories: updatedCategories,
    });

    // Construimos la URL con todas las categorías seleccionadas
    const url = buildUrl(updatedCategories);

    // Realizamos el fetch solo si hay categorías seleccionadas
    if (updatedCategories.length > 0) {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error('Failed to fetch products');
      }
      const json = await response.json();
      const productList = json || [];

      // Actualizamos el estado con la lista de productos y las categorías seleccionadas
      setState({
        ...state,
        productList,
        selectedCategories: updatedCategories,
      });

      // Actualizar la URL del navegador para reflejar la búsqueda
      const searchParams = new URLSearchParams({ categoryIDs: updatedCategories.join(',') });
      window.history.pushState(null, '', `${window.location.pathname}?${searchParams.toString()}`);
    } else {
      // Si no hay categorías seleccionadas, limpiamos la lista de productos
      setState({
        ...state,
        productList: [],
      });
      // Si no hay categorías seleccionadas, limpiamos la URL
      window.history.pushState(null, '', window.location.pathname);
    }
  };



  const handleSearch = async (event) => {
    if (!event || typeof event !== 'object' || !event.hasOwnProperty('target') || typeof event.target !== 'object') {
      throw new Error('Invalid event object');
    }
    event.preventDefault();
    const query = event.target.q.value.trim();
    const { selectedCategories } = state;

    // Construimos la URL con las categorías seleccionadas y la búsqueda
    let url = 'https://localhost:7043/api/Products';
    const queryParams = [];

    // Agregamos las categorías seleccionadas a los parámetros de la URL
    selectedCategories.forEach(categoryID => {
      queryParams.push(`categoryIDs=${categoryID}`);
    });

    // Agregamos el parámetro de búsqueda si hay una consulta
    if (query) {
      queryParams.push(`search=${encodeURIComponent(query)}`);
    } else {
      // Si no hay consulta, agregamos "null" como parámetro de búsqueda
      queryParams.push('search=null');
    }

    // Unimos todos los parámetros de la URL
    if (queryParams.length > 0) {
      url += `?${queryParams.join('&')}`;
    }

    const response = await fetch(url);
    if (!response.ok) {
      throw new Error('Failed to fetch products');
    }
    const productList = await response.json();

    setState((prevState) => ({
      ...prevState,
      productList,
    }));

    const searchParams = new URLSearchParams({ categoryIDs: selectedCategories.join(','), search: query || 'null' });
    window.history.pushState(null, '', `${window.location.pathname}?${searchParams.toString()}`);
  };

  const renderCategories = () => {
    const { categories } = state;

    return (
      <div>
        {/* Checkbox para "Todos Productos" */}
        <div key="all">
          <input
            type="checkbox"
            id="all"
            value="all"
            onChange={() => handleCategoryChange('all')}
            checked={state.selectedCategories.length === categories.length}
            style={{ marginRight: '5px' }}
          />
          <label htmlFor="all" style={{ marginRight: '10px' }}>Todos Productos</label>
        </div>

        {/* Checkbox para cada categoría */}
        {categories.map(category => (
          <div key={category.id}>
            <input
              type="checkbox"
              id={`category-${category.id}`}
              value={category.id}
              onChange={() => handleCategoryChange(category.id)}
              checked={state.selectedCategories.includes(category.id)}
              style={{ marginRight: '5px' }}
            />
            <label htmlFor={`category-${category.id}`} style={{ marginRight: '10px' }}>{category.name}</label>
          </div>
        ))}
      </div>
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
            <p className="card-text">${product.price}</p>

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
                <p>${product.price}</p>

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
      <div className="row align-items-center">
        {/* Formulario de búsqueda */}
        <div className="col-sm-6 text-center">
          <form onSubmit={handleSearch} className="d-flex justify-content-center align-items-center" autoComplete="off">
            <input type="text" name="q" placeholder="Buscar..." className="form-control me-2" />
            <button type="submit" className="btn btn-primary">
              Buscar
            </button>
          </form>
        </div>

        {/* Botones de Carrito y Admin */}
        <div className="col-sm-6 text-end">
          <div className="my-3">
            <Link href="/cart">
              <button className="btn btn-primary me-2">
                <i className="bi bi-cart-fill"></i> Carrito
              </button>
            </Link>
            <span className="badge rounded-pill bg-danger me-2">{state.cart.count}</span>
            <Link href="/admin">
              <button className="btn btn-primary">
                <i className="bi bi-person"></i> Admin
              </button>
            </Link>
          </div>
        </div>
      </div>

      {/* Categorías */}
      <div className="row">
        <div className="col-sm-4">
          {renderCategories()}
        </div>
      </div>

      {/* Lista de Productos */}
      <div className="row">
        <div className="col">
          <h1 className="mb-3">Lista de Productos</h1>
        </div>
      </div>

      <div className="row row-cols-4 g-4">
        {renderProducts()}
      </div>

      {/* Productos Destacados */}
      <div className="row mt-4">
        <div className="col">
          <h2 className="mb-3">Productos Destacados</h2>
          {renderCarouselItems()}
        </div>
      </div>

      {/* Footer */}
      <footer className="footer mt-auto py-3" style={{ backgroundColor: '#ADD8E6' }}>
        <div className="container text-center">
          <span className="text-muted">Mariano Duran Artavia</span>
        </div>
      </footer>
    </div>
  );
};

export default Page;
