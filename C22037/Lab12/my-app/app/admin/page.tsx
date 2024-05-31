"use client"; // Para utilizar el cliente en lugar del servidor
import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import "@/public/styles.css";
import Link from 'next/link';
import { jwtDecode } from 'jwt-decode';

export default function Admin() {
    const router = useRouter();
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        if (!name || !value) {
            throw new Error("Error, name and value are required.");
        }
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
    
        const email = formData.email;
        const password = formData.password;
    
        if (!email || !password) {
            throw new Error("Please enter the email and password.");
        }
    
        const response = await fetch('https://localhost:7067/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ User: email, Password: password })
        });
    
        if (response.ok) {
            const data = await response.json();
            sessionStorage.setItem('token', data.token);
            router.push("/admin/init");
        } else {
            alert('Login failed');
        }
    };    

    useEffect(() => {
        const checkTokenValidity = () => {
            const token = sessionStorage.getItem('token');
            if (token) {
                const decodedToken = jwtDecode(token);
                const exp = decodedToken.exp * 1000;
                if (Date.now() >= exp) {
                    sessionStorage.removeItem('token');
                    router.push('/admin');
                }
            } else {
                router.push('/admin');
            }
        };

        checkTokenValidity();
        const interval = setInterval(checkTokenValidity, 60000);

        return () => clearInterval(interval);
    }, [router]);

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Tienda</h1>
                </Link>
            </div>

            <div className="body">
                <h2>Log In</h2>
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