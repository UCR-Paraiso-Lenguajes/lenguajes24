'use client';
import React, { createContext, useContext, useState } from 'react';

const StateContext = createContext();
const Cart = {
  products: [],
  subtotal: 0,
  address: '',
  paymentMethod: 0,
};

export const StateProvider = ({ children }) => {
  var cartStoraged = JSON.parse(localStorage.getItem('Cart'));
  if (!cartStoraged) {
    localStorage.setItem('Cart', JSON.stringify(Cart));
    cartStoraged = JSON.parse(localStorage.getItem('Cart'));
  }
  const [cartState, setCartState] = useState(cartStoraged);

  return (
    <StateContext.Provider value={{ cartState, setCartState }}>
      {children}
    </StateContext.Provider>
  );
};

export const useStateValue = () => useContext(StateContext);
