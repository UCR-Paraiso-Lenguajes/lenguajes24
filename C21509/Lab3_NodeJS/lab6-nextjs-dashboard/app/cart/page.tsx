"use client"
import React, { useState, useEffect } from 'react';
import { ProductItem } from '../product/layout';
import Link from 'next/link';
import '../HTMLPageDemo.css';

const CartFunction = () => {
  const [cartState, setCartState] = useState({
    cart: {
      products: [],
      subtotal: 0,
      taxPercentage: 0.13,
      total: 0,
    }
  });

  useEffect(() => {
    const savedCartProducts = JSON.parse(localStorage.getItem('cartProducts') || '[]');
    const subtotal = savedCartProducts.reduce((acc: number, product: ProductItem) => acc + product.price, 0);
    const total = subtotal * (1 + cartState.cart.taxPercentage);

    setCartState(prevState => ({
      ...prevState,
      cart: {
        ...prevState.cart,
        products: savedCartProducts,
        subtotal: subtotal,
        total: total
      }
    }));
  }, []);

  const removeFromCart = (productId: number) => {
    const updatedCart = cartState.cart.products.filter((product: ProductItem) => product.id !== productId);
    const subtotal = updatedCart.reduce((acc: number, product: ProductItem) => acc + product.price, 0);
    const total = subtotal * (1 + cartState.cart.taxPercentage);

    setCartState(prevState => ({
      ...prevState,
      cart: {
        ...prevState.cart,
        products: updatedCart,
        subtotal: subtotal,
        total: total
      }
    }));

    localStorage.setItem('cartProducts', JSON.stringify(updatedCart));
  };

  return (
    <div>
      <h1>Carrito de compras</h1>
      {cartState.cart.products.length > 0 ? (
        <>
          <ul className="product col-sm-6">
            {cartState.cart.products.map((product: ProductItem) => (
              <li key={product.id}>
                <img src={product.imageURL} alt={product.name} />
                <p>{product.name}</p>
                <p>Precio: ${product.price}</p>
                <button onClick={() => removeFromCart(product.id)} className="button">Eliminar de carrito</button>
              </li>
            ))}
          </ul>
          <div>
            <p>Subtotal: ${cartState.cart.subtotal.toFixed(2)}</p>
            <p>Total (con impuestos): ${cartState.cart.total.toFixed(2)}</p>
          </div>
          <Link href="/payment">
            <button className="button">Continuar compra</button>
          </Link>
        </>
      ) : (
        <p>No hay productos en el carrito</p>
      )}
      <Link href="/product">
        <button className="button">Página Principal</button>
      </Link>
    </div>
  );
};

export default CartFunction;