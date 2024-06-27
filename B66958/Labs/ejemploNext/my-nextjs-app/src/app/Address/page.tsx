'use client'

import { useState, useEffect, ChangeEvent } from "react";
import PaymentForm from "../Payment/page";
import { CartState } from "../types/Cart";

const AddressForm = ({ handleAddressForm, cart, setCart, clearProducts }:
    {
        handleAddressForm: () => void, cart: CartState,
        setCart: (cart: CartState) => void, clearProducts: () => void
    }) => {
    const [showPaymentForm, setShowPaymentForm] = useState(false);

    const handleAddressChange = (event: ChangeEvent<HTMLInputElement>) => {
        const inputValue = event.target.value;
        const updatedCart: CartState = {
            ...cart,
            carrito: {
              ...cart.carrito,
              direccionEntrega: inputValue
            }
          };
        
          setCart(updatedCart);
    }

    function handlePaymentChange(show: boolean) {
        setShowPaymentForm(show);
    }

    return showPaymentForm ? <PaymentForm cart={cart} setCart={setCart} clearProducts={clearProducts} /> :
        <div className="d-flex justify-content-center gap-2">
            <div className="card w-25">
                <div className="card-body">
                    <div className="d-flex w-100 justify-content-center">
                        <div className="form-group">
                            <label htmlFor="exampleFormControlInput1">Dirección:</label>
                            <input type="text" className="form-control"
                                id="exampleFormControlInput1" placeholder="Ingrese su dirección"
                                value={cart?.carrito?.direccionEntrega || ''} onChange={handleAddressChange} />
                            <div className="d-flex w-100 justify-content-center">
                                <a className="btn btn-primary mr-2" onClick={() => handleAddressForm()}>Atrás</a>
                                <button className="btn btn-primary" disabled={(cart?.carrito ? !cart.carrito.direccionEntrega : true)}
                                    onClick={() => handlePaymentChange(true)}>Continuar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
}

export default AddressForm;