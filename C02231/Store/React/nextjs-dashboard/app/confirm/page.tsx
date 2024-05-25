'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

export default function ConfirmPage() {

    const [showConfirmation, setShowConfirmation] = useState(false);
    const [isButtonDisabled, setIsButtonDisabled] = useState(false);
    const [orderNumber, setOrderNumber] = useState('No disponible');

    useEffect(() => {
        const cartDataString = localStorage.getItem('cartItem');
        if (cartDataString) {
            const cartData = JSON.parse(cartDataString);
            const numeroCompra = cartData.numeroCompra;
            setOrderNumber(numeroCompra || 'No disponible');
        }

    }, []);


    const handleConfirmation = async () => {
        localStorage.removeItem('cartItem');
        setShowConfirmation(true);
        setIsButtonDisabled(true);
    };

    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-9">
                        <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" />
                    </div>
                    <div className="col-sm-3 d-flex justify-content-end align-items-center">
                        <Link href="/">
                            <button className="btn btn-dark"> Go Home</button>
                        </Link>
                    </div>
                </div>
            </header>


            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                <div style={{ backgroundColor: 'white', padding: '20px', borderRadius: '10px', textAlign: 'center' }}>
                    <p style={{ fontSize: '20px' }}>Order Number: {orderNumber}</p>
                    <button
                        className="btn btn-success"
                        onClick={handleConfirmation}
                        disabled={isButtonDisabled}
                    >
                        Confirm Payment
                    </button>
                    {showConfirmation && <p style={{ marginTop: '10px', color: 'green', fontSize: '18px' }}>
                        Thank you for your purchase!</p>}
                </div>
            </div>

            <footer className='footer' style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light"> Paula's Library</h5>
                </div>
            </footer>


        </div>
    );
}
