"use client"
import React, { useState, useEffect } from 'react';
import '../styles/cart.css';
import AddressForm from './addressUser';

const Cart_Store = () => {

    const storedData = localStorage.getItem('tienda');
    const dataObject = JSON.parse(storedData);
    
    const [cartEmpty, setCartEmpty] = useState(true); 
    const [showAddressForm, setShowAddressForm] = useState(false);
    const [cartUpdated, setCartUpdated] = useState(false);
    const [userAddress, setUserAddress] = useState(null);

    let subtotal = 0;
    let subtotalImpuesto = 0;
    let totalCompra = 0;


    const handleContinueBuy = () => {
        setShowAddressForm(true);
    }

    const handlePrice = () => {
        let totalPriceWithoutTax = 0;
        dataObject.products.forEach(product => {
            totalPriceWithoutTax += product.price; 
        }); 

        const totalPriceWithTax = totalPriceWithoutTax * (dataObject.cart.impVentas / 100);
        totalCompra = totalPriceWithoutTax + totalPriceWithTax;
        subtotal = totalPriceWithoutTax;
        subtotalImpuesto = totalPriceWithTax;
        setCartEmpty(dataObject.products.length === 0);
        updateStore(subtotal, subtotalImpuesto, totalCompra);
    }

    const handleRemove = (id) => {
        const updatedProducts = dataObject.products.filter((product) => product.id !== id); 
        const updatedCart = {...dataObject, products: updatedProducts};
        localStorage.setItem('tienda', JSON.stringify(updatedCart));
        setCartUpdated(!cartUpdated);
        window.location.reload();
    }
    
    const updateStore =(subtotalC, subtotalImpuestoCa, totalComp) => {
        const carritoActualizado = {
            ...dataObject,
            cart: {
                ...dataObject.cart,
                subtotal : subtotalC,
                subtotalImpuesto: subtotalImpuestoCa,
                total: totalComp    
            },
        };
        localStorage.setItem("tienda", JSON.stringify(carritoActualizado));
    };

    useEffect(() => {
        handlePrice();
    }, [cartUpdated]);

    if (!(dataObject.products.length > 0)) {
        return (
            <div>
                <p>No hay productos en el carrito.</p>
            </div>
        );
    }
    return (
        <div>
            {showAddressForm ? <AddressForm /> :
                <div>
                    {dataObject.products?.map((product) => (
                        <div className='cartBox' key={product.id}>
                            <div className='cart_Img'>
                                <img src={product.imageUrl} alt={product.name} />
                                <p>{product.name}</p>
                            </div>
    
                            <div>
                                <button> + </button>
                                <button>{product.amount}</button>
                                <button> - </button>
                            </div>
                            <div>
                                <span>{product.price}</span>
                                <button className='BtnEliminar' onClick={() => handleRemove(product.id)}>Eliminar</button>
                            </div>
                        </div>
                    ))}
                    <div className='Factura'>
                        <span>Total por pagar </span>
                        <br />
                    </div>
                    <div className='withoutTax'>
                        <span>Subtotal Sin impuestos: </span>
                        <span> $ {dataObject.cart.subtotal}</span>
                    </div>
                    <div className='withTax'>
                        <span >Con impuestos: </span>
                        <span> $ {dataObject.cart.subtotalImpuesto}</span>
                    </div>
                    <div className='withTax'>
                        <span >Total: </span>
                        <span> $ {dataObject.cart.total}</span>
                    </div>
                    <div className='BuyProduct'>
                        <button className='BtnBuy' onClick={handleContinueBuy} disabled={cartEmpty}>Continuar compra</button>
                    </div>
                </div>
            }
        </div>
    )
}

export default Cart_Store;
