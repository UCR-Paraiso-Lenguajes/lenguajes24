"use client";
import React, { useState, useEffect } from 'react';
import { ProductItem } from '../product/layout';
import '../HTMLPageDemo.css';
import Link from 'next/link';
import Head from 'next/head';
import Script from 'next/script';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

enum PaymentMethod {
  EFECTIVO = 0,
  SINPE = 1,
}

const provinces: Record<string, string[]> = {
  "San José": ["San José", "Escazú", "Desamparados", "Puriscal", "Tarrazú", "Aserrí", "Mora", "Goicoechea", "Santa Ana", "Alajuelita", "Vásquez de Coronado", "Acosta", "Tibás", "Moravia", "Montes de Oca", "Turrubares", "Dota", "Curridabat", "Pérez Zeledón", "León Cortés"],
  "Alajuela": ["Alajuela", "San Ramón", "Grecia", "San Mateo", "Atenas", "Naranjo", "Palmares", "Poás", "Orotina", "San Carlos", "Zarcero", "Valverde Vega", "Upala", "Los Chiles", "Guatuso", "Río Cuarto"],
  "Cartago": ["Cartago", "Paraíso", "La Unión", "Jiménez", "Turrialba", "Alvarado", "Oreamuno", "El Guarco"],
  "Heredia": ["Heredia", "Barva", "Santo Domingo", "Santa Bárbara", "San Rafael", "San Isidro", "Belén", "Flores", "San Pablo", "Sarapiquí"],
  "Guanacaste": ["Liberia", "Nicoya", "Santa Cruz", "Bagaces", "Carrillo", "Cañas", "Abangares", "Tilarán", "Nandayure", "La Cruz", "Hojancha"],
  "Puntarenas": ["Puntarenas", "Esparza", "Buenos Aires", "Montes de Oro", "Osa", "Quepos", "Golfito", "Coto Brus", "Parrita", "Corredores", "Garabito"],
  "Limón": ["Limón", "Pococí", "Siquirres", "Talamanca", "Matina", "Guácimo"]
};

const PurchasedItems = () => {
  const [cartState, setCartState] = useState({
    products: [],
    cart: {
      products: [],
      deliveryAddress: '',
      paymentMethod: '',
      subtotal: 0,
      total: 0,
    },
    paymentMethods: [
      {
        requiresVerification: false,
      },
    ],
  });

  const [showPaymentMethod, setShowPaymentMethod] = useState(false);
  const [selectedPaymentMethod, setSelectedPaymentMethod] = useState<PaymentMethod>(PaymentMethod.EFECTIVO);
  const [paymentConfirmation, setPaymentConfirmation] = useState('');
  const [paymentReceipt, setPaymentReceipt] = useState('');
  const [purchaseNumber, setPurchaseNumber] = useState('');
  const [generatedPhoneNumber, setGeneratedPhoneNumber] = useState('');

  const [selectedProvince, setSelectedProvince] = useState('');
  const [selectedCanton, setSelectedCanton] = useState('');
  const [extraAddress, setExtraAddress] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  useEffect(() => {
    const savedCartProducts = JSON.parse(localStorage.getItem('cartProducts') || '[]');
    const subtotal = savedCartProducts.reduce((acc: number, product: ProductItem) => acc + product.price * product.quantity, 0);
    const total = subtotal;

    setCartState((prevState) => ({
      ...prevState,
      cart: {
        ...prevState.cart,
        products: savedCartProducts,
        subtotal: subtotal,
        total: total,
      },
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

  const generatePhoneNumber = () => {
    const phoneNumber = Math.floor(10000000 + Math.random() * 90000000).toString();
    setGeneratedPhoneNumber(phoneNumber);
    return phoneNumber;
  };

  const managePaymentMethodSelection = (method: PaymentMethod) => {
    setSelectedPaymentMethod(method);
  };

  const managePaymentConfirmation = () => {
    if (selectedPaymentMethod === PaymentMethod.EFECTIVO) {
      setPaymentConfirmation(`Presione el boton "Confirmar compra" para finalizar`);
    } else if (selectedPaymentMethod === PaymentMethod.SINPE) {
      const phoneNumber = generatePhoneNumber();
      setPaymentConfirmation(`Por favor realice el pago a la cuenta indicada. El número de teléfono al cual depositar es: ${phoneNumber}. Una vez realizado, ingrese el comprobante y espere la confirmación del administrador.`);
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
      Subtotal: cartState.cart.subtotal,
    };

    try {
      const response = await fetch('https://localhost:7165/api/Cart', {
        method: 'POST',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(purchaseData),
      });

      if (response.ok) {
        const data = await response.json();
        const purchaseNum = generatePurchaseNumber();
        setPurchaseNumber(data.successPurchase || purchaseNum.toString());
        setPaymentConfirmation(`Su compra ha sido confirmada `);
        localStorage.removeItem('cartProducts');
        setCartState((prevState) => ({
          ...prevState,
          cart: {
            ...prevState.cart,
            products: [],
            deliveryAddress: '',
            subtotal: 0,
            total: 0,
          },
        }));
      } else {
        const errorResponseData = await response.json();
        throw new Error(errorResponseData.message || 'Purchase cannot be processed');
      }
    } catch (error) {
      throw new Error('Failed to fetch data');
    }
  };

  const isFormComplete = () => {
    if (selectedPaymentMethod === PaymentMethod.SINPE) {
      return (
        cartState.cart.deliveryAddress &&
        selectedPaymentMethod !== undefined &&
        paymentReceipt &&
        paymentConfirmation 
      );
    }
    return cartState.cart.deliveryAddress && selectedPaymentMethod !== undefined && paymentConfirmation;
  };

  const validateExtraAddress = (address: string) => {
    const regex = /^[a-zA-Z0-9\s,.-]+$/;
    return regex.test(address);
  };

  const handleAddressSubmit = () => {
    if (!selectedProvince || !selectedCanton) {
      setErrorMessage('Debe seleccionar una provincia y un cantón.');
      return;
    }
    if (!validateExtraAddress(extraAddress)) {
      setErrorMessage('La dirección específica contiene caracteres no válidos.');
      return;
    }
    const fullAddress = `${selectedProvince}, ${selectedCanton}, ${extraAddress}`;
    setCartState((prevState) => ({
      ...prevState,
      cart: {
        ...prevState.cart,
        deliveryAddress: fullAddress,
      },
    }));
    setErrorMessage('');
    handleContinue();
  };

  const handlePaymentReceiptChange = (e: any) => {
    const value = e.target.value;
    if (/^\d{0,8}$/.test(value)) {
      setPaymentReceipt(value);
    }
  };

  return (
    <div className="container mt-5">
      <Head>
        <title>Procesar Compra</title>
      </Head>
      <Script src="https://code.jquery.com/jquery-3.6.0.min.js"></Script>
      <Script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></Script>
      <h1 className="mb-4">Procesar Compra</h1>

      <button
        type="button"
        className="btn btn-info mb-3"
        data-toggle="modal"
        data-target="#addressModal"
      >
        Ingresar Dirección
      </button>

      {cartState.cart.deliveryAddress && (
        <div className="alert alert-info mt-3">
          Dirección ingresada: {cartState.cart.deliveryAddress}
        </div>
      )}

      {showPaymentMethod && (
        <>
          <div className="mt-4">
            <h2>Seleccione el método de pago:</h2>
            <div className="btn-group">
              <button
                onClick={() => managePaymentMethodSelection(PaymentMethod.EFECTIVO)}
                className={`btn ${selectedPaymentMethod === PaymentMethod.EFECTIVO ? 'btn-success' : 'btn-outline-success'}`}
              >
                Efectivo
              </button>
              <button
                onClick={() => managePaymentMethodSelection(PaymentMethod.SINPE)}
                className={`btn ${selectedPaymentMethod === PaymentMethod.SINPE ? 'btn-success' : 'btn-outline-success'}`}
              >
                Sinpe
              </button>
            </div>
          </div>

          {selectedPaymentMethod !== undefined && (
            <div className="mt-4">
              <h2>Confirmación de Pago</h2>
              <p>Método de Pago: {selectedPaymentMethod === PaymentMethod.EFECTIVO ? 'Efectivo' : 'Sinpe'}</p>
              <button onClick={managePaymentConfirmation} className="btn btn-primary mt-3">
                Confirmar Pago
              </button>
              {paymentConfirmation && <p className="alert alert-success mt-3">{paymentConfirmation}</p>}

              {selectedPaymentMethod === PaymentMethod.SINPE && paymentConfirmation && (
                <div className="mt-4">
                  <p>Ingrese el número donde se debe hacer el pago:</p>
                  <input
                    type="text"
                    className="form-control"
                    value={paymentReceipt}
                    onChange={handlePaymentReceiptChange}
                  />
                  <p className="mt-2">Una vez realizado el pago, espere la confirmación del administrador.</p>
                </div>
              )}
            </div>
          )}
        </>
      )}

      <div className="mt-5">
        <h2>Productos en el Carrito</h2>
        <ul className="list-group">
          {cartState.cart.products.map((product: ProductItem) => (
            <li key={product.id} className="list-group-item">
              <p>{product.name}</p>
              <p>Precio: ${product.price}</p>
              <p>Cantidad: {product.quantity}</p>
            </li>
          ))}
        </ul>
      </div>

      <Link href="/cart">
        <button className="btn btn-secondary mt-3">Volver al carrito</button>
      </Link>
      <button
        onClick={sendDataToAPI}
        className="btn btn-success mt-3"
        disabled={!isFormComplete()}
      >
        Confirmar Compra
      </button>
      {purchaseNumber && (
        <div className="alert alert-info mt-3">
          Número de Compra: {purchaseNumber}
        </div>
      )}

      <div className="modal fade" id="addressModal" tabIndex={-1} role="dialog" aria-labelledby="addressModalLabel" aria-hidden="true">
        <div className="modal-dialog" role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="addressModalLabel">Ingresar Dirección</h5>
              <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              {errorMessage && <div className="alert alert-danger">{errorMessage}</div>}
              <div className="form-group">
                <label htmlFor="province">Provincia</label>
                <select
                  className="form-control"
                  id="province"
                  value={selectedProvince}
                  onChange={(e) => {
                    setSelectedProvince(e.target.value);
                    setSelectedCanton('');
                  }}
                >
                  <option value="">Seleccione una provincia</option>
                  {Object.keys(provinces).map((province) => (
                    <option key={province} value={province}>
                      {province}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label htmlFor="canton">Cantón</label>
                <select
                  className="form-control"
                  id="canton"
                  value={selectedCanton}
                  onChange={(e) => setSelectedCanton(e.target.value)}
                  disabled={!selectedProvince}
                >
                  <option value="">Seleccione un cantón</option>
                  {selectedProvince && provinces[selectedProvince].map((canton) => (
                    <option key={canton} value={canton}>
                      {canton}
                    </option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label htmlFor="extraAddress">Dirección específica</label>
                <input
                  type="text"
                  className="form-control"
                  id="extraAddress"
                  value={extraAddress}
                  onChange={(e) => setExtraAddress(e.target.value)}
                  placeholder="Detalles adicionales de la dirección"
                />
              </div>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" data-dismiss="modal">Cerrar</button>
              <button
                type="button"
                className="btn btn-primary"
                onClick={handleAddressSubmit}
              >
                Guardar Dirección
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PurchasedItems;
