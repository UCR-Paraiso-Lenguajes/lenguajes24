'use client'

import { useState } from "react";
import AddressForm from "../address/page";

const Cart = ({ cart, setCart, toggleCart, removeProduct }: { cart: any, setCart: (cart: any) => void, toggleCart: (action: boolean) => void, 
    removeProduct: (product: any) => void }) => {
    const [showAddressForm, setShowAddressForm] = useState(false);

    function handleAddressForm() {
        setShowAddressForm(!showAddressForm);
    }

    return (
        showAddressForm ? <AddressForm handleAddressForm={handleAddressForm}
            cart={cart} setCart={setCart} /> :
            <div className="container">
                <h1>Tu carrito de compras:</h1>
            <table className="table">
                <thead>
                    <tr>
                        <th scope="col">Producto</th>
                        <th scope="col">Descripción</th>
                        <th scope="col">Precio Unidad</th>
                        <th scope="col">Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    {cart.carrito.productos.map((producto: any, index: number) =>
                        <tr key={index}>
                            <td><img src={producto.imageUrl} alt={producto.name} style={{ width: "100px", height: "auto" }} /></td>
                            <td>
                                <h5>{producto.name}</h5>
                                <p>{producto.description}</p>
                            </td>
                            <td>${producto.price}</td>
                            <td>
                                <button className="btn btn-outline-danger btn-sm" onClick={() => {
                                    removeProduct(producto);
                                    toggleCart(true);
                                }}>
                                    <i className="bi bi-trash"></i> Eliminar
                                </button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>

            <div className="d-flex justify-content-end">
                <h5 className="mr-2">Subtotal: ${cart.carrito.subtotal}</h5>
            </div>
            <div className="d-flex justify-content-end">
                <h5 className="mr-2">Total: ${cart.carrito.total}</h5>
            </div>
                <div className="d-flex justify-content-start align-items-center">
                    <div className="d-flex w-100 justify-content-center">
                        <button type="button" className="btn btn-primary mr-2" onClick={() => toggleCart(false)}>Atrás</button>
                        <button type="button" className="btn btn-primary mr-2" onClick={handleAddressForm}>Continuar compra</button>
                    </div>
                </div>
            </div>
    )
}

export default Cart;
