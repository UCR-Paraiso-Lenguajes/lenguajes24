"use client"; // Para utilizar el cliente en lugar del servidor
import { useEffect, useState } from 'react';
import "../../public/styles.css";
import Link from 'next/link';

const countries = [
    "Costa Rica",
    "United States",
    "Mexico",
    "Canada",
];

export default function Address() {
    const [address, setAddress] = useState('');
    const [country, setCountry] = useState('');
    const [isValid, setIsValid] = useState(true);
    const [errorMessage, setErrorMessage] = useState('');

    const handleAddressChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newAddress = e.target.value;

        if (newAddress === undefined) {
            throw new Error('Address cannot be undefined.');
        }

        setAddress(newAddress);
        validateAddress(newAddress, country);
    };

    const handleCountryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const newCountry = e.target.value;
        setCountry(newCountry);
        validateAddress(address, newCountry);
    };

    const validateAddress = (address: string, country: string) => {
        const regex = /^[a-zA-Z0-9\s,.-]+$/;
        const isValidAddress = regex.test(address) && address.length >= 10 && country !== '';

        setIsValid(isValidAddress);

        if (!isValidAddress) {
            setErrorMessage('The address must be at least 10 characters long, include only valid characters (letters, numbers, spaces, commas, periods, and hyphens), and a country must be selected.');
        } else {
            setErrorMessage('');
        }
    };

    const handleSaveAddress = () => {
        if (address.trim() !== '' && country !== '') {
            localStorage.setItem('address', address);
            localStorage.setItem('country', country);
        }
    };

    useEffect(() => {
        const storedAddress = localStorage.getItem('address');
        const storedCountry = localStorage.getItem('country');
        if (storedAddress) {
            setAddress(storedAddress);
        }
        if (storedCountry) {
            setCountry(storedCountry);
        }
    }, []);

    const isAddressEmpty = address.trim() === '';

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Store</h1>
                </Link>
            </div>

            <div className="body">
                <h2>Delivery Address</h2>
                <form>
                    <label htmlFor="country">Country:</label>
                    <select id="country" name="country" value={country} onChange={handleCountryChange}>
                        <option value="">Select a country</option>
                        {countries.map((country) => (
                            <option key={country} value={country}>{country}</option>
                        ))}
                    </select>
                    <br />
                    <label htmlFor="address">Address:</label>
                    <input
                        type="text"
                        id="address"
                        name="address"
                        value={address}
                        onChange={handleAddressChange}
                        className={isValid ? '' : 'input-error'}
                    />
                    {!isValid && <p className="error-message">{errorMessage}</p>}
                </form>
                <Link href="/payment">
                    <button className="Button" onClick={handleSaveAddress} disabled={isAddressEmpty || !isValid || country === ''}>
                        Save Address and Proceed
                    </button>
                </Link>
            </div>

            <div className="footer">
                <h2>Store.com</h2>
            </div>
        </div>
    );
}