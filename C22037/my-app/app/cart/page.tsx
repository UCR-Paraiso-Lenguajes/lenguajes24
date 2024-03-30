"use client";
import { useEffect, useState } from 'react';
import "@/public/styles.css";
import Link from 'next/link';

export default function CartPage() {
  const [cartItems, setCartItems] = useState({});
  const [subtotal, setSubtotal] = useState(0);

  useEffect(() => {
    const storedCartItems = JSON.parse(localStorage.getItem('cartItems'));
    if (storedCartItems) {
      setCartItems(storedCartItems);
    }
  }, []);

  useEffect(() => {
    let subTotal = 0;
    Object.values(cartItems).forEach(item => {
      subTotal += item.price;
    });
    setSubtotal(subTotal);
  }, [cartItems]);

  const handleRemoveFromCart = (productId) => {
    const updatedCartItems = { ...cartItems };
    delete updatedCartItems[productId];
    setCartItems(updatedCartItems);
    localStorage.setItem('cartItems', JSON.stringify(updatedCartItems));
  };

  const CartItems = () => {
    return Object.values(cartItems).map((item) => (
      <div key={item.id} className="cart-item">
        <img src={item.imageURL} alt={item.name} />
        <div>
          <h3>{item.name}</h3>
          <p>{item.description}</p>
          <p>Precio: ${item.price}</p>
          <button className="Button" onClick={() => handleRemoveFromCart(item.id)}>Remove</button>
        </div>
      </div>
    ));
  };

  const isCartEmpty = Object.keys(cartItems).length === 0;

  return (
    <div>
      <div className="header">
        <Link href="/">
          <h1>Amazon</h1>
        </Link>
      </div>

      <div className="body">
        <h2>Products</h2>
        {CartItems()}
      </div>

      <div className="totals">
        <p>Subtotal: ${subtotal.toFixed(2)}</p>
        <p>Taxes (13%): ${(subtotal * 0.13).toFixed(2)}</p>
        <p>Total: ${(subtotal + (subtotal * 0.13)).toFixed(2)}</p>
        <Link href={isCartEmpty ? "#" : "/checkout"}>
          <button className="Button" disabled={isCartEmpty}>Proceed to checkout</button>
        </Link>
        <div></div>
        <Link href="/">
          <button className="Button">Home page</button>
        </Link>
      </div>

      <div className="footer">
        <h2>Amazon.com</h2>
      </div>

    </div>
  );
}