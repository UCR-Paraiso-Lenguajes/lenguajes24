import React, { useState } from 'react';
import '../styles/paymentMethods.css';

const PaymentMethods = ({ methods }) => {
  const [selectedMethod, setSelectedMethod] = useState(null);
  const [purchaseNumber, setPurchaseNumber] = useState(null);
  const [paymentCode, setPaymentCode] = useState('');
  const [showMethod, setShowMethod] = useState(false);
  const [warning, setWarning] = useState(false);

  const tiendaPago = localStorage.getItem('tienda');
  const tiendaLocal = JSON.parse(tiendaPago);

  const handleMethodChange = (event) => {
    setSelectedMethod(event.target.value);
    const newPurchaseNumber = Math.floor(Math.random() * 1000000);
    setPurchaseNumber(newPurchaseNumber);
  };

  const handlePaymentCodeChange = (event) => {
    setPaymentCode(event.target.value);
  };

  const handlePaymentMethod = () => {
    if (!selectedMethod) {
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
      }, 2000);
    } else {
      const newp = {
        ...tiendaLocal,
        cart: {
          ...tiendaLocal.cart,
          metodoPago: selectedMethod
        },
        necesitaVerifica: true,
        idCompra: purchaseNumber
      };
      localStorage.setItem("tienda", JSON.stringify(newp));
      setShowMethod(true);
    }
  };

  const pagoEfectivo = () => {
    return (
      <div className="payment-info">
        <p className="payment-info-title">Detalles de Pago:</p>
        <p>Número de compra: {purchaseNumber}</p>
        <p>Espere la confirmación del administrador con respecto al pago.</p>
      </div>
    );
  };

  const pagoSinpe = () => {
    return (
      <div className="payment-info">
        <p className="payment-info-title">Detalles de Pago:</p>
        <p>Número de compra: {purchaseNumber}</p>
        <p>Número de cuenta: 1234-5678-9012</p>
        <input type="text" value={paymentCode} onChange={handlePaymentCodeChange} className="payment-code-input" placeholder="Ingrese el comprobante" />
        <p>Espere la confirmación del administrador con respecto al pago.</p>
      </div>
    );
  };

  return (
    <div>
      {showMethod ? (
        selectedMethod === 'cash' ? pagoEfectivo() : selectedMethod === 'sinpe' ? pagoSinpe() : null
      ) : (
        <fieldset className="payment-methods">
          <legend>Escoja el metodo de pago</legend>
          <div className="payment-method">
            <input type="radio" id="sinpe" name="paymentMethod" value="sinpe" checked={selectedMethod === 'sinpe'} onChange={handleMethodChange} />
            <label htmlFor="sinpe">Sinpe</label>
          </div>
          <div className="payment-method">
            <input type="radio" id="cash" name="paymentMethod" value="cash" checked={selectedMethod === 'cash'} onChange={handleMethodChange} />
            <label htmlFor="cash">Efectivo</label>
          </div>
          <button className='BtnBuy' onClick={handlePaymentMethod}>Continuar compra</button>
        </fieldset>
      )}
      {warning && <div className='alert'>Por favor, seleccione un método de pago.</div>}
    </div>
  );
};

export default PaymentMethods;

