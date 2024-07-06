'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const PaymentMethods = () => {
    const [methods, setMethods] = useState(() => {
        const storedMethods = localStorage.getItem('PaymentType');
        return storedMethods ? JSON.parse(storedMethods) : [];
    });
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);

    const URL = process.env.NEXT_PUBLIC_API_URL;
    if (!URL) {
        throw new Error('NEXT_PUBLIC_API_URL is not defined');
    }

    useEffect(() => {
        if (methods.length === 0) {
            const fetchMethods = async () => {
                try {
                    const response = await fetch(`${URL}/api/PaymentMethods`);
                    if (!response.ok) {
                        throw new Error('Failed to fetch payment methods');
                    }
                    const json = await response.json();
                    setMethods(json);
                    localStorage.setItem('paymentMethods', JSON.stringify(json));
                    setIsLoading(false);
                } catch (error) {
                    setError(error.message);
                    setIsLoading(false);
                }
            };

            fetchMethods();
        } else {
            setIsLoading(false);
        }
    }, [URL, methods]);

    const handleToggle = async (id, enabled) => {
        const paymentUpdate = {
            paymentType: id,
            enabled
        };
    
        try {
            const response = await fetch(`${URL}/api/PaymentMethods`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(paymentUpdate)
            });
    
            if (!response.ok) {
                throw new Error('Failed to update payment method');
            }
    
            const updatedMethods = methods.map(method => method.paymentType === id ? { ...method, enabled: enabled } : method);
            setMethods(updatedMethods);
            localStorage.setItem('paymentMethods', JSON.stringify(updatedMethods));
        } catch (error) {
            setError(error.message);
        }
    };

    const getPaymentMethodName = (paymentType) => {
        switch (paymentType) {
            case 0:
                return 'Cash';
            case 1:
                return 'Sinpe';
            default:
                return 'Unknown';
        }
    };

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <div className="container mt-5">
            <h2 className="mb-4">Payment Methods</h2>
            <ul className="list-group">
                {methods.map(method => (
                    <li key={method.paymentType} className="list-group-item d-flex justify-content-between align-items-center">
                        <span>{getPaymentMethodName(method.paymentType)}</span> {/* Usamos la función para obtener el nombre */}
                        <div>
                            <button
                                className={`btn ${method.enabled ? 'btn-success' : 'btn-secondary'}`}
                                onClick={() => handleToggle(method.paymentType, !method.enabled)}
                            >
                                {method.enabled ? 'Enabled' : 'Disabled'}
                            </button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default PaymentMethods;
