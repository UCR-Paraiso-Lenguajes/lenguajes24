import React, { useState, useEffect } from 'react';
import '../Styles/cart.css';
import AddressForm from './addressUser';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const Cart_Store = () => {
  const storedData = localStorage.getItem('tienda');
  const dataObject = JSON.parse(storedData);
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

  const [cartEmpty, setCartEmpty] = useState(true);
  const [showAddressForm, setShowAddressForm] = useState(false);
  const [cartUpdated, setCartUpdated] = useState(false);

  if (!dataObject || !dataObject.products) {
    throw new Error('No hay productos en el carrito.');
  }

  const [productQuantities, setProductQuantities] = useState(() => {
    const quantities = {};
    dataObject.products.forEach((product) => {
      quantities[product.id] = (dataObject.cart.productQuantities && dataObject.cart.productQuantities[product.id]) || product.amount || 1;
    });
    return quantities;
  });

  let subtotal = 0;
  let subtotalImpuesto = 0;
  let totalCompra = 0;

  const handleContinueBuy = () => {
    setShowAddressForm(true);
  }

  const handlePrice = (newQuantities = productQuantities) => {
    if (!newQuantities) {
      throw new Error('Las cantidades de productos son requeridas.');
    }
    let totalPriceWithoutTax = 0;
    dataObject.products.forEach(product => {
      totalPriceWithoutTax += (product.price * (newQuantities[product.id] || 0));
    });

    const totalPriceWithTax = totalPriceWithoutTax * (dataObject.cart.impVentas / 100);
    totalCompra = (totalPriceWithoutTax + totalPriceWithTax);
    subtotal = totalPriceWithoutTax;
    subtotalImpuesto = totalPriceWithTax;
    setCartEmpty(dataObject.products.length === 0);
    updateStore(subtotal, subtotalImpuesto, totalCompra,newQuantities);
  }

  const handleRemove = (id) => {
    const updatedProducts = dataObject.products.filter((product) => product.id !== id); 
    const updatedCart = {...dataObject, products: updatedProducts};
    localStorage.setItem('tienda', JSON.stringify(updatedCart));
    setCartUpdated(!cartUpdated);
    window.location.reload();
  }

  const updateStore = (subtotalC, subtotalImpuestoCa, totalComp, newQuantities) => {
    if (subtotalC === undefined || subtotalImpuestoCa === undefined || totalComp === undefined || newQuantities === undefined)
        {
        throw new Error('Los argumentos de la función updateStore son requeridos.');
    }
    const carritoActualizado = {
        ...dataObject,
        cart: {
            ...dataObject.cart,
            subtotal: subtotalC,
            subtotalImpuesto: subtotalImpuestoCa,
            total: totalComp,
            cartItems: newQuantities
        },
    };
    localStorage.setItem("tienda", JSON.stringify(carritoActualizado));
  };

  useEffect(() => {
    handlePrice();
  }, [cartUpdated]);

  if (!(dataObject.products.length > 0)) {
    throw new Error('No hay productos en el carrito.');
  }

  const handleQuantityChange = (productId, action) => {
    setProductQuantities((prevQuantities) => {
      const newQuantities = { ...prevQuantities };
      if (action === 'increment') {
        newQuantities[productId] = (newQuantities[productId] || 0) + 1;
      } else if (action === 'decrement') {
        newQuantities[productId] = Math.max((newQuantities[productId] || 1) - 1, 1); 
      }
      handlePrice(newQuantities);
      updateStore(subtotal, subtotalImpuesto, totalCompra,newQuantities);
      return newQuantities;
    });
  };

  return (
    <div>
      {showAddressForm ? <AddressForm /> :
        <div>
          {dataObject.products?.map((product) => (
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
                <span>{product.price * productQuantities[product.id]}</span>
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
            <span> $ {dataObject.cart.subtotal}</span>
          </div>
          <div className='withTax'>
            <span >Con impuestos: </span>
            <span> $ {dataObject.cart.subtotalImpuesto}</span>
          </div>
          <div className='withTax'>
            <span >Total: </span>
            <span> $ {dataObject.cart.total}</span>
          </div>
          <div className='BuyProduct'>
            <button className='BtnBuy' onClick={handleContinueBuy} disabled={cartEmpty}>Continuar compra</button>
          </div>
        </div>
      }
    </div>
  )
}

export default Cart_Store;
