import React, { useState, useEffect } from 'react';
import '../Styles/address.css';
import PaymentMethods from './payment_method';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const AddressForm: React.FC = () => {
  const [street, setStreet] = useState<string>('');
  const [city, setCity] = useState<string>('');
  const [state, setState] = useState<string>('');
  const [zipCode, setZipCode] = useState<string>('');
  const [country, setCountry] = useState<string>('');
  const [showPaymentMethod, setShowPaymentMethod] = useState<boolean>(false);
  const [warning, setWarning] = useState<string>('');
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

  const isValidAddress = () => {
    const zipCodePattern = /^[0-9]{5}(?:-[0-9]{4})?$/; // Simple US ZIP code validation
    return (
      street.trim().length >= 5 &&
      city.trim().length >= 2 &&
      state.trim().length >= 2 &&
      zipCodePattern.test(zipCode) &&
      country.trim().length >= 2
    );
  };

  const handleContinueBuy = (event: React.FormEvent) => {
    event.preventDefault();
    if (!isValidAddress()) {
      setWarning('Por favor, ingrese una dirección válida.');
      setTimeout(() => {
        setWarning('');
      }, 2000);
    } else {
      setShowPaymentMethod(true);
    }
  };

  return showPaymentMethod ? (
    <PaymentMethods
      address={{ street, city, state, zipCode, country }}
    />
  ) : (
    <form onSubmit={handleContinueBuy}>
      <div>Ingrese su dirección</div>
      <input
        type="text"
        value={street}
        onChange={(e) => setStreet(e.target.value)}
        placeholder="Calle y número"
      />
      <input
        type="text"
        value={city}
        onChange={(e) => setCity(e.target.value)}
        placeholder="Ciudad"
      />
      <input
        type="text"
        value={state}
        onChange={(e) => setState(e.target.value)}
        placeholder="Estado"
      />
      <input
        type="text"
        value={zipCode}
        onChange={(e) => setZipCode(e.target.value)}
        placeholder="Código Postal"
      />
      <input
        type="text"
        value={country}
        onChange={(e) => setCountry(e.target.value)}
        placeholder="País"
      />
      <button type="submit">Enviar</button>
      {warning && <div className='alert'>{warning}</div>}
    </form>
  );
};

export default AddressForm;