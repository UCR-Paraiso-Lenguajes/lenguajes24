'use client'
import { useEffect, useState } from "react";
import Alert from 'react-bootstrap/Alert';
import { CartState } from "../types/Cart";

const PaymentForm = ({ cart, setCart, clearProducts }:
    { cart: CartState, setCart: (cart: CartState) => void, clearProducts: () => void }) => {

    const [isMessageShowing, setIsMessageShowing] = useState(false);
    const [message, setMessage] = useState('');
    const [alertType, setAlertType] = useState(0);
    const [orderNumber, setOrderNumber] = useState('');
    const [selectedIndex, setSelectedIndex] = useState(0);
    const [finishedSale, setFinishedSale] = useState(false);
    const [confirmationNumber, setConfirmationNumber] = useState('');
    const isConfirmationNumberValid = confirmationNumber.trim() !== '';

    enum PaymentMethodEnum {
        EFECTIVO = 0,
        SINPE = 1
    }

    let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV;

    useEffect(() => {
        if (cart?.carrito) {
          const updatedCart: CartState = {
            ...cart,
            carrito: {
              ...cart.carrito,
              metodoDePago: cart.carrito.metodoDePago ?? PaymentMethodEnum.EFECTIVO,
            },
          };
          setCart(updatedCart);
          setSelectedIndex(cart.carrito.metodoDePago === PaymentMethodEnum.SINPE ? 1 : 0);
        }
    }, []);      

    function handleConfirmationNumber(event) {
        setConfirmationNumber(event.target.value);
    }

    function handleSelectPayment(event: any) {
        const selectedPaymentMethod = parseInt(event.target.value, 10);
        setSelectedIndex(event.target.selectedIndex);
        const updatedCart: CartState = {
          ...cart,
          carrito: {
            ...cart.carrito,
            metodoDePago: selectedPaymentMethod,
          },
          necesitaVerificacion: true,
        };
        
        setCart(updatedCart);
    }      

    async function persistPurchase() {

        let purchaseToPersist = {
            "productIds": cart.carrito.productos.map(product => ({
                id: product.uuid,
                quantity: product.quantity
            })),
            "address": cart.carrito.direccionEntrega,
            "paymentMethod": cart.carrito.metodoDePago,
            "confirmationNumber": confirmationNumber
        }

        try {
            const res = await fetch(`${environmentUrl}/api/cart`, {
                method: 'POST',
                body: JSON.stringify(purchaseToPersist),
                headers: {
                    'content-type': 'application/json'
                }
            })
            if (res.ok) {
                var order = await res.json();
                setOrderNumber(order.purchaseNumber);
                setMessage("Se realizó la compra");
                setAlertType(0);
                return true;
            }
            else { setMessage("Error al realizar la compra"); setAlertType(1); return false;}
        } catch (error) {
            setMessage(String(error));
            setAlertType(1);
            return false;
        } finally {
            setIsMessageShowing(true);
        }
    }

    const finishPurchase = async () => {
        var result = await persistPurchase();
        if(result){
            setFinishedSale(true);
            setConfirmationNumber('');
            clearProducts();
        }
    }

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
                    <select
                        className="form-control"
                        onChange={handleSelectPayment}
                        value={cart?.carrito?.metodoDePago ?? ''}
                        disabled={finishedSale}
                    >
                        {cart?.metodosDePago
                            ?.filter(method => method.isEnabled)
                            ?.sort((a, b) => a.paymentType - b.paymentType)
                            .map((method, index) => (
                                <option key={index} value={method.paymentType}>
                                    {PaymentMethodEnum[method.paymentType]}
                                </option>
                            ))}
                    </select>
                    {selectedIndex === PaymentMethodEnum.SINPE ? <>
                        <div className="container"><label>Número para realizar el pago: +506 8888 8888</label></div>
                        <label htmlFor="confirmationText">Número de confirmación:</label>
                        <input type="number" className="form-control" id="confirmationText"
                            placeholder="Ingrese el número de confirmación"
                            value={confirmationNumber}
                            disabled={finishedSale}
                            onChange={handleConfirmationNumber} />
                    </> : ''}
                </div>
                <div className="d-flex w-100 justify-content-center">
                    <button className="btn btn-primary"
                        disabled={
                            (selectedIndex === PaymentMethodEnum.SINPE) ?
                                (!isConfirmationNumberValid && selectedIndex === PaymentMethodEnum.SINPE) :
                                finishedSale
                        }
                        onClick={finishPurchase}>
                        Finalizar Compra</button>
                </div>
            </div>
            {finishedSale ? (selectedIndex === 0 ? <Sinpe /> : <Efectivo />) : ''}
            {finishedSale ? <div className="progress">
                <div className="progress-bar progress-bar-striped progress-bar-animated"
                    role="progressbar" aria-valuenow={75} aria-valuemin={0}
                    aria-valuemax={100} style={{ width: '100%' }}></div>
            </div> : ''}
            {isMessageShowing ?
                <div
                    style={{
                        position: 'fixed',
                        bottom: 20,
                        right: 20,
                        zIndex: 9999,
                    }}
                >
                    <Alert variant={alertType === 0 ? "success" : "danger"}
                        onClose={() => setIsMessageShowing(false)} dismissible>
                        <Alert.Heading>{alertType === 0 ? 'Información' : 'Error'}</Alert.Heading>
                        <p>{message.toString()}</p>
                    </Alert> </div> : ''
            }
        </div>
    </div>
}

export default PaymentForm;