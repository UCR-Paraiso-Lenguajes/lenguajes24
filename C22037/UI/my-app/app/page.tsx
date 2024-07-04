"use client"; // Para utilizar el cliente en lugar del servidor
import "bootstrap/dist/css/bootstrap.min.css";
import "../public/styles.css";
import { useState, useEffect } from "react";
import { Carousel, Dropdown } from 'react-bootstrap';
import { HubConnectionBuilder } from '@microsoft/signalr';
import Link from 'next/link';

interface Category {
  id: number;
  name: string;
}

interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageURL: string;
  category: Category;
}

interface Notification {
  id: number;
  message: string;
  read: boolean;
}

export default function Home() {
  const [searchQuery, setSearchQuery] = useState("");
  const [count, setCount] = useState(0);
  const [productList, setProductList] = useState<Product[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<Category[]>([]);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const URL = process.env.NEXT_PUBLIC_API_URL;

  useEffect(() => {
    const { search, categories: categoryIds } = getQueryParams();
    setSearchQuery(search);

    const loadData = async () => {
      try {
        const storeData = JSON.parse(localStorage.getItem('store') || '{}');
        if (storeData.products) {
          setProductList(storeData.products);
          setCategories(storeData.categories || []);
        } else {
          const response = await fetch(URL + `/api/store`);
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
          const searchResponse = await fetch(URL + `/api/store/search?search=${search}&categories=${categoryIds.join(',') || 'null'}`);
          if (!searchResponse.ok) {
            throw new Error('Failed to fetch data.');
          }
          const searchData = await searchResponse.json();
          setProductList(searchData.products);
        }
      } catch (error) {
        throw new Error('Error loading data.', error);
      }
    };

    const storedCart = localStorage.getItem('cart') ? JSON.parse(localStorage.getItem('cart')!) : {};
    if (storedCart.products) {
      setCount(Object.keys(storedCart.products).length);
    }

    const initialCart = localStorage.getItem('cart') ? JSON.parse(localStorage.getItem('cart')!) : { products: {} };
    localStorage.setItem('cart', JSON.stringify(initialCart));

    loadData();

    const storedNotifications = JSON.parse(localStorage.getItem('notifications') || '[]');
    setNotifications(storedNotifications);

    const connection = new HubConnectionBuilder()
      .withUrl(URL + "/campaignHub")
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(() => {
        connection.invoke("SendRecentMessagesAsync");
      });

    connection.on('ReceiveRecentMessagesAsync', (messages) => {
      setNotifications(messages.map(msg => ({ id: msg.id, message: msg.content, read: false })));
    });

    connection.on('ReceiveAllMessagesAsync', (messages) => {
      setNotifications(messages.map(msg => ({ id: msg.id, message: msg.content, read: false })));
    });

    return () => {
      connection.stop();
    };
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

    let url = URL + `/api/store/search?search=${searchQuery}`;

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
      throw new Error('Error fetching data.', error);
    }
  };

  const handleAddToCart = (productId) => {
    if (productId === undefined) {
      throw new Error('ProductId cannot be undefined.');
      return;
    }

    const storedCart = JSON.parse(localStorage.getItem('cart')) || { products: {} };
    const productToAdd = productList.find(product => product.id === productId);
    if (productToAdd) {
      const updatedCart = {
        ...storedCart,
        products: { 
          ...storedCart.products, 
          [productId]: { 
            ...productToAdd, 
            quantity: storedCart.products[productId] ? storedCart.products[productId].quantity + 1 : 1 
          } 
        }
      };
      localStorage.setItem('cart', JSON.stringify(updatedCart));
      setCount(Object.keys(updatedCart.products).length);
    }
  };

  const handleDropdownToggle = () => {
    setIsDropdownOpen(!isDropdownOpen);
  };

  const handleCategoryChange = async (category, isChecked) => {
    if (category === undefined) {
      throw new Error('Category cannot be undefined.');
      return;
    }

    const updatedCategories = isChecked
      ? [...(selectedCategory || []), category]
      : selectedCategory.filter(c => c.id !== category.id);

    setSelectedCategory(updatedCategories);
    setIsDropdownOpen(false);

    updateUrl(searchQuery, updatedCategories);

    try {
      const response = await fetch(URL + `/api/store/products?${updatedCategories.length > 0 ? `categories=${updatedCategories.map(c => c.id).join(',')}` : 'categories=null'}`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }

      const data = await response.json();
      setProductList(data.products);
    } catch (error) {
      throw new Error('Error fetching data.', error);
    }
  };

  const handleNotificationClick = (notificationId) => {
    const updatedNotifications = notifications.map(notification => {
      if (notification.id === notificationId) {
        return { ...notification, read: true };
      }
      return notification;
    });

    setNotifications(updatedNotifications);
  };

  const Product = ({ product }) => {
    if (product === undefined) {
      throw new Error('Product cannot be undefined.');
      return null;
    }

    return (
      <div className="col-sm-3">
        <div className="Product">
          <img src={product.imageURL} alt={product.name} style={{ width: '50%', height: '50%' }} />
          <h3>{product.name}</h3>
          <p dangerouslySetInnerHTML={{ __html: product.description }} />
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
            <button onClick={(e) => { e.stopPropagation(); handleAddToCart(product.id); }} className="Button">Add to Cart</button>
          </Carousel.Item>
        ))}
      </Carousel>
    );
  };

  const rows = (array, size) => {
    if (array === undefined) {
      throw new Error("There are no products.");
      return [];
    }

    if (size === undefined) {
      throw new Error("No index specified.");
      return [];
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
            <h1>Store</h1>
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
            <div className="position-relative">
              <Dropdown>
                <Dropdown.Toggle as="a" bsPrefix="notification-toggle">
                  <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" fill="currentColor" className="bi bi-bell" viewBox="0 0 16 16">
                    <path d="M8 16a2 2 0 0 0 2-2H6a2 2 0 0 0 2 2M8 1.918l-.797.161A4 4 0 0 0 4 6c0 .628-.134 2.197-.459 3.742-.16.767-.376 1.566-.663 2.258h10.244c-.287-.692-.502-1.49-.663-2.258C12.134 8.197 12 6.628 12 6a4 4 0 0 0-3.203-3.92zM14.22 12c.223.447.481.801.78 1H1c.299-.199.557-.553.78-1C2.68 10.2 3 6.88 3 6c0-2.42 1.72-4.44 4.005-4.901a1 1 0 1 1 1.99 0A5 5 0 0 1 13 6c0 .88.32 4.2 1.22 6" />
                  </svg>
                  {notifications.filter(notification => !notification.read).length > 0 && (
                    <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
                      {notifications.filter(notification => !notification.read).length}
                    </span>
                  )}
                </Dropdown.Toggle>

                <Dropdown.Menu>
                  <Dropdown.Header>Notifications</Dropdown.Header>
                  {notifications.map(notification => (
                    <Dropdown.Item key={notification.id} onClick={() => handleNotificationClick(notification.id)}>
                      <div className={notification.read ? '' : 'font-weight-bold'} dangerouslySetInnerHTML={{ __html: notification.message }}></div>
                    </Dropdown.Item>
                  ))}
                </Dropdown.Menu>
              </Dropdown>
            </div>
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