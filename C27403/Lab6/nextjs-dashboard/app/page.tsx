"use client";
import AcmeLogo from '@/app/ui/acme-logo';
import { ArrowRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import 'bootstrap/dist/css/bootstrap.min.css'; // Importar los estilos de Bootstrap
import 'bootstrap/dist/js/bootstrap.bundle.min'; // Importar los scripts de Bootstrap
import { useState } from 'react';

const products = [
  {
    id: 1,
    name: 'Towel',
    imgurl: 'https://m.media-amazon.com/images/I/41n5es7IdHL._AC_SY230_.jpg',
    width: '5cm',
    price: '$19.99'
  },
  {
    id: 2,
    name: 'Headphones',
    imgurl: 'https://walmartcr.vtexassets.com/arquivos/ids/340063/Auriculares-Blackweb-Gaming-7-1-Pc-1-49471.jpg?v=637983454210570000',
    width: '5cm',
    price: '$49.99'
  },
  {
    id: 3,
    name: 'Batidora Kitchen Aid',
    imgurl: 'https://m.media-amazon.com/images/I/51HXid8ExKL._AC_SX569_.jpg',
    width: '5cm',
    price: '$249.99'
  },
  {
    id: 4,
    name: 'Alfombra para baño',
    imgurl: 'https://m.media-amazon.com/images/I/91ppCguT44L._AC_SX450_.jpg',
    width: '5cm',
    price: '$12.86'
  },
  {
    id: 5,
    name: 'Pack de 6 Beauty Blender',
    imgurl: 'https://m.media-amazon.com/images/I/8122DlWL93L.jpg',
    width: '5cm',
    price: '$5.95'
  },
  {
    id: 6,
    name: 'Peluche de conejo reversible',
    imgurl: 'https://m.media-amazon.com/images/I/711dPaRMSVL._AC_SX569_.jpg',
    width: '5cm',
    price: '$13.99'
  },
  {
    id: 7,
    name: 'Teléfono Infinix Zero 30 5G',
    imgurl: 'https://www.intelec.co.cr/image/cache/catalog/catalogo/IN-X6837-6-250x250h.jpg.webp',
    width: '5cm',
    price: '$263.83'
  },
  {
    id: 8,
    name: 'Tableta de escritura LCD de 8,5 pulgadas',
    imgurl: 'https://m.media-amazon.com/images/I/71HmIoX+O2L._AC_SX466_.jpg',
    width: '5cm',
    price: '$14.99'
  }
];


//componente producto
const Product = ({ product, addToCart }) => {
  const { id, name, imgurl, width, price } = product;
  return (
    <div className="col-md-3" style={{ backgroundColor: "white", width: "260px", margin: "0.5rem" }}>
      <div style={{ textAlign: 'center' }}>
        <img src={imgurl} alt="centered image" style={{ height: "5cm" }} className="img-fluid" />
      </div><br></br>
      <h4 style={{ fontFamily: 'Trebuchet MS, sans-serif' }}>{name}</h4>
      <button className="button" style={{textAlign: "end", backgroundColor: "black", color: "white" }} onClick={() => addToCart(id)}>Agregar al carrito</button>
      <div style={{ textAlign: 'left' }}><h5>{price}</h5></div>
      <br></br>

    </div>
  );
};


const carouselImages = [
  [
    "https://todoaplazo.com/images/products/2feb67f6-52b9-4931-9900-ace9fab8d79f-AMAZON-37.png",
    "https://m.media-amazon.com/images/I/616vyWhcZaL._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/717xxg1Qn4L._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/71wl1AtXdCL._AC_SY200_.jpg"
  ],
  [
    "https://m.media-amazon.com/images/I/71CTPbqGkIL._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/61jCoTv9b0L._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/710bNparTlL._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/51kNn7n0dRL._AC_SY200_.jpg"
  ],
  [
    "https://m.media-amazon.com/images/I/81AOpcqJfHL._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/61l7VLu78ZL._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/71lYwpEFaqL._AC_SY200_.jpg",
    "https://m.media-amazon.com/images/I/81xiJRmOwUL._AC_SY200_.jpg"
  ],
];



// Componente Carousel
function Carousel() {
  return (
    <div className="h-56 sm:h-64 xl:h-80 2xl:h-96">
      <div id="myCarousel" className="carousel carousel-dark slide" data-bs-ride="carousel">
        <ol className="carousel-indicators">
          {carouselImages.map((_, index) => (
            <li key={index} data-bs-target="#myCarousel" data-bs-slide-to={index} className={index === 0 ? 'active' : ''}></li>
          ))}
        </ol>
        <div className="carousel-inner" style={{ backgroundColor: "white" }}>
          <div><h3 style={{ margin: "0.5rem" }}>Popular</h3></div>
          {carouselImages.map((imageGroup, index) => (
            <div key={index} className={`carousel-item ${index === 0 ? 'active' : ''}`}>
              <div className="row" style={{ backgroundColor: "white", textAlign: "center", height: "250px" }}>
                {imageGroup.map((imageUrl, i) => (
                  <div key={i} className="col-md-3" style={{ textAlign: "center" }}>
                    <img src={imageUrl} alt={`Slide ${index + 1}`} className="img-fluid" style={{ height: "190px" }} />
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
        <a className="carousel-control-prev" href="#myCarousel" role="button" data-bs-slide="prev">
          <span className="carousel-control-prev-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Previous</span>
        </a>
        <a className="carousel-control-next" href="#myCarousel" role="button" data-bs-slide="next">
          <span className="carousel-control-next-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Next</span>
        </a>
      </div>
    </div>
  );
}


export default function Page() {
  const cart = JSON.parse(localStorage.getItem('cart')) || [];
  const [cartCount, setCartCount] = useState(cart.length);
  const [cartOpen, setCartOpen] = useState(false);
  const addToCart = (productId) => {
    setCartCount(cartCount + 1);
    const product = products.find(product => product.id === productId);
    setCartCount(cart.length + 1);
    cart.push(product);
    localStorage.setItem('cart', JSON.stringify(cart));
  };

  const toggleCart = () => {
    setCartOpen(!cartOpen);
  };
  return (
    <main className="flex min-h-screen flex-col p-6">
      <header style={{ backgroundColor: "rgb(180, 180, 180);" }}>
        <div className="container ">
          <div className="row" >
            <div className="col" >
              <img src="https://i.imgur.com/g90hvoy.png"
                style={{ width: '140px', margin: '1rem', textAlign: 'center' }} className="img-fluid" /></div>
            <div className="col">
            </div>
            <div className="col">
              <nav className="navbar">
                <div className="container-fluid">
                  <form className="d-flex">
                    <input className="form-control me-2" type="search" placeholder="Search" aria-label="Search"
                      style={{ margin: '1rem', width: '15cm' }} />
                    <button className="btn" type="submit"
                      style={{ height: '1cm', margin: '1rem' }}>🔍</button>
                    <Link href="/cart"><button style={{ margin: '0.6rem', textAlign: 'center', height: '1.4cm', backgroundColor:"white", borderColor:"white" }}><img
                      src="https://cdn-icons-png.flaticon.com/512/3144/3144456.png"
                      style={{ height: '0.6cm', width: '0.8cm' }} className="img-fluid" /><p>{cartCount}</p></button></Link>
                  </form>
                </div>
              </nav>
            </div>
          </div>
        </div>
      </header>
      <div className='container'>
        <h1>Lista de Productos</h1>
        <div className="row" style={{ display: 'flex', flexWrap: 'wrap' }}>
          {products.map((product) => (
            <Product key={product.id} product={product} addToCart={addToCart} />
          ))}
        </div>
      </div>
      <div className='container'>
        <Carousel />
      </div>
    </main >
  );
}
