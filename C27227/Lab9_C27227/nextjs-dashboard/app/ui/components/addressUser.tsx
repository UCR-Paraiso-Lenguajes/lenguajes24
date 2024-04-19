import React, { useState } from 'react';
import '../styles/address.css';
import PaymentMethods from './payment_method';

const AddressForm = ({ onSubmit }) => {
  const [address, setAddress] = useState('');
  const [showPaymentMethod, setShowPaymentMethod] = useState(false);
  const store = localStorage.getItem('tienda');
  const tienda = JSON.parse(store);
  const [warning, setWarning] = useState(false);

  const handleInputChange = (event) => {
    setAddress(event.target.value);
  };

  const handleContinueBuy = (event) => {
    event.preventDefault();
    if (address.trim() === '') {
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
      }, 2000);
    } else {
      const updatedCart = {
        ...tienda,
        cart: {
          ...tienda.cart,
          direccionEntrega: address
        }
      };
      localStorage.setItem('tienda', JSON.stringify(updatedCart));
      setShowPaymentMethod(true);
    }
  };

  return showPaymentMethod ? (
    <PaymentMethods />
  ) : (
    <form onSubmit={handleContinueBuy}>
      <div>Ingrese su dirección</div>
      <input
        type="text"
        value={address}
        onChange={handleInputChange}
        placeholder="Ej: Calle 123, Ciudad, Estado, País"
      />
      <button type="submit">Enviar</button>
      {warning && <div className='alert'>Por favor, ingrese su dirección.</div>}
    </form>
  );
};

export default AddressForm;

