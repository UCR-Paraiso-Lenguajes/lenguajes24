"use client";
import React, { useState, useEffect } from 'react';
import { ProductItem } from '../product/layout';
import Link from 'next/link';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../HTMLPageDemo.css';

const CartFunction = () => {
  const [cartState, setCartState] = useState<{
    cart: {
      products: ProductItem[];
      subtotal: number;
      taxPercentage: number;
      total: number;
    };
  }>({
    cart: {
      products: [],
      subtotal: 0,
      taxPercentage: 0.13,
      total: 0,
    },
  });

  const [quantitiesValid, setQuantitiesValid] = useState(false);

  useEffect(() => {
    const savedCartProducts: ProductItem[] = JSON.parse(localStorage.getItem('cartProducts') || '[]');
    const subtotal = savedCartProducts.reduce((acc, product) => acc + product.price * product.quantity, 0);
    const total = subtotal * (1 + cartState.cart.taxPercentage);

    setCartState({
      cart: {
        products: savedCartProducts,
        subtotal: subtotal,
        total: total,
        taxPercentage: cartState.cart.taxPercentage,
      },
    });

    validateQuantities(savedCartProducts);
  }, []);

  const validateQuantities = (products: ProductItem[]) => {
    const allValid = products.every((product) => product.quantity >= 1);
    setQuantitiesValid(allValid);
  };

  const updateCart = (updatedCart: ProductItem[]) => {
    const subtotal = updatedCart.reduce((acc, product) => acc + product.price * product.quantity, 0);
    const total = subtotal * (1 + cartState.cart.taxPercentage);

    setCartState({
      cart: {
        products: updatedCart,
        subtotal: subtotal,
        total: total,
        taxPercentage: cartState.cart.taxPercentage,
      },
    });

    localStorage.setItem('cartProducts', JSON.stringify(updatedCart));
    validateQuantities(updatedCart);
  };

  const handleQuantityChange = (productId: number, quantity: number) => {
    if (quantity < 1 || isNaN(quantity)) return;

    const updatedCart = cartState.cart.products.map((product) => {
      if (product.id === productId) {
        return { ...product, quantity: quantity };
      }
      return product;
    });
    updateCart(updatedCart);
  };

  const removeFromCart = (productId: number) => {
    const updatedCart = cartState.cart.products.filter((product) => product.id !== productId);
    updateCart(updatedCart);
  };

  return (
    <div className="container">
      <h1 className="my-4">Carrito de compras</h1>
      {cartState.cart.products.length > 0 ? (
        <>
          <ul className="list-group mb-4">
            {cartState.cart.products.map((product) => (
              <li key={product.id} className="list-group-item d-flex justify-content-between align-items-center">
                <div className="d-flex align-items-center">
                  <img
                    src={product.imageURL}
                    alt={product.name}
                    className="img-thumbnail me-3"
                    style={{ width: '100px', height: '100px', objectFit: 'contain' }}
                  />
                  <div>
                    <p className="mb-1">{product.name}</p>
                    <p className="mb-1">Precio: ${product.price}</p>
                    <div className="d-flex align-items-center">
                      <label htmlFor={`quantity-${product.id}`} className="me-2 mb-0">
                        Cantidad:
                      </label>
                      <input
                        type="number"
                        id={`quantity-${product.id}`}
                        className="form-control"
                        value={product.quantity}
                        onChange={(e) => handleQuantityChange(product.id, parseInt(e.target.value))}
                        onInput={(e) => {
                          const input = e.target as HTMLInputElement;
                          input.value = input.value.replace(/[^0-9]/g, '');
                        }}
                        style={{ width: '80px' }}
                        min="1"
                        inputMode="numeric"
                        pattern="\d*"
                      />
                    </div>
                  </div>
                </div>
                <button onClick={() => removeFromCart(product.id)} className="btn btn-danger">
                  Eliminar
                </button>
              </li>
            ))}
          </ul>
          <div className="mb-4">
            <p>Subtotal: ${cartState.cart.subtotal.toFixed(2)}</p>
            <p>Total (con impuestos): ${cartState.cart.total.toFixed(2)}</p>
          </div>
          <Link href="/payment">
            <button className="btn btn-primary mb-2" disabled={!quantitiesValid}>
              Continuar compra
            </button>
          </Link>
        </>
      ) : (
        <p>No hay productos en el carrito</p>
      )}
      <Link href="/product">
        <button className="btn btn-secondary">Página Principal</button>
      </Link>
    </div>
  );
};

export default CartFunction;