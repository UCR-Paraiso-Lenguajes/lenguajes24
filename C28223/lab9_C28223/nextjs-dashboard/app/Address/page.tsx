import React, { useState } from 'react';
import MetodoPago from "@/app/Pagos/page";

const AddAddress = () => {
    const [showMethodPay, setShowMethodPay] = useState(false);
    const tiendaEnMemoria = JSON.parse(localStorage.getItem('tienda'));
    let direccionDelEnvio = '';

    const enviarForm = (eventoDeEnvio) => {
        eventoDeEnvio.preventDefault();
        const updatedCart = {
            ...tiendaEnMemoria,
            carrito: {
                ...tiendaEnMemoria.carrito,
                direccionEntrega: direccionDelEnvio
            }
        };
        localStorage.setItem("tienda", JSON.stringify(updatedCart));
        setShowMethodPay(true);
    };

    return (
        showMethodPay ? <MetodoPago />:
            <div className="p-pago">
                <div className="data">
                    <h1>Agregar Dirección</h1>
                    <form onSubmit={enviarForm}>
                        <div className="form-group">
                            <label htmlFor="direccion">Dirección exacta:</label>
                            <input type="text" className="form-control" id="direccion" placeholder="Ingrese su dirección exacta" onChange={(e) => (direccionDelEnvio = e.target.value)} minLength={5}required />
                        </div>
                        <button type="submit" className="btn btn-primary">
                            Continuar
                        </button>
                    </form>
                </div>
            </div>
    );
};

export default AddAddress;
