import React, { useState } from 'react';
import '../styles/header.css';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css";

const Navbar = ({ size, onShowLogin, onShowCart, onShowProducts, categories, setSelectedCategory}) => {
  const [showCategories, setShowCategories] = useState(false);

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

  const handleCategorySelect = (category) => {
    setSelectedCategory(category);
  };

  return (
    <nav>
      <div className="nav_box">
        <span className="my_shop" onClick={handleClickProducts}>
          KEStore
        </span>
        <div className="search_box">
          <input type="text" placeholder="Buscar..." className="search_input" />
          <button className="search_button">Buscar</button>
        </div>
      <div className="dropdown">
        <button className="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton1" data-bs-toggle="dropdown" aria-expanded="false">
          Categorias
        </button>
        <ul className="dropdown-menu" aria-labelledby="dropdownMenuButton1">
                  {categories && categories.map(category => (
            <li key={category.id}>
              <a className="dropdown-item" href="#" onClick={() => handleCategorySelect(category)}>{category.nombre}</a>
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
