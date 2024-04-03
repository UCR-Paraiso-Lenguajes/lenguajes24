"use client";
import { useEffect, useState } from 'react';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css";
import Payment from '../payment/page';


export default function Direccion() {
    const [direccion, setDireccion] = useState('');

    const manejarCambioDireccion = (e) => {
        setDireccion(e.target.value);
    };

    const manejarGuardarDireccion = () => {
        if (direccion.trim() !== '') {
            localStorage.setItem('direccion', direccion);
        }
    };

    useEffect(() => {
        const direccionAlmacenada = localStorage.getItem('direccion');
        if (direccionAlmacenada) {
            setDireccion(direccionAlmacenada);
        }
    }, []);

    const direccionVacia = direccion.trim() === '';

    return (
        <div className="container d-flex justify-content-center align-items-center" style={{ minHeight: '100vh', backgroundColor: 'pink' }}>
            <div className="card p-4">
                <div className="card-body">
                    <div className="text-center mb-4">
                        <h1 className="h3">Agregue su dirección</h1>
                    </div>
                    <form>
                        <div className="mb-3">
                            <label htmlFor="direccion" className="form-label"></label>
                            <input
                                type="text"
                                className="form-control"
                                id="direccion"
                                name="direccion"
                                value={direccion}
                                onChange={manejarCambioDireccion} />
                        </div>
                        <div className="text-center">
                            <Link href="/payment">
                                <button className="btn btn-primary" onClick={manejarGuardarDireccion} disabled={direccionVacia}>Confirmar dirección</button>
                            </Link>

                            <Link href="/">
                                <button className="Boton">Inicio</button>
                            </Link>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}