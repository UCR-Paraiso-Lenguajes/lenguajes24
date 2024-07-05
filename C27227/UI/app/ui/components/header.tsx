import React, { useState } from 'react';
import Link from 'next/link';
import { useWebSocket } from '../../hooks/WebSocketContext';
import '../Styles/header.css';
import "bootstrap/dist/css/bootstrap.min.css";

const Navbar = ({ size, onShowLogin, onShowCart, onShowProducts, categories, setSelectedCategory, onSearch, fetchProductsByCategory, fetchProductsBySearch }) => {
  const { messages } = useWebSocket();
  const [showCategories, setShowCategories] = useState(false);
  const [selectedCategories, setSelectedCategories] = useState([]);
  const [searchText, setSearchText] = useState('');
  const [warning, setWarning] = useState('');
  const [showNotifications, setShowNotifications] = useState(false);

  const handleClickLogin = () => {
    if (typeof onShowLogin === 'function') {
      onShowLogin();
    }
  };

  const handleClickCart = () => {
    if (typeof onShowCart === 'function') {
      onShowCart();
    }
  };

  const handleClickProducts = () => {
    if (typeof onShowProducts === 'function') {
      onShowProducts();
    }
  };

  const handleCategorySelect = async (selectedCategory) => {
    const isCategorySelected = selectedCategories.some(category => category.id === selectedCategory.id);
    let updatedCategories;
    if (isCategorySelected) {
      updatedCategories = selectedCategories.filter(category => category.id !== selectedCategory.id);
    } else {
      updatedCategories = [...selectedCategories, selectedCategory];
    }
    setSelectedCategories(updatedCategories);
    await fetchProductsByCategory(updatedCategories.map(category => category.id));
  };

  const handleSearch = async () => {
    const trimmedSearchText = searchText.trim();
    if (trimmedSearchText.length < 4) {
      setWarning('Por favor, ingrese un texto válido.');
      setTimeout(() => {
        setWarning('');
      }, 3000);
      return;
    }
    setWarning('');
    onSearch(trimmedSearchText, selectedCategories.length > 0 ? selectedCategories.map(category => category.id) : null);
  };

  return (
    <nav>
      <div className="nav_box">
        <span className="my_shop" onClick={handleClickProducts}>
          KEStore
        </span>
        <div className="search_box">
          <input
            type="text"
            placeholder="Buscar..."
            className="search_input"
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
          />
          <button className="search_button" onClick={handleSearch}>Buscar</button>
        </div>
        {warning && <div className='alert'>{warning}</div>}
        <div className="dropdown">
          <button className="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
            Categorias
          </button>
          <ul className="dropdown-menu" aria-labelledby="dropdownMenuButton1">
            {categories && categories.map(category => (
              <li key={category.id}>
                <div className="dropdown-item">
                  <input
                    type="checkbox"
                    checked={selectedCategories.some(cat => cat.id === category.id)}
                    onChange={() => handleCategorySelect(category)}
                  />
                  <span>{category.nombre}</span>
                </div>
              </li>
            ))}
          </ul>
        </div>
        <div className="cart" onClick={handleClickCart}>
          <div className="icono">
            <span>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="45"
                height="45"
                fill="currentColor"
                className="bi bi-cart-plus"
                viewBox="0 0 16 16"
              >
                <path d="M9 5.5a.5.5 0 0 0-1 0V7H6.5a.5.5 0 0 0 0 1H8v1.5a.5.5 0 0 0 1 0V8h1.5a.5.5 0 0 0 0-1H9z" />
                <path d="M.5 1a.5.5 0 0 0 0 1h1.11l.401 1.607 1.498 7.985A.5.5 0 0 0 4 12h1a2 2 0 1 0 0 4 2 2 0 0 0 0-4h7a2 2 0 1 0 0 4 2 2 0 0 0 0-4h1a.5.5 0 0 0 .491-.408l1.5-8A.5.5 0 0 0 14.5 3H2.89l-.405-1.621A.5.5 0 0 0 2 1zm3.915 10L3.102 4h10.796l-1.313 7zM6 14a1 1 0 1 1-2 0 1 1 0 0 1 2 0m7 0a1 1 0 1 1-2 0 1 1 0 0 1 2 0" />
              </svg>
            </span>
          </div>
          <span className="cart_items">{size}</span>
        </div>
        <div className="messages" onClick={() => setShowNotifications(!showNotifications)}>
          <div className="icono">
            <span>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="45"
                height="45"
                fill="white"
                className="bi bi-envelope"
                viewBox="0 0 16 16"
              >
                <path d="M0 4a2 2 0 0 1 2-2h12a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2V4zm2-1a1 1 0 0 0-1 1v.217l7 4.2 7-4.2V4a1 1 0 0 0-1-1H2zm13 2.383l-4.6 2.758L15 11.5V5.383zm-1.134 6.09l-4.768-2.86L8 10.56l-1.098-.647-4.768 2.86a1 1 0 0 0 .134.147h12a1 1 0 0 0 .134-.147zm-12.732-.862l4.6-2.758-4.6-2.758V11.5z"/>
              </svg>
            </span>
          </div>
          <span className="message_count">{messages.length}</span>
        </div>
        {showNotifications && (
          <div className="notifications_dropdown">
            <div className="notifications_header">
              <h4>Notificaciones</h4>
            </div>
            <div className="notifications_body">
              {messages.length > 0 ? (
                messages.slice(0, 3).map((message, index) => (
                  <div key={index} className="notification_item">
                    <div className="notification_title">{message.title}</div>
                    <div className="notification_content" dangerouslySetInnerHTML={{ __html: message.content }}></div>
                  </div>
                ))
              ) : (
                <div className="no_notifications">No hay notificaciones</div>
              )}
            </div>
          </div>
        )}
        <div className="loginUser" onClick={handleClickLogin}>
          <Link href="/admin" className="icono">
            <span>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="45"
                height="45"
                fill="currentColor"
                className="bi bi-person-circle"
                viewBox="0 0 16 16"
              >
                <path d="M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z" />
                <path
                  fillRule="evenodd"
                  d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z"
                />
              </svg>
            </span>
            <span className="login_text">Iniciar sesión</span>
          </Link>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
