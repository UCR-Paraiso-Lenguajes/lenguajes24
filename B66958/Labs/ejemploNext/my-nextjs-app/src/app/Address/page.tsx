'use client'

import { useState, useEffect, ChangeEvent, useRef } from "react";
import PaymentForm from "../Payment/page";
import { CartState } from "../types/Cart";
import AddressModal from "./addressSuggest";
import { validateAddressFormat } from "../utils/AddressValidation";
import { Alert } from "react-bootstrap";

const AddressForm = ({ handleAddressForm, cart, setCart, clearProducts }:
    {
        handleAddressForm: () => void, cart: CartState,
        setCart: (cart: CartState) => void, clearProducts: () => void
    }) => {
    const [showPaymentForm, setShowPaymentForm] = useState(false);
    const [inputValue, setInputValue] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [message, setMessage] = useState('');
    const [alertType, setAlertType] = useState(0);

    const handleAddressSelect = (address: string) => {
        if(validateAddressFormat(address)){
            setInputValue(address);
            setIsModalOpen(false);
        }else{
            setMessage("La dirección seleccionada debe indicar la ciudad, el cantón, la provincia y el código postal");
            setAlertType(1);
        }
    };

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

    return showPaymentForm ? (
      <PaymentForm
        cart={cart}
        setCart={setCart}
        clearProducts={clearProducts}
      />
    ) : (
      <>
      <div className="d-flex justify-content-center gap-2">
        <div className="card w-50">
          <div className="card-body">
            <div className="d-flex w-100 justify-content-center">
              <div className="form-group w-100">
                <label htmlFor="exampleFormControlInput1">Dirección:</label>
                <input
                  type="text"
                  className="form-control"
                  value={inputValue}
                  placeholder="Ingrese su dirección"
                  onClick={() => setIsModalOpen(true)}
                  readOnly
                  required
                />
                {isModalOpen && <AddressModal onSelect={handleAddressSelect} />}
                <input
                  type="text"
                  className="form-control w-100"
                  id="exampleFormControlInput1"
                  placeholder="Ingrese indicaciones exactas"
                  value={cart?.carrito?.direccionEntrega || ""}
                  onChange={handleAddressChange}
                />
                <div className="d-flex w-100 justify-content-center">
                  <a
                    className="btn btn-primary mr-2"
                    onClick={() => handleAddressForm()}
                  >
                    Atrás
                  </a>
                  <button
                    className="btn btn-primary"
                    disabled={
                      cart?.carrito ? (cart.carrito.direccionEntrega.length < 5) : true
                    }
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
      {message ?
        <div
            style={{
                position: 'fixed',
                bottom: 20,
                right: 20,
                zIndex: 9999,
            }}
        >
            <Alert variant={alertType === 0 ? "success" : "danger"}
                onClose={() => setMessage('')} dismissible>
                <Alert.Heading>{alertType === 0 ? 'Información' : 'Error'}</Alert.Heading>
                <p>{message.toString()}</p>
            </Alert> </div> : ''
    }
    </>
    );
}

export default AddressForm;