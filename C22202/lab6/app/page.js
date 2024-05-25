"use client";
import 'bootstrap/dist/css/bootstrap.min.css';
import { useState, useCallback, useLayoutEffect } from 'react';
import Carousel from 'react-bootstrap/Carousel';
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import Button from 'react-bootstrap/Button'
import React from 'react';
import { Card, Container } from "react-bootstrap";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useStateValue } from "./ui/StateContext";

const Product = ({ product, addToCart }) => {
  const { id, imgSource, name, price } = product;

  return (
    <Col sm='3' className="mt-5">

      <Card style={{ width: '20rem' }}>
      <div className="d-flex align-items-center justify-content-center" style={{ width: '100%', height: '300px', overflow: 'hidden' }}>
          <img src={imgSource} alt={name} style={{ maxWidth: '100%', maxHeight: '100%' }} />
        </div>
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

const Item = ({ carrouselItem }) => {
  const { id, imgSrc } = carrouselItem;

  return (
    <Carousel.Item >
      <img src={carrouselItem.imgSrc} />
    </Carousel.Item>
  );
};

export default function Home() {
  const searchParams = useSearchParams()
  const router = useRouter()
  const pathname = usePathname()
  const { cartState, setCartState } = useStateValue();
  const params = new URLSearchParams(searchParams)

  const createQueryString = useCallback(
    (name, value) => {
      params.set(name, value)

      return params.toString()
    },
    [searchParams]
  )

  if (!params.has('category')) {
    router.push(pathname + '?' + createQueryString('category', '0'))
  }

  var shopStorage = JSON.parse(localStorage.getItem('Shop'));
  if (!shopStorage) {
    shopStorage = { products: [], categories: [], slctCategory: 0 }
    localStorage.setItem('Shop', JSON.stringify(shopStorage));
  }
  const [shop, setShop] = useState(shopStorage);
  const [products, setProducts] = useState([]);

  useLayoutEffect(() => {
    fetchData();
  }, [searchParams]);

  const fetchData = async () => {
    try {
      let params = searchParams.toString()

      const response = await fetch(`https://localhost:7194/api/Store`); // Replace with your API endpoint
      if (!response.ok) {
        throw new Error('Network response was not ok.');
      }
      const data = await response.json();
      localStorage.setItem('Shop', JSON.stringify(data));
      setShop(data);

      if(params != null){
        const responseProducts = await fetch(`https://localhost:7194/api/Store/Products?${params}`); // Replace with your API endpoint
        if (!responseProducts.ok) {
          throw new Error('Network response was not ok.');
        }
        const dataProducts = await responseProducts.json();
        setProducts(dataProducts)

      } else {
        setProducts(shop.products)
      }

    } catch (error) {
      throw error
      setError(error.message);
      setLoading(false);
    }
  };

  const handleClick = (id) => {
    let copyOfCart = { ...cartState };
    const productToAdd = products.find(product => product.id === id);
    if (productToAdd) {
      copyOfCart.products = [...copyOfCart.products, productToAdd];

      const subtotal = copyOfCart.subtotal + productToAdd.price;
      const formattedSubtotal = Number(subtotal.toFixed(2));
      copyOfCart.subtotal = formattedSubtotal;
      setCartState(copyOfCart);
      localStorage.setItem('Cart', JSON.stringify(copyOfCart));
    }
  }

  return (
    <Container>
      {/* <Carousel>
        {carrouselItems.map(carouselItem =>
          <Carousel.Item key={carouselItem.id}><img src={carouselItem.imgSrc} width={'100%'}/></Carousel.Item>
          
        )}
      </Carousel> */}


      <div className="mx-auto">

        <h1>Lista de productos</h1>
      </div>
      <Row className="justify-content-md-center">
        {products.map(product =>
          <Product key={product.id} product={product} addToCart={handleClick} />
        )}
      </Row>
    </Container>
  );
}
