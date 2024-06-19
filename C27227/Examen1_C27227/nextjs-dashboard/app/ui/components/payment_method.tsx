import React, { useState, useEffect, ChangeEvent } from 'react';
import '../Styles/paymentMethods.css';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

interface PaymentMethodsProps {
  address: string;
}

const PaymentMethods: React.FC<PaymentMethodsProps> = ({ address }) => {
  const [paymentmethod, setPaymentMethod] = useState<string>('');
  const [paymentCode, setPaymentCode] = useState<string>('');
  const [ConfirmationPurchase, setConfirmationPurchase] = useState<boolean>(false);
  const [warning, setWarning] = useState<boolean>(false);
  const [warningMessage, setWarningMessage] = useState<string>('');
  const [purchaseNumber, setPurchaseNumber] = useState<string>('');
  const tiendaPago = localStorage.getItem('tienda');
  const tiendaLocal = tiendaPago ? JSON.parse(tiendaPago) : {};
  const URLConection = process.env.NEXT_PUBLIC_API as string;
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

  const handlePaymentCodeChange = (event: ChangeEvent<HTMLInputElement>) => {
    setPaymentCode(event.target.value);
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
          metodoPago: paymentmethod,
          direccionEntrega: address
        },
        necesitaVerifica: true,
        idCompra: 1
      };
      localStorage.setItem("tienda", JSON.stringify(newp));
      setConfirmationPurchase(true);
    }
  };

  const enviarDatosPago = async () => {
    if (paymentmethod === 'sinpe' && !paymentCode.trim()) {
      setWarning(true);
      setWarningMessage('Por favor, ingrese el comprobante de pago.');
      setTimeout(() => {
        setWarning(false);
        setWarningMessage('');
      }, 2000);
      return;
    }

    const productQuantities = Object.keys(tiendaLocal.cart.cartItems).map(productId => ({
      id: parseInt(productId),
      quantity: tiendaLocal.cart.cartItems[productId]
    }));

    const paymentMethodValue = paymentmethod === 'sinpe' ? 1 : 0;

    const dataToSend = {
      product: productQuantities,
      address: address,
      paymentMethod: paymentMethodValue,
      paymentCode: paymentmethod === 'sinpe' ? paymentCode : undefined
    };

    try {
      const response = await fetch(`${URLConection}/api/cart`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(dataToSend)
      });

      if (!response.ok) {
        const errorResponseData = await response.text();
        try {
          const errorJson = JSON.parse(errorResponseData);
          throw new Error(errorJson.message || 'Error');
        } catch (jsonError) {
          throw new Error(errorResponseData);
        }
      }

      const data = await response.json();
      setPurchaseNumber(data.purchaseNumber);
    } catch (error) {
      setWarningMessage('Error al enviar datos: ' + error.message);
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
        <p>Número de compra: {purchaseNumber}</p>
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
        <p>Número de compra: {purchaseNumber}</p>
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
            <input type="radio" id="sinpe" name="paymentMethod" value="sinpe" checked={paymentmethod === 'sinpe'} onChange={() => setPaymentMethod('sinpe')} />
            <label htmlFor="sinpe">Sinpe</label>
          </div>
          <div className="payment-method">
            <input type="radio" id="cash" name="paymentMethod" value="cash" checked={paymentmethod === 'cash'} onChange={() => setPaymentMethod('cash')} />
            <label htmlFor="cash">Efectivo</label>
          </div>
          <button className='BtnBuy' onClick={handlePaymentMethod}>Continuar compra</button>
        </fieldset>
      )}
    </div>
  );
};

export default PaymentMethods;
