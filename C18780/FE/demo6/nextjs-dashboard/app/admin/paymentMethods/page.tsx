'use client'
import { useEffect, useState } from 'react';
import { useFetchGetPaymentMethods, useFetchUpdatePaymentIsEnabled, useSignalRGetPaymentMethods } from "@/app/api/http.initialStore";
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import 'bootstrap/dist/css/bootstrap.min.css';
import './../../styles/switchYesNo.css';

export default function PaymentMethods() {
    const initialPaymentMethods = useFetchGetPaymentMethods();
    const [index, setIndex] = useState<number | null>(null);
    useFetchUpdatePaymentIsEnabled(index);
    const { paymentMethods: signalRPaymentMethods } = useSignalRGetPaymentMethods();

    const currentPaymentMethods = signalRPaymentMethods.length > 0 ? signalRPaymentMethods : initialPaymentMethods;

    function handleChangeIsEnabled(paymentType: number) {
        setIndex(paymentType);
    }

    useEffect(() => {
        if (index !== null) {
            setIndex(null);
        }
    }, [index]);

    return (
        <div className="container">
            <div className="row">
                {currentPaymentMethods.map((paymentMethod, index) => (
                    <div className="col-md-4" key={index}>
                        <div className="card mb-4 shadow-sm">
                            <div className="card-header">
                                <div className="container">
                                    <div className="row">
                                        <div className="col-md-8">
                                            <h4 className="my-0 fw-normal">{paymentMethod.name}</h4>
                                        </div>
                                        <div className="col-md-4">
                                            <button className="button" onClick={() => handleChangeIsEnabled(paymentMethod.paymentType)}>
                                                {paymentMethod.isEnabled ? "true" : "false"}
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}
