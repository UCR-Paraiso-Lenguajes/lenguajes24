"use client"
import { Route, Routes, BrowserRouter as Router } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import { Header } from './components/header';
import { Products } from './components/products';
import { Cart } from './components/Cart';
import { Address } from './components/Address';
import { Payment } from './components/Payment';
import Page from './Admin/page';

export default function page() {
  const tiendaGuardado = {
    carrito: {
      subtotal: 0,
      porcentajeImpuesto: 0.13,
      total: 0,
      direccionEntrega: '',
      metodoDePago: 0
    },
    necesitaVerificacion: false,
    productos: [],
    idCompra: 0
  };

  const [store, setStore] = useState(() => {
    const storedStore = localStorage.getItem("tienda");

    return storedStore ? JSON.parse(storedStore) : tiendaGuardado;
  });


  useEffect(() => {
    localStorage.setItem("tienda", JSON.stringify(store));

  }, [store]);




  return (
    <main className="flex min-h-screen flex-col p-6">
      <Router>
        <Routes>
          <Route path='/' element={<Home />} />
         
        </Routes>
         
      </Router>



    </main>
  );
}

function Home() {

  const [Actualpage, setPage] = useState(0);

  const goToPage = (pageNumber: React.SetStateAction<number>) => {
    setPage(pageNumber);
  };
  

  return (
    <div>

      <Header goToPage={goToPage} />

      {Actualpage === 0 ? (

        <Products />
      ) : Actualpage === 1 ? (
        <Cart goToPage={goToPage} />
      ) : Actualpage === 2 ? (
        <Address goToPage={goToPage} />
      ) : (
        <Payment />
      )}
   
    </div>
  );
}