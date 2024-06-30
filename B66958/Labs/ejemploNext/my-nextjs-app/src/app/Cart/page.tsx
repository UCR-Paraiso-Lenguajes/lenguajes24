'use client'

import { useState } from "react";
import AddressForm from "../Address/page";
import { CartState } from "../types/Cart";

const Cart = ({ cart, setCart, toggleCart, clearProducts }:
    { cart: CartState, setCart: (cart: CartState) => void, toggleCart: (action: boolean) => void, clearProducts: () => void }) => {

    const [showAddressForm, setShowAddressForm] = useState(false);

    function handleAddressForm() {
        setShowAddressForm(!showAddressForm);
    }

    const handleQuantityChange = (id, newQuantity) => {
        const updatedProducts = cart.carrito.productos.map(product => {
            if (product.uuid === id) {
                if(newQuantity > 0)
                return {
                    ...product,
                    quantity: newQuantity,
                };
            }
            return product;
        });

        const newSubTotal = updatedProducts.reduce((acc, product) => acc + (product.price * product.quantity), 0);
        const newTotal = newSubTotal + (newSubTotal * (cart.carrito.porcentajeImpuesto / 100));

        const updatedCart: CartState = {
            ...cart,
            carrito: {
                ...cart.carrito,
                productos: updatedProducts,
                subtotal: newSubTotal,
                total: newTotal,
            },
        }

        setCart(updatedCart);
    };

    return (
        showAddressForm ? <AddressForm handleAddressForm={handleAddressForm}
            cart={cart} setCart={setCart} clearProducts={clearProducts} /> :
            <div className="container">
                <h1>Tu carrito de compras:</h1>
                <div className="list-group">
                    <a className="list-group-item list-group-item-action flex-column align-items-start">
                        <div className="d-flex justify-content-start align-items-center">
                            <div className="d-flex w-100 justify-content-center">
                                <h5 className="mr-2">Cantidad de Productos:</h5>
                                <h5>{cart?.carrito?.productos.length ?? 0}</h5>
                            </div>
                        </div>
                    </a>
                    {cart?.carrito?.productos?.map((producto, index) => (
                    <div key={index} className="list-group-item list-group-item-action flex-column align-items-start">
                        <div className="d-flex justify-content-start align-items-center">
                            <img className="card-img-top mr-3"
                                src={producto?.imageUrl ?? ''}
                                style={{ width: "200px", height: "90px" }} />
                            <div className="d-flex w-100 justify-content-between">
                                <div>
                                    <h5 className="mb-1">{producto?.name ?? 'Nombre no disponible'}</h5>
                                    <p className="mb-1">{producto?.description ?? 'Descripción no disponible'}</p>
                                </div>
                                <div>
                                    <button className="btn btn-sm btn-outline-primary mx-2" onClick={() => handleQuantityChange(producto.uuid, (producto.quantity ?? 1) - 1)}>-</button>
                                    <span>{producto.quantity ?? 1}</span>
                                    <button className="btn btn-sm btn-outline-primary mx-2" onClick={() => handleQuantityChange(producto.uuid, (producto.quantity ?? 1) + 1)}>+</button>
                                </div>
                            </div>
                        </div>
                        <div className="d-flex justify-content-between align-items-center">
                            <small>${producto?.price ?? 'Precio no disponible'}</small>
                            <small>Subtotal: ${producto.price * (producto.quantity ? producto.quantity : 1)}</small>
                        </div>
                    </div>
                ))}
                    <a className="list-group-item list-group-item-action flex-column align-items-start">
                        <div className="d-flex justify-content-start align-items-center">
                            <div className="d-flex w-100 justify-content-center">
                                <h5 className="mr-2">Subtotal:</h5>
                                <h5>${cart?.carrito?.subtotal ?? 'Subtotal no disponible'}</h5>
                            </div>
                        </div>
                    </a>
                    <a className="list-group-item list-group-item-action flex-column align-items-start">
                        <div className="d-flex justify-content-start align-items-center">
                            <div className="d-flex w-100 justify-content-center">
                                <h5 className="mr-2">Total:</h5>
                                <h5>${cart?.carrito?.total ?? 'Total no disponible'}</h5>
                            </div>
                        </div>
                    </a>
                </div>
                <div className="d-flex justify-content-start align-items-center">
                    <div className="d-flex w-100 justify-content-center">
                        <button type="button" className="btn btn-primary mr-2"
                            data-mdb-ripple-init onClick={() => toggleCart(false)}>Atrás</button>
                        <button type="button" className="btn btn-primary mr-2"
                            data-mdb-ripple-init
                            disabled={!cart?.carrito?.productos || cart.carrito.productos.length === 0}
                            onClick={handleAddressForm}>Continuar compra</button>
                        <button type="button" className="btn btn-danger mr-2"
                            data-mdb-ripple-init
                            disabled={!cart?.carrito?.productos || cart.carrito.productos.length === 0}
                            onClick={clearProducts}>Cancelar compra</button>
                    </div>
                </div>
            </div>
    )
}

export default Cart;