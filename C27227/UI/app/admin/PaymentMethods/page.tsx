"use client";
import React, { useState, useEffect } from "react";
import "@/app/ui/Styles/adminPaymentMethods.css";
import Sidebar from "../init/page";
interface PaymentMethod {
  paymentType: number;
  isEnabled: boolean;
}

const AdminPaymentMethods: React.FC = () => {
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>("");

  const URLConection = process.env.NEXT_PUBLIC_API as string;

  useEffect(() => {
    const fetchPaymentMethods = async () => {
      try {
        const response = await fetch(`${URLConection}/api/store/paymentMethods`);
        if (response.ok) {
          const data = await response.json();
          setPaymentMethods(data);
        } else {
          throw new Error("Error fetching payment methods");
        }
      } catch (error) {
        setError(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchPaymentMethods();
  }, [URLConection]);

  const togglePaymentMethod = async (method: PaymentMethod) => {
    setLoading(true);
    try {
      const response = await fetch(`${URLConection}/api/store/paymentMethods/disable`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ paymentType: method.paymentType, isEnabled: !method.isEnabled }),
      });
  
      if (response.ok) {
        setPaymentMethods((prevMethods) =>
          prevMethods.map((m) =>
            m.paymentType === method.paymentType ? { ...m, isEnabled: !m.isEnabled } : m
          )
        );
      } else {
        throw new Error("Error updating payment method");
      }
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };
  

  const getPaymentMethodName = (type: number) => {
    switch (type) {
      case 0:
        return "Efectivo";
      case 1:
        return "Sinpe";
      default:
        return "Desconocido";
    }
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div className="error-message">{error}</div>;
  }

  return (
    <div>
      <Sidebar/>
    <div className="admin-payment-methods">
      <h2>Administración de Métodos de Pago</h2>
      <table className="payment-methods-table">
        <thead>
          <tr>
            <th>Método de Pago</th>
            <th>Estado</th>
          </tr>
        </thead>
        <tbody>
          {paymentMethods.map((method) => (
            <tr key={method.paymentType}>
              <td>{getPaymentMethodName(method.paymentType)}</td>
              <td>
                <input
                  type="checkbox"
                  checked={method.isEnabled}
                  onChange={() => togglePaymentMethod(method)}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    </div>
  );
};

export default AdminPaymentMethods;
