"use client";
import Image from "next/image";
import 'bootstrap/dist/css/bootstrap.min.css';
import { useState, useEffect } from 'react';
import Carousel from 'react-bootstrap/Carousel';
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import Button from 'react-bootstrap/Button'
import React from 'react';
import { Card, Container } from "react-bootstrap";

// const products = await fetch('https://localhost:7194/api/Store').JSON()

const Cart = {
  products: [],
  subtotal: 0,
  address: '',
  paymentMethod: 0,
};

const CartComponent = ({ count, total }) => {

  return (
    <div className='container'>
      <Button href="/cart">
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" className="w-6 h-6">
          <path strokeLinecap="round" strokeLinejoin="round" d="M2.25 3h1.386c.51 0 .955.343 1.087.835l.383 1.437M7.5 14.25a3 3 0 0 0-3 3h15.75m-12.75-3h11.218c1.121-2.3 2.1-4.684 2.924-7.138a60.114 60.114 0 0 0-16.536-1.84M7.5 14.25 5.106 5.272M6 20.25a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0Zm12.75 0a.75.75 0 1 1-1.5 0 .75.75 0 0 1 1.5 0Z" />
        </svg>
        Products: {count} Total: ${total}
      </Button>

    </div>
  )
};


const Product = ({ product, addToCart }) => {
  const { id, imgSource, name, price } = product;

  return (
    <Col sm='3' className="mt-5">

      <Card style={{ width: '20rem' }}>
        <Card.Img variant="top" src={product.imgSource} />
        <Card.Body>
          <Card.Title>{product.name}</Card.Title>
          <Card.Text>
            Precio: ${product.price}
          </Card.Text>
          <Button variant="primary" onClick={() => addToCart(id)}>Comprar</Button>
        </Card.Body>
      </Card>
    </Col>

  )
};

const carrouselItems = [
  {
    id: 0,
    imgSrc: "Producto1.jpeg"
  },
  {
    id: 1,
    imgSrc: "https://cdn.mos.cms.futurecdn.net/vhvu7HGyknSTnuQWhjxarS-650-80.png.webp"
  },
  {
    id: 2,
    imgSrc: "https://i.blogs.es/ed843e/superpc-ap/1366_2000.jpeg"
  },
  {
    id: 4,
    imgSrc: "https://cdn.mos.cms.futurecdn.net/vhvu7HGyknSTnuQWhjxarS-650-80.png.webp"
  }
];

const Item = React.forwardRef(({ carrouselItem }, ref) => {
  const { id, imgSrc } = carrouselItem;

  return (
    <Carousel.Item >
      <img src={carrouselItem.imgSrc} width="100%" />
    </Carousel.Item>
  );
});

export default function Home() {

  // debugger
  var cartStoraged = JSON.parse(localStorage.getItem('Cart'));
  if (!cartStoraged) {
    localStorage.setItem('Cart', JSON.stringify(Cart));
    cartStoraged = JSON.parse(localStorage.getItem('Cart'));
  }

  const [cartState, setCartState] = useState(cartStoraged)
  const [shop, setShop] = useState({ products: [] });

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      const response = await fetch('https://localhost:7194/api/Store'); // Replace with your API endpoint
      if (!response.ok) {
        throw new Error('Network response was not ok.');
      }
      const data = await response.json();
      localStorage.setItem('Shop', JSON.stringify(data));
      setShop(data);
    } catch (error) {
      setError(error.message);
      setLoading(false);
    }
  };

  // console.log(shop)

  const handleClick = (id) => {
    // debugger
    let copyOfCart = { ...cartState };
    const productToAdd = shop.products.find(product => product.id === id);
    if (productToAdd) {
      copyOfCart.products = [...copyOfCart.products, productToAdd];

      const subtotal = copyOfCart.subtotal + productToAdd.price;
      const formattedSubtotal = Number(subtotal.toFixed(2));
      copyOfCart.subtotal = formattedSubtotal;
      setCartState(copyOfCart)
      localStorage.setItem('Cart', JSON.stringify(copyOfCart));
    }
  }

  return (
    <Container>
      <CartComponent count={cartState.products.length} total={cartState.subtotal} />
      {/* <Carousel>
        {carrouselItems.map(carouselItem =>
          <Item key={carouselItem.id} carrouselItem={carouselItem} />
        )}
      </Carousel> */}


      <div className="mx-auto">

        <h1>Lista de productos</h1>
      </div>
      <Row className="justify-content-md-center">
        {shop.products.map(product =>
          <Product key={product.id} product={product} addToCart={handleClick} />
        )}
      </Row>
    </Container>
  );
}
