"use client"
import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import "bootstrap/dist/css/bootstrap.min.css";
import Carousel from 'react-bootstrap/Carousel';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import eventEmitter from './eventEmitter';

export const Products = () => {
  const [productList, setProductList] = useState(() => {
    const storedProductList = localStorage.getItem('productList');
    return storedProductList ? JSON.parse(storedProductList) : [];
  });

  const [paymentMethods, setPaymentMethods] = useState(() => {
    const storedPaymentMethods = localStorage.getItem('paymentMethods');
    return storedPaymentMethods ? JSON.parse(storedPaymentMethods) : [];
  });

  const URL = process.env.NEXT_PUBLIC_API_URL;
  if (!URL) {
    throw new Error('NEXT_PUBLIC_API_URL is not defined');
  }

  const [listNames, setListNames] = useState(() => {
    const storedListNames = localStorage.getItem('listNames');
    return storedListNames ? JSON.parse(storedListNames) : [];
  });
  const [selectedCategories, setSelectedCategories] = useState([]);
  const [searchText, setSearchText] = useState('');

  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    const fetchPaymentMethods = async () => {
      try {
        const response = await fetch(`${URL}/api/PaymentMethods`);
        if (!response.ok) {
          throw new Error('Failed to fetch payment methods');
        }
        const json = await response.json();
        localStorage.setItem('paymentMethods', JSON.stringify(json));
      } catch (error) {
        console.error('Error loading payment methods:', error);
      }
    };

    fetchPaymentMethods();
  }, [URL]);

  useEffect(() => {
    const handleStorageChange = () => {
      const updatedProductList = localStorage.getItem('productList');
      if (updatedProductList) {
        setProductList(JSON.parse(updatedProductList));
      }
    };

    window.addEventListener('storage', handleStorageChange);

    return () => {
      window.removeEventListener('storage', handleStorageChange);
    };
  }, []);

  useEffect(() => {
    const params = new URLSearchParams(location.search);
    const categoriesString = params.get('categories') || '';
    const searchText = params.get('search') || '';

    setSelectedCategories(categoriesString ? categoriesString.split(',').map(Number) : []);
    setSearchText(searchText);

    if (searchText || categoriesString) {
      loadFilteredData(categoriesString ? categoriesString.split(',') : [], searchText);
    } else {
      loadData();
    }
  }, [location.search]);

  useEffect(() => {
    localStorage.setItem('productList', JSON.stringify(productList));
  }, [productList]);

  useEffect(() => {
    localStorage.setItem('listNames', JSON.stringify(listNames));
  }, [listNames]);

  const loadData = async () => {
    try {
      const response = await fetch(`${URL}/api/store`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      if (!json) throw new Error('Failed to fetch data, null response');
      setListNames(json.categoriesNames);
      setProductList(json.products);
      setPaymentMethods(json.paymentMethods);
    } catch (error) {
      console.error('Error loading data:', error);
    }
  };

  const loadFilteredData = async (categories, searchText) => {
    try {
      const categoriesString = categories.length === 0 ? '0' : categories.join(',');
      const searchTextEncoded = searchText === '' ? '@' : encodeURIComponent(searchText);

      const url = `${URL}/api/store/products?categoriesString=${categoriesString}&searchText=${searchTextEncoded}`;
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      if (!json) throw new Error('Failed to fetch data, null response');
      setProductList(json.products);
    } catch (error) {
      console.error('Error loading filtered data:', error);
    }
  };

  const handleSearch = (categories, searchText) => {
    const categoriesString = categories.join(',');
    navigate(`/?categories=${categoriesString}&search=${encodeURIComponent(searchText)}`);
  };

  const toggleCategory = (categoryId) => {
    if (selectedCategories.includes(categoryId)) {
      setSelectedCategories(selectedCategories.filter(id => id !== categoryId));
    } else {
      setSelectedCategories([...selectedCategories, categoryId]);
    }
  };

  const addProduct = (product) => {
    const storedStore = localStorage.getItem("tienda");
    const storeData = JSON.parse(storedStore) || { carrito: { subtotal: 0, porcentajeImpuesto: 0.13, total: 0 }, productos: [] };
  
    if (storeData.productos.some(item => item.id === product.id)) {
      // Manejar producto ya en el carrito (puedes agregar lógica aquí si es necesario)
    } else {
      // Asegurarse de que el subtotal y total no sean null
      const updatedSubtotal = (storeData.carrito.subtotal || 0) + product.price;
      const updatedTotal = updatedSubtotal + (updatedSubtotal * (storeData.carrito.porcentajeImpuesto || 0));
  
      const updatedStore = {
        ...storeData,
        carrito: {
          ...storeData.carrito,
          subtotal: updatedSubtotal,
          total: updatedTotal
        },
        productos: [...storeData.productos, product]
      };
  
      localStorage.setItem("tienda", JSON.stringify(updatedStore));
      eventEmitter.emit('cartUpdated', updatedStore);
    }
  };
  

  const Product = ({ product }) => {
    const { name, description, imageURL, price } = product;
    return (
      <div className="col-sm-3">
        <div className='info-product'>
          <h2>{name}</h2>
          <div className='price' dangerouslySetInnerHTML={{ __html: description }}></div>
          <div className='price'>Precio: ₡{price}</div>
          <img src={imageURL} alt={name} />
          <button onClick={() => addProduct(product)}>Agregar al Carrito</button>
        </div>
      </div>
    );
  };

  return (
    <div className="container-fluid vh-100">
      <form className="form-inline my-2 my-lg-0" onSubmit={(event) => {
        event.preventDefault();
        handleSearch(selectedCategories, searchText);
      }}>
        <input
          className="form-control mr-sm-2"
          type="search"
          placeholder="Search"
          aria-label="Search"
          value={searchText}
          onChange={(e) => setSearchText(e.target.value)}
        />
        <button
          className="btn btn-outline-success my-2 my-sm-0"
          type="submit"
        >
          Search
        </button>
      </form>
      <nav className="navbar navbar-expand-lg bg-body-tertiary">
        <div className="container-fluid">
          <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup"
            aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
            <div className="navbar-nav">
              <div className="secciones">
                <div className="row">
                  <button className="nav-link btn btn-primary" onClick={() => loadData()}>Página Principal</button>
                  <div className="col-sm-2">
                    <div className="dropdown">
                      <button className="btn btn-primary dropdown-toggle" type="button" id="dropdownMenuButton" data-bs-toggle="dropdown" aria-expanded="false">
                        Selecciona una categoría
                      </button>
                      <ul className="dropdown-menu" aria-labelledby="dropdownMenuButton">
                        {listNames.map((category) => (
                          <li key={category.id}>
                            <label className="dropdown-item">
                              <input type="checkbox" onChange={() => toggleCategory(category.id)} checked={selectedCategories.includes(category.id)} />
                              {category.name}
                            </label>
                          </li>
                        ))}
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </nav>
      <div className="row flex-grow-1">
        {productList && Array.isArray(productList) && productList.map(product => (
          <Product key={product.id} product={product} />
        ))}
      </div>
      <Carousel data-bs-theme="dark" className="flex-grow-1">
        {productList && productList.map(product => (
          <Carousel.Item key={product.id} interval={1000}>
            <div className="info-product">
              <h2>{product.name}</h2>
              <img src={product.imageURL} alt={product.name} />
              <button onClick={() => addProduct(product)}>Agregar al Carrito</button>
            </div>
          </Carousel.Item>
        ))}
      </Carousel>
    </div>
  );
}
