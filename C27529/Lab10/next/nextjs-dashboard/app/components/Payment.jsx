"use client"
import React, { useState, useEffect } from 'react';
import eventEmitter from './eventEmitter';

export const Payment = ({ goToPage }) => {
    const [page, setPage] = useState(0);
    const [randomNumber, setRandomNumber] = useState(Math.floor(Math.random() * (1000 - 1) + 1));
    const [purchaseNumber, setPurchaseNumber] = useState('');
    const [store, setStore] = useState(() => {
        const storedStore = localStorage.getItem("tienda");
        return JSON.parse(storedStore) || { carrito: { metodoDePago: '', direccionEntrega: '', total: 0 }, productos: [] };
    });

    const [paymentMethods, setPaymentMethods] = useState([]);
    const [paymentAvailable, setPaymentAvailable] = useState({ CASH: false, SINPE: false });

    const URL = process.env.NEXT_PUBLIC_API_URL;
    if (!URL) {
        throw new Error('NEXT_PUBLIC_API_URL is not defined');
    }

    useEffect(() => {
        // Obtener métodos de pago del localStorage
        const storedPaymentMethods = localStorage.getItem('paymentMethods');
        if (storedPaymentMethods) {
            const paymentMethods = JSON.parse(storedPaymentMethods);

            // Filtrar métodos de pago habilitados
            const enabledPaymentMethods = paymentMethods.filter(pm => pm.enabled);
            console.log('Enabled payment methods:', enabledPaymentMethods);

            // Actualizar los métodos de pago y la disponibilidad de métodos de pago
            setPaymentMethods(enabledPaymentMethods);
            const availablePayments = {
                CASH: enabledPaymentMethods.some(pm => pm.paymentType === 0),
                SINPE: enabledPaymentMethods.some(pm => pm.paymentType === 1)
            };
            console.log('Available payments:', availablePayments);
            setPaymentAvailable(availablePayments);

            // Configurar la página predeterminada según los métodos de pago disponibles
            if (!availablePayments.CASH && availablePayments.SINPE) {
                setPage(1);
            } else if (availablePayments.CASH && !availablePayments.SINPE) {
                setPage(0);
            }
        } else {
            console.error('No payment methods found in localStorage');
        }
    }, []);




    const updateStore = (updatedStore) => {
        setStore(updatedStore);
        localStorage.setItem("tienda", JSON.stringify(updatedStore));
        eventEmitter.emit('cartUpdated', updatedStore);
    };

    const postData = async () => {
        const cartData = {
            productIds: store.productos.map(product => product.id.toString()), // Convertir IDs a cadenas
            address: store.carrito.direccionEntrega,
            paymentMethod: store.carrito.metodoDePago,
            total: store.carrito.total
        };

        console.log("Cart Data:", cartData);

        try {
            const requestOptions = {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(cartData) // Enviar cartData directamente
            };

            const response = await fetch(`${URL}/api/Cart`, requestOptions);
            if (!response.ok) {
                throw new Error('Failed to post data');
            } else {
                const purchaseNumberApp = await response.json();
                setPurchaseNumber(purchaseNumberApp.purchaseNumberResponse);
                onCleanCart();
            }
        } catch (error) {
            console.error("Error creating the database:", error);
        }
    };

    const handleCheckboxChange = (pageNumber) => {
        setPage(pageNumber);
    };

    const onCleanCart = () => {
        const updatedStore = {
            productos: [],
            carrito: {
                subtotal: 0,
                total: 0,
                direccionEntrega: '',
                metodoDePago: ''
            }
        };
        updateStore(updatedStore);
    };


    const sendPayment = (metodoDePago) => {
        const paymentType = metodoDePago === 'CASH' ? 0 : 1;
        const updatedStore = {
          ...store,
          idCompra: randomNumber,
          carrito: {
            ...store.carrito,
            metodoDePago: paymentType
          }
        };
        updateStore(updatedStore);
        setTimeout(() => postData(updatedStore), 100); 
      };
      

    useEffect(() => {
        if (store.carrito.metodoDePago === 0 && paymentAvailable.CASH) {
            setPage(0);
        } else if (store.carrito.metodoDePago === 1 && paymentAvailable.SINPE) {
            setPage(1);
        }
    }, [store.carrito.metodoDePago, paymentAvailable]);

    return (
        <div className="center-content">
            {/* Mostrar mensaje si no hay métodos de pago disponibles */}
            {!paymentAvailable.CASH && !paymentAvailable.SINPE && (
                <p>No hay métodos de pago disponibles</p>
            )}
            {/* Mostrar opción de efectivo si está disponible */}
            {paymentAvailable.CASH && (
                <div className="form-check">
                    <input
                        type="radio"
                        className="form-check-input"
                        checked={page === 0}
                        onChange={() => handleCheckboxChange(0)}
                        id="cashRadio"
                    />
                    <label className="form-check-label" htmlFor="cashRadio">Efectivo</label>
                </div>
            )}
            {/* Mostrar opción de SINPE si está disponible */}
            {paymentAvailable.SINPE && (
                <div className="form-check">
                    <input
                        type="radio"
                        className="form-check-input"
                        checked={page === 1}
                        onChange={() => handleCheckboxChange(1)}
                        id="sinpeRadio"
                    />
                    <label className="form-check-label" htmlFor="sinpeRadio">Sinpe</label>
                </div>
            )}

            {/* Mostrar contenido de efectivo si la página es 0 y efectivo está disponible */}
            {page === 0 && paymentAvailable.CASH && (
                <>
                    <h4>Efectivo</h4>
                    <p>Número de transacción: {randomNumber}</p>
                    <button className="btn btn-primary" onClick={() => sendPayment('CASH')}>
                        Confirmar
                    </button>
                    <div className="d-flex justify-content-center mt-3">
                        <div className="spinner-border text-primary" role="status">
                            <span className="visually-hidden">Cargando...</span>
                        </div>
                        <div className="center-content">
                            <h6>Esperando confirmación del Administrador</h6>
                            <p>Número de Compra: {purchaseNumber}.</p>
                        </div>
                    </div>
                </>
            )}
            {/* Mostrar contenido de SINPE si la página es 1 y SINPE está disponible */}
            {page === 1 && paymentAvailable.SINPE && (
                <>
                    <h4>Sinpe</h4>
                    <p>Número de transacción: {randomNumber}</p>
                    <button className="btn btn-primary" onClick={() => sendPayment('SINPE')}>
                        Confirmar
                    </button>
                    <div className="d-flex justify-content-center mt-3">
                        <div className="spinner-border text-primary" role="status">
                            <span className="visually-hidden">Cargando...</span>
                        </div>
                        <div className="center-content">
                            <h6>Esperando confirmación del Administrador</h6>
                            <p>Número de Compra: {purchaseNumber}.</p>
                        </div>
                    </div>
                </>
            )}
            <div className="btn-cartPayment mt-4">
                <button onClick={() => goToPage(0)} className="btn btn-secondary" style={{ padding: '10px 20px', fontSize: '16px' }}>
                    Volver a la Página Principal
                </button>
            </div>
        </div>
    );
};
