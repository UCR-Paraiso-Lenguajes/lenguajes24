'use client'
import AcmeLogo from '@/app/ui/acme-logo';
import { ArrowRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import './ui/style/Ecomerce.css';
import './ui/style/products.css';
import "bootstrap/dist/css/bootstrap.min.css";
import { useEffect, useState } from 'react'
import { FaShoppingCart } from 'react-icons/fa'; // Importa un ícono de carrito 

const carousels = [
  {
    id: 1,
    imageUrl: "https://elchapuzasinformatico.com/wp-content/uploads/2024/01/Samsung-Galaxy-S24-Ultra-Galaxy-AI.jpg"
  },
  {
    id: 2,
    imageUrl: "https://images.augustman.com/wp-content/uploads/sites/6/2024/02/29135325/op-12r-gi-996x560.jpg"
  },
  {
    id: 3,
    imageUrl: "https://i02.appmifile.com/982_operator_sg/29/02/2024/2812fa98470fc74af39d0c420f50bf09.jpg?f=webp"
  },
  {
    id: 4,
    imageUrl: "https://nextrift.com/wp-content/uploads/2022/10/oneplus-ace-pro-genshin-impact-hu-tao-8.jpg"
  }
]
const Header = ({ count }) => {

  return <>
      <nav className="navbar navbar-expand-lg sticky-top navbar-light bg-custom-blue">
            <div className="container">
             <div className="navbar-left vet-poveda">
                  Vet Poveda
             </div>
                <div className="navbar-center w-75">
                    <div className="input-group w-100">
                        <input className="form-control mr-2 w-25" type="search" placeholder="Buscar productos" aria-label="Search" />
                        <div className="input-group-append">
                            <button className="btn btn-primary" type="submit">
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="24" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16">
                                    <path
                                        d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 
                                        3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 
                                        1-11 0 5.5 5.5 0 0 1 11 0" />
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
                <div className="navbar-right d-flex align-items-center">   
            <button className="btn btn-light cart-icon" >
               <Cart count={count} /> 
            </button>
                </div>
            </div>
        </nav >
    </>
};
const products = [
  {
    id: 1,
    name: "Taza Acero Inoxidable 350 ml",
    description: "La Taza Elevada para Comida es la elección perfecta para consentir a tu perro de raza pequeña durante sus comidas diarias. Diseñada pensando en la comodidad y el estilo, esta taza elevada no solo facilita el acceso a la comida, sino que también agrega un toque moderno a su espacio de alimentación.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/157968-800-auto?v=638442984629570000&width=800&height=auto&aspect=true",
    price: 5300
  },
  {
    id: 2,
    name: "Bowl de aventura",
    description: "El Cuenco Portátil ZippyPaws Adventure es tu compañero ideal para mantener a tu perro alimentado e hidratado en cualquier aventura.Resistente al agua con un forro interior a prueba de agua y fugas, asegurando una experiencia limpia y sin desorden. Gran capacidad: contiene hasta 22 onzas de agua o 3 tazas de alimento, perfecto tanto para perros pequeños como grandes.Fácil de guardar y transportar: una vez que hayas terminado, simplemente dóblalo y guárdalo en tu bolsillo o mochila.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/157787-800-auto?v=638372464039270000&width=800&height=auto&aspect=true",
    price: 2600
  },
  {
    id: 3,
    name: "Desparasitante Heartgard ",
    description: "Heartgard Plus es un eficaz desparasitante y larvacida que previene también la infección del gusano del corazón. Contiene 6 tabletas masticables por caja. Es el único que se puede administrar a cachorros a partir de 6 semanas de edad. Amplio margen de seguridad para todas las razas (incluyendo animales sensibles a la Ivermectina como Collies y Husky Siberianos). Perros de 12 a 22 Kg. de peso: (cada tableta contiene 136 mcg de ivermectina y 114 mg. de pirantel en forma de sal de pamoato), administrar una tableta mensual (Caja Verde)",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/156969-800-auto?v=638218745252630000&width=800&height=auto&aspect=true",
    price: 15920
  },
  {
    id: 4,
    name: "Halo Alimento Vegano Holístico",
    description: "Elaborados con ingredientes 100% veganos, incluyen proteína vegetal de alta calidad y sin ingredientes animales, lo que significa menores emisiones de gases de efecto invernadero. También usamos solo ingredientes no modificados genéticamente, lo que respalda la biodiversidad ecológica y el bienestar. Un sistema inmunológico fuerte comienza con un intestino saludable, y Halo Holistic es el único alimento para perros que contiene prebióticos, probióticos y posbióticos para una salud digestiva completa.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/156815-800-auto?v=638200473971270000&width=800&height=auto&aspect=true",
    price: 19900
  },
  {
    id: 5,
    name: "Taza Acero Inoxidable 350 ml",
    description: "La Taza Elevada para Comida es la elección perfecta para consentir a tu perro de raza pequeña durante sus comidas diarias. Diseñada pensando en la comodidad y el estilo, esta taza elevada no solo facilita el acceso a la comida, sino que también agrega un toque moderno a su espacio de alimentación.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/157968-800-auto?v=638442984629570000&width=800&height=auto&aspect=true",
    price: 5300
  },
  {
    id: 6,
    name: "Bowl de aventura",
    description: "El Cuenco Portátil ZippyPaws Adventure es tu compañero ideal para mantener a tu perro alimentado e hidratado en cualquier aventura.Resistente al agua con un forro interior a prueba de agua y fugas, asegurando una experiencia limpia y sin desorden. Gran capacidad: contiene hasta 22 onzas de agua o 3 tazas de alimento, perfecto tanto para perros pequeños como grandes.Fácil de guardar y transportar: una vez que hayas terminado, simplemente dóblalo y guárdalo en tu bolsillo o mochila.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/157787-800-auto?v=638372464039270000&width=800&height=auto&aspect=true",
    price: 2600
  },
  {
    id: 7,
    name: "Desparasitante Heartgard ",
    description: "Heartgard Plus es un eficaz desparasitante y larvacida que previene también la infección del gusano del corazón. Contiene 6 tabletas masticables por caja. Es el único que se puede administrar a cachorros a partir de 6 semanas de edad. Amplio margen de seguridad para todas las razas (incluyendo animales sensibles a la Ivermectina como Collies y Husky Siberianos). Perros de 12 a 22 Kg. de peso: (cada tableta contiene 136 mcg de ivermectina y 114 mg. de pirantel en forma de sal de pamoato), administrar una tableta mensual (Caja Verde)",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/156969-800-auto?v=638218745252630000&width=800&height=auto&aspect=true",
    price: 15920
  },
  {
    id: 8,
    name: "Halo Alimento Vegano Holístico",
    description: "Elaborados con ingredientes 100% veganos, incluyen proteína vegetal de alta calidad y sin ingredientes animales, lo que significa menores emisiones de gases de efecto invernadero. También usamos solo ingredientes no modificados genéticamente, lo que respalda la biodiversidad ecológica y el bienestar. Un sistema inmunológico fuerte comienza con un intestino saludable, y Halo Holistic es el único alimento para perros que contiene prebióticos, probióticos y posbióticos para una salud digestiva completa.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/156815-800-auto?v=638200473971270000&width=800&height=auto&aspect=true",
    price: 19900
  },
  {
    id: 9,
    name: "Taza Acero Inoxidable 350 ml",
    description: "La Taza Elevada para Comida es la elección perfecta para consentir a tu perro de raza pequeña durante sus comidas diarias. Diseñada pensando en la comodidad y el estilo, esta taza elevada no solo facilita el acceso a la comida, sino que también agrega un toque moderno a su espacio de alimentación.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/157968-800-auto?v=638442984629570000&width=800&height=auto&aspect=true",
    price: 5300
  },
  {
    id: 10,
    name: "Bowl de aventura",
    description: "El Cuenco Portátil ZippyPaws Adventure es tu compañero ideal para mantener a tu perro alimentado e hidratado en cualquier aventura.Resistente al agua con un forro interior a prueba de agua y fugas, asegurando una experiencia limpia y sin desorden. Gran capacidad: contiene hasta 22 onzas de agua o 3 tazas de alimento, perfecto tanto para perros pequeños como grandes.Fácil de guardar y transportar: una vez que hayas terminado, simplemente dóblalo y guárdalo en tu bolsillo o mochila.",
    imageUrl: "https://manchascr.vtexassets.com/arquivos/ids/157787-800-auto?v=638372464039270000&width=800&height=auto&aspect=true",
    price: 2600
  },
];
const Carousel = ({ }) => {
  return (
    <div className="container-products">
      <div id="carouselExampleIndicators" className="carousel slide carousel-fade" data-bs-ride="carousel"
        data-interval="5000" data-pause="hover">

        <div className="carousel-indicators">
          <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="0"
            className="active" aria-current="true" aria-label="Slide 1"></button>
          <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="1"
            aria-label="Slide 2"></button>
          <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="2"
            aria-label="Slide 3"></button>
          <button type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to="3"
            aria-label="Slide 4"></button>
        </div>

        <div className="carousel-inner">
          {carousels.map((carousel, index) => (
            <div key={index} className={`carousel-item ${index === 0 ? "active" : ""}`}>
              <img src={carousel.imageUrl} className="d-block w-100" alt={`Slide ${index + 1}`} />
            </div>
          ))}
        </div>

        <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators"
          data-bs-slide="prev">
          <span className="carousel-control-prev-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Previous</span>
        </button>
        <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators"
          data-bs-slide="next">
          <span className="carousel-control-next-icon" aria-hidden="true"></span>
          <span className="visually-hidden">Next</span>
        </button>
      </div>
    </div>
  )
}

const Product = ({ product, onAdd }) => { 
 
  const handleClick = ()=> {
    
    onAdd(product);
    //setCount(count + 1);
  };

  const [count, setCount] = useState(0);
  
  const { name, description, imageUrl, price } = product;
  return (
    <div className="col-sm-3">
      <div className="card">
        <img src={product.imageUrl}
          className="card-img-top" alt={product.name} />
        <div className="card-body">
          <div className="mb-3">
            <span className="float-start badge rounded-pill bg-primary">{product.name}</span>
            <span className="float-end price-hp">CRC {product.price}</span>
          </div>
          <div className="specifications">
            <div className="specifications-content">
              <p>{product.description}</p>
            </div>
          </div>
          <div className="text-center my-4">
            {/* <button onClick={handleClick} className="btn btn-warning">Buy</button> */}
            <button onClick={handleClick} className=" btn-warning" >Buy</button>
            {/* Hiciste click {count} veces */}
          </div>
        </div>
      </div>
    </div>
  );
};


function Cart({count}){
  return (
    <div className="cart-icon">
      <FaShoppingCart size={30} />
      <span className="badge bg-secondary">{count}</span> {/* Muestra la cantidad de productos en el carrito */}
    </div>
  );
}

//Componente principal
const ProductsRow =  ({ count, handleAddToCart }) => {
  return (
    
    <div className="row">
      <Header count={count}/>
       {/* <Cart count={count} /> */}
      {products.map(product =>
        <Product key= {product.id} product={product} onAdd={handleAddToCart}/>
      )}
      <Carousel />
    </div>
  );
};

export default function Page() {

 // const [count, setCount] = useState(0);
  const [cartItems, setCartItems] = useState([]);
  const [isCartOpen, setIsCartOpen] = useState(false);

  const handleToggleCart = () => {
    setIsCartOpen(!isCartOpen);
    console.log("isCartOpen:", isCartOpen); // Agregar esta línea
  };

 const handleAddToCart=(product) => {
    const isInCart= cartItems.some((item)=> item.id === product.id);

    if(!isInCart){
      setCartItems([...cartItems, product]);
    //setCount(count + 1);
  }
  };


  return (
    <div>
      <ProductsRow count={cartItems.length} handleAddToCart={handleAddToCart} />
    </div>
   
  );
}
