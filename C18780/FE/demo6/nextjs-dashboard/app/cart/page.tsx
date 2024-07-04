'use client'
import React, { useEffect, useState } from 'react';
import { ShoppingCartIcon, ArrowLeftIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import { Cart, Product } from '../utils/data-definitions';
import CardProduct from '../components/CardProduct';
import { findProductsDuplicates, getInitialCartLocalStorage, getProductQuantity, saveInitialCartLocalStorage } from '../utils/utils';
import '../styles/cart.css';
const ProductCart = ({ product, quantity, onRemove }: { product: Product, quantity: number, onRemove: any }) => {
    return (
        <div className="table-responsive">
            <table className="table shoping-cart-table">
                <tbody>
                    <tr>
                        <td width="200">
                            <div>
                                <img src={product.imageUrl}
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
                                <a onClick={() => onRemove(product)} href="#" className="text-muted"><i className="fa fa-trash"></i> Remove item</a>
                            </div>
                        </td>
                        <td>
                            ₡{product.price}
                        </td>
                        <td width="65">
                            <input type="text" className="form-control" placeholder={quantity.toString()} readOnly />
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

const ProductsCart = ({ productCart, onRemove }: { productCart: Product[], onRemove: any }) => {
    const filteredCart = findProductsDuplicates(productCart);

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

const CartSummary = ({ initialCart }: { initialCart: Cart }) => {
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
                    Please contact with us if you have any questions. We are available 24h.
                </span>
            </div>
        </div>
    );
};

const InterestingProducts = ({ products, onAdd }: { products: Product[], onAdd: any }) => {
    const [randomProduct, setRandomProduct] = useState<Product>();
    const randomIndex = Math.floor(Math.random() * products.length);

    useEffect(() => {
        setRandomProduct(products[randomIndex]);
    }, [randomIndex]);

    return (
        <div className="ibox">
            <div className="ibox-content">
                <p className="font-bold">
                    Other products you may be interested
                </p>
                <hr />
                <div className='interested'>
                    {randomProduct && <CardProduct product={randomProduct} onAdd={onAdd} />}
                </div>
            </div>
        </div>
    );
}

export default function MyCart() {
    const [initialCart, setInitialCart] = useState<Cart>(getInitialCartLocalStorage());

    const handleAddToCart = (product: Product) => {
        const updatedCart = { ...initialCart };
        updatedCart.cart.products.push(product);
        updatedCart.cart.subtotal += product.price;
        updatedCart.cart.total = updatedCart.cart.subtotal + updatedCart.cart.subtotal * updatedCart.cart.taxPercentage;
        saveInitialCartLocalStorage(updatedCart);
        setInitialCart(updatedCart);
    }

    const handleRemoveToCart = (product: Product) => {
        const updatedCart = { ...initialCart };
        const productIndex = updatedCart.cart.products.findIndex(item => item.uuid === product.uuid);

        if (productIndex !== -1) {
            updatedCart.cart.products.splice(productIndex, 1);
            updatedCart.cart.subtotal -= product.price;
            updatedCart.cart.total = updatedCart.cart.subtotal + updatedCart.cart.subtotal * updatedCart.cart.taxPercentage;
            saveInitialCartLocalStorage(updatedCart);
            setInitialCart(updatedCart);
        }
    }

    return (
        <div className="container">
            <div className="wrapper wrapper-content animated fadeInRight">
                <div className="row">
                    <div className="col-md-9">
                        <div className="ibox">
                            <div className="ibox-title">
                                <span className="pull-right">{initialCart.cart.products.length} items</span>
                                <h5>Items in your cart</h5>
                            </div>
                            <div className="ibox-content">
                                <div className='flex items-center justify-center p-6 md:w-3/5 md:px-28 md:py-12'>
                                    {initialCart.cart.total === 0 ?
                                        <img src='/others/cat.jpg' width={300} height={300} className="hidden md:block" alt='cat' /> :
                                        <ProductsCart productCart={initialCart.cart.products} onRemove={handleRemoveToCart} />
                                    }
                                </div>
                            </div>
                            <div className="ibox-content">
                                <Link href='/abacaxi'>
                                    <button className="btn btn-white">
                                        <ArrowLeftIcon style={{ width: '20px' }} /> Continue shopping
                                    </button>
                                </Link>
                                <Link href='/checkout'>
                                    <button className="CartBtn" style={{ marginLeft: '40%', display: initialCart.cart.total > 0 ? 'inline-block' : 'none' }}>
                                        <ShoppingCartIcon style={{ width: '20px' }} className='IconContainer'/> Checkout
                                    </button>
                                </Link>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-3">
                        <CartSummary initialCart={initialCart} />
                        <Support />
                    </div>
                </div>
            </div>
        </div>
    );
}
