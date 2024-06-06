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
        paymentMethod: '',
        receipt: '',
        confirmation: '',
        total: '',
        isCartEmpty: true
    });

    const [selectedProvince, setSelectedProvince] = useState<string>('');
    const [selectedCanton, setSelectedCanton] = useState('');
    const [specificDetails, setSpecificDetails] = useState('');

    useEffect(() => {
        let cartItemStored = localStorage.getItem('cartItem');
        if (cartItemStored !== null) {
            let cartData = JSON.parse(cartItemStored);
            const validProducts = cartItemStored && Array.isArray(cartData.products) && cartData.products.length > 0;
            if (validProducts) {
                setCart({ ...cartData, isCartEmpty: false });
            } else {
                localStorage.removeItem('cartItem');
                setCart({ ...cart, isCartEmpty: true });
            }
        } else {
            setCart({ ...cart, isCartEmpty: true });
        }
        // Reemplazar la entrada actual del historial con una nueva entrada que apunte a la página de inicio
        window.history.replaceState(null, '', '/'); // Esto hace que no sea posible retroceder a esta página
    }, []);

    const handleProvinceChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const province = e.target.value;
        setSelectedProvince(province);
        setSelectedCanton('');
        updateDeliveryAddress(province, '', specificDetails);
    };

    const handleCantonChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const canton = e.target.value;
        setSelectedCanton(canton);
        updateDeliveryAddress(selectedProvince, canton, specificDetails);
    };

    const handleSpecificDetailsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const details = e.target.value;
        setSpecificDetails(details);
        updateDeliveryAddress(selectedProvince, selectedCanton, details);
    };

    const updateDeliveryAddress = (province: string, canton: string, details: string) => {
        const address = `${province}, ${canton}, ${details}`;
        setCart(prevCart => ({ ...prevCart, deliveryAddress: address }));
        updateLocalStorage({ ...cart, deliveryAddress: address });
    };

    const handlePaymentMethodChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, paymentMethod: value }));
        updateLocalStorage({ ...cart, paymentMethod: value });
    };

    const updateLocalStorage = (updatedCart: any) => {
        localStorage.setItem('cartItem', JSON.stringify(updatedCart));
    };

    const handleReceiptChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setCart(prevCart => ({ ...prevCart, receipt: value }));
        updateLocalStorage({ ...cart, receipt: value });
        setPaymentConfirmed(true);
    };

    const handleSubmit = async () => {
        const { deliveryAddress, paymentMethod, products, total } = cart;
        const validOrder = deliveryAddress && paymentMethod && (paymentMethod === 'cash' || paymentMethod === 'sinpe');
        if (validOrder) {
            const productIds = products.map((producto: any) => String(producto.id));
            let paymentMethodValue = 0;
            if (paymentMethod === 'sinpe') {
                paymentMethodValue = 1;
            }

            const dataToSend = {
                productIds: productIds,
                address: deliveryAddress,
                paymentMethod: paymentMethodValue,
                total: total
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
                        paymentMethod: '',
                        isCartEmpty: true,
                        numeroCompra: orderNumber
                    });
                    window.location.href = "/confirm";
                } else {
                    const errorResponseData = await response.json();
                    throw new Error('Error to send data: ' + JSON.stringify(errorResponseData));
                }
            } catch (error) {
                throw error;
            }
        } else {
            setCart(prevCart => ({ ...prevCart, confirmation: 'Please complete all required fields or add items to the cart.' }));
        }
    };

    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-9">
                        <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" />
                    </div>

                    <div className="col-sm-3 d-flex justify-content-end align-items-center">
                        <Link href="/cart">
                            <button className="btn btn-dark">Go Cart</button>
                        </Link>
                        <Link href="/">
                            <button className="btn btn-dark">Go Home</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div className='container'>
                <h2>Payment Page</h2>
                <div className="form-group">
                    <label htmlFor="province">Province:</label>
                    <select id="province" className="form-control" value={selectedProvince} onChange={handleProvinceChange}>
                        <option value="">Select Province</option>
                        {Object.keys(provinces).map(province => (
                            <option key={province} value={province}>{province}</option>
                        ))}
                    </select>
                </div>

                {selectedProvince && (
                    <div className="form-group">
                        <label htmlFor="canton">Canton:</label>
                        <select id="canton" className="form-control" value={selectedCanton} onChange={handleCantonChange}>
                            <option value="">Select Canton</option>
                            {provinces[selectedProvince].map(canton => (
                                <option key={canton} value={canton}>{canton}</option>
                            ))}
                        </select>
                    </div>
                )}

                {selectedCanton && (
                    <div className="form-group">
                        <label htmlFor="specificDetails">Specific Details:</label>
                        <input
                            type="text"
                            id="specificDetails"
                            className="form-control"
                            value={specificDetails}
                            onChange={handleSpecificDetailsChange}
                        />
                    </div>
                )}

                {cart.deliveryAddress && (
                    <div className="form-group">
                        <label htmlFor="paymentMethod">Payment Method:</label>
                        <select
                            id="paymentMethod"
                            className="form-control"
                            value={cart.paymentMethod}
                            onChange={handlePaymentMethodChange}
                        >
                            <option value="">Select Payment Method</option>
                            <option value="sinpe">Sinpe</option>
                            <option value="cash">Cash</option>
                        </select>
                    </div>
                )}

                {cart.paymentMethod === 'cash' && (
                    <div>
                        <button className="btn btn-success" style={{ margin: '1px' }} onClick={handleSubmit} disabled={!specificDetails}>
                            {specificDetails ? 'Confirm Payment' : 'Fill out all the fields'}
                        </button>
                        <p>{cart.confirmation}</p>
                    </div>
                )}



                {cart.paymentMethod === 'sinpe' && (
                    <div>
                        <p>Make payment for the purchase at the following Sinpe Number: +506 86920997</p>
                        <div className="form-group">
                            <label htmlFor="receipt">Receipt:</label>
                            <input
                                type="text"
                                id="receipt"
                                className="form-control"
                                value={cart.receipt}
                                onChange={handleReceiptChange}
                            />
                        </div>
                        <div>
                            {specificDetails.length > 10 && cart && cart.receipt && cart.receipt.length > 0 ? (
                                <button className="btn btn-success" onClick={handleSubmit}>
                                    Confirm Payment
                                </button>
                            ) : (
                                <button className="btn btn-success" disabled>Confirm Payment</button>
                            )}
                        </div>
                        <p>{cart.confirmation}</p>
                    </div>
                )}
            </div>

            <footer className='footer' style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light">Paula's Library</h5>
                </div>
            </footer>
        </div>
    );
};
