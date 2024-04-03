"use client";
import Link from 'next/link';
import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; 


export default function PaginaCarrito() {

  const [productosCarrito, setProductosCarrito] = useState({});
  const [subtotal, setSubtotal] = useState(0);
  const tasaImpuesto = 0.13;

  useEffect(() => {
    const productosCarritoAlmacenados = JSON.parse(localStorage.getItem('productosCarrito'));
    if (productosCarritoAlmacenados) {
      setProductosCarrito(productosCarritoAlmacenados);
    }
  }, []);

  useEffect(() => {
    let subtotalCalculado = 0;
    Object.values(productosCarrito).forEach(item => {
      subtotalCalculado += item.precio;
    });
    setSubtotal(subtotalCalculado);
  }, [productosCarrito]);

  useEffect(() => {
    localStorage.setItem('tasaImpuesto', tasaImpuesto.toString());
  }, [tasaImpuesto]);

  const manejarEliminarDelCarrito = (idProducto) => {
    const productosCarritoActualizados = { ...productosCarrito };
    delete productosCarritoActualizados[idProducto];
    setProductosCarrito(productosCarritoActualizados);
    localStorage.setItem('productosCarrito', JSON.stringify(productosCarritoActualizados));
  };

  const ProductosEnCarrito = () => {
    return Object.values(productosCarrito).map((item) => (
      <div key={item.id} className="col-sm-12 col-md-3 col-lg-3 col-xl-3">
        <div className="cart-item">
          <img src={item.imagen} alt={item.name} style={{ height: '220px', width: '100%' }} />
          <div>
            <h3>{item.name}</h3>
            <p>Precio: ${item.precio}</p>
            <button className="Boton" onClick={() => manejarEliminarDelCarrito(item.id)} style={{ backgroundColor: 'pink' }}>Eliminar</button>
          </div>
        </div>
      </div>
    ));
  };


  const carritoVacio = Object.keys(productosCarrito).length === 0;
  return (

    <div className="container" style={{ backgroundColor: 'lightBlue' }}>


      <div style={{ margin: '20px 0', display: 'flex', alignItems: 'center' }}>
        
        <h2 style={{ margin: '10px 0', color: 'Black' }}>Productos agregados</h2>
      </div>
      <div className="container overflow-hidden text-center">
        <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 gy-5">
          {ProductosEnCarrito()}
        </div>
      </div>


      <div className="container-fluid bg-dark text-light p-4">
  <div className="row justify-content-center">
    <div className="col">
      <div className="d-flex flex-wrap justify-content-center">
        <div className="p-2">
          <p style={{ fontWeight: 'bold' }}>Subtotal: ${subtotal.toFixed(2)}</p>
        </div>
        <div className="p-2">
          <p style={{ fontWeight: 'bold' }}>Impuestos ({(tasaImpuesto * 100)}%): ${(subtotal * tasaImpuesto)}</p>
        </div>
        <div className="p-2">
          <p style={{ fontWeight: 'bold' }}>Total: ${(subtotal + (subtotal * tasaImpuesto)).toFixed(2)}</p>
        </div>
          <Link href="/address">
          <button className='btn btn-primary' style={{ backgroundColor: 'pink', color: 'black', marginRight: '10px' }}>Continuar con checkout</button>
        </Link>
        <Link href="/">
            <button className="btn btn-light" style={{ backgroundColor: 'pink', color: 'black', marginRight: '10px' }}>Inicio</button>
          </Link>
        </div>
        <div className="p-2">
         
        </div>
      </div>
    </div>
  </div>
</div>
  );
}