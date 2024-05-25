'use client';
import Link from "next/link";
import { useEffect, useState } from "react";
import { Container, FloatingLabel, Form, Col, Button, Card } from "react-bootstrap";
import 'bootstrap/dist/css/bootstrap.min.css';

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min) + min);
}

export default function Page() {
    const [cart, setCartState] = useState(JSON.parse(localStorage.getItem('Cart')));

    const [validated, setValidated] = useState(false);
    const [empty, setEmpty] = useState(true);
    const [numComprobante, setNumComprobante] = useState('')
    const [purchaseNumber, setPurchaseNumber] = useState('');
    const [hideComponents, setHideComponents] = useState({ hideSinpe: true, hideConfirmation: true, disablePurchase: false });

    const cartSent = {
        productsIds: [],
        subtotal: 0,
        address: "",
        paymentMethod: 0
    };

    // useEffect(() => {
    //     let hideComponentsCopy = { ...hideComponents };
    //     hideComponentsCopy.hideConfirmation = false;
    //     setHideComponents(hideComponentsCopy);
    // }, [purchaseNumber]);

    const handleSubmit = async (event) => {
        let hideComponentsCopy = { ...hideComponents };
        setValidated(true);

        const productIds = cart.products.map((producto) => String(producto.id));
        // debugger

        cartSent.productsIds = productIds;
        cartSent.subtotal = cart.subtotal;
        cartSent.address = cart.address;
        cartSent.paymentMethod = cart.paymentMethod;

        try {
            const response = await fetch('https://localhost:7194/api/Cart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(cartSent)
            });
            if (response.ok) {
                const responsePurchaseNum = await response.json();
                setPurchaseNumber(responsePurchaseNum.purchaseNumber)
                const emptyCart = {
                    products: [],
                    subtotal: 0,
                    address: '',
                    paymentMethod: 0,
                }
                setCartState(emptyCart)
                localStorage.setItem('Cart', JSON.stringify(emptyCart));

                hideComponentsCopy.hideConfirmation = false;
                hideComponentsCopy.disablePurchase = true;
                setEmpty(true)
                // console.log(responsePurchaseNum.purchaseNumber)
            }
        } catch (error) {
            console.error('Error al enviar datos:', error);
        }

        setHideComponents(hideComponentsCopy);
    };

    const handleChange = (event) => {
        const addressFormData = event.currentTarget;
        if (addressFormData.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
        }

        setValidated(true);
        setNumComprobante(event.target.value);
        // console.log(address)
        setEmpty(false)
    }

    const handleChangeSelect = (event) => {
        const slctPaymentMethodValue = event.target.value;
        let hideComponentsCopy = { ...hideComponents };
        let paymentMethod = 0
        // console.log(slctPaymentMethodValue);
        if (slctPaymentMethodValue === '0') { //Efectivo
            // hideComponentsCopy.hideConfirmation = false;
            hideComponentsCopy.hideSinpe = true;
            paymentMethod = 0
            setEmpty(false)
        } else if (slctPaymentMethodValue === '1') { //sinpe
            hideComponentsCopy.hideConfirmation = true;
            hideComponentsCopy.hideSinpe = false;
            paymentMethod = 1
            setEmpty(true)
        } else { //default
            hideComponentsCopy.hideConfirmation = true;
            hideComponentsCopy.hideSinpe = true;
            setEmpty(true)
        }

        let copyOfCart = { ...cart };
        copyOfCart.paymentMethod = paymentMethod;
        setCartState(copyOfCart)
        localStorage.setItem('Cart', JSON.stringify(cart));

        setHideComponents(hideComponentsCopy);
    }

    return (
        <Container className="d-grid gap-3">
            <Form.Select aria-label="Tipo de pago" onChange={handleChangeSelect}
                disabled={hideComponents.disablePurchase} active={(!hideComponents.disablePurchase).toString()}>
                <option>Tipo de pago</option>
                {/*cart.paymentMethods.map((paymentMethod, index) => (
                    <option key={index} value={index}>{paymentMethod}</option>
                ))*/}
                <option key={0} value={0}>{'Efectivo'}</option>
                <option key={1} value={1}>{'Sinpe'}</option>
            </Form.Select>
            <div hidden={hideComponents.hideSinpe}>
                <h4>Por favor, hacer el pago al numero: 7084-0594</h4>
                <Form noValidate validated={validated}>
                    <Form.Group controlId="validationCustom01">
                        <FloatingLabel
                            controlId="floatingInput"
                            label="Numero de comprobante"
                            className="mb-3"
                        >
                            <Form.Control required name="address" type="text" placeholder="Numero de comprobante"
                                onChange={handleChange} disabled={hideComponents.disablePurchase} active={(!hideComponents.disablePurchase).toString()} />
                        </FloatingLabel>
                    </Form.Group>
                </Form>
            </div>
            <Button disabled={empty} active={!empty} onClick={handleSubmit}>Continuar Compra</Button>
            <div className="text-center" hidden={hideComponents.hideConfirmation}>
                <Card className="mx-auto" style={{ width: '22rem' }}>
                    <Card.Body>
                        <Card.Title>Numero de Orden: {purchaseNumber}</Card.Title>
                        <Card.Text>
                            Por favor, esperar la confirmacion del administrador con respecto al pago
                        </Card.Text>
                        <Link href={'/.'}>
                            <h5>Volver a la pantalla principal</h5>
                        </Link>
                    </Card.Body>
                </Card>
            </div>
        </Container>

    );
}