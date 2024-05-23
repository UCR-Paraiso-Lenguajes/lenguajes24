// Navbar.js
import React from 'react';
import Link from 'next/link';
import '../styles/navbar.css'

const Navbar = ({cantidad_Productos}) => {
  if (cantidad_Productos == undefined) {
    throw new Error("La cantidad de productos no puede estar indefinida");
  }
  if(cantidad_Productos == 0){
    throw new Error("La cantidad de productos no puede ser 0");
  }
  return (
    <nav className="navbar navbar-expand-lg navbar-dark">
      <div className="container-fluid">
        <a href="/" className="my_shop">
        <button className="my_shop_button">
           <img src="https://img.icons8.com/clouds/100/technology.png" alt="GeekGadgets" className="button_image" /> GeekGadgets
         </button>
        </a>
        <div className='d-flex'>
            <input className="form-control me-2" type="search" placeholder="Search" aria-label="Search" style={{ color: 'black' }} />
            <button className="btn btn-outline-success" type="submit" style={{ color: 'white' }}>Buscar</button>
        </div>
        <Link className='login' href="/admin">
        <div className='d-flex' style={{alignItems: 'flex-end'}}>
          <svg 
          xmlns="http://www.w3.org/2000/svg" 
          width="16" 
          height="16" 
          fill="currentColor" 
          className="bi bi-person-fill-lock" 
          viewBox="0 0 16 16">
          <path d="M11 5a3 3 0 1 1-6 0 3 3 0 0 1 6 0m-9 8c0 1 1 1 1 1h5v-1a2 2 0 0 1 .01-.2 4.49 4.49 0 0 1 1.534-3.693Q8.844 9.002 8 9c-5 0-6 3-6 4m7 0a1 1 0 0 1 1-1v-1a2 2 0 1 1 4 0v1a1 1 0 0 1 1 1v2a1 1 0 0 1-1 1h-4a1 1 0 0 1-1-1zm3-3a1 1 0 0 0-1 1v1h2v-1a1 1 0 0 0-1-1"/>
          </svg>
          </div>
        </Link>
        <Link className='numero_carrito' href="/cart">
        <div>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="25"
            height="25"
            fill="#bfbfbf"
            className="bi bi-cart-plus"
            viewBox="0 0 16 16"
          >
            <path d="M9 5.5a.5.5 0 0 0-1 0V7H6.5a.5.5 0 0 0 0 1H8v1.5a.5.5 0 0 0 1 0V8h1.5a.5.5 0 0 0 0-1H9z" />
            <path d="M.5 1a.5.5 0 0 0 0 1h1.11l.401 1.607 1.498 7.985A.5.5 0 0 0 4 12h1a2 2 0 1 0 0 4 2 2 0 0 0 0-4h7a2 2 0 1 0 0 4 2 2 0 0 0 0-4h1a.5.5 0 0 0 .491-.408l1.5-8A.5.5 0 0 0 14.5 3H2.89l-.405-1.621A.5.5 0 0 0 2 1zm3.915 10L3.102 4h10.796l-1.313 7zM6 14a1 1 0 1 1-2 0 1 1 0 0 1 2 0m7 0a1 1 0 1 1-2 0 1 1 0 0 1 2 0" />
          </svg>
          <span className='number'>{cantidad_Productos}</span>
          </div>
        </Link>     
      </div>
    </nav>
  );
}

export default Navbar;
