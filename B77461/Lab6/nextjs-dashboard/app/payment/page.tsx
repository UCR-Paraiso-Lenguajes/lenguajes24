'use client'

import { useEffect, useState } from "react";
import Alert from 'react-bootstrap/Alert';
import AddressForm from "../address/page";

const PaymentForm = ({ cart, setCart }:
    { cart: any, setCart: (cart: any) => void }) => {

    const [MessageShowing, setMessageShowing] = useState(false);
    const [message, setMessage] = useState('');
    const [alertType, setAlertType] = useState(0);
    const [orderNumber, setOrderNumber] = useState('');
    const [selectedIndex, setSelectedIndex] = useState(0);
    const [finishedSale, setFinishedSale] = useState(false);

    const [showAddressForm, setShowAddressForm] = useState(false);

    function handleAddressForm() {
        setShowAddressForm(!showAddressForm);
    }

    enum PaymentMethod {
        CASH = 0,
        SINPE = 1
    }

    useEffect(() => {
        setCart(cart => ({
            ...cart,
            carrito: {
                ...cart.carrito,
                metodoDePago: cart.carrito.metodoPago ? cart.carrito.metodoDePago : PaymentMethod.CASH
                
            }
        }));
        setSelectedIndex((cart.carrito.metodoDePago === PaymentMethod.CASH) ? 0 : 1);
    }, []);

    function handleSelectPayment(event: any) {
        setSelectedIndex(event.target.selectedIndex);
        setCart(cart => ({
            ...cart,
            carrito: {
                ...cart.carrito,
                metodoDePago: event.target.selectedIndex === 0 ? PaymentMethod.CASH : PaymentMethod.SINPE
            },
            verificacion: true
        }));
    }

    function generateReceiptNumber() {
        const timestamp = Date.now().toString();
        const randomNumber = Math.floor(Math.random() * 10000).toString().padStart(4, '0');
        setOrderNumber(timestamp + randomNumber);
    }


    async function finishPurchase() {
        generateReceiptNumber();
        setFinishedSale(true);
        await persistPurchase();
    }

    async function persistPurchase() {

        let purchaseToPersist = {
            "productIds": cart.carrito.productos.map(product => product.uuid),
            "address": cart.carrito.direccionEntrega,
            "paymentMethod": cart.carrito.metodoDePago
        }

        try {
            const res = await fetch('https://localhost:7075/api/Cart ', {
                method: 'POST',
                body: JSON.stringify(purchaseToPersist),
                headers: {
                    'content-type': 'application/json'
                }
            })
            if (res.ok) { setMessage("Compra Exitosa"); setAlertType(0) }
            else { setMessage("Error en la compra"); setAlertType(1) }
        } catch (error) {
            setMessage(error);
            setAlertType(1)
        } finally {
            setMessageShowing(true);
        }
    }

    const Cash = () => {
        return <div className="card effect-card w-75">
            <div className="card-body">
                <div className="d-grid w-100 justify-content-center">
                    <strong>Número de compra: {orderNumber}</strong>
                    <p className=""></p>
                    <em>Por favor, espere la confirmación de pago del admin</em>
                </div>
            </div>
        </div>
    }

    const Sinpe = () => {

        const [comprobante, setVoucher] = useState('');
    
        const handleVoucherChange = (event) => {
            setVoucher(event.target.value);
        };

        return  <div className="card sinpe-card w-75">
            <div className="card-body">
                <div className="d-grid w-100 justify-content-center">
                <strong>Número de compra: {orderNumber}</strong>
                        <strong>Número para realizar el pago: +506 6270 6880</strong>
                        <input
                            type="text"
                            className="form-control mt-3"
                            placeholder="Indique el comprobante"
                            value={comprobante}
                            onChange={handleVoucherChange}
                            
                        />
                        <p className=""></p>
                        <em>Por favor, espere la confirmación del admin</em>
                </div>
            </div>
        </div>
    }

    return (
      <div className="container">
          {showAddressForm ? (
              <AddressForm handleAddressForm={handleAddressForm} cart={cart} setCart={setCart} />
          ) : (
              <div className="d-grid justify-content-center gap-4">
                  <div className="container">
                      <h3>Métodos de pago</h3>
                      <div className="form-group">
                          <select className="form-control" onChange={handleSelectPayment} value={cart.carrito.metodoDePago} disabled={finishedSale}>
                              {cart.metodosDePago.map((method: any, index: number) => <option key={index}>{method}</option>)}
                          </select>
                      </div>
                      <div className="d-flex w-100 justify-content-center">
                          <button className="btn btn-primary" disabled={finishedSale} onClick={finishPurchase}>
                              Finalizar Compra
                          </button>
                      </div>
                  </div>
  
                  <div className="d-flex w-100 justify-content-center">
                      <button type="button" className="btn btn-primary mr-2" >
                          Atrás
                      </button>
                  </div>
  
                  {finishedSale ? (selectedIndex === 0 ? <Cash /> : <Sinpe />) : ''}
                  {MessageShowing ? (
                      <div
                          style={{
                              position: 'fixed',
                              bottom: 20,
                              right: 20,
                              zIndex: 9999,
                          }}
                      >
                          <Alert variant={alertType === 0 ? 'success' : 'danger'} onClose={() => setMessageShowing(false)} dismissible>
                              <Alert.Heading>{alertType === 0 ? 'Información' : 'Error'}</Alert.Heading>
                              <p>{message.toString()}</p>
                          </Alert>
                      </div>
                  ) : (
                      ''
                  )}
              </div>
          )}
      </div>
  );
  
      
}

export default PaymentForm;