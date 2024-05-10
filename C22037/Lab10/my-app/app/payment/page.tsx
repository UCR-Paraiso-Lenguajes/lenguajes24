"use client"; //Para utilizar el cliente en lugar del servidor
import { useEffect, useState } from 'react';
import "@/public/styles.css";
import Link from 'next/link';

const PaymentMethodsEnum = {
  Cash: 0,
  Sinpe: 1
};

export default function Payment() {
  const [selectedMethod, setSelectedMethod] = useState(PaymentMethodsEnum.Cash);
  const [paymentConfirmed, setPaymentConfirmed] = useState(false);
  const [cartProducts, setCartProducts] = useState([]);
  const [address, setAddress] = useState('');
  const [total, setTotal] = useState('');
  const [purchaseNumber, setpurchaseNumber] = useState('');

  useEffect(() => {
    const storedCart = JSON.parse(localStorage.getItem('cart')) || { products: {} };
    const productIds = Object.keys(storedCart.products);
    setCartProducts(productIds);
    const storedAddress = localStorage.getItem('address') || '';
    setAddress(storedAddress);
    const storedTotal = localStorage.getItem('total') || '';
    setTotal(storedTotal);
  }, []);

  useEffect(() => {
    localStorage.setItem('selectedMethod', selectedMethod);
  }, [selectedMethod]);

  const handleMethodSelect = (method) => {
    setSelectedMethod(method);
    setPaymentConfirmed(false);
  };

  const handleSinpePaymentConfirmation = () => {
    setPaymentConfirmed(true);
  };

  const handleConfirmation = async () => {
    const paymentMethodValue = selectedMethod === PaymentMethodsEnum.Sinpe ? PaymentMethodsEnum.Sinpe : PaymentMethodsEnum.Cash;
    try {
      const dataSend = {
        ProductIds: cartProducts,
        Address: address,
        PaymentMethod: selectedMethod,
        Total: total
      };

      const response = await fetch('https://localhost:7067/api/Cart', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(dataSend)
      });

      if (!response.ok) {
        throw new Error('Failed to confirm purchase.');
      } else {
        const purchaseNumberApp = await response.json();
        setpurchaseNumber(purchaseNumberApp.purchaseNumberResponse);
      }

      localStorage.removeItem('cart');
      localStorage.removeItem('address');
      localStorage.removeItem('selectedMethod');

    } catch (error) {
      throw new Error('Error confirming purchase:', error.message);
    }
  };

  return (
    <div>
      <div className="header">
        <Link href="/">
          <h1>Tienda</h1>
        </Link>
      </div>

      <div className="body">
        <h2>Payment Method</h2>
        <div>
          <button className="Button" onClick={() => handleMethodSelect(PaymentMethodsEnum.Cash)}>Cash</button>
          <button className="Button" onClick={() => handleMethodSelect(PaymentMethodsEnum.Sinpe)}>Sinpe</button>
        </div>
        {selectedMethod === PaymentMethodsEnum.Cash && (
          <div>
            <p>Purchase number: {purchaseNumber}.</p>
            <p>Awaiting confirmation from the administrator regarding the payment.</p>
          </div>
        )}
        {selectedMethod === PaymentMethodsEnum.Sinpe && (
          <div>
            <p>Purchase number: {purchaseNumber}.</p>
            <p>Make the payment through Sinpe to the number 8596-1362.</p>
            <input type="text" placeholder="Enter the receipt code here" />
            <button className="Button" onClick={handleSinpePaymentConfirmation}>Confirm</button>
            {paymentConfirmed && <p>Awaiting confirmation from the administrator regarding the payment.</p>}
          </div>
        )}
        <button className="Button" onClick={handleConfirmation}>Confirm Purchase</button>
      </div>

      <div className="footer">
        <h2>Tienda.com</h2>
      </div>
    </div>
  );
}