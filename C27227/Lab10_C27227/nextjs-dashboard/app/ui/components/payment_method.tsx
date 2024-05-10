import React, { useState } from 'react';
import '../styles/paymentMethods.css';

const PaymentMethods = () => {
  const [paymentmethod, setpaymentmethod] = useState('');
  const [paymentCode, setPaymentCode] = useState('');
  const [ConfirmationPurchase, setConfirmationPurchase] = useState(false);
  const [warning, setWarning] = useState(false);
  const [warningMessage, setWarningMessage] = useState('');
  const [purchaseNumber, setPurchaseNumber] = useState('');
  const tiendaPago = localStorage.getItem('tienda');
  const tiendaLocal = JSON.parse(tiendaPago);

  const handlePaymentCodeChange = (event) => {
    if (event && event.target && typeof event.target.value === 'string') {
      setPaymentCode(event.target.value);
    } else {
      throw new Error('El argumento del evento no es válido');
    }
  };

  const handlePaymentMethod = () => {
    if (!paymentmethod) {
      setWarning(true);
      setWarningMessage('Por favor, seleccione un método de pago.');
      setTimeout(() => {
        setWarning(false);
        setWarningMessage('');
      }, 2000);
    } else {
      const newp = {
        ...tiendaLocal,
        cart: {
          ...tiendaLocal.cart,
          metodoPago: paymentmethod
        },
        necesitaVerifica: true,
        idCompra: 1
      };
      localStorage.setItem("tienda", JSON.stringify(newp));
      setConfirmationPurchase(true);
    }
  };

  const enviarDatosPago = async () => {
    const datosDePagoValidos = tiendaLocal.cart.direccionEntrega && tiendaLocal.cart.metodoPago && (tiendaLocal.cart.metodoPago === 'cash' || (tiendaLocal.cart.metodoPago === 'sinpe' && paymentCode.trim() !== ''));

    if (datosDePagoValidos) {
      // Extraer las cantidades de cada producto del localStorage
      const productQuantities = Object.keys(tiendaLocal.cart.cartItems).map(productId => ({
        id: parseInt(productId), // Convertir el ID de string a número
        quantity: tiendaLocal.cart.cartItems[productId]
      }));

      const paymentMethodValue = tiendaLocal.cart.metodoPago === 'sinpe' ? 1 : 0;

      // Construir el cuerpo de la solicitud
      const dataToSend = {
        product: productQuantities,
        address: tiendaLocal.cart.direccionEntrega,
        paymentMethod: paymentMethodValue
      };

      try {
        const response = await fetch('http://localhost:5072/api/Cart', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(dataToSend)
        });

        if (response.ok) {
          const data = await response.json();
          setPurchaseNumber(data.purchaseNumber);
        } else {
          const errorResponseData = await response.json();
          throw new Error(errorResponseData.message || 'Error');
        }
      } catch (error) {
        throw new Error('Error al enviar datos: ' + error.message);
      }
    } else {
      if (tiendaLocal.cart.metodoPago === 'sinpe' && paymentCode.trim() === '') {
        setWarningMessage('Por favor, ingrese el comprobante de pago.');
      } else {
        setWarningMessage('Por favor, complete los datos de pago.');
      }
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
        setWarningMessage('');
      }, 2000);
    }
  };

  const pagoEfectivo = () => {
    return (
      <div className="payment-info">
        <p className="payment-info-title">Detalles de Pago:</p>
        <p>Número de compra:{purchaseNumber} </p>
        <p>Espere la confirmación del administrador con respecto al pago.</p>
        <button className='BtnBuy' onClick={enviarDatosPago}>Confirmar compra</button>
      </div>
    );
  };

  const pagoSinpe = () => {
    return (
      <div className="payment-info">
        <p className="payment-info-title">Detalles de Pago:</p>
        <p>Número de cuenta: +506-5678-9012</p>
        <p>Número de compra:{purchaseNumber} </p>
        <input type="text" value={paymentCode} onChange={handlePaymentCodeChange} className="payment-code-input" placeholder="Ingrese el comprobante" />
        <p>Espere la confirmación del administrador con respecto al pago.</p>
        <button className='BtnBuy' onClick={enviarDatosPago}>Confirmar compra</button>
      </div>
    );
  };

  return (
    <div>
      {warning && <div className='alert'>{warningMessage}</div>}
      {ConfirmationPurchase ? (
        paymentmethod === 'cash' ? pagoEfectivo() : paymentmethod === 'sinpe' ? pagoSinpe() : null
      ) : (
        <fieldset className="payment-methods">
          <legend>Escoja el método de pago</legend>
          <div className="payment-method">
            <input type="radio" id="sinpe" name="paymentMethod" value="sinpe" checked={paymentmethod === 'sinpe'} onChange={() => setpaymentmethod('sinpe')} />
            <label htmlFor="sinpe">Sinpe</label>
          </div>
          <div className="payment-method">
            <input type="radio" id="cash" name="paymentMethod" value="cash" checked={paymentmethod === 'cash'} onChange={() => setpaymentmethod('cash')} />
            <label htmlFor="cash">Efectivo</label>
          </div>
          <button className='BtnBuy' onClick={handlePaymentMethod}>Continuar compra</button>
        </fieldset>
      )}
    </div>
  );
};

export default PaymentMethods;