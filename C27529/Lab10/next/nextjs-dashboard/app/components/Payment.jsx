"use client"
import React, { useState, useEffect } from 'react';

export const Payment = () => {
    const [page, setPage] = useState(0);
    const [randomNumber, setRandomNumber] = useState(Math.floor(Math.random() * (1000 - 1) + 1));
    const [purchaseNumber, setpurchaseNumber] = useState('');
    const [store, setStore] = useState(() => {
        const storedStore = localStorage.getItem("tienda");
        return JSON.parse(storedStore) || { carrito: { metodoDePago: '' } };
    });



    const productIds = store.productos.map(producto => producto.id.toString());

    const data = {
        productIds: productIds,
        address: store.carrito.direccionEntrega.toString(),
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
            const response = await fetch('https://localhost:7280/api/Cart', requestOptions);
            if (!response.ok) {
                throw new Error('Failed to post data');
            } else {
                const purchaseNumberApp = await response.json();
                setpurchaseNumber(purchaseNumberApp.purchaseNumberResponse);
            }


        } catch (error) {
            throw new Error("Error creating the database");
        }
    };

    const handleCheckboxChange = (pageNumber) => {
        setPage(pageNumber);

    };

    const onCleanCart = () => {
        localStorage.removeItem("tienda");
        setStore({ productos: [], carrito: { subtotal: 0, total: 0 } });

    };

    const sendPayment = (metodoDePago) => {
        const updatedStore = {
            ...store,
            idCompra: randomNumber,
            necesitaVerificacion: true,
            carrito: {
                ...store.carrito,
                metodoDePago: metodoDePago

            },

        };
        localStorage.setItem("tienda", JSON.stringify(updatedStore));
        setStore(updatedStore);
        postData();
    }
    useEffect(() => {
        setPage(store.carrito.metodoDePago === 'Efectivo' ? 0 : 1);
    }, []);


    return (
        <div className="center-content">
            <input
                type="checkbox"
                checked={page === 0}
                onChange={() => handleCheckboxChange(0, 0)}
            /><label> Efectivo </label>

            <input
                type="checkbox"
                checked={page === 1}
                onChange={() => handleCheckboxChange(1, 1)}
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
        </div>
    );
};
