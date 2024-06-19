'use client'
import { useEffect, useState } from 'react';
import ProductItem from '../dashboard/product';
import SideNav from '../ui/dashboard/sidenav';
import { Cart, Category, Product } from '../lib/data-definitions';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import React from 'react';
import { getInitialCartLocalStorage, saveInitialCartLocalStorage } from '../lib/cart_data_localeStore';
import useFetchInitialStore from '../api/http.initialStore';
import { useRouter, useSearchParams } from 'next/navigation';

const Carousel = ({ products, onAdd }: { products: Product[], onAdd: any }) => {
  const chunkSize = 4;
  const productChunks = [];
  for (let i = 0; i < products.length; i += chunkSize) {
    productChunks.push(products.slice(i, i + chunkSize));
  }
  return (
    <div className="container-products">
      <div id="carouselExampleDark" className="carousel carousel-dark slide" data-bs-ride="carousel"
        data-interval="5000" data-pause="hover">

        <div className="carousel-inner">
          {productChunks.map((chunk, index) => (
            <div key={index} className={`carousel-item ${index === 0 ? "active" : ""} `}>
              <div className="row d-flex flex-row justify-content-center align-items-center">
                {chunk.map((product, index) => (
                  <ProductItem key={index} product={product} onAdd={onAdd} />
                ))}
              </div>
            </div>
          ))}
        </div>

        <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleDark"
          data-bs-slide="prev">
          <span className="carousel-control-prev-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Previous</span>
        </button>
        <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleDark"
          data-bs-slide="next">
          <span className="carousel-control-next-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Next</span>
        </button>
      </div>
    </div>
  )
}

const ProductsRow = ({ products, onAdd }: { products: Product[], onAdd: any }) => {
  let number = (products.length - (products.length % 4)) / 2
  return (
    <div className='containerProducts'>
      <div className="row">
        {products.map((product, index) => (
          <React.Fragment key={index}>
            {index === number && <Carousel key={`carousel-${index}`} onAdd={onAdd} products={products} />}
            <ProductItem key={`product-${product.uuid}`} product={product} onAdd={onAdd} />
          </React.Fragment>
        ))}
      </div>
    </div>
  );
};

export default function Page() {
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

  const [count, setCount] = useState(initialCart.cart.products.length > 0 ? initialCart.cart.products.length : 0);

  const handleAddToCart = ({ product }: { product: Product }) => {
    initialCart.cart.products.push(product);
    initialCart.cart.subtotal += product.price;
    initialCart.cart.total = initialCart.cart.subtotal + initialCart.cart.subtotal * initialCart.cart.taxPercentage;
    setCount(count + 1);
    saveInitialCartLocalStorage(initialCart);
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
      router.push(`/dashboard?${queryString}`);
      return newCategories;
    });
  }

  const handleAddtoSearch = (searchQuery: string) => {
    if (searchQuery && searchQuery.trim().length !== 0) {
      setSearch(searchQuery.trim());
      const queryString = `category=${category.join('&category=')}&search=${searchQuery.trim()}`;
      router.push(`/dashboard?${queryString}`);
    } else {
      setSearch("none");
      const queryString = `category=${category.join('&category=')}&search=none`;
      router.push(`/dashboard?${queryString}`);
    }
  }

  return (
    <>
      <SideNav countCart={count} onAddCategory={handleAddtoCategory} onAddSearch={handleAddtoSearch} />
      {<ProductsRow products={initialStore ? initialStore : []} onAdd={handleAddToCart} />}
    </>
  );
}