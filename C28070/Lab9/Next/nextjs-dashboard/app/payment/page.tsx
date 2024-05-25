"use client";
import { useEffect, useState } from 'react';
import "bootstrap/dist/css/bootstrap.min.css";
import Link from 'next/link';

export default function Pago() {

    const [metodoSeleccionado, setMetodoSeleccionado] = useState('');

    const [pagoConfirmado, setPagoConfirmado] = useState(false);

    const [cartProducts, setCartProducts] = useState([]);

    const [address, setAddress] = useState('');

    const [purchaseNumberOficial, setpurchaseNumber] = useState('');

    const [confirmacionPendiente, setConfirmacionPendiente] = useState(false); 


    useEffect(() => {
        const storedCartString = localStorage.getItem('productosCarrito');
        if (storedCartString) {
            const storedCart = JSON.parse(storedCartString);
            console.log('Productos del carrito:', storedCart);
            const productIds = Object.keys(storedCart || {});
            setCartProducts(productIds);
        } else {
            setCartProducts([]);
        }
        const storedAddress = localStorage.getItem('direccion') || '';
         
        
        setAddress(storedAddress);
    
    }, []);

    useEffect(() => {
        localStorage.setItem('metodoSeleccionado', metodoSeleccionado);
    }, [metodoSeleccionado]);

    const seleccionMetodoPago = (metodo) => {
        setMetodoSeleccionado(metodo);
        setPagoConfirmado(false);
     
    };


    const manejarConfirmacionPago = async () => {
   
        try {
                 
        setConfirmacionPendiente(true);

          const paymentMethodValue = metodoSeleccionado === 'Sinpe' ? 1 : 0;
    
          const dataSend = {
            ProductIds: cartProducts,
            Address: address,
            PaymentMethod: paymentMethodValue

          };
    
          const response = await fetch('http://localhost:5133/api/Cart', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataSend)


          });
          
          if (!response.ok) {
            throw new Error('Fallo al confirmar la compra.');
          }
          else if (response.ok) {
           const purchaseNumber = await response.json();
           setpurchaseNumber(purchaseNumber.purchaseNumber);
        }
        } catch (error) {
        throw new Error('Error al confirmar la compra:', error.message);
        }
      };


    return (
        <div className="container d-flex justify-content-center align-items-center" style={{ minHeight: '100vh', backgroundColor: 'lightBlue' }}>
            <div className="card p-4" style={{ width: '50%' }}>
                <div className="card-body">
                    <div className="text-center mb-4">
                        <h1 className="h3">Seleccione el método de Pago</h1>
                    </div>
                    <div className="d-flex justify-content-center">
                        <button className="btn btn-primary me-3 btn-lg" onClick={() => seleccionMetodoPago('Efectivo')}>Efectivo</button>
                        <button className="btn btn-primary btn-lg" onClick={() => seleccionMetodoPago('Sinpe')}>Sinpe</button>
                    </div>
                    {metodoSeleccionado === 'Efectivo' && (
                        <div>
                            <p>Número de compra: {purchaseNumberOficial} .</p>
                            <p>Por favor espere, hasta que el administrador confirme su pago...</p>
                            <button className="btn btn-primary me-3 btn-lg" onClick={manejarConfirmacionPago} disabled={confirmacionPendiente}>Confirmar compra</button>
                        </div>
                    )}
                    {metodoSeleccionado === 'Sinpe' && (
                        <div>
                            <p>Número de compra: {purchaseNumberOficial} .</p>
                            <p>Realice el pago por medio de SinpeMovil al número 8655-8255.</p>
                            <input type="text" className="form-control mb-3" placeholder="Ingrese el código de recibo aquí" />
                            <button className="btn btn-primary me-3 btn-lg" onClick={manejarConfirmacionPago} disabled={confirmacionPendiente}>Confirmar compra</button>
                            {pagoConfirmado && <p>Por favor espere, hasta que el administrador confirme su pago...</p>}
                        </div>
                    )}
                </div>
            </div>
            <Link href="/">
                <button className="btn btn-light" style={{ backgroundColor: 'pink', color: 'black', marginRight: '10px' }}>Inicio</button>
            </Link>
        </div>
    );
}