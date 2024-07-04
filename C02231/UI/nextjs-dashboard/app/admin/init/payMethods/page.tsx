'use client';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import Link from 'next/link';
import '/app/ui/global.css';

const PaymentMethods = () => {
    const [paymentMethods, setPaymentMethods] = useState([]);
    const URL = process.env.NEXT_PUBLIC_API_URL;

    useEffect(() => {
        fetchPaymentMethods();
    }, []);

    const fetchPaymentMethods = async () => {
        try {
            const response = await fetch(`${URL}/active`);
            if (response.ok) {
                const data = await response.json();
                setPaymentMethods(data);
            }
        } catch (error) {
            throw new Error("Error fetching payment methods:", error);
        }
    };

    const disablePaymentMethod = async (id) => {
        try {
            const response = await fetch(`${URL}/api/paymentMethods/${id}/disable`, {
                method: 'POST',
            });
            if (response.ok) {
                console.log(`Payment method with ID ${id} disabled successfully.`);
                fetchPaymentMethods();
            } else {
                console.error(`Failed to disable payment method with ID ${id}.`);
            }
        } catch (error) {
            console.error("Error disabling payment method:", error);
        }
    };

    const activePaymentMethod = async (id) => {
        try {
            const response = await fetch(`${URL}/api/paymentMethods/${id}/active`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            if (response.ok) {
                console.log(`Payment method with ID ${id} toggled successfully.`);
                fetchPaymentMethods();
            } else {
                console.error(`Failed to toggle payment method with ID ${id}.`);
            }
        } catch (error) {
            console.error("Error toggling payment method:", error);
        }
    };

    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-12 d-flex justify-content-end align-items-center">
                        <Link href="/admin/init">
                            <button className="btn btn-dark">GO Back</button>
                        </Link>
                    </div>
                </div>
            </header>
            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-3 bg-custom">
                        <div className="sidebar d-flex flex-column justify-content-between align-items-center" style={{ height: '100vh', marginTop: '200px' }}>
                            <div className="flex-grow-1"></div>
                        </div>
                    </div>
                    <div className="col-md-9">
                        <div className="content">
                            <h2>Manage Payment Methods</h2>
                            <table className="table">
                                <thead>
                                    <tr>
                                        <th>Method Name</th>
                                        <th>Status</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {paymentMethods.map(method => (
                                        <tr key={method.id}>
                                            <td>{method.method_name}</td>
                                            <td>{method.active ? 'Active' : 'Inactive'}</td>
                                            <td>
                                                <button
                                                    className={`btn ${method.active ? 'btn-danger' : 'btn-success'}`}
                                                    onClick={() => activePaymentMethod(method.id)}
                                                >
                                                    'Activate'
                                                </button>
                                                <button
                                                    className="btn btn-warning ml-2"
                                                    onClick={() => disablePaymentMethod(method.id)}
                                                >
                                                    Disable
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <footer className="footer" style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light">Biblioteca de Paula</h5>
                </div>
            </footer>
        </div>
    );
};

export default PaymentMethods;
