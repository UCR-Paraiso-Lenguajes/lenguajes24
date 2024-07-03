'use client'
import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/globals.css';
const URL = process.env.NEXT_PUBLIC_API;

interface PaymentMethod {
  paymentType: number;
  isActive: boolean;
}

const PaymentMethods: React.FC = () => {
  const [paymentMethods, setPaymentMethods] = useState<{ [key: string]: PaymentMethod }>({});

  useEffect(() => {
    fetch(URL+'/api/Payment/payment-methods') // Update with your actual endpoint
      .then(response => response.json())
      .then(data => {
        const methods = Object.keys(data).reduce((acc, key) => {
          acc[key] = { ...data[key], isActive: true }; // Assuming all methods are active initially
          return acc;
        }, {} as { [key: string]: PaymentMethod });
        setPaymentMethods(methods);
      });
  }, []);

  const toggleActive = (method: string) => {
    const updatedMethods = {
      ...paymentMethods,
      [method]: {
        ...paymentMethods[method],
        isActive: !paymentMethods[method].isActive,
      },
    };
    setPaymentMethods(updatedMethods);

    fetch(URL+'/api/Payment/update-status', { // Update with your actual endpoint
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        method,
        state: updatedMethods[method].isActive ? 'activo' : 'desactivado',
      }),
    });
  };

  return (
    <div className="container mt-4">
      <h1 className="text-center">Payment Methods</h1>
      <table className="table table-striped">
        <thead>
          <tr>
            <th>Method</th>
            <th>Type</th>
            <th>Status</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {Object.keys(paymentMethods).map(method => (
            <tr key={method}>
              <td>{method}</td>
              <td>{paymentMethods[method].paymentType}</td>
              <td>{paymentMethods[method].isActive ? 'Active' : 'Inactive'}</td>
              <td>
                <button
                  className="btn btn-sm btn-primary"
                  onClick={() => toggleActive(method)}
                >
                  {paymentMethods[method].isActive ? 'Deactivate' : 'Activate'}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default PaymentMethods;
