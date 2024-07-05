"use client";
import { useEffect, useState } from 'react';
import "../../public/styles.css";
import Link from 'next/link';

const PaymentMethodsEnum = {
  Cash: 0,
  Sinpe: 1
};

export default function Payment() {
  const [selectedMethod, setSelectedMethod] = useState(null);
  const [paymentConfirmed, setPaymentConfirmed] = useState(false);
  const [cartProducts, setCartProducts] = useState<string[]>([]);
  const [address, setAddress] = useState('');
  const [total, setTotal] = useState('');
  const [purchaseNumber, setPurchaseNumber] = useState('');
  const [storedCart, setStoredCart] = useState({ products: {} });
  const [activePaymentMethods, setActivePaymentMethods] = useState([]);
  const URL = process.env.NEXT_PUBLIC_API_URL;

  useEffect(() => {
    const cart = localStorage.getItem('cart');
    const parsedCart = cart ? JSON.parse(cart) : { products: {} };
    setStoredCart(parsedCart);
    const productIds = Object.keys(parsedCart.products);
    setCartProducts(productIds);

    const storedAddress = localStorage.getItem('address') || '';
    setAddress(storedAddress);

    const storedTotal = localStorage.getItem('total') || '';
    setTotal(storedTotal);

    fetchActivePaymentMethods();
  }, []);

  const fetchActivePaymentMethods = async () => {
    try {
      const response = await fetch(URL + '/api/PaymentMethods/getPaymentMethods');
      const data = await response.json();
      setActivePaymentMethods(data.filter(method => method.isActive));
    } catch (error) {
      throw new Error('Error fetching active payment methods.');
    }
  };

  const handleMethodSelect = (method) => {
    setSelectedMethod(method);
    setPaymentConfirmed(false);
  };

  const handleSinpePaymentConfirmation = () => {
    setPaymentConfirmed(true);
  };

  const handleConfirmation = async () => {
    try {
      const dataSend = {
        ProductIds: cartProducts.map(id => id),
        Address: address,
        PaymentMethod: selectedMethod,
        Total: total,
        ProductQuantities: cartProducts.reduce((acc, id) => {
          acc[id] = storedCart.products[id].quantity;
          return acc;
        }, {})
      };

      const response = await fetch(URL + '/api/cart', {
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
        setPurchaseNumber(purchaseNumberApp.purchaseNumberResponse);
      }

      localStorage.removeItem('cart');
      localStorage.removeItem('address');
      localStorage.removeItem('selectedMethod');
    } catch (error) {
      throw new Error('Error confirming purchase.');
    }
  };

  return (
    <div>
      <div className="header">
        <Link href="/">
          <h1>Store</h1>
        </Link>
      </div>

      <div className="body">
        <h2>Payment Method</h2>
        {activePaymentMethods.length > 0 ? (
          <div>
            {activePaymentMethods.map(method => (
              <button
                key={method.paymentType}
                className="Button"
                onClick={() => handleMethodSelect(method.paymentType)}
              >
                {method.paymentName}
              </button>
            ))}
          </div>
        ) : (
          <p>No active payment methods available.</p>
        )}

        {selectedMethod !== null && (
          <div>
            <p>Purchase number: {purchaseNumber}.</p>
            {selectedMethod === PaymentMethodsEnum.Cash ? (
              <p>Awaiting confirmation from the administrator regarding the payment.</p>
            ) : (
              <div>
                <p>Make the payment through Sinpe to the number 8596-1362.</p>
                <input type="text" placeholder="Enter the receipt code here" />
                <button className="Button" onClick={handleSinpePaymentConfirmation}>Confirm</button>
                {paymentConfirmed && <p>Awaiting confirmation from the administrator regarding the payment.</p>}
              </div>
            )}
            <button className="Button" onClick={handleConfirmation}>Confirm Purchase</button>
          </div>
        )}
      </div>

      <div className="footer">
        <h2>Tienda.com</h2>
      </div>
    </div>
  );
}