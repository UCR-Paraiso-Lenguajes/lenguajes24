'use client';
import Link from "next/link";
import { useEffect, useState } from "react";
import { Container, FloatingLabel, Form, Col, Button } from "react-bootstrap";
import 'bootstrap/dist/css/bootstrap.min.css';


export default function Page() {
    const [cartState, setCartState] = useState(JSON.parse(localStorage.getItem('Cart')));

    const [validated, setValidated] = useState(false);
    const [empty, setEmpty] = useState(true);
    const [address, setAddress] = useState('');


    const handleSubmit = (event) => {
        let copyOfCart = { ...cartState };
        copyOfCart.address = address;
        // setCartState(copyOfCart)
        localStorage.setItem('Cart', JSON.stringify(copyOfCart));

        setValidated(true);
    };

    const handleChange = (event) => {

        const addressFormData = event.currentTarget;
        if (addressFormData.checkValidity() === false) {
            event.preventDefault();
            event.stopPropagation();
        }

        setValidated(true);
        setAddress(event.target.value);
        // console.log(address)
        setEmpty(false)
    }

    return (
        <Container>
            <Form noValidate validated={validated}>
                <Form.Group controlId="validationCustom01">
                    <FloatingLabel
                        controlId="floatingInput"
                        label="Address"
                        className="mb-3"
                    >
                        <Form.Control required name="address" type="text" placeholder="Address" onChange={handleChange} />
                    </FloatingLabel>
                </Form.Group>
                <Button type="submit" disabled={empty} active={!empty} href='/payment' onClick={handleSubmit}>Continuar Compra</Button>
            </Form>
        </Container>

    );
}