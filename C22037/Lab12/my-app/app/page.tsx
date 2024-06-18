"use client"; // Para utilizar el cliente en lugar del servidor
import "bootstrap/dist/css/bootstrap.min.css";
import "@/public/styles.css";
import { useState, useEffect } from "react";
import { Carousel } from 'react-bootstrap';
import Link from 'next/link';

export default function Home() {
  const [searchQuery, setSearchQuery] = useState("");
  const [count, setCount] = useState(0);
  const [productList, setProductList] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState([]);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  useEffect(() => {
    const { search, categories: categoryIds } = getQueryParams();
    setSearchQuery(search);

    const loadData = async () => {
      try {
        const storeData = JSON.parse(localStorage.getItem('store')) || {};
        if (storeData.products) {
          setProductList(storeData.products);
          setCategories(storeData.categories || []);
        } else {
          const response = await fetch(`https://localhost:7067/api/store`);
          if (!response.ok) {
            throw new Error('Failed to fetch data');
          }
          const data = await response.json();
          setProductList(data.store.products);
          setCategories(data.categories);

          localStorage.setItem('store', JSON.stringify(data.store));
        }

        const selectedCategories = categories.filter(category => categoryIds.includes(category.id.toString()));
        setSelectedCategory(selectedCategories);

        if (search || categoryIds.length > 0) {
          const searchResponse = await fetch(`https://localhost:7067/api/store/search?search=${search}&categories=${categoryIds.join(',') || 'null'}`);
          if (!searchResponse.ok) {
            throw new Error('Failed to fetch data.');
          }
          const searchData = await searchResponse.json();
          setProductList(searchData.products);
        }
      } catch (error) {
        throw new Error('Error loading data.');
      }
    };

    const storedCart = JSON.parse(localStorage.getItem('cart')) || {};
    if (storedCart.products) {
      setCount(Object.keys(storedCart.products).length);
    }

    const initialCart = JSON.parse(localStorage.getItem('cart')) || { products: {} };
    localStorage.setItem('cart', JSON.stringify(initialCart));

    loadData();
  }, []);

  const updateUrl = (searchQuery, selectedCategory) => {
    const categoryIds = selectedCategory.map(category => category.id).join(',');
    const url = `?search=${searchQuery}&categories=${categoryIds}`;
    window.history.pushState(null, '', url);
  };

  const getQueryParams = () => {
    const params = new URLSearchParams(window.location.search);
    const search = params.get('search') || '';
    const categories = params.get('categories') ? params.get('categories').split(',') : [];
    return { search, categories };
  };

  const handleSearchInputChange = (e) => {
    setSearchQuery(e.target.value);
  };

  const handleSearchSubmit = async (e) => {
    e.preventDefault();

    updateUrl(searchQuery, selectedCategory);

    let url = `https://localhost:7067/api/store/search?search=${searchQuery}`;

    if (selectedCategory.length > 0) {
      const categoryIds = selectedCategory.map(category => category.id).join(',');
      url += `&categories=${categoryIds}`;
    } else {
      url += "&categories=null";
    }

    try {
      const response = await fetch(url);
      const data = await response.json();

      if (!response.ok) {
        throw new Error('Failed to fetch data.');
      }

      setProductList(data.products);
    } catch (error) {
      throw new Error('Error fetching data.');
    }
  };

  const handleAddToCart = (productId) => {
    if (productId === undefined) {
      throw new Error('ProductId cannot be undefined.');
    }

    const storedCart = JSON.parse(localStorage.getItem('cart')) || { products: {} };
    const productToAdd = productList.find(product => product.id === productId);
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

  const handleCategoryChange = async (category, isChecked) => {
    try {
      if (category === undefined) {
        throw new Error('Category cannot be undefined.');
      }

      const updatedCategories = isChecked
        ? [...(selectedCategory || []), category]
        : selectedCategory.filter(c => c.id !== category.id);

      setSelectedCategory(updatedCategories);
      setIsDropdownOpen(false);

      updateUrl(searchQuery, updatedCategories);

      const response = await fetch(`https://localhost:7067/api/store/products?${updatedCategories.length > 0 ? `categories=${updatedCategories.map(c => c.id).join(',')}` : 'categories=null'}`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }

      const data = await response.json();
      setProductList(data.products);
    } catch (error) {
      throw new Error('Error fetching data.');
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

  const rows = (array, size) => {
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
  };

  return (
    <div>
      <div className="header">
        <div className="row">
          <div className="col-sm-2">
            <h1>Tienda</h1>
          </div>
          <div className="col-sm-6">
            <div className="col-sm-6 d-flex align-items-center">
              <form onSubmit={handleSearchSubmit}>
                <label>
                  <input
                    type="search"
                    name="q"
                    autoComplete="off"
                    value={searchQuery}
                    onChange={handleSearchInputChange}
                  />
                </label>
                <button type="submit" disabled={!searchQuery.trim()}>
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="30"
                    height="30"
                    fill="currentColor"
                    className="bi bi-search"
                    viewBox="0 0 16 16"
                  >
                    <path
                      d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0"
                    />
                  </svg>
                </button>
              </form>
            </div>
            <div className="dropdown">
              <button
                className="btn btn-secondary dropdown-toggle"
                type="button"
                onClick={handleDropdownToggle}
              >
                {selectedCategory.length > 0 ? `Categories Selected: ${selectedCategory.length}` : "Categories"}
              </button>
              <ul className={`dropdown-menu ${isDropdownOpen ? "show" : ""}`} aria-labelledby="dropdownMenuButton">
                {categories.map((category) => (
                  <li key={category.id}>
                    <div className="form-check">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        checked={selectedCategory.some(c => c.id === category.id)}
                        onChange={(e) => handleCategoryChange(category, e.target.checked)}
                      />
                      <label className="form-check-label">{category.name}</label>
                    </div>
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