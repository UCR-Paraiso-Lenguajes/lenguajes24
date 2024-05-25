'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

export default function PaymentPage() {
    const [selectMethod, setSelectMethod] = useState('');
    const cartDataString = localStorage.getItem('cartItem');
    const cartData = cartDataString ? JSON.parse(cartDataString) : {};
    const orderNumber = cartData.numeroCompra || 'No disponible';
    const [PaymenthConfirmed, setPaymenthConfirmed] = useState(false);

    const [cart, setCart] = useState({
        products: [],
        deliveryAddress: '',
        paymentMethod: '',
        receipt: '',
        confirmation: '',
        total: '',
        isCartEmpty: true
    });


    useEffect(() => {
        let cartItemStored = localStorage.getItem('cartItem');
        if (cartItemStored !== null) {
            let cartData = JSON.parse(cartItemStored);
            const validProducts = cartItemStored && Array.isArray(cartData.products) && cartData.products.length > 0;
            if (validProducts) {
                setCart({ ...cartData, isCartEmpty: false });
            } else {
                localStorage.removeItem('cartItem');
                setCart({ ...cart, isCartEmpty: true });
            }
        } else {
            setCart({ ...cart, isCartEmpty: true });
        }
    }, []);


    const handleDeliveryAddress = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, deliveryAddress: value }));
        updateLocalStorage({ ...cart, deliveryAddress: value });
    };

    const handlePaymentMethodChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, paymentMethod: value }));
        updateLocalStorage({ ...cart, paymentMethod: value });
    };

    const updateLocalStorage = (updatedCart: any) => {
        localStorage.setItem('cartItem', JSON.stringify(updatedCart));
    };


    const handleReceiptChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, receipt: value }));
        updateLocalStorage({ ...cart, receipt: value });
        setPaymenthConfirmed(true);
    };

    const handleSubmit = async () => {
        const { deliveryAddress, paymentMethod, products, total } = cart;
        const validOrder = deliveryAddress && paymentMethod && (paymentMethod === 'cash' || paymentMethod === 'sinpe');
        if (validOrder) {
            const productIds = products.map((producto: any) => String(producto.id));
            let paymentMethodValue = 0;
            if (paymentMethod === 'sinpe') {
                paymentMethodValue = 1;
            }

            const dataToSend = {
                productIds: productIds,
                address: deliveryAddress,
                paymentMethod: paymentMethodValue,
                total: total
            };
            try {
                //const response = await fetch('http://localhost:5000/api/Cart', {
                const response = await fetch('http://localhost:5207/api/Cart', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(dataToSend)
                });

                if (response.ok) {
                    // debugger
                    const responseData = await response.json();
                    const { purchaseNumber } = responseData;
                    const orderNumber = purchaseNumber;

                    updateLocalStorage({
                        products: [],
                        deliveryAddress: '',
                        paymentMethod: '',
                        isCartEmpty: true,
                        numeroCompra: orderNumber 
                    });
                    window.location.href = "/confirm";
                } else {
                    const errorResponseData = await response.json();
                    throw new Error('Error to send data: ' + JSON.stringify(errorResponseData));
                }
            } catch (error) {
                throw error;
            }
        } else {
            setCart(prevCart => ({ ...prevCart, confirmation: 'Please complete all required fields or add items to the cart.' }));
        }
    };

    
    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-9">
                        <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" />
                    </div>

                    <div className="col-sm-3 d-flex justify-content-end align-items-center">
                        <Link href="/cart">
                            <button className="btn btn-dark"> Go Cart</button>
                        </Link>
                        <Link href="/">
                            <button className="btn btn-dark"> Go Home</button>
                        </Link>
                    </div>

                </div>
            </header>

            <div className='container'>
                <h2>Payment Page</h2>
                <div className="form-group">
                    <label htmlFor="deliveryAddress">Delivery Address:</label>
                    <input
                        type="text"
                        id="address"
                        className="form-control"
                        value={cart.deliveryAddress}
                        onChange={handleDeliveryAddress}
                    />
                </div>

                {cart.deliveryAddress && (
                    <div className="form-group">
                        <label htmlFor="paymentMethod">Payment method:</label>
                        <select
                            id="paymentMethod"
                            className="form-control"
                            value={cart.paymentMethod}
                            onChange={handlePaymentMethodChange}
                        >
                            <option value="">Select a payment method:</option>
                            <option value="sinpe">Sinpe</option>
                            <option value="cash">Cash</option>
                        </select>
                    </div>
                )}

                {cart.paymentMethod === 'cash' && (
                    <div>
                        <div >
                      
                        <button className="btn btn-success" style={{margin: '1px'}} onClick={handleSubmit} >
                            Confirm Payment
                        </button>
                     
                        </div>
                        <p>{cart.confirmation}</p>
                        
                    </div>
                )}

                {cart.paymentMethod === 'sinpe' && (
                    <div>
                        <p>Make payment for the purchase at the following Sinpe Number: +506 86920997</p>
                        <div className="form-group">
                            <label htmlFor="comprobante">Receipt:</label>
                            <input
                                type="text"
                                id="receipt"
                                className="form-control"
                                value={cart.receipt}
                                onChange={handleReceiptChange}
                            />
                        </div>
                        <div>
                            {cart && cart.receipt && cart.receipt.length > 0 ? (
                                <button className="btn btn-success" onClick={handleSubmit}  >
                                    Confirm Payment
                                </button>                            
                            ) : (
                                <button className="btn btn-success" disabled >Confirm Payment</button>
                            )}
                        </div>
                        <p>{cart.confirmation}</p>
                    </div>
                )}
            </div>


            <footer className='footer' style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light"> Paula's Library</h5>
                </div>
            </footer>
        </div>
    );
};
