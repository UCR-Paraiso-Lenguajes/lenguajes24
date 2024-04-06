'use client';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useEffect, useState } from 'react';
import React from 'react';
import { Product } from '../lib/products-data-definitions';
import ProductItem from '../dashboard/product';
import useInitialStore from '../lib/products-data';
import Navbar from '../ui/dashboard/navbar';

const carousels = [
  {
    id: 1,
    imageUrl:
      'https://elchapuzasinformatico.com/wp-content/uploads/2024/01/Samsung-Galaxy-S24-Ultra-Galaxy-AI.jpg',
  },
  {
    id: 2,
    imageUrl:
      'https://images.augustman.com/wp-content/uploads/sites/6/2024/02/29135325/op-12r-gi-996x560.jpg',
  },
  {
    id: 3,
    imageUrl:
      'https://i02.appmifile.com/982_operator_sg/29/02/2024/2812fa98470fc74af39d0c420f50bf09.jpg?f=webp',
  },
  {
    id: 4,
    imageUrl:
      'https://nextrift.com/wp-content/uploads/2022/10/oneplus-ace-pro-genshin-impact-hu-tao-8.jpg',
  },
];

const Carousel = ({}) => {
  return (
    <div className="container-products">
      <div
        id="carouselExampleIndicators"
        className="carousel slide carousel-fade"
        data-bs-ride="carousel"
        data-interval="5000"
        data-pause="hover"
      >
        <div className="carousel-indicators">
          <button
            type="button"
            data-bs-target="#carouselExampleIndicators"
            data-bs-slide-to="0"
            className="active"
            aria-current="true"
            aria-label="Slide 1"
          ></button>
          <button
            type="button"
            data-bs-target="#carouselExampleIndicators"
            data-bs-slide-to="1"
            aria-label="Slide 2"
          ></button>
          <button
            type="button"
            data-bs-target="#carouselExampleIndicators"
            data-bs-slide-to="2"
            aria-label="Slide 3"
          ></button>
          <button
            type="button"
            data-bs-target="#carouselExampleIndicators"
            data-bs-slide-to="3"
            aria-label="Slide 4"
          ></button>
        </div>

        <div className="carousel-inner">
          {carousels.map((carousel, index) => (
            <div
              key={index}
              className={`carousel-item ${index === 0 ? 'active' : ''}`}
            >
              <img
                src={carousel.imageUrl}
                className="d-block w-100"
                alt={`Slide ${index + 1}`}
              />
            </div>
          ))}
        </div>

        <button
          className="carousel-control-prev"
          type="button"
          data-bs-target="#carouselExampleIndicators"
          data-bs-slide="prev"
        >
          <span
            className="carousel-control-prev-icon"
            aria-hidden="true"
          ></span>
          <span className="visually-hidden">Previous</span>
        </button>
        <button
          className="carousel-control-next"
          type="button"
          data-bs-target="#carouselExampleIndicators"
          data-bs-slide="next"
        >
          <span
            className="carousel-control-next-icon"
            aria-hidden="true"
          ></span>
          <span className="visually-hidden">Next</span>
        </button>
      </div>
    </div>
  );
};

//principal
const ProductsRow = ({products,onAdd,}: { products: Product[], onAdd: any;}) => {
  //const ProductsRow =  ({ produc, handleAddToCart }) => {
  return (
    <div className="row">
      {products.map((product) => (
        <ProductItem
          key={`product-${product.uuid}`}
          product={product}
          onAdd={onAdd}
        />
      ))}
      {/* <Carousel /> */}
    </div>
  );
};

export default function Page() {
  const [count, setCount] = useState(0);
  const initialStore = useInitialStore();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (initialStore.products.length > 0) {
      setLoading(false);
    }
  }, [initialStore.products]);

  const handleAddToCart = ({ product }: { product: Product }) => {
    const cartProducts = initialStore.cart.products;
    console.log('primer prodcu ' + product.uuid);
    console.log('ID DEL PRODUCTO ' + product.uuid);
    console.log('cart tama; ' + cartProducts.length);
    
    cartProducts.forEach(product => {
      console.log("Producto cart:", product);
    });

    const isInCart = initialStore.cart.products.some(
      (item) => item.uuid === product.uuid
    );
  
    if (!isInCart) {
      setCount(count + 1);
      initialStore.cart.products.push(product);
      console.log('ID DEL PRODUCTO ' + product.uuid);
      console.log('cart tama DETRO; ' + cartProducts.length);
      initialStore.cart.subtotal = initialStore.cart.subtotal + product.price;

      initialStore.cart.total =
        initialStore.cart.subtotal +
        initialStore.cart.subtotal * initialStore.cart.taxPercentage;
    }
  };

  return (
    <>
      <Navbar
        countCart={
          initialStore.cart.products.length > count
            ? initialStore.cart.products.length
            : count
        }
      />
      {!loading && (
        <ProductsRow products={initialStore.products} onAdd={handleAddToCart} />
      )}
    </>
  );
}
