"use client"; //Para utilizar el cliente en lugar del servidor
import { useEffect, useState } from 'react';
import "@/public/styles.css";
import Link from 'next/link';

export default function CartPage() {
  const [cartItems, setCartItems] = useState({});
  const [subtotal, setSubtotal] = useState(0);
  const taxRate = 0.13;

  useEffect(() => {
    const storedCart = JSON.parse(localStorage.getItem('cart')) || { products: {} };
    setCartItems(storedCart.products);
    setSubtotal(storedCart.subtotal || 0);
  }, []);

  useEffect(() => {
    let subTotal = 0;
    Object.values(cartItems).forEach(item => {
      subTotal += item.price;
    });
    setSubtotal(subTotal);
    const total = subTotal + (subTotal * taxRate);
    localStorage.setItem('subtotal', subTotal.toFixed(2));
    localStorage.setItem('total', total.toFixed(2));
  }, [cartItems, taxRate]);

  useEffect(() => {
    localStorage.setItem('taxRate', taxRate.toString());
  }, [taxRate]);

  const handleRemoveFromCart = (productId) => {
    const updatedCartItems = { ...cartItems };
    delete updatedCartItems[productId];
    setCartItems(updatedCartItems);
    localStorage.setItem('cart', JSON.stringify({ products: updatedCartItems }));
  };

  const CartItems = () => {
    return Object.values(cartItems).map((item) => (
      <div key={item.id} className="cart-item">
        <img src={item.imageURL} alt={item.name} />
        <div>
          <h3>{item.name}</h3>
          <p>{item.decription}</p>
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
          <h1>Tienda</h1>
        </Link>
      </div>

      <div className="body">
        <h2>Productos</h2>
        {CartItems()}
      </div>

      <div className="totals">
        <p>Subtotal: ${subtotal.toFixed(2)}</p>
        <p>Impuestos ({(taxRate * 100)}%): ${(subtotal * taxRate).toFixed(2)}</p> 
        <p>Total: ${(subtotal + (subtotal * taxRate)).toFixed(2)}</p>
        <Link href={isCartEmpty ? "#" : "/checkout"}>
          <button className="Button" disabled={isCartEmpty}>Proceed to checkout</button>
        </Link>
        <div></div>
        <Link href="/">
          <button className="Button">Home page</button>
        </Link>
      </div>

      <div className="footer">
        <h2>Tienda.com</h2>
      </div>

    </div>
  )
}