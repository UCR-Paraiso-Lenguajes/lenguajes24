'use client'

import { useEffect, useState } from "react";

const PaymentForm = ({ cart, setCart }: { cart: any, setCart: (cart: any) => void }) => {
    const [orderNumber, setOrderNumber] = useState('');
    const [selectedIndex, setSelectedIndex] = useState(0);

    enum PaymentMethod {
        EFECTIVO = 'Efectivo',
        SINPE = 'Sinpe'
    }

    useEffect(() => {
        setCart(cart => ({
            ...cart,
            carrito: {
                ...cart.carrito,
                metodoDePago: cart.carrito.metodoDePago ? cart.carrito.metodoDePago : PaymentMethod.EFECTIVO
            }
        }));
        setSelectedIndex((cart.carrito.metodoDePago === PaymentMethod.EFECTIVO) ? 0 : 1);
    }, []);

    function handleSelectPayment(event: any) {
        setSelectedIndex(event.target.selectedIndex);
        setCart(cart => ({
            ...cart,
            carrito: {
                ...cart.carrito,
                metodoDePago: event.target.selectedIndex === 0 ? PaymentMethod.EFECTIVO : PaymentMethod.SINPE
            },
            necesitaVerificacion: true
        }));
    }

    function generateReceiptNumber() {
        const timestamp = Date.now().toString();
        const randomNumber = Math.floor(Math.random() * 10000).toString().padStart(4, '0');
        setOrderNumber(timestamp + randomNumber);
    }

    useEffect(() => {
        generateReceiptNumber();
    }, [])

    const Efectivo = () => {
        return <div className="card w-100">
            <div className="card-body">
                <div className="d-grid w-100 justify-content-center">
                    <label>Número de compra: {orderNumber}</label>
                    <label>Por favor espere la confirmación de pago por parte del administrador</label>
                </div>
            </div>
        </div>
    }

    const Sinpe = () => {
        return <div className="card w-100">
            <div className="card-body">
                <div className="d-grid w-100 justify-content-center">
                    <label>Número de compra: {orderNumber}</label>
                    <label>Número para realizar el pago: +506 8888 8888</label>
                    <label>Por favor espere la confirmación de pago por parte del administrador</label>
                </div>
            </div>
        </div>
    }

    return <div className="container">
        <div className="d-grid justify-content-center gap-4">
            <div className="container">
                <h3>Métodos de pago</h3>
                <div className="form-group">
                    <select className="form-control" onChange={handleSelectPayment} value={cart.carrito.metodoDePago}>
                        {cart.metodosDePago.map((method: any, index: number) => <option key={index}>{method}</option>)}
                    </select>
                </div>
            </div>
            {(selectedIndex === 0 ? <Efectivo /> : <Sinpe />)}
            <div className="progress">
                <div className="progress-bar progress-bar-striped progress-bar-animated"
                    role="progressbar" aria-valuenow={75} aria-valuemin={0} aria-valuemax={100} style={{ width: '100%' }}></div>
            </div>
        </div>
    </div>
}

export default PaymentForm;