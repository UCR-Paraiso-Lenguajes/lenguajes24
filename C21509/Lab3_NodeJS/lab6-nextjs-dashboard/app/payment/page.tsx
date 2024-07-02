"use client"
import React, { useState, useEffect } from 'react';
import { ProductItem } from '../product/layout';
import '../HTMLPageDemo.css';
import Link from 'next/link';

enum PaymentMethod {
  EFECTIVO = 0,
  SINPE = 1
}

const PurchasedItems = () => {
  const [cartState, setCartState] = useState({
    products: [],
    cart: {
      products: [],
      deliveryAddress: '',
      paymentMethod: '',
      subtotal: 0, 
      total: 0 
    },
    paymentMethods: [
      {
        requiresVerification: false
      }
    ]
  });

  const [showPaymentMethod, setShowPaymentMethod] = useState(false);
  const [selectedPaymentMethod, setSelectedPaymentMethod] = useState<PaymentMethod>(PaymentMethod.EFECTIVO);
  const [paymentConfirmation, setPaymentConfirmation] = useState('');
  const [paymentReceipt, setPaymentReceipt] = useState('');
  const [purchaseNumber, setPurchaseNumber] = useState('');

  useEffect(() => {
    const savedCartProducts = JSON.parse(localStorage.getItem('cartProducts') || '[]');
    const subtotal = savedCartProducts.reduce((acc: number, product: ProductItem) => acc + product.price * product.quantity, 0);
    const total = subtotal; 

    setCartState(prevState => ({
      ...prevState,
      cart: {
        ...prevState.cart,
        products: savedCartProducts,
        subtotal: subtotal,
        total: total
      }
    }));
  }, []);

  const handleContinue = () => {
    if (cartState.cart.products.length > 0) {
      setShowPaymentMethod(true);
    }
  };

  const generatePurchaseNumber = () => {
    return Math.floor(10000000 + Math.random() * 90000000);
  };

  const managePaymentMethodSelection = (method: PaymentMethod) => {
    setSelectedPaymentMethod(method);
  };

  const managePaymentConfirmation = () => {
    if (selectedPaymentMethod === PaymentMethod.EFECTIVO) {
      const purchaseNum = generatePurchaseNumber();
      setPaymentConfirmation(`Su compra ha sido confirmada.`);
      setPurchaseNumber(purchaseNum.toString());
    } else if (selectedPaymentMethod === PaymentMethod.SINPE) {
      const purchaseNum = generatePurchaseNumber();
      setPaymentConfirmation(`Por favor realice el pago a la cuenta indicada. El número de teléfono al cual depositar es: ${purchaseNum}. Una vez realizado, ingrese el comprobante y espere la confirmación del administrador.`);
      setPurchaseNumber(purchaseNum.toString());
    }
  };

  const sendDataToAPI = async () => {
    const productIdsAndQuantities = cartState.cart.products.map((product: ProductItem) => ({
      id: Number(product.id),
      quantity: product.quantity,
    }));
    const paymentMethodValue = selectedPaymentMethod === PaymentMethod.EFECTIVO ? 0 : 1; 
    const purchaseData = {
      Products: productIdsAndQuantities,
      Address: cartState.cart.deliveryAddress,
      PaymentMethod: paymentMethodValue, 
      Total: cartState.cart.total, 
      Subtotal: cartState.cart.subtotal 
    };
  
    try {
      const response = await fetch('https://localhost:7165/api/Cart', {
        method: 'POST',
        headers: {
          "Accept": "application/json",
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(purchaseData)
      });
  
      if (response.ok) {
        const data = await response.json();
        setPurchaseNumber(data.successPurchase);
        setPaymentConfirmation(`Su compra ha sido confirmada.`);
        localStorage.removeItem('cartProducts');
      } else {
        const errorResponseData = await response.json();
        throw new Error(errorResponseData.message || 'Purchase cannot be processed');
      }
    } catch (error) {
      throw new Error('Failed to fetch data');
    }
  };

  return (
    <div>
      <h1>Procesar Compra</h1>

      <input
        type="text"
        value={cartState.cart.deliveryAddress}
        onChange={(e) => setCartState(prevState => ({
          ...prevState,
          cart: {
            ...prevState.cart,
            deliveryAddress: e.target.value
          }
        }))}
        placeholder="Ingrese la dirección de entrega"
      />

      <button onClick={handleContinue} disabled={!cartState.cart.deliveryAddress} className="button">Continuar</button>

      {showPaymentMethod && (
        <div>
          <h2>Seleccione el método de pago:</h2>
          <button onClick={() => managePaymentMethodSelection(PaymentMethod.EFECTIVO)} className="button">Efectivo</button>
          <button onClick={() => managePaymentMethodSelection(PaymentMethod.SINPE)} className="button">Sinpe</button>
        </div>
      )}

      {selectedPaymentMethod !== undefined && (
        <div>
          <h2>Confirmación de Pago</h2>
          <p>Método de Pago: {selectedPaymentMethod === PaymentMethod.EFECTIVO ? 'Efectivo' : 'Sinpe'}</p>
          <button onClick={managePaymentConfirmation} className="button">Confirmar Pago</button>
          {paymentConfirmation && <p>{paymentConfirmation}</p>}
          {paymentReceipt && <p>Adjunte el comprobante: {paymentReceipt}</p>}

          {selectedPaymentMethod === PaymentMethod.SINPE && paymentConfirmation && (
            <div>
              <p>Número donde realizar el pago: {generatePurchaseNumber()}</p>
              <p>Ingrese el número donde se realiza el pago:</p>
              <input type="text" value={paymentReceipt} onChange={(e) => setPaymentReceipt(e.target.value)} placeholder="Ingrese el comprobante" />
              <p>Una vez realizado el pago, espere la confirmación del administrador.</p>
            </div>
          )}
        </div>
      )}

      <div>
        <h2>Productos en el Carrito</h2>
        <ul>
          {cartState.cart.products.map((product: ProductItem) => (
            <li key={product.id}>
              <p>{product.name}</p>
              <p>Precio: ${product.price}</p>
              <p>Cantidad: {product.quantity}</p>
            </li>
          ))}
        </ul>
      </div>
      <Link href="/cart">
        <button className="button">Volver al carrito</button>
      </Link>
      <button onClick={sendDataToAPI} className="button">Confirmar Compra</button>
      <p>Número de Compra: {purchaseNumber}</p>
    </div>
  );
};

export default PurchasedItems;