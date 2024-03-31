'use client'

import { useState } from "react";
import AddressForm from "../address/page";


const Cart = ({ cart, setCart, toggleCart, removeProduct }: 
    { cart: any, setCart: (cart: any) => void, toggleCart: (action: boolean) => void, removeProduct: (product: any) => void }) => {

    const [showAddressForm, setShowAddressForm] = useState(false);
    

    function handleAddressForm() {
        setShowAddressForm(!showAddressForm);
    }

    return (
        showAddressForm ? <AddressForm handleAddressForm={handleAddressForm} cart={cart} setCart={setCart} /> : <div className="container">
            <h1>Tu carrito de compras:</h1>
            <div className="list-group">
                {cart.carrito.productos.map((producto: any, index: number) =>
                    <a key={index} className="list-group-item list-group-item-action flex-column align-items-start">
                        <div className="d-flex justify-content-start align-items-center">
                            <img className="card-img-top mr-3"
                                src={producto.imageUrl}
                                style={{ width: "200px", height: "90px" }} />
                            <div className="d-flex w-100 justify-content-between">
                                <h5 className="mb-1">{producto.name}</h5>
                                <strong>${producto.price}</strong>
                                <button className="btn btn-outline-danger btn-sm" onClick={() => {
                                    removeProduct(producto);
                                    toggleCart(true);
                                }}>
                                    <i className="bi bi-trash"></i>
                                </button>
                            </div>
                        </div>
                        <p className="mb-1">{producto.description}</p>
                    </a>
                )}
                <a className="list-group-item list-group-item-action flex-column align-items-start">
                    <div className="d-flex justify-content-start align-items-center">
                        <div className="d-flex w-100 justify-content-end">
                            <h5 className="mr-2">Cantidad de Productos:</h5>
                            <h5>{cart.carrito.productos.length}</h5>
                        </div>
                    </div>
                </a>
                <a className="list-group-item list-group-item-action flex-column align-items-start">
                    <div className="d-flex justify-content-start align-items-center">
                        <div className="d-flex w-100 justify-content-end">
                            <h5 className="mr-2">Subtotal:</h5>
                            <h5>${cart.carrito.subtotal}</h5>
                        </div>
                    </div>
                </a>
                <a className="list-group-item list-group-item-action flex-column align-items-start">
                    <div className="d-flex justify-content-start align-items-center">
                        <div className="d-flex w-100 justify-content-end">
                            <h5 className="mr-2">Total:</h5>
                            <h5>${cart.carrito.total}</h5>
                        </div>
                    </div>
                </a>
            </div>
            <div className="d-flex justify-content-end">
                <button type="button" className="btn btn-primary mr-2"
                    data-mdb-ripple-init onClick={() => toggleCart(false)}>Atrás</button>
                <button type="button" className="btn btn-primary"
                    data-mdb-ripple-init
                    disabled={cart.carrito.productos.length === 0}
                    onClick={handleAddressForm}>Continuar compra</button>
            </div>
        </div>
    )
    
}


export default Cart;