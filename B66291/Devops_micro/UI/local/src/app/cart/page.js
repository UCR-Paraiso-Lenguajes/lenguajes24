"use client"
import React, { useState, useEffect } from "react";
import "../../styles/cart.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from '../../components/Navbar';

const Cart = () => {

  const [cartData, setCartData] = useState(undefined);

  useEffect(() => {
    const storedData = localStorage.getItem('tienda');
    const dataObject = JSON.parse(storedData);
    setCartData(dataObject);
  }, []);

  useEffect(() => {
    if (cartData) {
      localStorage.setItem('tienda', JSON.stringify(cartData));
    }
  }, [cartData]);

  function borrarProducto(item) {
    if(item == null){
      throw new Error("El objeto a comprar se encuentra vacio")
    }
    const updatedProducts = cartData.cart.productos.filter(product => product.id !== item.id);
    const updatedCart = {
      ...cartData.cart,
      productos: updatedProducts,
    };

    let subtotalCalc = 0;
    let nuevoTotalCalc = 0;

    updatedCart.productos.forEach((item) => {
      if(item == null){
        throw new Error("El objeto a comprar se encuentra vacio")
      }
      subtotalCalc += item.price;
    });

    const nuevoSubtotal = subtotalCalc;

    updatedCart.productos.forEach(() => {
      nuevoTotalCalc = nuevoSubtotal * (1 + cartData.impVentas / 100);
    });

    const updatedCartWithTotals = {
      ...updatedCart,
      subtotal: nuevoSubtotal,
      total: nuevoTotalCalc,
    };

    const updatedDataObject = { ...cartData, cart: updatedCartWithTotals };
    setCartData(updatedDataObject);
  }

  const isCartEmpty = cartData ? cartData.cart.productos.length == 0 : true;

  return (
    <article>
      <div>
        <Navbar cantidad_Productos={cartData ? cartData.cart.productos.length : 0} />
      </div>

      {cartData && cartData.cart.productos.map((item) => (
        <div className="cart_box" key={item.id}>

          <div className="cart_id">
            <span>{item.name}</span>
          </div>
          <div className="cart_description">
            <span>{item.description}</span>
          </div>
          <div className="cart_precio">
            <span>${item.price}</span>
          </div>
          <div className="cart_image">
            <img
              src={item.imageUrl}
              alt="Product Image"
              style={{ height: '70px', width: '80%' }}
              className="imgProduct"
            />
          </div>
          <button
            className="btn btn-danger mt-3"
            onClick={() => borrarProducto(item)}
          >
            Eliminar producto
          </button>
        </div>
      ))}

      {cartData && (
        <div>
          <div className="cart_box" style={{ display: 'flex', justifyContent: 'flex-end' }}>
            <div>
              <span>Subtotal de la compra: {cartData.cart.subtotal}</span>
            </div>
          </div>
        </div>
      )}

      {cartData && (
        <div>
          <div className="cart_box" style={{ display: 'flex', justifyContent: 'flex-end' }}>
            <div>
              <span>Total de la compra: {(cartData.cart.total).toFixed(2)}</span>
            </div>
          </div>
        </div>
      )}


      <div className="cart_box" style={{ flex: 1, justifyContent: 'flex-end' }}>
        <a
          href="/direccion"
          className="btn btn-info mt-3"
          disabled={isCartEmpty}
        >
          Continuar con la compra
        </a>
      </div>

    </article>
  );
}

export default Cart;

