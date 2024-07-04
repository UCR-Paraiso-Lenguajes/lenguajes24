"use client"; // Para utilizar el cliente en lugar del servidor
import { useEffect, useState } from 'react';
import "../../public/styles.css";
import Link from 'next/link';

interface CartItem {
  id: string;
  name: string;
  description: string;
  price: number;
  imageURL: string;
  quantity: number;
}

interface Cart {
  products: { [key: string]: CartItem };
  subtotal?: number;
}

export default function CartPage() {
  const [cartItems, setCartItems] = useState<{ [key: string]: CartItem }>({});
  const [subtotal, setSubtotal] = useState(0);
  const taxRate = 0.13;

  useEffect(() => {
    const storedCart = localStorage.getItem('cart');
    if (storedCart) {
      const parsedCart: Cart = JSON.parse(storedCart);
      setCartItems(parsedCart.products || {});
      setSubtotal(parsedCart.subtotal || 0);
    }
  }, []);

  useEffect(() => {
    let subTotal = 0;
    Object.values(cartItems).forEach(item => {
      subTotal += item.price * item.quantity;
    });
    setSubtotal(subTotal);
    const total = subTotal + (subTotal * taxRate);
    localStorage.setItem('subtotal', subTotal.toFixed(2));
    localStorage.setItem('total', total.toFixed(2));
  }, [cartItems, taxRate]);

  useEffect(() => {
    localStorage.setItem('taxRate', taxRate.toString());
  }, [taxRate]);

  const handleRemoveFromCart = (productId: string) => {
    const updatedCartItems = { ...cartItems };
    delete updatedCartItems[productId];
    setCartItems(updatedCartItems);
    localStorage.setItem('cart', JSON.stringify({ products: updatedCartItems }));
  };

  const handleQuantityChange = (productId: string, newQuantity: number) => {
    if (newQuantity < 1) return;
    const updatedCartItems = {
      ...cartItems,
      [productId]: {
        ...cartItems[productId],
        quantity: newQuantity
      }
    };
    setCartItems(updatedCartItems);
    localStorage.setItem('cart', JSON.stringify({ products: updatedCartItems }));
  };

  const CartItems = () => {
    return Object.values(cartItems).map((item) => (
      <div key={item.id} className="cart-item">
        <img src={item.imageURL} alt={item.name} />
        <div>
          <h3>{item.name}</h3>
          <p>{item.description}</p>
          <p>Precio: ${item.price}</p>
          <label>
            Cantidad:
            <input
              type="number"
              value={item.quantity}
              onChange={(e) => handleQuantityChange(item.id, parseInt(e.target.value))}
              min="1"
            />
          </label>
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
          <h1>Store</h1>
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
  );
}