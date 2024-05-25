'use client'
import React, { useState, useEffect } from 'react';
import { useRouter } from 'next/router'; // Importa el hook useRouter
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/globals.css';

const ConfirmacionEfectivoPage = () => {
    
    const [formState, setFormState] = useState({
        comprobante: '',
        confirmado: false,
    });

    const [numeroCompra, setNumeroCompra] = useState('Cargando...');

    useEffect(() => {
        const cartDataString = localStorage.getItem('cartData');
        const cartData = cartDataString ? JSON.parse(cartDataString) : {};
        const numeroCompra = cartData.numeroCompra || 'No disponible';
        setNumeroCompra(numeroCompra);
    }, []); 

    const router = useRouter(); 

    const handleConfirmar = () => {
        localStorage.removeItem('cartData');
        setFormState(prevState => ({
            ...prevState,
            confirmado: true,
        }));
    };

    const handleVolver = () => {
        router.push('/'); 
    };
 
    return (
        <div className="container mt-5">
            <h1 className="mb-4">Confirmación Efectivo</h1>
            <p className="mb-4">Número de Compra: {numeroCompra}</p>
            <div className="mb-4">
                <button className="btn btn-primary me-3" onClick={handleConfirmar}>
                    Confirmar
                </button>
            </div>
        
            {formState.confirmado && (
                <div className="mb-4">
                    <p>Esperando confirmación del administrador...</p>
                </div>
            )}
          
            {formState.confirmado && (
                <div>
                    <button className="btn btn-secondary" onClick={handleVolver}>
                        Volver a página principal
                    </button>
                </div>
            )}
        </div>
    );
};

export default ConfirmacionEfectivoPage;
