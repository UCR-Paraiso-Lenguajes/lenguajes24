"use client";
import AcmeLogo from '@/app/ui/acme-logo';
import { ArrowRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import React, { useState, useEffect } from 'react';


const products = [
  {
    id: 1,
    name: 'Acessorios Gaming',
    precio: 30,
    Imagen: 'https://sm.mashable.com/t/mashable_in/photo/default/gaming-gadgets-copy_xeq6.1248.jpg',
    descripcion: '¡Entra aquí si estás buscando los mejores accesorios de gaming para llevar tu experiencia al siguiente nivel!'
  },
  {
    id: 2,
    name: 'Oferta de calzado',
    precio: 20,
    Imagen: 'https://img.michollo.com/app/deal/355387-1586342518814.png',
    descripcion: 'Descubre las últimas tendencias en moda que te harán destacar en cualquier ocasión.'
  },
  {
    id: 3,
    name: 'Vestido de fiesta elegante',
    precio: 50,
    Imagen: 'https://silviafernandez.com/wp-content/uploads/2023/12/Vestidos_de_fiesta_2024_Silvia_Fernandez_PROMESA_FRONTAL.jpg',
    descripcion: 'Compra tus looks para los próximos eventos por mucho menor coste, diseños que no pasan de moda.'
  },
  {
    id: 4,
    name: 'Lámpara de luz led',
    precio: 20,
    Imagen: 'https://www.ofertasb.com/upload/f28564.jpg',
    descripcion: 'OFERTA.'
  },
  {
    id: 5,
    name: 'SkinCare',
    precio: 30,
    Imagen: 'https://karunaskin.com/cdn/shop/files/K-FEB-13_1024x1024.jpg?v=1697073575',
    descripcion: 'OFERTA.'
  },
  {
    id: 6,
    name: 'Kit de Esmaltes semipermantes',
    precio: 15,
    Imagen: 'https://www.dd2.com.ar/image/cache/ML/MLA1382471373_0_15e8dfc851b9244816120f22f362bb93-550x550.jpg',
    descripcion: 'OFERTA.'
  },
  {
    id: 7,
    name: 'Lazos para el cabello',
    precio: 10,
    Imagen: 'https://calalunacr.com/wp-content/uploads/2024/02/7306E711-DEE7-44B2-B3CE-2873B8FE366E-600x791.jpeg',
    descripcion: 'OFERTA.'
  },
  {
    id: 8,
    name: 'Maquillaje',
    precio: 40,
    Imagen: 'https://us.123rf.com/450wm/belchonock/belchonock1711/belchonock171101715/90496402-concepto-de-venta-de-maquillaje-y-belleza-cosm%C3%A9ticos-en-el-fondo-blanco.jpg',
    descripcion: 'OFERTA.'
  }
];


const Product = ({ product, handleAddToCart}) => {
  const { name, precio, Imagen, descripcion } = product;

  const handleClick = () => {

    handleAddToCart(product.id);

  };

  return (
    <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3">
      <div className="card" style={{ width: '18rem', height: '30rem', backgroundColor: 'lightYellow' }} >
        <div className="card-body">
          <h5 className="card-title">{name}</h5>
          <img src={Imagen} alt={Imagen} style={{ height: '220px', width: '100%' }} />
          <p className="card-text">{descripcion} </p>
          <p>${precio} </p>
          <button onClick={handleClick}>Añadir al carrito</button>
        </div>
      </div>
    </div>
  );
};


//Carrusel

const imgCarrusel = [
  { id: 1, Imagen: 'https://cdnx.jumpseller.com/perfumesdelujo/theme_option/8779387/thumb/730/460?1596055544' },
  { id: 2, Imagen: 'https://minisocol.vtexassets.com/assets/vtex.file-manager-graphql/images/7f70a6cf-2f2b-48ae-b7e0-bad30bcab841___9704c3132539484424396171c1bedfd7.png' },
  { id: 3, Imagen: 'https://www.okchicas.com/wp-content/uploads/2015/11/regalos-productos-de-belleza.jpg' }
]

const Slider = ({ imgCarrusel }) => {
  const [currentImageIndex, setCurrentImageIndex] = React.useState(0);


  const PreviousSlide = () => {
    setCurrentImageIndex((prevIndex) =>
      prevIndex === 0 ? imgCarrusel.length - 1 : prevIndex - 1
    );
  };


  const NextSlide = () => {
    setCurrentImageIndex((prevIndex) =>
      prevIndex === imgCarrusel.length - 1 ? 0 : prevIndex + 1
    );
  };

  const carouselContainerStyle = {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
  };

  const carouselStyle = {
    maxWidth: '1000px',
    maxHeight: '300px',
    overflow: 'hidden',
  };

  const sliderbutton = {
    backgroundColor: '#000000',
    color: '#ffffff',
    border: 'none',
    margin: '0 10px',
    cursor: 'pointer',
    transition: 'background-color 0.3s ease',
    padding: '8px 16px', 
  };

  return (
    <div className="carousel-container" style={carouselContainerStyle}>
      <button className="custom-button" onClick={PreviousSlide} style={sliderbutton}>
        Prev
      </button>
      <div className="carousel" style={carouselStyle}>
        <img src={imgCarrusel[currentImageIndex].Imagen} alt={`Banner ${currentImageIndex}`}
          style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
      </div>
      <button type="button" className="btn btn-primary" onClick={NextSlide} style={sliderbutton}>
        Next
      </button>
    </div>
  );
};



const Header = ({ countS }) => (
  <div className="header">
    <div className="container-fluid" style={{ backgroundColor: 'rgb(178, 177, 177)' }}>
      <nav className="navbar navbar-expand-lg bg-body-tertiary">
        <div className="container-fluid">
          <a className="navbar-brand" href="#" style={{ color: 'black' }}>Compra Online</a>
          <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarSupportedContent">
            <ul className="navbar-nav me-auto mb-2 mb-lg-0">
              <li className="nav-item">
                <a className="nav-link active" aria-current="page" href="#" style={{ color: 'black' }}>Opiniones</a>
              </li>
              <li className="nav-item">
                <a className="nav-link" href="#"></a>
              </li>
              <li className="nav-item dropdown">
                <a className="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false"
                  style={{ color: 'black' }}>
                  Categoria
                </a>
                <ul className="dropdown-menu">
                  <li><a className="dropdown-item" href="#" style={{ color: 'black' }}>Moda</a></li>
                  <li><a className="dropdown-item" href="#" style={{ color: 'black' }}>Infantil</a></li>
                  <li>
                    <hr className="dropdown-divider" />
                  </li>
                  <li><a className="dropdown-item" href="#" style={{ color: 'black' }}>Hogar</a></li>
                </ul>
              </li>

              <li className="nav-item">
                <a className="nav-link disabled" aria-disabled="true" style={{ color: 'black' }}>Ayuda</a>
              </li>
            </ul>
            <div className="cart-counter">
              <span className="badge bg-primary" style={{ marginRight: '10px' }}>{countS}</span>
              <Link href="/cart">
                <button className='btn btn-primary'>Ver carrito</button>
              </Link>
              <img src="https://chichokers.com/wp-content/uploads/carrito_rosa.png" className="card-img-top" alt="..."
                style={{ height: '30px', width: '40px', marginRight: '1rem' }} className="img-fluid" />
            </div>
            <form className="d-flex" role="search">
              <input className="form-control me-2" type="Buscar" placeholder="Escribe lo que buscas" aria-label="Buscar"
                style={{ color: 'black' }} />
              <button className="btn btn-outline-success" type="submit" style={{ color: 'black' }}>Buscar</button>
            </form>
          </div>
        </div>
      </nav>
    </div>
  </div>

)

  export default function Page() {
   
    // Local storage
    const [countS, setCountS] = useState(0);
    const [count, setCount] = useState(0);
    const [subtotal, setSubtotal] = useState(0);
    const [taxRate, setTaxRate] = useState(0.13); // Tax rate
  
    useEffect(() => {
      const storedCartItems = JSON.parse(localStorage.getItem('cartItems'));
      if (storedCartItems) {
        setCount(Object.keys(storedCartItems).length);
        setCountS(Object.keys(storedCartItems).length);
        let subTotal = 0;
        Object.values(storedCartItems).forEach(item => {
          subTotal += item.precio; 
        });
        setSubtotal(subTotal);
      }
    }, []);
  
    useEffect(() => {
      localStorage.setItem('taxRate', taxRate.toString());
    }, [taxRate]);
  
    const handleAddToCart = (productId) => {
      const storedCartItems = JSON.parse(localStorage.getItem('cartItems')) || {};
      const productToAdd = products.find(product => product.id === productId);
      if (productToAdd) {
        const updatedCartItems = { ...storedCartItems, [productId]: productToAdd };
        localStorage.setItem('cartItems', JSON.stringify(updatedCartItems));
        
        //Incrementar countS solo cuando se agrega un nuevo producto al carrito
        if (!storedCartItems[productId]) {
          setCountS(countS + 1);
        }

        let subTotal = 0;
        Object.values(updatedCartItems).forEach(item => {
          subTotal += item.precio; 
        });
        setSubtotal(subTotal);
      }
    }
  
    const taxes = subtotal * taxRate;
    const total = subtotal + taxes;
  
  return (
    <div>
      <div>
        <Header countS={countS}  />
      </div>
      <div>
        <div className="row" style={{ display: 'flex', flexWrap: 'wrap' }}>
          {products.map(product => (
            <Product key={product.id} product={product} handleAddToCart={handleAddToCart} />
          ))}
        </div>
        <br />
        <br />
        <div className="carousel-container">
          <Slider imgCarrusel={imgCarrusel} />
        </div>
      </div>
      <main className="flex min-h-screen flex-col p-6">
        <div className="flex h-20 shrink-0 items-end rounded-lg bg-blue-500 p-4 md:h-52"></div>
        <div className="mt-4 flex grow flex-col gap-4 md:flex-row">
          <div className="flex flex-col justify-center gap-6 rounded-lg bg-gray-50 px-6 py-10 md:w-2/5 md:px-20">
            <Link href="/login" className="flex items-center gap-5 self-start rounded-lg bg-blue-500 px-6 py-3 text-sm font-medium text-white transition-colors hover:bg-blue-400 md:text-base">
              <span>Log in</span> <ArrowRightIcon className="w-5 md:w-6" />
            </Link>
          </div>
          <div className="flex items-center justify-center p-6 md:w-3/5 md:px-28 md:py-12"></div>
        </div>
      </main>
      <div className="container overflow-hidden text-center">
        <div className="row gy-5">
          <div className="col-12">
            <div style={{ display: 'flex', justifyContent: 'center' }}>
              <div>
                <p style={{ fontWeight: 'bold' }}>Subtotal: ${subtotal.toFixed(2)}</p>
                <p style={{ fontWeight: 'bold' }}>Taxes ({(taxRate * 100)}%): ${taxes.toFixed(2)}</p>
                <p style={{ fontWeight: 'bold' }}>Total: ${total.toFixed(2)}</p>
                <Link href={countS === 0 ? "#" : "/"}>
                  <button className="Button" disabled={countS === 0}>Continuar con Checkout</button>
                </Link>
                <div></div>
                <Link href="/">
                  <button className="Button">Inicio</button>
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
