'use client';
import React, { useState } from 'react';
import { Button, Container } from 'react-bootstrap';
import Table from 'react-bootstrap/Table';
import 'bootstrap/dist/css/bootstrap.min.css';

const Tupla = ({ item, deleteAction, index }) => {
    const { id, imgSource, name, price } = item;

    return (

        <tr>
            <td><img src={item.imgSource} width="100px" className="img-fluid" /></td>
            <td>{item.name}</td>
            <td><Button onClick={() => deleteAction(index)}>
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" className="w-6 h-6">
                    <path strokeLinecap="round" strokeLinejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
                </svg>

            </Button></td>
            <td>${item.price}</td>
        </tr>

    )
};

export default function Page() {
    const [cartState, setCartState] = useState(JSON.parse(localStorage.getItem('Cart')));
    const [shopState, setShopState] = useState(JSON.parse(localStorage.getItem('Shop')));
    const [hasProducts, toggleHasProducts] = useState(cartState.products.length > 0)

    const handleClick = (index) => {
        let copyOfCart = { ...cartState };
        const deletedProduct = copyOfCart.products.splice(index, 1);
        // copyOfCart.subtotal -= deletedProduct[0].price;
        const subtotal = copyOfCart.subtotal - deletedProduct[0].price;
        const formattedSubtotal = Number(subtotal.toFixed(2));
        copyOfCart.subtotal = formattedSubtotal;
        setCartState(copyOfCart)
        localStorage.setItem('Cart', JSON.stringify(copyOfCart));
        toggleHasProducts(cartState.products.length > 0)
    }

    return (
        <Container>
            <h1>Carrito de Compras</h1>

            <Table striped bordered variant='dark'>
                <thead>
                    <tr>
                        <th width={500}>Product</th>
                        <th width={500}>Description</th>
                        <th width={100}></th>
                        <th width={500}>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {cartState.products.map((item, index) => (
                        <Tupla key={index} item={item} deleteAction={handleClick} index={index} />
                    ))}
                </tbody>
                <tfoot>
                    <tr>
                        <td rowSpan={3} colSpan={2}></td>
                        <td>Subtotal</td>
                        <td>${cartState.subtotal}</td>
                    </tr>
                    <tr>
                        <td>+ IVA {shopState.taxPercentage}%</td>
                        <td>${(cartState.subtotal * shopState.taxPercentage / 100).toFixed(2)}</td>
                    </tr>
                    <tr>
                        <td>Total</td>
                        <td>${((cartState.subtotal * shopState.taxPercentage / 100) + cartState.subtotal).toFixed(2)}</td>
                    </tr>
                </tfoot>
            </Table>
            <Button disabled={!hasProducts} active={hasProducts} href='/checkout'>Comprar</Button>
        </Container>

    );
}