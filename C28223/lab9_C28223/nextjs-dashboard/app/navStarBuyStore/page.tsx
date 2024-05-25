"use client";
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import React, { useState, useEffect } from 'react';
import "@/app/ui/styles.css";

const Header = ({ size, setShow }) => {

  const handleReloadPage = () => {
    window.location.reload();
  };

  return (
    <div>
      <div className='col'>
        <div className='barra' >
          <title >StarBuyStore</title>
          <h1 onClick={handleReloadPage}>StarBuyStore</h1>
          <div className='search'>
            <input id="search" type="search" placeholder="Buscar" />
            <button className="btn btn-outline-dark mr-2">Buscar</button>
          </div>
          <div className="access" >
            <div className='carro' onClick={() => setShow(false)}  >
              <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="black" className="bi bi-cart" viewBox="0 0 16 16">
                <path d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 1.61 2H.5a.5.5 0 0 1-.5-.5M3.102 4l1.313 7h8.17l1.313-7zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 0-4m7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4m-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2m7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2" />
              </svg>
              <span>{size}</span>
            </div>
            <button className="btn btn-outline-dark">Perfil</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Header;