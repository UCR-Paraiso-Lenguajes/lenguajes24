'use client'
import React from 'react';
import { Cart, Product } from '../../lib/data-definitions';
import Image from 'next/image';
import { getInitialCartLocalStorage, saveInitialCartLocalStorage } from '../../lib/cart_data_localeStore';
import { ShoppingCartIcon, ArrowLeftIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import ProductItem from '../product';
import { useEffect, useState } from 'react';
import useFetchInitialStore from '@/app/api/http.initialStore';
import { findProductsDuplicates, getProductQuantity } from '../../lib/utils';

const ProductCart = ({ product, quantity, onRemove }: { product: Product, quantity: number, onRemove: any }) => {
    return (
        <div className="table-responsive">
            <table className="table shoping-cart-table">
                <tbody>
                    <tr>
                        <td width="200">
                            <div>
                                <Image src={product.imageUrl}
                                    width={150}
                                    height={150}
                                    className="hidden md:block" alt={product.name} />
                            </div>
                        </td>
                        <td className="desc">
                            <h3>
                                <a href="#" className="text-navy">
                                    {product.name}
                                </a>
                            </h3>
                            <dl className="small m-b-none">
                                <dt>Description lists</dt>
                                {product.description}
                            </dl>
                            <div className="m-t-sm">
                                <a onClick={() => onRemove({ product })} href="#" className="text-muted"><i className="fa fa-trash"></i> Remove item</a>
                            </div>
                        </td>
                        <td>
                            ₡{product.price}
                        </td>
                        <td width="65">
                            <input type="text" className="form-control" placeholder={quantity.toString()} />
                        </td>
                        <td>
                            <h4>
                                ₡{product.price * quantity}
                            </h4>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};

const ProductsCart = ({ productCart, onRemove }: { productCart: Product[] | null, onRemove: any }) => {
    let filteredCart: Product[] = [];
    if (productCart) {
        filteredCart = findProductsDuplicates(productCart);
    } else {
        return (<></>);
    }

    return (
        <div className='container'>
            <div>
                {filteredCart.map((item, index) =>
                    <ProductCart key={index} product={item} quantity={getProductQuantity(item, productCart)} onRemove={onRemove} />
                )}
            </div>
        </div>
    );
};

const CartSummary = ({ initialCart }: { initialCart: Cart | null }) => {
    if (!initialCart) {
        return (<></>);
    }
    return (
        <div className="ibox">
            <div className="ibox-title">
                <h5>Cart Summary</h5>
            </div>
            <div className="ibox-content">
                <span>
                    SubTotal
                </span>
                <h2 className="font-bold">
                    ₡{initialCart.cart.subtotal}
                </h2>
                <span>
                    Total
                </span>
                <h2 className="font-bold">
                    ₡{initialCart.cart.total}
                </h2>

            </div>
        </div>
    );
};

const Support = () => {
    return (
        <div className="ibox">
            <div className="ibox-title">
                <h5>Support</h5>
            </div>
            <div className="ibox-content text-center">
                <h3><i className="fa fa-phone"></i> +506 8888-8888</h3>
                <span className="small">
                    Please contact with us if you have any questions. We are avalible 24h.
                </span>
            </div>
        </div>
    );
};

const InterestingProducts = ({ products, onAdd }: { products: Product[] | null, onAdd: any }) => {
    if (!products) {
        return (<></>);
    }

    const [randomProduct, setRandomProduct] = useState<Product>();
    const randomIndex = Math.floor(Math.random() * products.length);

    useEffect(() => {
        setRandomProduct(products[5]);
    }, [randomIndex]);

    return (
        <div className="ibox">
            <div className="ibox-content" >

                <p className="font-bold">
                    Other products you may be interested
                </p>
                <hr />
                <div className='interested'>
                    {randomProduct && <ProductItem product={randomProduct} onAdd={onAdd} />}
                </div>
            </div>
        </div>
    );
}

export default function MyCart() {
    const category: string[] = ["All"];
    const search = "none";
    const initialStore = useFetchInitialStore({ category, search });
    const [initialCart, setInitialCart] = useState<Cart>();
    useEffect(() => {
        const cartFromLocalStorage = getInitialCartLocalStorage();
        setInitialCart(cartFromLocalStorage);
    }, []);
    const handleAddToCart = ({ product }: { product: Product }) => {
        if (initialCart) {
            initialCart.cart.products.push(product);
            initialCart.cart.subtotal = initialCart.cart.subtotal + product.price;
            initialCart.cart.total = initialCart.cart.subtotal + initialCart.cart.subtotal * initialCart.cart.taxPercentage;
            saveInitialCartLocalStorage(initialCart);
        }
    }
    const handleRemoveToCart = ({ product }: { product: Product }) => {
        if (initialCart) {
            initialCart.cart.subtotal = initialCart.cart.subtotal - product.price;
            initialCart.cart.total = initialCart.cart.subtotal + initialCart.cart.subtotal * initialCart.cart.taxPercentage;
            initialCart.cart.products.splice(initialCart.cart.products.findIndex(item => item.uuid === product.uuid), 1);
            saveInitialCartLocalStorage(initialCart);
        }
    }
    return (
        <div className="container">
            <div className="wrapper wrapper-content animated fadeInRight">
                <div className="row">

                    <div className="col-md-9">
                        <div className="ibox">
                            <div className="ibox-title">
                                <span className="pull-right">{initialCart ? initialCart.cart.products.length : 0} items</span>
                                <h5>Items in your cart</h5>
                            </div>

                            <div className="ibox-content">
                                <div className='flex items-center justify-center p-6 md:w-3/5 md:px-28 md:py-12'>
                                    {initialCart?.cart.total === 0 ? <Image src='/others/cat.jpg'
                                        width={300}
                                        height={300}
                                        className="hidden md:block" alt='cat' /> : <ProductsCart productCart={initialCart ? initialCart.cart.products : null} onRemove={handleRemoveToCart} />}
                                </div>


                            </div>

                            <div className="ibox-content">
                                <Link href='/dashboard'>
                                    <button className="btn btn-white"> <ArrowLeftIcon style={{ width: '20px' }} /> Continue shopping</button>
                                </Link>
                                <Link href='/dashboard/checkout'>
                                    <button className="btn btn-primary pull-right" style={{ marginLeft: '40%', display: initialCart && initialCart.cart.total > 0 ? 'inline-block' : 'none' }}><ShoppingCartIcon style={{ width: '20px' }} /> Checkout</button>
                                </Link>
                            </div>
                        </div>
                    </div>

                    <div className="col-md-3">
                        <CartSummary initialCart={initialCart ? initialCart : null} />

                        <Support />

                        <InterestingProducts onAdd={handleAddToCart} products={initialStore} />
                    </div>
                </div>
            </div>
        </div>
    );
}