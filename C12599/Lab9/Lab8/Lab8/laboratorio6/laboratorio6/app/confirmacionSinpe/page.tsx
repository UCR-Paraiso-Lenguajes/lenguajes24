'use client'
import React, { useState } from 'react';
import { useRouter } from 'next/router'; // Importa el hook useRouter
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/globals.css';

const ConfirmacionSinpePage = () => {
    const [formState, setFormState] = useState({
        comprobante: '',
        confirmado: false, 
    });

    const handleComprobanteChange = (e) => {
        setFormState((prevState) => ({
            ...prevState,
            comprobante: e.target.value,
        }));
    };

    const handleConfirmar = () => {
        localStorage.removeItem('cartData'); 
        setFormState((prevState) => ({
            ...prevState,
            confirmado: true, 
        }));
    };

    const router = useRouter(); 

    const handleVolver = () => {
        router.push('/'); 
    };

    const cartDataString = localStorage.getItem('cartData');
    const cartData = cartDataString ? JSON.parse(cartDataString) : {};
    const numeroCompra = cartData.numeroCompra || 'No disponible';

    return (
        <div className="container mt-5">
            <h1 className="mb-4">Confirmación Sinpe</h1>
            <p className="mb-4">Número de Pago: 71133894</p>
            <p className="mb-4">Número de Compra: {numeroCompra}</p>
            <div className="mb-4">
                <label htmlFor="comprobante" className="form-label">Comprobante:</label>
                <textarea
                    id="comprobante"
                    className="form-control"
                    value={formState.comprobante}
                    onChange={handleComprobanteChange}
                    rows={4}
                ></textarea>
            </div>
            {!formState.confirmado && ( // Renderizar solo si no se ha confirmado el pago
                <div className="mb-4">
                    <button className="btn btn-primary me-3" onClick={handleConfirmar}>Confirmar</button>
                </div>
            )}
            {formState.confirmado && ( // Renderizar solo si se ha confirmado el pago
                <div className="mb-4">
                    <p>Esperando confirmación del administrador...</p>
                </div>
            )}
            {formState.confirmado && ( // Renderizar solo si se ha confirmado el pago
                <div>
                    {/* Botón para volver a la página principal */}
                    <button className="btn btn-secondary" onClick={handleVolver}>Volver a página principal</button>
                </div>
            )}
        </div>
    );
};

export default ConfirmacionSinpePage;
