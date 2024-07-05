'use client';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

const provinces = {
    'San José': ['Central', 'Escazú', 'Desamparados', 'Puriscal', 'Tarrazú', 'Aserrí', 'Mora', 'Goicoechea', 'Santa Ana', 'Alajuelita', 'Vásquez de Coronado', 'Acosta', 'Tibás', 'Moravia', 'Montes de Oca', 'Turrubares', 'Dota', 'Curridabat', 'Pérez Zeledón', 'León Cortés'],
    'Alajuela': ['Central', 'San Ramón', 'Grecia', 'San Mateo', 'Atenas', 'Naranjo', 'Palmares', 'Poás', 'Orotina', 'San Carlos', 'Zarcero', 'Valverde Vega', 'Upala', 'Los Chiles', 'Guatuso', 'Río Cuarto'],
    'Cartago': ['Central', 'Paraíso', 'La Unión', 'Jiménez', 'Turrialba', 'Alvarado', 'Oreamuno', 'El Guarco'],
    'Heredia': ['Central', 'Barva', 'Santo Domingo', 'Santa Bárbara', 'San Rafael', 'San Isidro', 'Belén', 'Flores', 'San Pablo', 'Sarapiquí'],
    'Guanacaste': ['Liberia', 'Nicoya', 'Santa Cruz', 'Bagaces', 'Carrillo', 'Cañas', 'Abangares', 'Tilarán', 'Nandayure', 'La Cruz', 'Hojancha'],
    'Puntarenas': ['Central', 'Esparza', 'Buenos Aires', 'Montes de Oro', 'Osa', 'Quepos', 'Golfito', 'Coto Brus', 'Parrita', 'Corredores', 'Garabito'],
    'Limón': ['Central', 'Pococí', 'Siquirres', 'Talamanca', 'Matina', 'Guácimo'],
};

export default function PaymentPage() {
    const cartDataString = localStorage.getItem('cartItem');
    const cartData = cartDataString ? JSON.parse(cartDataString) : {};
    const orderNumber = cartData.numeroCompra || 'No disponible';
    const [paymentConfirmed, setPaymentConfirmed] = useState(false);
    const URL = process.env.NEXT_PUBLIC_API_URL;

    const [cart, setCart] = useState({
        products: [],
        deliveryAddress: '',
        paymentMethod: { id: null, name: '', active: false },
        receipt: '',
        confirmation: '',
        total: '',
        cant: 1,
        isCartEmpty: true
    });

    const [selectedProvince, setSelectedProvince] = useState('');
    const [selectedCanton, setSelectedCanton] = useState('');
    const [specificDetails, setSpecificDetails] = useState('');
    const [paymentMethods, setPaymentMethods] = useState([]);

    useEffect(() => {
        let cartItemStored = localStorage.getItem('cartItem');
        if (cartItemStored !== null) {
            let cartData = JSON.parse(cartItemStored);
            const validProducts = cartData.products && Array.isArray(cartData.products) && cartData.products.length > 0;
            if (validProducts) {
                setCart({ ...cartData, isCartEmpty: false });
            } else {
                localStorage.removeItem('cartItem');
                setCart(prevCart => ({ ...prevCart, isCartEmpty: true }));
            }
        } else {
            setCart(prevCart => ({ ...prevCart, isCartEmpty: true }));
        }
        window.history.replaceState(null, '', '/');
        fetchPaymentMethods();
    }, []);

    const fetchPaymentMethods = async () => {
        try {
            const response = await fetch(`${URL}/api/paymentMethods`);
            if (response.ok) {
                const data = await response.json();
                setPaymentMethods(data);
            }
        } catch (error) {
            console.error("Error fetching payment methods:", error);
        }
    };

    const handleProvinceChange = (e) => {
        const province = e.target.value;
        setSelectedProvince(province);
        setSelectedCanton('');
        updateDeliveryAddress(province, '', specificDetails);
    };

    const handleCantonChange = (e) => {
        const canton = e.target.value;
        setSelectedCanton(canton);
        updateDeliveryAddress(selectedProvince, canton, specificDetails);
    };

    const handleSpecificDetailsChange = (e) => {
        const details = e.target.value;
        setSpecificDetails(details);
        updateDeliveryAddress(selectedProvince, selectedCanton, details);
    };

    const updateDeliveryAddress = (province, canton, details) => {
        const address = `${province}, ${canton}, ${details}`;
        setCart(prevCart => {
            const updatedCart = { ...prevCart, deliveryAddress: address };
            updateLocalStorage(updatedCart);
            return updatedCart;
        });
    };

    const handlePaymentMethodChange = (e) => {
        const selectedId = parseInt(e.target.value, 10);
        const selectedMethod = paymentMethods.find(method => method.id === selectedId);
        setCart(prevCart => ({
            ...prevCart,
            paymentMethod: selectedMethod || { id: null, name: '', active: false }
        }));
        updateLocalStorage({
            ...cart,
            paymentMethod: selectedMethod || { id: null, name: '', active: false }
        });
    };

    const updateLocalStorage = (updatedCart) => {
        localStorage.setItem('cartItem', JSON.stringify(updatedCart));
    };

    const handleReceiptChange = (e) => {
        const value = e.target.value;
        setCart(prevCart => {
            const updatedCart = { ...prevCart, receipt: value };
            updateLocalStorage(updatedCart);
            return updatedCart;
        });
        setPaymentConfirmed(true);
    };

    const handleSubmit = async () => {
        const { deliveryAddress, paymentMethod, products, total, receipt } = cart;
        const validOrder = deliveryAddress && paymentMethod.id !== null && products.length > 0 && receipt;

        if (validOrder) {
            const productIds = products.map(producto => ({
                productId: String(producto.id),
                quantity: producto.cant
            }));

            const dataToSend = {
                productIds: productIds,
                address: deliveryAddress,
                paymentMethod: {
                    id: paymentMethod.id,
                    name: paymentMethod.name,
                    active: paymentMethod.active
                },
                total: total,
                receipt: receipt
            };

            try {
                const response = await fetch(URL + '/api/Cart', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(dataToSend)
                });

                if (response.ok) {
                    const responseData = await response.json();
                    const { purchaseNumber } = responseData;
                    const orderNumber = purchaseNumber;

                    updateLocalStorage({
                        products: [],
                        deliveryAddress: '',
                        paymentMethod: { id: null, name: '', active: false },
                        isCartEmpty: true,
                        numeroCompra: orderNumber
                    });
                    window.location.href = "/confirm";
                } else {
                    const errorResponseData = await response.json();
                    throw new Error('Error to send data: ' + JSON.stringify(errorResponseData));
                }
            } catch (error) {
                console.error(error);
                setCart(prevCart => ({ ...prevCart, confirmation: 'Error processing your order. Please try again.' }));
            }
        } else {
            setCart(prevCart => ({ ...prevCart, confirmation: 'Please complete all required fields or add items to the cart.' }));
        }
    };

    const renderPaymentMethods = () => {
        if (paymentMethods.length === 0) {
            return null;
        }
        return paymentMethods.filter(method => method.active === 1).map(method => (
            <div key={method.id} className="form-check">
                <input
                    className="form-check-input"
                    type="radio"
                    name="paymentMethod"
                    id={`paymentMethod${method.id}`}
                    value={method.id}
                    checked={cart.paymentMethod?.id === method.id}
                    onChange={handlePaymentMethodChange}
                />
                <label className="form-check-label" htmlFor={`paymentMethod${method.id}`}>
                    {method.name}
                </label>
            </div>
        ));
    };


    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-9">
                        <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" />
                    </div>

                    <div className="col-sm-3 d-flex justify-content-end align-items-center">
                        <Link href="/api/auth/logout" className="nav-link px-3 text-white">
                            <button type="button" className="btn btn-success" style={{ height: '2.8rem' }}>Log out</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div className="container">
                <div className="row mt-4">
                    <div className="col-md-12">
                        <div className="card">
                            <div className="card-header">
                                <h3 className="text-center mb-0">Checkout</h3>
                            </div>
                            <div className="card-body">
                                <div className="form-group">
                                    <label>Provincia:</label>
                                    <select className="form-control" value={selectedProvince} onChange={handleProvinceChange}>
                                        <option value="">Seleccione una provincia</option>
                                        {Object.keys(provinces).map(province => (
                                            <option key={province} value={province}>{province}</option>
                                        ))}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label>Cantón:</label>
                                    <select className="form-control" value={selectedCanton} onChange={handleCantonChange} disabled={!selectedProvince}>
                                        <option value="">Seleccione un cantón</option>
                                        {provinces[selectedProvince] && provinces[selectedProvince].map(canton => (
                                            <option key={canton} value={canton}>{canton}</option>
                                        ))}
                                    </select>
                                </div>
                                <div className="form-group">
                                    <label>Detalles específicos:</label>
                                    <input type="text" className="form-control" value={specificDetails} onChange={handleSpecificDetailsChange} />
                                </div>
                                {paymentMethods.length > 0 && (
                                    <div className="form-group">
                                        <label>Método de Pago:</label>
                                        {renderPaymentMethods()}
                                    </div>
                                )}
                                <div className="form-group">
                                    <label>Receipt:</label>
                                    <input type="text" className="form-control" value={cart.receipt} onChange={handleReceiptChange} />
                                </div>
                                <button className="btn btn-secondary" onClick={handleSubmit}>Submit</button>
                                {cart.confirmation && <div className="alert alert-info mt-3">{cart.confirmation}</div>}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
