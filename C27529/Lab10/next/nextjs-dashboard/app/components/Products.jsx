"use client"
import React, { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import "bootstrap/dist/css/bootstrap.min.css";
import Carousel from 'react-bootstrap/Carousel';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

export const Products = () => {
  const [productList, setProductList] = useState([]);
  const [listNames, setListNames] = useState([]);
  const [selectedCategories, setSelectedCategories] = useState([]);
  const [searchText, setSearchText] = useState('');

  const navigate = useNavigate();
  const location = useLocation();


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

  const loadData = async () => {
    try {
      const response = await fetch(`https://localhost:7280/api/store`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      if (!json) throw new Error('Failed to fetch data, null response');
      setListNames(json.categoriesNames);
      setProductList(json.products); 
    } catch (error) {
      console.error('Error loading data:', error);
    }
  };

  const loadFilteredData = async (categories, searchText) => {
    try {
      const categoriesString = categories.length === 0 ? '0' : categories.join(',');
      const searchTextEncoded = searchText === '' ? '@' : encodeURIComponent(searchText);

      const url = `https://localhost:7280/api/store/products?categoriesString=${categoriesString}&searchText=${searchTextEncoded}`;
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

  

  const [storeData, setStoreData] = useState(() => {
    const storedStoreData = localStorage.getItem("tienda");
    return JSON.parse(storedStoreData) || { carrito: { subtotal: 0, porcentajeImpuesto: 0 }, productos: [] };
  });

  const [showModal, setShowModal] = useState(false);

  const closeModal = () => {
    setShowModal(false);
  };

  const onAddProduct = (product) => {
    if (!product) throw new Error('Invalid Parameter');
    if (storeData.productos.some(item => item.id === product.id)) {
      setShowModal(true);
    } else {
      const updatedStore = {
        ...storeData,
        carrito: {
          ...storeData.carrito,
          subtotal: storeData.carrito.subtotal + product.price,
          total: ((storeData.carrito.subtotal + product.price) * storeData.carrito.porcentajeImpuesto) + (storeData.carrito.subtotal + product.price)
        },
        productos: [...storeData.productos, product]
      };
      setStoreData(updatedStore);
      localStorage.setItem("tienda", JSON.stringify(updatedStore));
    }
  };

  const Product = ({ product, onAddProduct }) => {
    const { name, description, imageURL, price } = product;
    return (
      <div className="col-sm-3">
        <div className='info-product'>
          <h2>{name}</h2>
          <div className='price'>{description}</div>
          <div className='price'>Precio: ₡{price}</div>
          <img src={imageURL} alt={name} />
          <button onClick={() => onAddProduct(product)}>Agregar al Carrito</button>
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
                  <button className="nav-link btn btn-primary" onClick={()=>loadData()}>Página Principal</button>
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
      {showModal && <ModalError closeModal={closeModal} />}
      <div className="row flex-grow-1">
        {productList && Array.isArray(productList) && productList.map(product => (
          <Product key={product.id} product={product} onAddProduct={onAddProduct} />
        ))}
      </div>
      <Carousel data-bs-theme="dark" className="flex-grow-1">
        {productList && productList.map(product => (
          <Carousel.Item key={product.id} interval={1000}>
            <div className="info-product">
              <h2>{product.name}</h2>
              <img src={product.imageURL} alt={product.name} />
            </div>
          </Carousel.Item>
        ))}
      </Carousel>
    </div>
  );
}

const ModalError = ({ closeModal }) => {
  return (
    <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Producto ya agregado</h5>
            <button type="button" onClick={closeModal} className="close" aria-label="Close">
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
