"use client"; //Para utilizar el cliente en lugar del servidor
import { useEffect, useState } from 'react';
import "@/public/styles.css";
import Link from 'next/link';

export default function Address() {
    const [address, setAddress] = useState('');

    const handleAddressChange = (e) => {
        const newAddress = e.target.value;
        
        if (newAddress === undefined) {
          throw new Error('Address cannot be undefined.');
        }
      
        setAddress(newAddress);
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
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Tienda</h1>
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
                    <button className="Button" onClick={handleSaveAddress} disabled={isAddressEmpty}>Save Address and Proceed</button>
                </Link>
                    
            </div>

            <div className="footer">
                <h2>Tienda.com</h2>
            </div>
        </div>
    );
}