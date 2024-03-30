"use client";
import { useEffect, useState } from 'react';
import "@/public/styles.css";
import Link from 'next/link';

export default function Address() {
    const [address, setAddress] = useState('');

    const handleAddressChange = (e) => {
        setAddress(e.target.value);
    };

    const handleSaveAddress = () => {
        localStorage.setItem('address', address);
    };

    useEffect(() => {
        const storedAddress = localStorage.getItem('address');
        if (storedAddress) {
            setAddress(storedAddress);
        }
    }, []);

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Amazon</h1>
                </Link>
            </div>

            <div className="body">
                <h2>Address</h2>
                <form>
                    <input
                        type="text"
                        id="address"
                        name="address"
                        value={address}
                        onChange={handleAddressChange}/>
                </form>
                <Link href="/payment">
                    <button className="Button" onClick={handleSaveAddress}>Save Address</button>
                </Link>
                    
            </div>

            <div className="footer">
                <h2>Amazon.com</h2>
            </div>
        </div>
    );
}