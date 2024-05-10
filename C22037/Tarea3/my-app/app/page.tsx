"use client"; //Para utilizar el cliente en lugar del servidor
import "bootstrap/dist/css/bootstrap.min.css";
import "@/public/styles.css";
import { useState, useEffect } from "react";
import { Carousel } from 'react-bootstrap';
import Link from 'next/link';

export default function Home() {
  const [count, setCount] = useState(0);
  const [productList, setProductList] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await fetch(`https://localhost:7067/api/Store`);
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const data = await response.json();
        setProductList(data.store.products);
        setCategories(data.categories);
      } catch (error) {
        throw new Error('Failed to fetch data.');
      }
    };

    const storedCart = JSON.parse(localStorage.getItem('cart')) || {};
    if (storedCart.products) {
      setCount(Object.keys(storedCart.products).length);
    }

    const initialCart = JSON.parse(localStorage.getItem('cart')) || {
      products: {},
    };

    localStorage.setItem('cart', JSON.stringify(initialCart));

    loadData();
  }, []);

  const handleAddToCart = (productId) => {

    if (productId === undefined) {
      throw new Error('ProductId cannot be undefined.');
    }

    const storedCart = JSON.parse(localStorage.getItem('cart')) || { products: {} };
    const productToAdd = productList.products.find(product => product.id === productId);
    if (productToAdd) {
      const updatedCart = {
        ...storedCart,
        products: { ...storedCart.products, [productId]: productToAdd }
      };
      localStorage.setItem('cart', JSON.stringify(updatedCart));
      setCount(Object.keys(updatedCart.products).length);
    }
  };

  const handleDropdownToggle = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };

  const handleCategoryChange = async (category) => {
    try {
      if (category === undefined) {
        throw new Error('Category cannot be undefined.');
      }

      setSelectedCategory(category);
      setIsDropdownOpen(false);

      const response = await fetch(`https://localhost:7067/api/Store/products?category=${category.id}`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }

      const data = await response.json();

      const products = data.products.result.products;
      setProductList(products);
    } catch (error) {
      throw new Error('Failed to fetch data');
    }
  };

  const Product = ({ product }) => {
    if (product === undefined) {
      throw new Error('Product cannot be undefined.');
    }

    return (
      <div className="col-sm-3">
        <div className="Product">
          <img src={product.imageURL} alt={product.name} style={{ width: '50%', height: '50%' }} />
          <h3>{product.name}</h3>
          <p>{product.description}</p>
          <p>Precio: ${product.price}</p>
          <button onClick={() => handleAddToCart(product.id)} className="Button">Add to Cart</button>
        </div>
      </div>
    );
  };

  const ProductCarousel = () => {
    return (
      <Carousel>
        {productList && productList.map((product, index) => (
          <Carousel.Item key={product.id}>
            <img
              className="d-block w-100"
              src={product.imageURL}
              alt={product.name}
            />
            <button onClick={() => handleAddToCart(product.id)} className="Button">Add to Cart</button>
          </Carousel.Item>
        ))}
      </Carousel>
    );
  };


  function rows(array, size) {

    if (array === undefined) {
      throw new Error("There are no products.");
    }

    if (size === undefined) {
      throw new Error("No index specified.");
    }

    const row = [];
    for (let i = 0; i < array.length; i += size) {
      row.push(array.slice(i, i + size));
    }

    return row;
  }

  return (
    <div>
      <div className="header">
        <div className="row">
          <div className="col-sm-2">
            <h1>Tienda</h1>
          </div>
          <div className="col-sm-6">
            <div className="col-sm-6 d-flex align-items-center">
              <search>
                <label>
                  <input type="search" name="q" autoComplete="off" />
                </label>
                <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16">
                  <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0" />
                </svg>
              </search>
            </div>
            <div className="dropdown">
              <button
                className="btn btn-secondary dropdown-toggle"
                type="button"
                onClick={handleDropdownToggle}
              >
                {selectedCategory ? selectedCategory.name : "Categories"}
              </button>
              <ul className={`dropdown-menu ${isDropdownOpen ? "show" : ""}`} aria-labelledby="dropdownMenuButton">
                {categories.map((category) => (
                  <li key={category.id}>
                    <button className="dropdown-item" onClick={() => handleCategoryChange(category)}>
                      {category.name}
                    </button>
                  </li>
                ))}
              </ul>
            </div>
          </div>
          <div className="col-sm-4 d-flex justify-content-end">
            <Link href="/cart">
              <div className="position-relative">
                <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" fill="currentColor" className="bi bi-bag" viewBox="0 0 16 16">
                  <path d="M8 1a2.5 2.5 0 0 1 2.5 2.5V4h-5v-.5A2.5 2.5 0 0 1 8 1m3.5 3v-.5a3.5 3.5 0 1 0-7 0V4H1v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V4zM2 5h12v9a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1z" />
                </svg>
                {count > 0 && <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">{count}</span>}
              </div>
            </Link>
            <Link href="/admin">
              <svg xmlns="http://www.w3.org/2000/svg" width="60" height="60" fill="currentColor" className="bi bi-box-arrow-in-right" viewBox="0 0 16 16">
                <path fillRule="evenodd" d="M6 3.5a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-2a.5.5 0 0 0-1 0v2A1.5 1.5 0 0 0 6.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-8A1.5 1.5 0 0 0 5 3.5v2a.5.5 0 0 0 1 0z" />
                <path fillRule="evenodd" d="M11.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H1.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708z" />
              </svg>
            </Link>
          </div>
        </div>
      </div>

      <div className="body">
        <h2>Products List</h2>
        {productList && rows(productList, 4).map((row, index) => (
          <div key={index} className="body">
            <div style={{ display: 'flex', flexWrap: 'wrap' }}>
              {row.map(product => (
                <Product key={product.id} product={product} />
              ))}
            </div>
            <div className="carousel-container">
              <ProductCarousel id={index} />
            </div>
          </div>
        ))}
      </div>

      <div className="footer">
        <div className="row">
          <div className="col-sm-12">
            <h3>Tienda.com</h3>
          </div>
        </div>
      </div>

    </div>
  );
}