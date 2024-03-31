"use client";
import React, { useEffect, useState } from 'react';
import Link from 'next/link';
import { text } from 'stream/consumers';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import 'bootstrap/dist/css/bootstrap.min.css'; // Importar los estilos de Bootstrap
import 'bootstrap/dist/js/bootstrap.bundle.min'; // Importar los scripts de Bootstrap

// Página del carrito
function CartPage({ cart, removeItem }) {

  //Función suma total productos
  const getSubTotalPrice = () => {
    return cart.reduce((total, product) => total + parseFloat(product.price.replace('$', '')), 0).toFixed(2);
  };

  // Función para sumar el precio total del carrito
  const getTotalPrice = () => {
    const subtotal = cart.reduce((total, product) => total + parseFloat(product.price.replace('$', '')), 0);

    const tax = subtotal * 0.15;
    const totalPriceWithTax = subtotal + tax;
    return totalPriceWithTax.toFixed(2);
  };

  return (
    <div>
      <Link href={"/"}><img src='https://cdn.pixabay.com/photo/2020/03/22/15/19/arrow-4957487_1280.png' style={{ height: "1cm" }}></img></Link>
      <h1>Carrito de compras</h1>
      {cart.map(item => (
        item && item.name ? (
          <div key={item.id} className='row'>
            <div className='col-md-3'><h2>{item.name}</h2></div>
            <div className='col-md-3' >
              <img src={item.imgurl} alt={item.name} style={{ height: "5cm", width: "auto" }} /></div>
            <div className='col-md-3'><h4>{item.price}</h4></div>
            <br></br>
            <div className='col-md-3'><button className="button" style={{ backgroundColor: "black", color: "white" , textAlign: "end" }} onClick={() => removeItem(item.id)}>Eliminar del carrito</button></div>
          </div>
        ) : null
      ))}
      <div style={{ textAlign: "right" }}>
        <h3>Subtotal: ${getSubTotalPrice()}</h3>
        <h3>Total: ${getTotalPrice()}</h3>
      </div>

    </div>
  );
}

export default function Cart() {
  const [cart, setCart] = useState([]);

  useEffect(() => {
    if (typeof window !== 'undefined') {
      const newCart = JSON.parse(localStorage.getItem('cart')) || [];
      setCart(newCart);
    }
  }, []);

  const isEmpty = Object.keys(cart).length == 0 ? true : false;
  

  
const removeItem = (productId) => {
  const index = cart.findIndex(item => item.id === productId);

  if (index !== -1) {
    const updatedCart = [...cart];
    // Elimina solo 1 de los elementos, en caso de que hayan varios con el mismo id
    updatedCart.splice(index, 1);
    setCart(updatedCart);
    localStorage.setItem('cart', JSON.stringify(updatedCart));
  }
};
return (
  <div className='container'>
    <div className='row'>
      <CartPage cart={cart} removeItem={removeItem} />
    </div>
    <div>
      <Link href="/pago">
        <button className="button" style={{textAlign: "end", backgroundColor: "black", color: "white"  }} disabled={isEmpty}> Siguiente</button>
      </Link>
    </div>
  </div>
);
}
