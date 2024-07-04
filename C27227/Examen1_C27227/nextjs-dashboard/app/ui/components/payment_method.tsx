import React, { useState, useEffect, ChangeEvent } from "react";
import "../Styles/paymentMethods.css";
import { decodeToken, checkTokenDate } from "../../hooks/jwtHooks";
import { useRouter } from "next/navigation";

interface PaymentMethodsProps {
  address: {
    street: string;
    specificAddress: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
  };
}

interface PaymentMethod {
  paymentType: number;
  isEnabled: boolean;
}

const PaymentMethods: React.FC<PaymentMethodsProps> = ({ address }) => {
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
  const [paymentmethod, setPaymentMethod] = useState<string>("");
  const [paymentCode, setPaymentCode] = useState<string>("");
  const [confirmationPurchase, setConfirmationPurchase] = useState<boolean>(false);
  const [purchaseConfirmed, setPurchaseConfirmed] = useState<boolean>(false);
  const [warning, setWarning] = useState<boolean>(false);
  const [warningMessage, setWarningMessage] = useState<string>("");
  const [purchaseNumber, setPurchaseNumber] = useState<string>("");
  const tiendaPago = localStorage.getItem("tienda");
  const tiendaLocal = tiendaPago ? JSON.parse(tiendaPago) : null;
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

  useEffect(() => {
    const fetchPaymentMethods = async () => {
      try {
        const response = await fetch(`${URLConection}/api/store/paymentMethods`);
        if (response.ok) {
          const data = await response.json();
          setPaymentMethods(data);
        } else {
          throw new Error("Error fetching payment methods");
        }
      } catch (error) {
        setWarningMessage("Error fetching payment methods: " + error.message);
        setWarning(true);
        setTimeout(() => {
          setWarning(false);
          setWarningMessage("");
        }, 2000);
      }
    };

    fetchPaymentMethods();
  }, [URLConection]);

  const handlePaymentCodeChange = (event: ChangeEvent<HTMLInputElement>) => {
    setPaymentCode(event.target.value);
  };

  const handlePaymentMethod = () => {
    if (!paymentmethod) {
      setWarning(true);
      setWarningMessage("Por favor, seleccione un método de pago.");
      setTimeout(() => {
        setWarning(false);
        setWarningMessage("");
      }, 2000);
      return;
    }

    const selectedMethod = paymentMethods.find((method) => method.paymentType.toString() === paymentmethod);
    if (selectedMethod && !selectedMethod.isEnabled) {
      setWarning(true);
      setWarningMessage("El método de pago seleccionado está deshabilitado.");
      setTimeout(() => {
        setWarning(false);
        setWarningMessage("");
      }, 2000);
      return;
    }

    const newp = {
      ...tiendaLocal,
      cart: {
        ...tiendaLocal.cart,
        metodoPago: paymentmethod,
        direccionEntrega: `${address.street}, ${address.city}, ${address.state}, ${address.zipCode}, ${address.country}`,
      },
      necesitaVerifica: true,
      idCompra: 1,
    };
    localStorage.setItem("tienda", JSON.stringify(newp));
    setConfirmationPurchase(true);
  };

  const enviarDatosPago = async () => {
    if (paymentmethod === "1" && !paymentCode.trim()) {
      setWarning(true);
      setWarningMessage("Por favor, ingrese el comprobante de pago.");
      setTimeout(() => {
        setWarning(false);
        setWarningMessage("");
      }, 2000);
      return;
    }

    const productQuantities = tiendaLocal && tiendaLocal.cart && tiendaLocal.cart.cartItems ? Object.keys(tiendaLocal.cart.cartItems).map(
      (productId) => ({
        id: parseInt(productId),
        quantity: tiendaLocal.cart.cartItems[productId],
      })
    ) : [];

    const paymentMethodValue = parseInt(paymentmethod); // Ensure this is correctly set

    const dataToSend = {
      cart: {
        product: productQuantities,
        address: {
          street: address.street,
          specificAddress: address.specificAddress,
          city: address.city,
          state: address.state,
          zipCode: address.zipCode,
          country: address.country,
        },
        paymentMethod: paymentMethodValue,  // Send the correct value
        paymentCode: paymentMethodValue === 1 ? paymentCode : undefined,
      },
    };

    try {
      const response = await fetch(`${URLConection}/api/cart`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(dataToSend),
      });

      if (!response.ok) {
        const errorResponseData = await response.text();
        try {
          const errorJson = JSON.parse(errorResponseData);
          throw new Error(errorJson.message || "Error");
        } catch (jsonError) {
          throw new Error(errorResponseData);
        }
      }

      const data = await response.json();
      setPurchaseNumber(data.purchaseNumber);
      localStorage.removeItem("tienda");
      setPurchaseConfirmed(true);
    } catch (error) {
      setWarningMessage("Error al enviar datos: " + error.message);
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
        setWarningMessage("");
      }, 2000);
    }
  };

  const handleModalClose = () => {
    localStorage.removeItem("tienda");
    setPurchaseConfirmed(false);
    window.location.replace('/');
  };

  const getPaymentMethodName = (type: number) => {
    switch (type) {
      case 0:
        return "Efectivo";
      case 1:
        return "Sinpe";
      default:
        return "Desconocido";
    }
  };

  return (
    <div>
      {warning && (
        <div className="alert-container">
          <div className="alert">{warningMessage}</div>
        </div>
      )}
      {confirmationPurchase ? (
        <div className="payment-info">
          <h2>Resumen de la compra</h2>
          <div className="purchase-details">
            <h3>Detalles del pedido</h3>
            {tiendaLocal && tiendaLocal.cart && tiendaLocal.cart.cartItems && Object.keys(tiendaLocal.cart.cartItems).map((productId) => {
              const product = tiendaLocal.products.find(p => p.id === parseInt(productId));
              return (
                <p key={productId}>
                  <strong>Producto:</strong> {product.name}<br />
                  <strong>Cantidad:</strong> {tiendaLocal.cart.cartItems[productId]}<br />
                  <strong>Precio:</strong> ${product.price}
                </p>
              );
            })}
            <p>
              <strong>Dirección de envío:</strong>{" "}
              {`${address.street}, ${address.city}, ${address.state}, ${address.zipCode}, ${address.country}`}
            </p>
            <p>
              <strong>Método de pago:</strong> {getPaymentMethodName(parseInt(paymentmethod))}
            </p>
            {paymentmethod === "1" && (
              <>
                <p>
                  <strong>Número de cuenta:</strong> +506-5678-9012
                </p>
                <input
                  type="text"
                  value={paymentCode}
                  onChange={handlePaymentCodeChange}
                  className="payment-code-input"
                  placeholder="Ingrese el comprobante"
                  style={{ fontSize: '14px', height: '40px', marginTop: '10px' }}
                />
              </>
            )}
            <h3>Detalles del pago</h3>
            <p>
              <strong>Total:</strong> {tiendaLocal && tiendaLocal.cart ? tiendaLocal.cart.total : 0}
            </p>
            <div className="buttons-container">
              <button className="BtnBuy" onClick={enviarDatosPago}>
                Confirmar compra
              </button>
              <button className="BtnBuy" onClick={() => setConfirmationPurchase(false)}>
                Regresar
              </button>
            </div>
          </div>
        </div>
      ) : (
        <fieldset className="payment-methods">
          <legend>Escoja el método de pago</legend>
          <div className="payment-methods-container">
            {paymentMethods.map((method) => (
              method.isEnabled ? (
                <div key={method.paymentType} className="payment-method">
                  <input
                    type="radio"
                    id={method.paymentType.toString()}
                    name="paymentMethod"
                    value={method.paymentType.toString()}
                    checked={paymentmethod === method.paymentType.toString()}
                    onChange={() => setPaymentMethod(method.paymentType.toString())}
                  />
                  <label htmlFor={method.paymentType.toString()}>{getPaymentMethodName(method.paymentType)}</label>
                </div>
              ) : (
                <div key={method.paymentType} className="payment-method">
                  <input
                    type="radio"
                    id={method.paymentType.toString()}
                    name="paymentMethod"
                    value={method.paymentType.toString()}
                    checked={false}
                    disabled
                  />
                  <label htmlFor={method.paymentType.toString()}>{getPaymentMethodName(method.paymentType)} (Deshabilitado)</label>
                </div>
              )
            ))}
          </div>
          <div className="buttons-container">
            <button className="BtnBuy" onClick={handlePaymentMethod}>
              Continuar compra
            </button>
          </div>
        </fieldset>
      )}

      {purchaseConfirmed && (
        <div className="modal">
          <div className="modal-content">
            <h2>Compra Confirmada</h2>
            <p>Su compra se ha realizado con éxito.</p>
            <p>Número de compra: {purchaseNumber}</p>
            <button className="BtnBuy" onClick={handleModalClose}>
              Cerrar
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default PaymentMethods;
