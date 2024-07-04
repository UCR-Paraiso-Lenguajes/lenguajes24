'use client'
import { useEffect, useState } from "react";
import {useFetchInitialStore} from "../api/http.initialStore";
import { getInitialCartLocalStorage, saveInitialCartLocalStorage } from "../utils/utils";
import { useRouter, useSearchParams } from 'next/navigation';
import { Category, Product } from "../utils/data-definitions";
import React from "react";
import CardProduct from "../components/CardProduct";
import '../styles/product.css';
import Navbar from "../components/Navbar/Navbar";
import Footer from "../components/Footer";
import Carousel from "../components/Carousel";
import '../styles/hourglass.css';

const ProductsRow = ({ products, onAdd }: { products: Product[], onAdd: any }) => {
    let number = (products.length - (products.length % 4)) / 2;
    return (
        <div className='containerProducts'>
            <div className="row">
                {products.map((product, index) => (
                    <React.Fragment key={index}>
                        {index === number && <Carousel key={`carousel-${index}`} products={products} onAdd={onAdd}/>}
                        <CardProduct key={`product-${product.uuid}`} product={product} onAdd={onAdd} />
                    </React.Fragment>
                ))}
            </div>
        </div>
    );
};

export default function Page() {
    const [isClient, setIsClient] = useState(false);

    const [countCart, setCountCart] = useState<number>(getInitialCartLocalStorage().cart.products.length);

    const router = useRouter();
    const searchParams = useSearchParams();

    let initialCategory: string[] = [];
    let initialSearch = 'none';

    if (searchParams) {
        const categoryParams = searchParams.getAll('category');
        initialCategory = categoryParams.length > 0 ? categoryParams : ['All'];
        initialSearch = searchParams.get('search') || 'none';
    }

    const [category, setCategory] = useState<string[]>(initialCategory);
    const [search, setSearch] = useState<string>(initialSearch);

    const initialStore = useFetchInitialStore({ category, search });
    const initialCart = getInitialCartLocalStorage();

    const handleAddToCart = ({ product }: { product: Product }) => {
        initialCart.cart.products.push(product);
        initialCart.cart.subtotal += product.price;
        initialCart.cart.total = initialCart.cart.subtotal + initialCart.cart.subtotal * initialCart.cart.taxPercentage;
        saveInitialCartLocalStorage(initialCart);
        setCountCart(countCart + 1);
    }

    const handleAddtoCategory = ({ category }: { category: Category }) => {
        setCategory(prevCategories => {
            let newCategories;
            if (category.name === 'All') {
                newCategories = ['All'];
            } else {
                newCategories = prevCategories.includes('All')
                    ? [category.name]
                    : prevCategories.includes(category.name)
                        ? prevCategories.filter(c => c !== category.name)
                        : [...prevCategories, category.name];
            }
            const queryString = `category=${newCategories.join('&category=')}&search=${search}`;
            router.push(`/abacaxi?${queryString}`);
            return newCategories;
        });
    }

    const handleAddtoSearch = (searchQuery: string) => {
        if (searchQuery && searchQuery.trim().length !== 0) {
            setSearch(searchQuery.trim());
            const queryString = `category=${category.join('&category=')}&search=${searchQuery.trim()}`;
            router.push(`/abacaxi?${queryString}`);
        } else {
            setSearch("none");
            const queryString = `category=${category.join('&category=')}&search=none`;
            router.push(`/abacaxi?${queryString}`);
        }
    }

    useEffect(() => {
        setIsClient(true);
    }, []);

    if (!isClient) {
        return <>
            <div className="hourglassBackground">
                <div className="hourglassContainer">
                    <div className="hourglassCurves"></div>
                    <div className="hourglassCapTop"></div>
                    <div className="hourglassGlassTop"></div>
                    <div className="hourglassSand"></div>
                    <div className="hourglassSandStream"></div>
                    <div className="hourglassCapBottom"></div>
                    <div className="hourglassGlass"></div>
                </div>
            </div>
        </>;
    }

    return (
        <>
            <Navbar onAddCategory={handleAddtoCategory} onAddSearch={handleAddtoSearch} countCart={countCart} />
            {<ProductsRow products={initialStore ? initialStore : []} onAdd={handleAddToCart} />}
            <Footer />
        </>
    );
}