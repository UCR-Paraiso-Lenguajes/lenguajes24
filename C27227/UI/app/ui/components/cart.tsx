"use client";
import React, { useState, useEffect } from 'react';
import '../Styles/cart.css';
import AddressForm from './addressUser';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const Cart_Store: React.FC = () => {
  const storedData = localStorage.getItem('tienda');
  const initialData = storedData ? JSON.parse(storedData) : { products: [], cart: { subtotal: 0, subtotalImpuesto: 0, total: 0, impVentas: 13, cartItems: {} } };
  const [dataObject, setDataObject] = useState(initialData);
  const router = useRouter();

  useEffect(() => {
    const token = sessionStorage.getItem("sessionToken");
    if (token) {
      const decodedToken = decodeToken(token);
      const isTokenAlive = checkTokenDate(decodedToken?.exp);
      if (!isTokenAlive) {
        sessionStorage.removeItem("sessionToken");
        sessionStorage.removeItem("expiracyToken");
        router.push("/admin");
      }
    }
  }, [router]);

  const [cartEmpty, setCartEmpty] = useState(!initialData.products || initialData.products.length === 0);
  const [showAddressForm, setShowAddressForm] = useState(false);
  const [productQuantities, setProductQuantities] = useState(() => {
    const quantities = {};
    if (initialData && initialData.products) {
      initialData.products.forEach((product) => {
        quantities[product.id] = product.amount || 1;
      });
    }
    return quantities;
  });

  useEffect(() => {
    if (initialData && initialData.products) {
      handlePrice(productQuantities, initialData);
    }
  }, []);

  const handleContinueBuy = () => {
    setShowAddressForm(true);
  };

  const handlePrice = (newQuantities, data = dataObject) => {
    let totalPriceWithoutTax = 0;
    if (data && data.products) {
      data.products.forEach(product => {
        totalPriceWithoutTax += (product.price * (newQuantities[product.id] || 0));
      });
    }

    const totalPriceWithTax = totalPriceWithoutTax * (data.cart.impVentas / 100);
    const totalCompra = totalPriceWithoutTax + totalPriceWithTax;
    const subtotal = totalPriceWithoutTax;
    const subtotalImpuesto = totalPriceWithTax;

    updateStore(subtotal, subtotalImpuesto, totalCompra, newQuantities, data);
  };

  const handleRemove = (id) => {
    const updatedProducts = dataObject.products.filter((product) => product.id !== id);
    const updatedDataObject = { ...dataObject, products: updatedProducts };
    setDataObject(updatedDataObject);
    localStorage.setItem('tienda', JSON.stringify(updatedDataObject));
    setCartEmpty(updatedProducts.length === 0);
    setProductQuantities((prevQuantities) => {
      const newQuantities = { ...prevQuantities };
      delete newQuantities[id];
      handlePrice(newQuantities, updatedDataObject);
      return newQuantities;
    });
  };

  const updateStore = (subtotal, subtotalImpuesto, total, newQuantities, data = dataObject) => {
    const carritoActualizado = {
      ...data,
      cart: {
        ...data.cart,
        subtotal: subtotal,
        subtotalImpuesto: subtotalImpuesto,
        total: total,
        cartItems: newQuantities
      },
    };
    setDataObject(carritoActualizado);
    localStorage.setItem("tienda", JSON.stringify(carritoActualizado));
    setCartEmpty(carritoActualizado.products.length === 0);
  };

  const handleQuantityChange = (productId, action) => {
    setProductQuantities((prevQuantities) => {
      const newQuantities = { ...prevQuantities };
      if (action === 'increment') {
        newQuantities[productId] = (newQuantities[productId] || 0) + 1;
      } else if (action === 'decrement') {
        newQuantities[productId] = Math.max((newQuantities[productId] || 1) - 1, 1);
      }
      handlePrice(newQuantities);
      return newQuantities;
    });
  };

  return (
    <div>
      {showAddressForm ? <AddressForm /> :
        <div>
          {cartEmpty ? (
            <div>No hay productos en el carrito.</div>
          ) : (
            <>
              {dataObject.products.map((product) => (
                <div className='cartBox' key={product.id}>
                  <div className='cart_Img'>
                    <img src={product.imageUrl} alt={product.name} />
                    <p>{product.name}</p>
                  </div>
                  <div>
                    <button onClick={() => handleQuantityChange(product.id, 'increment')}>+</button>
                    <button>{productQuantities[product.id]}</button>
                    <button onClick={() => handleQuantityChange(product.id, 'decrement')}>-</button>
                  </div>
                  <div>
                    <span>{(product.price * productQuantities[product.id]).toFixed(2)}</span>
                    <button className='BtnEliminar' onClick={() => handleRemove(product.id)}>Eliminar</button>
                  </div>
                </div>
              ))}
              <div className='Factura'>
                <span>Total por pagar </span>
                <br />
              </div>
              <div className='withoutTax'>
                <span>Subtotal Sin impuestos: </span>
                <span> $ {(dataObject.cart.subtotal || 0).toFixed(2)}</span>
              </div>
              <div className='withTax'>
                <span>Con impuestos: </span>
                <span> $ {(dataObject.cart.subtotalImpuesto || 0).toFixed(2)}</span>
              </div>
              <div className='withTax'>
                <span>Total: </span>
                <span> $ {(dataObject.cart.total || 0).toFixed(2)}</span>
              </div>
              <div className='BuyProduct'>
                <button className='BtnBuy' onClick={handleContinueBuy} disabled={cartEmpty}>Continuar compra</button>
              </div>
            </>
          )}
        </div>
      }
    </div>
  );
};

export default Cart_Store;
