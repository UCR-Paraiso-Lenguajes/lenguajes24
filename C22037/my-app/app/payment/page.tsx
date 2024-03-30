"use client"; //Para utilizar el cliente en lugar del servidor
import { useEffect, useState } from 'react';
import "@/public/styles.css";
import Link from 'next/link';

export default function Payment() {
    const [selectedMethod, setSelectedMethod] = useState('');
    const [paymentConfirmed, setPaymentConfirmed] = useState(false);

    useEffect(() => {
        localStorage.setItem('selectedMethod', selectedMethod);
    }, [selectedMethod]);
  
    const handleMethodSelect = (method) => {
        setSelectedMethod(method);
        setPaymentConfirmed(false);
    };

    const handleSinpePaymentConfirmation = () => {
        setPaymentConfirmed(true);
    };

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Amazon</h1>
                </Link>
            </div>

            <div className="body">
                <h2>Payment Method</h2>
                <div>
                    <button className="Button" onClick={() => handleMethodSelect('Cash')}>Cash</button>
                    <button className="Button" onClick={() => handleMethodSelect('Sinpe')}>Sinpe</button>
                </div>
                {selectedMethod === 'Cash' && (
                    <div>
                        <p>Purchase number: 00000.</p>
                        <p>Awaiting confirmation from the administrator regarding the payment.</p>
                    </div>
                )}
                {selectedMethod === 'Sinpe' && (
                    <div>
                        <p>Purchase number: 00000.</p>
                        <p>Make the payment through Sinpe to the number 8596-1362.</p>
                        <input type="text" placeholder="Enter the receipt code here" />
                        <button className="Button" onClick={handleSinpePaymentConfirmation}>Confirm purchase</button>
                        {paymentConfirmed && <p>Awaiting confirmation from the administrator regarding the payment.</p>}
                    </div>
                )}
            </div>

            <div className="footer">
                <h2>Amazon.com</h2>
            </div>
        </div>
    );
}