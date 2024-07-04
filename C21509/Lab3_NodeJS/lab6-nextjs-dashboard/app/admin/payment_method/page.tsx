"use client";
import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import VerifyToken, { useTokenContext } from '@/app/components/verify_token';
import { useRouter } from 'next/navigation';

interface PaymentMethod {
    paymentMethodId: number;
    paymentMethodName: string;
    isEnabled: boolean;
}

const PaymentMethodsAdmin: React.FC = () => {
    const { isValidToken, isVerifying } = useTokenContext();
    const router = useRouter();
    const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const fetchPaymentMethods = async () => {
        try {
            const response = await fetch('https://localhost:7165/api/paymentmethod');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data: PaymentMethod[] = await response.json();
            setPaymentMethods(data);
            localStorage.setItem('paymentMethods', JSON.stringify(data));
        } catch (error) {
            setError('Error al cargar los métodos de pago');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isValidToken && !isVerifying) {
            fetchPaymentMethods();
        }
    }, [isValidToken, isVerifying]);

    useEffect(() => {
        if (!isValidToken && !isVerifying) {
            router.push("/../admin");
        }
    }, [isValidToken, isVerifying, router]);

    const togglePaymentMethod = async (id: number, isEnabled: boolean) => {
        try {
            const response = await fetch(`https://localhost:7165/api/paymentmethod/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(!isEnabled),
            });
            if (!response.ok) {
                throw new Error('Failed to update payment method status');
            }
            fetchPaymentMethods();
        } catch (error) {
            setError('Error al actualizar el estado del método de pago');
        }
    };

    if (isVerifying || !isValidToken) {
        return <p></p>;
    }

    return (
        <div className="container mt-5">
            <h2 className="mb-4">Administrar Métodos de Pago</h2>
            {loading ? (
                <div>Cargando...</div>
            ) : error ? (
                <div className="alert alert-danger">{error}</div>
            ) : (
                <table className="table table-striped table-bordered">
                    <thead className="thead-dark">
                        <tr>
                            <th>ID</th>
                            <th>Método de Pago</th>
                            <th>Estado</th>
                            <th>Acción</th>
                        </tr>
                    </thead>
                    <tbody>
                        {paymentMethods.map((method) => (
                            <tr key={method.paymentMethodId}>
                                <td>{method.paymentMethodId}</td>
                                <td>{method.paymentMethodName}</td>
                                <td>{method.isEnabled ? 'Habilitado' : 'Deshabilitado'}</td>
                                <td>
                                    <button 
                                        className={`btn ${method.isEnabled ? 'btn-danger' : 'btn-success'}`} 
                                        onClick={() => togglePaymentMethod(method.paymentMethodId, method.isEnabled)}
                                    >
                                        {method.isEnabled ? 'Deshabilitar' : 'Habilitar'}
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

const WrappedPaymentMethodsAdmin = () => (
    <VerifyToken>
        <PaymentMethodsAdmin />
    </VerifyToken>
);

export default WrappedPaymentMethodsAdmin;