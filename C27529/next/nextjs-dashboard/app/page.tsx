"use client"
import React, { useState, useEffect } from 'react';
import { Header } from './components/header';
import { Products } from './components/products';
import { Cart } from './components/Cart';
import { Address } from './components/Address';
import { Payment } from './components/Payment';

export default function Page() {   
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

 



  const [page, setPage] = useState(0);

  const goToPage = (pageNumber: React.SetStateAction<number>) => {
    setPage(pageNumber);
  };

  useEffect(() => {
    localStorage.setItem("tienda", JSON.stringify(store));

  }, [store]);




  return (
    <main className="flex min-h-screen flex-col p-6">
      <Header goToPage={goToPage} />

      {page === 0 ? (
        
        <Products />
      ) : page === 1 ? (
        <Cart goToPage={goToPage} />
      ) : page === 2 ? (
        <Address goToPage={goToPage} />
      ) : (
        <Payment />
      )}
    </main>
  );
}
