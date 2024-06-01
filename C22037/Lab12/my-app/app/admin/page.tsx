"use client"; // Para utilizar el cliente en lugar del servidor
import { useState } from 'react';
import { useRouter } from 'next/navigation';
import "@/public/styles.css";
import Link from 'next/link';
import { jwtDecode } from 'jwt-decode';
import { Alert } from 'react-bootstrap';

export default function Admin() {
    const router = useRouter();
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });
    const [errorMessage, setErrorMessage] = useState(null);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
    
        const { email, password } = formData;
    
        if (!email || !password) {
            setErrorMessage('Please enter the email and password.');
            return;
        }
    
        try {
            const response = await fetch('https://localhost:7067/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ User: email, Password: password })
            });
    
            if (response.ok) {
                const data = await response.json();
                const decodedToken = jwtDecode(data.token);
                const userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
                
                if (userRole === "Admin") {
                    sessionStorage.setItem('token', data.token);
                    router.push("/admin/init");
                } else {
                    setErrorMessage('Access Denied: Only admins can log in here.');
                }
            } else {
                setErrorMessage('Login failed');
            }
        } catch (error) {
            setErrorMessage('An error occurred during login');
        }
    };

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Tienda</h1>
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