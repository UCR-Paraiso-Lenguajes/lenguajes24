"use client";
import { useEffect, useState } from 'react';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css";
import Payment from '../payment/page';


export default function Address() {
    const [address, setAddress] = useState('');

    const handleAddressChange = (e) => {
        setAddress(e.target.value);
    };

    const handleSaveAddress = () => {
        if (address.trim() !== '') {
            localStorage.setItem('address', address);
        }
    };

    useEffect(() => {
        const storedAddress = localStorage.getItem('address');
        if (storedAddress) {
            setAddress(storedAddress);
        }
    }, []);

    const isAddressEmpty = address.trim() === '';

    return (
        <div className="container d-flex justify-content-center align-items-center" style={{ minHeight: '100vh', backgroundColor: 'pink' }}>
            <div className="card p-4">
                <div className="card-body">
                    <div className="text-center mb-4">
                        <h1 className="h3">Agregue su dirección</h1>
                    </div>
                    <form>
                        <div className="mb-3">
                            <label htmlFor="address" className="form-label"></label>
                            <input
                                type="text"
                                className="form-control"
                                id="address"
                                name="address"
                                value={address}
                                onChange={handleAddressChange} />
                        </div>
                        <div className="text-center">
                            <Link href="/payment">
                                <button className="btn btn-primary" onClick={handleSaveAddress} disabled={isAddressEmpty}>Confirmar dirección</button>
                            </Link>

                            <Link href="/">
                                <button className="Button">Inicio</button>
                            </Link>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
