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
        paymentMethod: { id: 0, name: '', active: 0 },
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
    const [isFormValid, setIsFormValid] = useState(false);


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
            throw new Error("Error fetching payment methods:", error);
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
        const paymentMethodId = parseInt(e.target.value);
        const selectedPaymentMethod = paymentMethods.find(method => method.id === paymentMethodId);
        setCart(prevCart => {
            const updatedCart = { ...prevCart, paymentMethod: selectedPaymentMethod || { id: 0, name: '', active: 0 } };
            updateLocalStorage(updatedCart);
            return updatedCart;
        });
        setIsFormValid(selectedPaymentMethod?.name === 'CASH' || selectedPaymentMethod?.name === 'SINPE');
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
        const isReceiptRequired = paymentMethod.name !== 'CASH' && paymentMethod.id !== 0;
        const validOrder = deliveryAddress && paymentMethod.id && products.length > 0 && (isReceiptRequired || receipt);
        if (validOrder) {
            const productIds = products.map(producto => ({
                productId: String(producto.id),
                quantity: producto.cant 
            }));

            const dataToSend = {
                productIds: productIds,
                address: deliveryAddress,
                paymentMethod: paymentMethod,
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
                        paymentMethod: { id: 0, name: '', active: 0 },
                        isCartEmpty: true,
                        numeroCompra: orderNumber
                    });
                    window.location.href = "/confirm";
                } else {
                    const errorResponseData = await response.json();
                    throw new Error('Error to send data: ' + JSON.stringify(errorResponseData));
                }
            } catch (error) {
                throw new Error(error);
                setCart(prevCart => ({ ...prevCart, confirmation: 'Error processing your order. Please try again.' }));
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

                <div className="form-group">
                    <label htmlFor="specificDetails">Specific Details:</label>
                    <input type="text" id="specificDetails" className="form-control" value={specificDetails} onChange={handleSpecificDetailsChange} />
                </div>

                <div className="form-group">
                    <label htmlFor="paymentMethod">Payment Method:</label>
                    <select id="paymentMethod" className="form-control" value={cart.paymentMethod?.id || ''} onChange={handlePaymentMethodChange}>
                        <option value="">Select Payment Method</option>
                        {paymentMethods.filter(method => method.active).map(method => (
                            <option key={method.id} value={method.id}>{method.name}</option>
                        ))}
                    </select>
                </div>

                {cart.paymentMethod?.name === 'SINPE' && (
                    <div className="form-group">
                        <label htmlFor="receipt">Receipt:</label>
                        <input type="text" id="receipt" className="form-control" value={cart.receipt} onChange={handleReceiptChange} />
                    </div>
                )}

                <button className="btn btn-secondary" onClick={handleSubmit} disabled={!isFormValid}>Submit</button>

                {cart.confirmation && (
                    <div className="alert alert-warning mt-3">
                        {cart.confirmation}
                    </div>
                )}
            </div>

            <footer className='footer'>
                <div className="text-center p-3">
                    <h5 className="text-light">Paula's Library</h5>
                </div>
            </footer>
        </div>
    );
}
