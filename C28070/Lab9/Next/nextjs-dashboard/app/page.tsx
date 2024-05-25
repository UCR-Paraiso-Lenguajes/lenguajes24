"use client";
import AcmeLogo from '@/app/ui/acme-logo';
import { ArrowRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css"; // Importar CSS de Bootstrap
import React, { useState, useEffect } from 'react';

const Producto = ({ Product, manejarAgregarAlCarrito }) => {
  const { name, price, image, description } = Product;

  const handleClick = () => {
    manejarAgregarAlCarrito(Product.id);
  };

  return (
    <div className="col-sm-12 col-md-3 col-lg-3 col-xl-3">
      <div className="card" style={{ width: '18rem', height: '30rem', backgroundColor: 'lightYellow' }} >
        <div className="card-body">
          <h5 className="card-title">{name}</h5>
          <img src={image} alt={name} style={{ height: '220px', width: '100%' }} />
          <p className="card-text">{description} </p>
          <p>${price} </p>
          <button onClick={handleClick}>Añadir al carrito</button>
        </div>
      </div>
    </div>
  );
};

const Carrusel = ({ imagenes }) => {
  const [indiceImagenActual, setIndiceImagenActual] = React.useState(0);

  const irImagenAnterior = () => {
    setIndiceImagenActual((indiceAnterior) =>
      indiceAnterior === 0 ? imagenes.length - 1 : indiceAnterior - 1
    );
  };

  const irImagenSiguiente = () => {
    setIndiceImagenActual((indiceAnterior) =>
      indiceAnterior === imagenes.length - 1 ? 0 : indiceAnterior + 1
    );
  };

  const estiloContenedorCarrusel = {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
  };

  const estiloCarrusel = {
    maxWidth: '1000px',
    maxHeight: '300px',
    overflow: 'hidden',
  };

  const estiloBotonSlider = {
    backgroundColor: '#000000',
    color: '#ffffff',
    border: 'none',
    margin: '0 10px',
    cursor: 'pointer',
    transition: 'background-color 0.3s ease',
    padding: '8px 16px',
  };

  return (

    <div className="contenedor-carrusel" style={estiloContenedorCarrusel}>
      <button className="boton-slider" onClick={irImagenAnterior} style={estiloBotonSlider}>
        Anterior
      </button>
      <div className="carrusel" style={estiloCarrusel}>
        {imagenes && imagenes.length > 0 &&
          <img src={imagenes[indiceImagenActual]} alt={`Banner ${indiceImagenActual}`}
            style={{ width: '100%', height: '100%', objectFit: 'cover' }} />
        }
      </div>
      <button type="button" className="btn btn-primary" onClick={irImagenSiguiente} style={estiloBotonSlider}>
        Siguiente
      </button>
    </div>
  );
};

const Encabezado = ({ contadorCarrito }) => (
  <div className="encabezado">
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
                  Categoría
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
            <div className="contador-carrito">
              <span className="badge bg-primary" style={{ marginRight: '10px' }}>{contadorCarrito}</span>
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

export default function Pagina() {

  const [productos, setProductos] = useState([]);
  const [tasaImpuesto, setTasaImpuesto] = useState(0); //Tasa de impuesto
  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await fetch('http://localhost:5133/api/Store');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
  
        const storeJson = await response.json();
        setProductos(storeJson);
        setTasaImpuesto(storeJson.taxPercentage);
      } catch (error) {
        throw new Error('Error fetching data:', error);
      }
    };
  
    loadData();
  }, []); 


  const [contadorCarritoSesion, setContadorCarritoSesion] = useState(0);
  const [subtotal, setSubtotal] = useState(0);

  useEffect(() => {
    const productosCarritoAlmacenados = JSON.parse(localStorage.getItem('productosCarrito'));
    if (productosCarritoAlmacenados) {
      setContadorCarritoSesion(Object.keys(productosCarritoAlmacenados).length);
      let subtotalCalculado = 0;
      Object.values(productosCarritoAlmacenados).forEach(item => {
        subtotalCalculado += item.price;
      });
      setSubtotal(subtotalCalculado);
    }
  }, []);

  const manejarAgregarAlCarrito = (idProducto) => {
    const productosCarritoAlmacenados = JSON.parse(localStorage.getItem('productosCarrito'));
    const productoAAgregar = productos && productos.products ? productos.products.find(product => product.id === idProducto) : null;
    if (productoAAgregar) {
      const productosCarritoActualizados = { ...productosCarritoAlmacenados, [idProducto]: productoAAgregar };
      localStorage.setItem('productosCarrito', JSON.stringify(productosCarritoActualizados));
      if (!productosCarritoAlmacenados[idProducto]) {
        setContadorCarritoSesion(contadorCarritoSesion + 1);
      }
  
      let subtotalCalculado = 0;
      Object.values(productosCarritoActualizados).forEach(item => {
        subtotalCalculado += item.price;
      });
      setSubtotal(subtotalCalculado);
    }
  };

  const impuestos = subtotal * tasaImpuesto;
  const total = subtotal + impuestos;

  return (

    <div>
      <div>
        <Encabezado contadorCarrito={contadorCarritoSesion} />
      </div>
      <div>

        <div className="row" style={{ display: 'flex', flexWrap: 'wrap' }}>
          {productos && productos.products && productos.products.map((Product) => (
            <Producto key={Product.id} Product={Product} manejarAgregarAlCarrito={manejarAgregarAlCarrito} />
          ))}
        </div>
        <br />
        <br />
        <div className="contenedor-carrusel">

          <Carrusel imagenes={productos && productos.products && productos.products.map(product => product.image)} />

        </div>
      </div>
      <main className="flex min-h-screen flex-col p-6">
        <div className="flex h-20 shrink-0 items-end rounded-lg bg-blue-500 p-4 md:h-52"></div>
        <div className="mt-4 flex grow flex-col gap-4 md:flex-row">
          <div className="flex flex-col justify-center gap-6 rounded-lg bg-gray-50 px-6 py-10 md:w-2/5 md:px-20">
            <Link href="/login" className="flex items-center gap-5 self-start rounded-lg bg-blue-500 px-6 py-3 text-sm font-medium text-white transition-colors hover:bg-blue-400 md:text-base">
              <span>Iniciar sesión</span> <ArrowRightIcon className="w-5 md:w-6" />
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
                <p style={{ fontWeight: 'bold' }}>Impuestos ({(tasaImpuesto * 100)}%): ${impuestos.toFixed(2)}</p>
                <p style={{ fontWeight: 'bold' }}>Total: ${total.toFixed(2)}</p>
                <Link href={contadorCarritoSesion === 0 ? "#" : "/"}>
                  <button className="Boton" disabled={contadorCarritoSesion === 0}>Continuar con el checkout</button>
                </Link>
                <div></div>
                <Link href="/">
                  <button className="Boton">Inicio</button>
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
