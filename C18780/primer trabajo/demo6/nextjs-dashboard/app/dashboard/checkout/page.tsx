'use client'
import React, { useEffect, useState } from 'react';
import { Cart, Product } from '../../lib/data-definitions';
import Link from 'next/link';
import Modal from '../modal';
import ModalInput from '../modalInput';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { ArrowLeftIcon } from '@heroicons/react/24/outline';
import { deleteCartLocalStorage, getInitialCartLocalStorage, saveInitialCartLocalStorage } from '@/app/lib/cart_data_localeStore';
import { findProductsDuplicates, getProductQuantity } from '@/app/lib/utils';
import useFetchCartPurchase from '@/app/api/http.cart';
import { useFetchSinpePurchase } from '@/app/api/http.sinpe';
const ListProducts = ({ product, quantity }: { product: Product, quantity: number }) => {
    return (
        <tr>
            <th scope="row"><img src={product.imageUrl} alt="product-img" title="product-img" className="avatar-lg rounded" /></th>
            <td>
                <h5 className="font-size-16 text-truncate"><a href="#" className="text-dark">{product.name}</a></h5>
                <p className="text-muted mb-0">
                    <i className="bx bxs-star text-warning"></i>
                    <i className="bx bxs-star text-warning"></i>
                    <i className="bx bxs-star text-warning"></i>
                    <i className="bx bxs-star text-warning"></i>
                    <i className="bx bxs-star-half text-warning"></i>
                </p>
                <p className="text-muted mb-0 mt-1">$ {product.price} x {quantity}</p>
            </td>
            <td>₡{product.price * quantity}</td>
        </tr>
    );
}

const OrderSummary = ({ initialCart }: { initialCart: Cart }) => {
    const initialCartFilter = findProductsDuplicates(initialCart.cart.products);
    return (
        <div className="card-body">
            <div className="p-3 bg-light mb-3">
                <h5 className="font-size-16 mb-0">Order Summary <span className="float-end ms-2">#M09090</span></h5>
            </div>
            <div className="table-responsive">
                <table className="table table-centered mb-0 table-nowrap">
                    <thead>
                        <tr>
                            <th className="border-top-0" style={{ width: '110px' }} scope="col">Product</th>
                            <th className="border-top-0" scope="col">Description</th>
                            <th className="border-top-0" scope="col">Quantity</th>
                        </tr>
                    </thead>
                    <tbody>
                        {initialCartFilter.map((item, index) =>
                            <ListProducts key={index} product={item} quantity={getProductQuantity(item, initialCart.cart.products)} />
                        )}

                        <tr>
                            <td colSpan={2}>
                                <h5 className="font-size-14 m-0">Sub Total :</h5>
                            </td>
                            <td>
                                ₡{initialCart.cart.subtotal}
                            </td>
                        </tr>

                        <tr>
                            <td colSpan={2}>
                                <h5 className="font-size-14 m-0">Estimated Tax :</h5>
                            </td>
                            <td>
                                ₡{initialCart.cart.subtotal * initialCart.cart.taxPercentage}
                            </td>
                        </tr>

                        <tr className="bg-light">
                            <td colSpan={2}>
                                <h5 className="font-size-14 m-0">Total:</h5>
                            </td>
                            <td>
                                ₡{initialCart.cart.total}
                            </td>
                        </tr>
                    </tbody>
                </table>

            </div>
        </div>
    );
};

const BillingInfo = ({ onAddress }: { onAddress: any }) => {

    const handleChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        const newAddress = e.target.value;
        onAddress(newAddress);
    };
    return (
        <div className="feed-item-list">
            <div>
                <h5 className="font-size-16 mb-1">Billing Info</h5>
                <p className="text-muted text-truncate mb-4">Delivery address</p>
                <div className="mb-3">
                    <form>
                        <div className="mb-3">
                            <label className="form-label" htmlFor="billing-address">Address</label>
                            <textarea className="form-control" id="billing-address" rows={3} placeholder="Enter full address" onChange={handleChange}></textarea>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

const Payment = ({ onSelectPayment }: { onSelectPayment: any }) => {
    const handlePaymentSelection = (paymentOption: number) => {
        onSelectPayment(paymentOption);
    };

    return (
        <div className="feed-item-list">
            <div>
                <h5 className="font-size-16 mb-1">Payment Info</h5>
                <p className="text-muted text-truncate mb-4">Choose how you want to pay</p>
            </div>
            <div>
                <h5 className="font-size-14 mb-3">Payment method :</h5>
                <div className="row">
                    <div className="col-lg-3 col-sm-6">
                        <div data-bs-toggle="collapse">
                            <label className="card-radio-label">
                                <input type="radio" name="pay-method" id="pay-methodoption1" className="card-radio-input" />
                                <span className="card-radio py-3 text-center text-truncate" onClick={() => handlePaymentSelection(0)}>
                                    Cash
                                </span>
                            </label>
                        </div>
                    </div>

                    <div className="col-lg-3 col-sm-6" >
                        <div>
                            <label className="card-radio-label">
                                <input type="radio" name="pay-method" id="pay-methodoption3" className="card-radio-input" checked={undefined} />
                                <span className="card-radio py-3 text-center text-truncate" onClick={() => handlePaymentSelection(1)}>
                                    Sinpe Movil
                                </span>
                            </label>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    );
};

export default function Checkout() {
    const initialCart = getInitialCartLocalStorage();
    const [payment, setPayment] = useState<number>();
    const [text, setText] = useState<string>("");
    const [inputAddress, setInputAddress] = useState('');
    const [uuidSales, setUuidSales] = useState("");
    const [confirmationNumber, setconfirmationNumber] = useState("");
    const payMethod = {
        cash: 0,
        sinpe: 1
    }

    const updatePayment = (paymentOption: number) => {
        setPayment(paymentOption);
        if (paymentOption === payMethod.cash) {
            setText('Dear customer, \nplease wait for our administrator to confirm your method of payment.\nThank you very much for choosing us');

        } else {
            setText('Dear customer, \nour Sinpe Movil number is the following: \n70790629\nThank you very much for choosing us');

        }
    };

    const onAddress = (address: string) => {
        setInputAddress(address);
        initialCart.cart.deliveryAddress = address;
        saveInitialCartLocalStorage(initialCart);
    };

    const purchase = async () => {
        if (payment !== undefined)
            initialCart.cart.methodPayment = payment;
        saveInitialCartLocalStorage(initialCart);
        setUuidSales(await useFetchCartPurchase());
        if (payment === 0) {
            deleteCartLocalStorage();
        }
    }

    const sinpePurchase = async (confirmationNumber: string) => {
        setconfirmationNumber(confirmationNumber);
        try {
            await useFetchSinpePurchase(uuidSales, confirmationNumber);
        } catch (exeption) {

        } finally {
            deleteCartLocalStorage();
        }
    }

    return (
        <div className="container">

            <div className="row">
                <div className="col-xl-8">

                    <div className="card">
                        <div className="card-body">
                            <ol className="activity-checkout mb-0 px-4 mt-3">
                                <li className="checkout-item">
                                    <div className="avatar checkout-icon p-1">
                                        <div className="avatar-title rounded-circle bg-primary">
                                            <i className="bx bxs-receipt text-white font-size-20"></i>
                                        </div>
                                    </div>

                                    <BillingInfo onAddress={onAddress} />

                                </li>

                                <li className="checkout-item">
                                    <div className="avatar checkout-icon p-1">
                                        <div className="avatar-title rounded-circle bg-primary">
                                            <i className="bx bxs-wallet-alt text-white font-size-20"></i>
                                        </div>
                                    </div>
                                    <Payment onSelectPayment={updatePayment} />
                                </li>
                            </ol>
                        </div>
                    </div>

                    {/*Button */}
                    <div className="row my-4">
                        <div className="col">
                            <Link href='/dashboard'>
                                <button className="btn btn-white"> <ArrowLeftIcon style={{ width: '20px' }} /> Continue shopping</button>
                            </Link>
                        </div>
                        <div className="col">
                            <div className="text-end mt-2 mt-sm-0">
                                <i className="mdi mdi-cart-outline me-1"></i>
                                <button type="button" className="btn btn-success" data-bs-toggle="modal" data-bs-target="#staticBackdrop" disabled={payment !== undefined && inputAddress !== "" && initialCart.cart.total !== 0 ? false : true} onClick={purchase}>
                                    PROCCED
                                </button>
                                {payment === payMethod.cash
                                    ? <Modal title={'#' + uuidSales} text={text} />
                                    : <ModalInput title={'#' + uuidSales} text={text} onInput={sinpePurchase} />}
                            </div>
                        </div>
                    </div>
                </div>

                <div className="col-xl-4">
                    <div className="card checkout-order-summary">
                        <OrderSummary initialCart={initialCart} />
                    </div>
                </div>
            </div>
        </div>
    );
}