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

    const URL = process.env.NEXT_PUBLIC_API_URL;
    if (!URL) {
        throw new Error('NEXT_PUBLIC_API_URL is not defined');
    }
    const productIds = store.productos.map(producto => producto.id.toString());

    const data = {
        productIds: productIds,
        address: store.carrito.direccionEntrega ? store.carrito.direccionEntrega.toString() : '',
        paymentMethod: store.carrito.metodoDePago,
        total: store.carrito.total
    };

    const postData = async () => {
        try {
            const requestOptions = {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            };
            const response = await fetch(`${URL}/api/Cart`, requestOptions);
            const responseData = await response.json();
            if (!response.ok) {
                console.error('Response data:', responseData); // Log the response data
                throw new Error('Failed to post data');
            } else {
                setPurchaseNumber(responseData.purchaseNumberResponse);
                onCleanCart(); // Limpiar el carrito después de una compra exitosa
            }
        } catch (error) {
            console.error('Error creating the database:', error); // Log the error
            throw new Error("Error creating the database");
        }
    };

    const handleCheckboxChange = (pageNumber) => {
        setPage(pageNumber);
    };

    const onCleanCart = () => {
        localStorage.removeItem("tienda");
        const updatedStore = { productos: [], carrito: { subtotal: 0, total: 0 } };
        setStore(updatedStore);
        eventEmitter.emit('cartUpdated', updatedStore);
    };

    const sendPayment = (metodoDePago) => {
        const updatedStore = {
            ...store,
            idCompra: randomNumber,
            necesitaVerificacion: true,
            carrito: {
                ...store.carrito,
                metodoDePago: metodoDePago
            }
        };
        localStorage.setItem("tienda", JSON.stringify(updatedStore));
        setStore(updatedStore);
        postData();
    };

    useEffect(() => {
        setPage(store.carrito.metodoDePago === 'Efectivo' ? 0 : 1);
    }, [store.carrito.metodoDePago]);

    return (
        <div className="center-content">
            <input
                type="checkbox"
                checked={page === 0}
                onChange={() => handleCheckboxChange(0)}
            /><label> Efectivo </label>

            <input
                type="checkbox"
                checked={page === 1}
                onChange={() => handleCheckboxChange(1)}
            /><label> Sinpe </label>

            {page === 0 ? (
                <>
                    <p>Efectivo</p>
                    <p>Número de transacción: {randomNumber}</p>
                    <button onClick={() => sendPayment(0)}>
                        Confirmar
                    </button>
                    <div className="d-flex justify-content-center">
                        <div className="spinner-border text-primary" role="status">
                            <span className="visually-hidden">Cargando...</span>
                        </div>
                        <div className="center-content">
                            <h6>Esperando confirmación del Administrador</h6>
                            <p>Número de Compra: {purchaseNumber}.</p>
                        </div>
                    </div>
                </>
            ) : (
                <>
                    <p>Sinpe</p>
                    <p>Número de transacción: {randomNumber}</p>
                    <button onClick={() => sendPayment(1)}>
                        Confirmar
                    </button>
                    <div className="d-flex justify-content-center">
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
            <div className="btn-cartPayment">
                <button onClick={() => goToPage(0)} className="btn btn-primary" style={{ marginTop: '20px', padding: '10px 20px', fontSize: '16px' }}>
                    Volver a la Página Principal
                </button>
            </div>
        </div>
    );
};
