import React, { useState } from 'react';
import '../styles/address.css';
import PaymentMethods from './payment_method';

const AddressForm = ({ onSubmit }) => {
  const [address, setAddress] = useState({
    street: '',
    city: '',
    state: '',
    country: ''
  });

  const [showPaymentMethod, setShowPaymentMethod] = useState(false);
  const store = localStorage.getItem('tienda');
  const tienda = JSON.parse(store);
  const [warning, setWarning] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setAddress({
      ...address,
      [name]: value
    });
  };

  const handleContinueBuy = (e) => {
    e.preventDefault();
    if (
      address.street.trim() === '' ||
      address.city.trim() === '' ||
      address.state.trim() === '' ||
      address.country.trim() === ''
    ) {
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
      }, 2000);
    } else {
      const updatedCart = {
        ...tienda,
        cart: {
          ...tienda.cart,
          direccionEntrega: {
            ...address
          }
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
      <label>
        Calle:
        <input
          type="text"
          name="street"
          value={address.street}
          onChange={handleChange}
        />
      </label>
      <label>
        Ciudad:
        <input
          type="text"
          name="city"
          value={address.city}
          onChange={handleChange}
        />
      </label>
      <label>
        Estado/Provincia:
        <input
          type="text"
          name="state"
          value={address.state}
          onChange={handleChange}
        />
      </label>
      <label>
        País:
        <input
          type="text"
          name="country"
          value={address.country}
          onChange={handleChange}
        />
      </label>
      <button type="submit">Enviar</button>
      {warning && <div className='alert'>Por favor, complete todos los campos de dirección.</div>}
    </form>
  );
};

export default AddressForm;
