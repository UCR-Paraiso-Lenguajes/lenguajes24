'use client'
import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';

const PayPage: React.FC = () => {
    const [direccionEntrega, setDireccionEntrega] = useState('');
    const [metodoPago, setMetodoPago] = useState('');
    const [comprobante, setComprobante] = useState('');
    const [confirmacion, setConfirmacion] = useState('');
    const [numeroCompra, setNumeroCompra] = useState(Math.floor(Math.random() * 1000) + 1);
    const [numeroPago, setNumeroPago] = useState(Math.floor(Math.random() * 10000) + 1);

    const handleDireccionEntregaChange = (value: string) => {
        setDireccionEntrega(value);
        // Resetear método de pago cuando se cambia la dirección de entrega
        setMetodoPago('');
    };

    const handleMetodoPagoChange = (value: string) => {
        setMetodoPago(value);
        setComprobante('');
        setConfirmacion('');
    };

    const handleComprobanteChange = (value: string) => {
        setComprobante(value);
    };


    const handleSubmit = () => {
        setConfirmacion('Esperando confirmación del administrador'); // Actualiza el estado con el mensaje de confirmación
        localStorage.setItem('cartItems', '');
        localStorage.setItem('count', '');
        setComprobante('');
        setNumeroCompra(0);
        setDireccionEntrega('');
    }

    return (
        <div>
            <h1>Pay Page</h1>
            <div className="form-group">
                <label htmlFor="direccionEntrega">Dirección de Entrega:</label>
                <input
                    type="text"
                    id="direccionEntrega"
                    className="form-control"
                    value={direccionEntrega}
                    onChange={(e) => handleDireccionEntregaChange(e.target.value)}
                />
            </div>

            {direccionEntrega && (
                <div className="form-group">
                    <label htmlFor="metodoPago">Método de Pago:</label>
                    <select
                        id="metodoPago"
                        className="form-control"
                        value={metodoPago}
                        onChange={(e) => handleMetodoPagoChange(e.target.value)}
                    >
                        <option value="">Seleccione un método de pago</option>
                        <option value="sinpe">Sinpe</option>
                        <option value="efectivo">Efectivo</option>
                    </select>
                </div>
            )}

            {metodoPago === 'sinpe' && (
                <div>
                    <p>Número de Compra: {numeroCompra}</p>
                    <p>Número de Pago: {numeroPago}</p>
                    <div className="form-group">
                        <label htmlFor="comprobante">Comprobante:</label>
                        <input
                            type="text"
                            id="comprobante"
                            className="form-control"
                            value={comprobante}
                            onChange={(e) => handleComprobanteChange(e.target.value)}
                        />
                    </div>
                    <button className="btn btn-primary" onClick={handleSubmit}>
                        Confirmar Pago
                    </button>
                    <p>{confirmacion}</p>
                </div>
            )}
            {metodoPago === 'efectivo' && (
                <div>
                    <p>Número de Compra: {numeroCompra}</p>
                    <button className="btn btn-primary" onClick={handleSubmit}>
                        Confirmar Pago
                    </button>
                    <p>{confirmacion}</p>
                </div>
            )}
        </div>
    );
};

export default PayPage;
