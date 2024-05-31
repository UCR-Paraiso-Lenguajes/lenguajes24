import React, { useState, useEffect } from 'react';
import '../styles/address.css';
import PaymentMethods from './payment_method';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const AddressForm = ({ onSubmit }) => {
  const [address, setAddress] = useState('');
  const [showPaymentMethod, setShowPaymentMethod] = useState(false);
  const [warning, setWarning] = useState(false);
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

  const handleInputChange = (event) => {
    if (!event || !event.target) {
      throw new Error('El evento y el objetivo son requeridos.');
    }
    setAddress(event.target.value);
  };

  const isValidAddress = (address) => {
    if (!address) {
      throw new Error('La dirección es requerida.');
    }
    const minLength = 10;
    return address.trim().length >= minLength;
  };

  const handleContinueBuy = (event) => {
    if (!event) {
      throw new Error('El evento es requerido.');
    }
    event.preventDefault();
    if (!isValidAddress(address)) {
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
      }, 2000);
    } else {
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
      {warning && <div className='alert'>Por favor, ingrese una dirección válida.</div>}
    </form>
  );
};

export default AddressForm;
