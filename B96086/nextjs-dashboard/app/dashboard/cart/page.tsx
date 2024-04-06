'use client'
import { InitialStore, Product } from '../../lib/products-data-definitions';
import Image from 'next/image';
import useInitialStore from '../../lib/products-data';
import { ShoppingCartIcon, ArrowLeftIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import ProductItem from '../product';
import { useEffect, useState } from 'react';
const ProductCart = ({ product }: { product: Product }) => {
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
                                <a href="#" className="text-muted"><i className="fa fa-gift"></i> Add gift package</a>
                                |
                                <a href="#" className="text-muted"><i className="fa fa-trash"></i> Remove item</a>
                            </div>
                        </td>
                        <td>
                            ₡{product.price}
                        </td>
                        <td width="65">
                            <input type="text" className="form-control" placeholder={"1"} />
                        </td>
                        <td>
                            <h4>
                                ₡{product.price}
                            </h4>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};
const ProductsCart = ({initialStore}:{initialStore:InitialStore}) => {

    return (
        <div className='container'>
            <div>
                {initialStore.cart.products.map((item, index) =>
                    <ProductCart key={index} product={item} />
                )}
            </div>
        </div>
    );
};
const CartSummary = ({initialStore}:{initialStore:InitialStore}) => {
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
                    ₡{initialStore.cart.subtotal}
                </h2>
                <span>
                    Total
                </span>
                <h2 className="font-bold">
                    ₡{initialStore.cart.total}
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

const InterestingProducts = ({ products, onAdd }: { products: Product[], onAdd: any }) => {
    const [randomProduct, setRandomProduct] = useState<Product>();
    const randomIndex = Math.floor(Math.random() * products.length);
    useEffect(() => {
        setRandomProduct(products[randomIndex]);
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

export default function Cart() {
    const initialStore = useInitialStore();
    const handleAddToCart = ({ product }: { product: Product }) => {
        initialStore.cart.products.push(product);

        initialStore.cart.subtotal = initialStore.cart.subtotal + product.price;

        initialStore.cart.total = initialStore.cart.subtotal + initialStore.cart.subtotal * initialStore.cart.taxPercentage;
    };

    return (
        <div className="container">
            <div className="wrapper wrapper-content animated fadeInRight">
                <div className="row">

                    <div className="col-md-9">
                        <div className="ibox">
                            <div className="ibox-title">
                                <span className="pull-right">(<strong>{initialStore.cart.products ? initialStore.cart.products.length : 0}</strong>) items</span>
                                <h5>Items in your cart</h5>
                            </div>

                            <div className="ibox-content">
                                <div className='flex items-center justify-center p-6 md:w-3/5 md:px-28 md:py-12'>
                                    {initialStore.cart.total === 0 ? <Image src='/others/cat.jpg'
                                        width={300}
                                        height={300}
                                        className="hidden md:block" alt='cat' /> : <ProductsCart initialStore={initialStore} />}
                                </div>


                            </div>

                            <div className="ibox-content">
                                <Link href='/dashboard'>
                                    <button className="btn btn-white"> <ArrowLeftIcon style={{ width: '20px' }} /> Continue shopping</button>
                                </Link>
                                <Link href='/dashboard/checkout'>
                                    <button className="btn btn-primary pull-right" style={{ marginLeft: '40%', display: initialStore.cart.total > 0 ? 'inline-block' : 'none' }}><ShoppingCartIcon style={{ width: '20px' }} /> Checkout</button>
                                </Link>
                            </div>
                        </div>
                    </div>

                    <div className="col-md-3">
                        <CartSummary initialStore={initialStore} />

                        <Support />

                        <InterestingProducts onAdd={handleAddToCart} products={initialStore.products} />
                    </div>
                </div>
            </div>
        </div>
    );
}