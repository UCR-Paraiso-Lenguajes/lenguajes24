'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

export default function PaymentPage() {

    const [selectMethod, setSelectMethod] = useState('');
    const [PaymenthConfirmed, setPaymenthConfirmed] = useState(false);
    const [cartProducts, setCartProducts] = useState([]);
    const [address, setAdress] = useState('');

    const [cart, setCart] = useState({
        products: [],
        subtotal: 0,
        deliveryAddress: '',
        paymentMethod: '',
        receipt: '',
        confirmation: '',
        orderNumber: 0,
        isCartEmpty: true
    });


    useEffect(() => {
        let cartItemStored = JSON.parse(localStorage.getItem('cartItem')) || { products: {} };
        const productIds = cartItemStored && cartItemStored.products ? Object.keys(cartItemStored.products) : [];
        setCartProducts(productIds);

        setCart(prevCart => ({
            ...prevCart,
            orderNumber: Math.floor((Math.random() * 15000) + 1)
        }));
    }, []);

    useEffect(() => {
        localStorage.setItem('selectedMethod', selectMethod);
    }, [selectMethod]);

    /*   useEffect(() => {
           let cartItemStored = localStorage.getItem('cartItem');
           if (cartItemStored != null) {
               const storedCart = JSON.parse(cartItemStored);
               const productIds = Object.keys(storedCart || {});
               setCartProducts(productIds);
           } else {
               setCartProducts([]);
           }
           setCart(prevCart => ({
               ...prevCart,
               orderNumber: Math.floor((Math.random() * 15000) + 1)
           }));
       }, []); */



    const handleDeliveryAddress = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, deliveryAddress: value }));
        updateLocalStorage({ ...cart, deliveryAddress: value });
    };

    const handlePaymentMethodChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, paymentMethod: value }));
        updateLocalStorage({ ...cart, paymentMethod: value });
        setSelectMethod(value);
        setPaymenthConfirmed(false);
    };

    const handleReceiptChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, receipt: value }));
        updateLocalStorage({ ...cart, receipt: value });
        setPaymenthConfirmed(true);
    };

    const updateLocalStorage = (updatedValues: any) => {
        let cartItemString = localStorage.getItem('cartItem');
        let cartItem = cartItemString ? JSON.parse(cartItemString) : {};
        let updatedCartItem = { ...cartItem, ...updatedValues };

        localStorage.setItem('cartItem', JSON.stringify(updatedCartItem));
    };

    const handleSubmit = () => {
        const allFieldsCompleted = (cart.deliveryAddress) && cart.paymentMethod &&
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

    const handleConfirmation = () => {
        const send = async () => {

            let paymentMethodValue = 0;
            if (selectMethod == 'Sinpe') {
                paymentMethodValue = 1;
            }

            const dataSend = {
                ProductsIds: cartProducts,
                Addres: address,
                PaymentMethod: paymentMethodValue
            };

            try {
                const response = await fetch('http://localhost:5207/api/Cart', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(dataSend)
                });

                if (response.ok) {
                    //console.log('Data Sended');
                } else {
                    const errorResponseData = await response.json();
                    throw new Error(errorResponseData.message);
                }
            } catch (error) {
                //   console.error(error);
            }
        }

        send();
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

                {selectMethod === 'cash' && (
                    <div>
                        <p>Order Num: {cart.orderNumber}</p>
                        <button className="btn btn-success" onClick={handleSubmit} >
                            Confirm Payment
                        </button>
                        <p>{cart.confirmation}</p>
                    </div>
                )}

                {selectMethod === 'sinpe' && (
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
