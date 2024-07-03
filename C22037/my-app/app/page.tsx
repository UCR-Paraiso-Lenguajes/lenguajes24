"use client"; //Para utilizar el cliente en lugar del servidor
import "bootstrap/dist/css/bootstrap.min.css";
import "@/public/styles.css";
import { useState, useEffect } from "react";
import Link from 'next/link';

const products = [
  {
    id: 1,
    name: "Producto 1",
    decription: "Descripción 1",
    imageURL: "https://images-na.ssl-images-amazon.com/images/I/71JSM9i1bQL.AC_UL160_SR160,160.jpg",
    price: 10
  },
  {
    id: 2,
    name: "Producto 2",
    decription: "Descripción 2",
    imageURL: "https://images-na.ssl-images-amazon.com/images/I/418UoVylqyL._AC_UL160_SR160,160_.jpg",
    price: 20
  },
  {
    id: 3,
    name: "Producto 3",
    decription: "Descripción 3",
    imageURL: "https://images-na.ssl-images-amazon.com/images/I/81WsSyAYxHL._AC_UL160_SR160,160_.jpg",
    price: 30
  },
  {
    id: 4,
    name: "Producto 4",
    decription: "Descripción 4",
    imageURL: "https://images-na.ssl-images-amazon.com/images/I/51-lOBlIrFL._AC_UL160_SR160,160_.jpg",
    price: 40
  },
  {
    id: 5,
    name: "Producto 2",
    decription: "Descripción 5",
    imageURL: "https://images-na.ssl-images-amazon.com/images/I/51wD-xrtyWL._AC_UL160_SR160,160_.jpg",
    price: 50
  },
  {
    id: 6,
    name: "Producto 6",
    decription: "Descripción 6",
    imageURL: "https://images-na.ssl-images-amazon.com/images/I/71EZAE6fljL._AC_UL160_SR160,160_.jpg",
    price: 60
  },
  {
    id: 7,
    name: "Producto 7",
    decription: "Descripción 7",
    imageURL: "https://m.media-amazon.com/images/I/817EyM89DtL._AC_SY100_.jpg",
    price: 70
  },
  {
    id: 8,
    name: "Producto 8",
    decription: "Descripción 8",
    imageURL: "https://m.media-amazon.com/images/I/61J0e7d0GEL._AC_SY100_.jpg",
    price: 80
  },
  {
    id: 9,
    name: "Producto 9",
    decription: "Descripción 9",
    imageURL: "https://m.media-amazon.com/images/I/81mzvAGkHkL._AC_SY100_.jpg",
    price: 90
  },
  {
    id: 10,
    name: "Producto 10",
    decription: "Descripción 10",
    imageURL: "https://m.media-amazon.com/images/I/51YlAYwPx6L._AC_SY100_.jpg",
    price: 100
  },
  {
    id: 11,
    name: "Producto 11",
    decription: "Descripción 11",
    imageURL: "https://m.media-amazon.com/images/I/71cj5cNm7ZL._AC_UY218_.jpg",
    price: 110
  },
  {
    id: 12,
    name: "Producto 12",
    decription: "Descripción 12",
    imageURL: "https://m.media-amazon.com/images/I/7148mbvrbWL._AC_UL320_.jpg",
    price: 120
  },
  {
    id: 13,
    name: "Producto 12",
    decription: "Descripción 13",
    imageURL: "https://m.media-amazon.com/images/I/71Pf0aGicBL._AC_UY218_.jpg",
    price: 130
  },
  {
    id: 14,
    name: "Producto 14",
    decription: "Descripción 14",
    imageURL: "https://m.media-amazon.com/images/I/71P84KYUfrL._AC_UL320_.jpg",
    price: 140
  },
  {
    id: 15,
    name: "Producto 15",
    decription: "Descripción 15",
    imageURL: "https://m.media-amazon.com/images/I/51gJxciP-qL._AC_UY218_T2F_.jpg",
    price: 150
  },
  {
    id: 16,
    name: "Producto 16",
    decription: "Descripción 16",
    imageURL: "https://m.media-amazon.com/images/I/61OI1MNjZZL._AC_UY218_T2F_.jpg",
    price: 160
  }
];

const Product = ({ product, handleAddToCart }) => {
  const { id, name, description, imageURL, price } = product;

  return (
    <div className="col-sm-3">
      <img src={imageURL} alt={name} style={{ width: '50%', height: '50%' }} />
      <h3>{name}</h3>
      <p>{description}</p>
      <p>Precio: ${price}</p>
      <button type="button" onClick={() => handleAddToCart(id)} className="Button">Add to Cart</button>
    </div>
  );
};

function rows(array, size) {
  const row = [];
  for (let i = 0; i < array.length; i += size) {
    row.push(array.slice(i, i + size));
  }
  return row;
}

export default function Home() {
  const [count, setCount] = useState(0);

  useEffect(() => {
    const storedCart = JSON.parse(localStorage.getItem('cart')) || {};
    if (storedCart.products) {
      setCount(Object.keys(storedCart.products).length);
    }
  }, []);

  const handleAddToCart = (productId) => {
    const storedCart = JSON.parse(localStorage.getItem('cart')) || { products: {} };
    const productToAdd = products.find(product => product.id === productId);
    if (productToAdd) {
      const updatedCart = { 
        ...storedCart, 
        products: { ...storedCart.products, [productId]: productToAdd }
      };
      localStorage.setItem('cart', JSON.stringify(updatedCart));
      setCount(Object.keys(updatedCart.products).length);
    }
  };

  useEffect(() => {
    const initialCart = JSON.parse(localStorage.getItem('cart')) || {
      products: {},
    };
    localStorage.setItem('cart', JSON.stringify(initialCart));
  }, []);

  useEffect(() => {
    localStorage.setItem('products', JSON.stringify(products));
  }, []);

  return (
    <div>
      <div className="header">
        <div className="row">
          <div className="col-sm-2">
            <h1>Tienda</h1>
          </div>
          <div className="col-sm-6">
            <div className="col-sm-6 d-flex align-items-center">
              <search>
                <label>
                  <input type="search" name="q" autoComplete="off" />
                </label>
                <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="currentColor" class="bi bi-search" viewBox="0 0 16 16">
                  <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0" />
                </svg>
              </search>
            </div>
          </div>
          <div className="col-sm-4 d-flex justify-content-end">
            <Link href="/cart">
              <div className="position-relative">
                <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" fill="currentColor" className="bi bi-bag" viewBox="0 0 16 16">
                  <path d="M8 1a2.5 2.5 0 0 1 2.5 2.5V4h-5v-.5A2.5 2.5 0 0 1 8 1m3.5 3v-.5a3.5 3.5 0 1 0-7 0V4H1v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V4zM2 5h12v9a1 1 0 0 1-1 1H3a1 1 0 0 1-1-1z" />
                </svg>
                {count > 0 && <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">{count}</span>}
              </div>
            </Link>
            <svg xmlns="http://www.w3.org/2000/svg" width="60" height="60" fill="currentColor" className="bi bi-box-arrow-in-right" viewBox="0 0 16 16">
              <path fillRule="evenodd" d="M6 3.5a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-2a.5.5 0 0 0-1 0v2A1.5 1.5 0 0 0 6.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-8A1.5 1.5 0 0 0 5 3.5v2a.5.5 0 0 0 1 0z" />
              <path fillRule="evenodd" d="M11.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H1.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708z" />
            </svg>
          </div>
        </div>
      </div>

      <div className="body">
        <h2>Lista de Productos</h2>
        {rows(products, 4).map((row, index) => (
          <div className="body">
            <div style={{ display: 'flex', flexWrap: 'wrap' }}>
              {row.map(product => (
                <Product key={product.id} product={product} handleAddToCart={handleAddToCart} />
              ))}
            </div>
          </div>
        ))}
      </div>

      <div className="footer">
        <div className="row">
          <div className="col-sm-12">
            <h3>Tienda.com</h3>
          </div>
        </div>
      </div>

    </div>
  );
}