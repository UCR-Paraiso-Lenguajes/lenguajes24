'use client'

import { useState, useEffect } from "react";
import PaymentForm from "../payment/page";

const AddressForm = ({ handleAddressForm, cart, setCart }:
    {
        handleAddressForm: () => void, cart: any,
        setCart: (cart: any) => void}) => {
    const [showPaymentForm, setShowPaymentForm] = useState(false);

    function handleAddressChange(event: any) {
        const inputValue = event.target.value;
        setCart(cart => ({
            ...cart,
            carrito: {
                ...cart.carrito,
                direccionEntrega: inputValue
            }
        }));
    }

    function handlePaymentChange(show: boolean) {
       
        setShowPaymentForm(show);
    }

    return showPaymentForm ? (
        <PaymentForm cart={cart} setCart={setCart} />
    ) : (
        <div className="d-flex justify-content-center gap-2">
            <div className="card w-25">
                <div className="card-body">
                    <div className="d-flex w-100 justify-content-center">
                        <div className="form-group">
                            <label htmlFor="exampleFormControlInput1">Dirección:</label>
                            <input
                                type="text"
                                className="form-control"
                                id="exampleFormControlInput1"
                                placeholder="Ingrese su dirección"
                                value={cart.carrito.direccionEntrega}
                                onChange={handleAddressChange}
                            />
                            <div className="d-flex w-100 justify-content-center">
                                <a className="btn btn-primary mr-2" onClick={() => handleAddressForm()}>
                                    Atrás
                                </a>
                                <button
                                    className="btn btn-primary"
                                    disabled={!cart.carrito.direccionEntrega}
                                    onClick={() => handlePaymentChange(true)}
                                >
                                    Continuar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
    
}

export default AddressForm;