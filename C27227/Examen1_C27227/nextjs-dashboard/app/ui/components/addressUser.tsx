import React, { useState, useEffect } from 'react';
import '../Styles/address.css';
import PaymentMethods from './payment_method';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const AddressForm: React.FC = () => {
  const [address, setAddress] = useState<string>('');
  const [showPaymentMethod, setShowPaymentMethod] = useState<boolean>(false);
  const [warning, setWarning] = useState<boolean>(false);
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

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setAddress(event.target.value);
  };

  const isValidAddress = (address: string): boolean => {
    return address.trim().length >= 10;
  };

  const handleContinueBuy = (event: React.FormEvent) => {
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
    <PaymentMethods address={address} />
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
