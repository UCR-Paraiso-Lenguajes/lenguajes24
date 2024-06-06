'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

export default function PaymentPage() {

    const [cart, setCart] = useState({
        products: [],
        subtotal: 0,
        taxes: 0.13,
        total: 0,
        count: 0,
        deliveryAddress: '',
        paymentMethod: '',
        receipt: '',
        confirmation: '',
        orderNumber: 0,
        isCartEmpty: true
    });

    

    useEffect(() => {
        let cartItemString = localStorage.getItem('cartItem');
        if (cartItemString !== null) {
                setCart(prevCart => ({ ...prevCart, isCartEmpty: false }));
            }else {
            setCart(prevCart => ({ ...prevCart, isCartEmpty: true }));
        }
        setCart(prevCart => ({
            ...prevCart,
            orderNumber: Math.floor((Math.random() * 15000)+1)
        }));
    }, []);



    const handleDeliveryAddress =(e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, deliveryAddress: value }));
        updateLocalStorage({ ...cart, deliveryAddress: value });
    };

    const handlePaymentMethodChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, paymentMethod: value }));
        updateLocalStorage({ ...cart, paymentMethod: value });
    };
    
    const handleReceiptChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, receipt: value }));
        updateLocalStorage({ ...cart, receipt: value });
    };
    
    const updateLocalStorage = (updatedValues: any) => {
        let cartItemString = localStorage.getItem('cartItem');
        let cartItem = cartItemString ? JSON.parse(cartItemString) : {};
        let updatedCartItem = { ...cartItem, ...updatedValues };
    
        localStorage.setItem('cartItem', JSON.stringify(updatedCartItem));
    };
    
    const handleSubmit = () => {
        const allFieldsCompleted = (cart.deliveryAddress )&& cart.paymentMethod &&
            (cart.paymentMethod === 'cash' || (cart.paymentMethod === 'sinpe' && cart.receipt));
        if (allFieldsCompleted) {
            const updatedCart = {
                ...cart,
                confirmation: 'Waiting for administrator confirmation',
            };
            setCart(updatedCart);
            localStorage.setItem('cartItem', '');
            setCart(prevCart => ({
                ...prevCart,
                receipt: '',
                deliveryAddress: '',
            }));
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
                        <p>Order Num: {cart.orderNumber}</p>
                        <button className="btn btn-success" onClick={handleSubmit} >
                            Confirm Payment
                        </button>
                        <p>{cart.confirmation}</p>
                    </div>
                )}

                {cart.paymentMethod === 'sinpe' && (
                    <div>
                        <p>Order Number: {cart.orderNumber}</p>
                        <p>Sinpe Number: +506 86920997</p>
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
                        {cart && cart.receipt && cart.receipt.length > 0 ? (
                            <button className="btn btn-success" onClick={handleSubmit} >
                                Confirm Payment
                            </button>
                        ) : (
                            <button className="btn btn-success" disabled >Confirm Payment</button>
                        )}
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
