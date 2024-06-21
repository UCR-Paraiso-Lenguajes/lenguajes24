"use client";
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

export default function CartPage() {
    const cartDataString = localStorage.getItem('cartItem');
    const cartData = cartDataString ? JSON.parse(cartDataString) : {};
    const [cartUpdated, setCartUpdated] = useState(false);
    const impuesto = 0.13;
    const [cart, setCart] = useState({
        products: [],
        subtotal: 0,
        total: 0,
        cant: 0
    });

    const handleRemove = (id) => {
        if (id == undefined) { throw new Error('The arguments to delete the product cant be null.'); }

        const updatedProducts = cart.products.filter((item) => item.id !== id);
        const updatedCartData = {
            ...cartData,
            products: updatedProducts
        };

        localStorage.setItem('cartItem', JSON.stringify(updatedCartData));
        setCart(updatedCartData);
        setCartUpdated(!cartUpdated);
    };

    const handleQuantityChange = (id, cant) => {
        if (cant == undefined || id == undefined) {
            throw new Error('Los argumentos para calcular el precio no pueden ser nulos.');
        }
        if (cant < 1) {
            return; // Evita que la cantidad sea menor que 1
        }
        const updatedProducts = cartData.products.map((item) => {
            if (item.id === id) {
                return { ...item, cant };
            }
            return item;
        });
        const updatedStore = { ...cart, products: updatedProducts };
        localStorage.setItem("cartItem", JSON.stringify(updatedStore));
        setCart(updatedStore);
        setCartUpdated(!cartUpdated);
    };

    useEffect(() => {
        const cartItem = JSON.parse(localStorage.getItem('cartItem') || '{}');
        setCart(cartItem);
    }, []);

    useEffect(() => {
        let productsInCart = cart.products && cart.products.length > 0;
        if (productsInCart) {
            let calculatedSubtotal = cart.products.reduce((total, item) => total + item.price * item.cant, 0);
            let calculatedTotal = (calculatedSubtotal * impuesto) + calculatedSubtotal;
            setCart(prevCart => ({
                ...prevCart,
                subtotal: calculatedSubtotal,
                total: calculatedTotal
            }));
        }
    }, [cart.products]);

    var esCarritoVacio = cartData.products.length === 0;
    if (esCarritoVacio) {
        return (
            <div>
                <header className="p-3 text-bg-dark">
                    <div className="row" style={{ color: 'gray' }}>
                        <div className="col-sm-9">
                            <Link href="/">
                                <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" />
                            </Link>
                        </div>
                        <div className="col-sm-3 d-flex justify-content-end align-items-center">
                            <Link href="/">
                                <button className="btn btn-dark"> Go Home</button>
                            </Link>
                        </div>
                    </div>
                </header>

                <div className='total'>
                    <span>Your cart is empty</span>
                </div>
            </div>
        );
    }

    return (
        <div style={{ backgroundColor: 'silver' }}>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-9">
                        <Link href="/">
                            <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" />
                        </Link>
                    </div>
                    <div className="col-sm-3 d-flex justify-content-end align-items-center">
                        <Link href="/">
                            <button className="btn btn-dark"> Go Home</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div className='container'>
                <h2 className='text-left mt-5 mb-5'>Shopping cart</h2>

                <div id="order-detail-content" className="table_block table-responsive">
                    <table id="cart_summary" className="table table-bordered stock-management-on">
                        <thead>
                            <tr>
                                <th className="cart_product first_item">Name</th>
                                <th className="cart_description item">Author</th>
                                <th className="cart_unit item text-right">Price</th>
                                <th className="cart_quantity item text-center">Quantity</th>
                                <th className="cart_total item text-right">Total</th>
                                <th className="cart_delete last_item">&nbsp;</th>
                            </tr>
                        </thead>
                        <tbody>
                            {cartData && cartData.products && cartData.products.map(item => (
                                <tr key={item.id}>
                                    <td className="cart_product text-center">
                                        <a href={item.imgUrl}>
                                            <img src={item.imgUrl} alt={item.name} style={{ height: 'auto', width: '100px', marginLeft: 'auto', marginRight: 'auto', display: 'block' }} />
                                        </a>
                                    </td>
                                    <td className="cart_description text-center">
                                        <p className="product-name">
                                            {item.name}
                                        </p>
                                        <small dangerouslySetInnerHTML={{ __html: item.author }} />
                                    </td>
                                    <td className="cart_unit" data-title="Precio unitario">
                                        ₡{item.price}
                                    </td>
                                    <td className="cart_quantity text-center" data-title="Cantidad">
                                        <div className="input-group" style={{ width: '125px', marginLeft: 'auto', marginRight: 'auto' }}>
                                            <button className="btn btn-outline-secondary" type="button" style={{ width: '30px', padding: '5px 10px' }}
                                                onClick={() => handleQuantityChange(item.id, item.cant - 1)}>-</button>
                                            <input type="text" className="form-control text-center" value={item.cant} readOnly />
                                            <button className="btn btn-outline-secondary" type="button" style={{ width: '30px', padding: '5px 10px' }}
                                                onClick={() => handleQuantityChange(item.id, item.cant + 1)}>+</button>
                                        </div>
                                    </td>
                                    <td className="cart_total text-center" data-title="Total">
                                        ₡{item.price * 1 * item.cant}
                                    </td>
                                    <td className="cart_delete text-center" data-title="Borrar">
                                        <button className="btn btn-dark" onClick={() => handleRemove(item.id)}>Delete</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>

            <div className="d-flex justify-content-end" style={{ position: 'fixed', bottom: '65px', right: '10px', zIndex: '1000' }}>
                <div style={{ backgroundColor: 'white', margin: '100' }}>
                    <h2>Subtotal: ₡{cart.subtotal ? cart.subtotal.toFixed(2) : '0.00'}</h2>
                    <h2 className='my-3'>Taxes 13%: ₡{(cart.total ? (cart.total - cart.subtotal).toFixed(2) : '0.00')}</h2>
                    <h2 className='my-3'>Total: ₡{cart.total ? cart.total.toFixed(2) : '0.00'}</h2>
                    {cart && cart.products && cart.products.length > 0 ? (
                        <Link href="/payment">
                            <button className="btn btn-success" style={{ display: 'flex', justifyContent: 'left' }}>Click to Buy</button>
                        </Link>
                    ) : (
                        <button className="btn btn-success" style={{ display: 'flex', justifyContent: 'left' }} disabled>Click to Buy</button>
                    )}
                </div>
            </div>
            <footer className='footer'>
                <div className="text-center p-3">
                    <h5 className="text-light">Paula's Library</h5>
                </div>
            </footer>
        </div>
    );
};
