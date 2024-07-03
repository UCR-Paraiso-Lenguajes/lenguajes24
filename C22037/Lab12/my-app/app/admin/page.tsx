"use client"; // Para utilizar el cliente en lugar del servidor
import { useState, useEffect } from 'react';
import Link from 'next/link';
import "../../public/styles.css";
import "react-datepicker/dist/react-datepicker.css";
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'next/navigation';
import { Alert } from 'react-bootstrap';

interface JwtPayload {
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: string;
}

export default function Admin() {
    const router = useRouter();
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const URL = process.env.NEXT_PUBLIC_API_URL;
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const { email, password } = formData;

        if (!email || !password) {
            setErrorMessage("Please enter the email and password.");
            return;
        }

        try {
            const response = await fetch(URL + '/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ User: email, Password: password })
            });

            if (response.ok) {
                const data = await response.json();
                const decodedToken = jwtDecode<JwtPayload>(data.token);
                const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                
                if (userRole === "Admin") {
                    sessionStorage.setItem('token', data.token);
                    router.push("/admin/init");
                } else {
                    setErrorMessage("Access Denied: Only admins can log in here.");
                }
            } else {
                setErrorMessage("Login failed");
            }
        } catch (error) {
            setErrorMessage("An error occurred during login.");
        }
    };

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Store</h1>
                </Link>
            </div>

            <div className="body">
                <h2>Log In</h2>
                {errorMessage && <Alert variant="danger">{errorMessage}</Alert>}
                <form onSubmit={handleSubmit}>
                    <div>
                        <label htmlFor="email">Email:</label>
                        <input
                            type="email"
                            id="email"
                            name="email"
                            value={formData.email}
                            onChange={handleInputChange}
                            required
                        />
                    </div>
                    <div>
                        <label htmlFor="password">Password:</label>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            value={formData.password}
                            onChange={handleInputChange}
                            required
                        />
                    </div>
                    <button className='Button'>Log in</button>
                </form>
            </div>

            <div className="footer">
                <h2>Tienda.com</h2>
            </div>
        </div>
    );
}