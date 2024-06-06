"use client";
import AcmeLogo from '@/app/ui/acme-logo';
import { ArrowRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import { useState } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS

//import * as React from 'react';
//import "../styles/globals.css";

// Marca este componente como un componente del lado del cliente

const products = [
  {
    id: 1,
    name: 'Cinder',
    autor: 'Marissa Meyer',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768889_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 9500
  },
  {
    id: 2,
    name: 'Scarlet',
    autor: 'Marissa Meyer',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768896_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 9500
  },
  {
    id: 3,
    name: 'Cress',
    autor: 'Marissa Meyer',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768902_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 9500
  },
  {
    id: 4,
    name: 'Winter',
    autor: 'Marissa Meyer',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9781250768926_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 11900
  },
  {
    id: 5,
    name: 'Fairest',
    autor: 'Marissa Meyer',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9781250774057_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 8700
  },
  {
    id: 6,
    name: 'La Sociedad de la Nieve',
    autor: 'Pablo Vierci',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9786070794162_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 12800
  },
  {
    id: 7,
    name: 'En Agosto nos vemos',
    autor: 'Gabriel García Márquez',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9786073911290_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 14900
  },
  {
    id: 8,
    name: 'El estrecho sendero entre deseos',
    autor: 'Patrick Rothfuss',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9789585457935_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 12800
  },
  {
    id: 9,
    name: 'Alas de Sangre',
    autor: 'Rebecca Yarros',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9788408279990_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 19800
  },
  {
    id: 10,
    name: 'Corona de Medianoche',
    autor: 'Sarah J. Mass',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9786073143691_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 15800
  },
  {
    id: 11,
    name: 'Carta de Amor a los Muertos',
    autor: "Ava Dellaira",
    imageurl: 'https://m.media-amazon.com/images/I/41IETN4YxGL._SY445_SX342_.jpg',
    price: 8900
  },
  {
    id: 12,
    name: 'Alicia en el país de las Maravillas',
    autor: 'Lewis Carrol',
    imageurl: 'https://www.libreriainternacional.com/media/catalog/product/9/7/9788415618713_1_1.jpg?optimize=medium&bg-color=255,255,255&fit=bounds&height=1320&width=1000',
    price: 7900
  }
];



//{name: string, autor: string, imageurl: string, price: int}
const Product = ({ product }) => {

  const [count, setCount] = useState(0);

  function handleClick() {
    setCount(count + 1);
  }


  const { name, autor, imageurl, price } = product;
  return (
    <div className='col-md-3' style={{ background: 'white', width: '300px', margin: '0.5rem' }}>
      <img src={imageurl} style={{ margin: '1.4rem', width: '250px'}} className='img-fluid'  />
      <h2> {name} </h2>
      <p> Autor: {autor} </p>
      <p>Precio: ₡{price}</p>
      <button onClick={handleClick}> Comprar {count} </button>
    </div>
  );
};

const Row = () => {
  return (
    <div className="row">
      {products.map(product => (
        <Product key={product.id} product={product} />
      ))}
    </div>
  );
};



export default function Page() {
  const [counter, setCounter] = useState(0);
  


  return (
    <main className="flex min-h-screen flex-col p-6">
      <header className="p-3 text-bg-dark">
        <div className="row" style={{color: 'gray'}}>
          <div className="col-sm-2">
            <img src="Logo1.jpg" style={{height: '75px', width: '200px', margin: '1.4rem'}} className="img-fluid"/>
          </div>
          <div className="col-sm-8 d-flex justify-content-center align-items-center">
            <form className="d-flex justify-content-center">
              <input type="search" name="search" style={{width: '805%'}} placeholder="Buscar"></input>
              <button type="submit">Buscar</button>
            </form>
          </div>
          <div className="col-sm-2 d-flex justify-content-end align-items-center">
            <button type="button">Cart</button>
          </div>
        </div>
      </header>

      <div>
        <h1>Lista de Libros </h1>
        <div style={{ display: 'flex', flexWrap: 'wrap' }}>
          <Row />
        </div>

      </div >
    </main>
      
      );
}
