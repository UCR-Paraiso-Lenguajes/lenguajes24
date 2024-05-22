"use client";
import Link from 'next/link';
import { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css";
import { Console } from 'console';
import { loadEnvConfig } from '@next/env';


export default function Home() {

  const [storeProducts, setStoreProducts] = useState([]);
  const [carrusel, setCarrusel] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState('');
  const [filteredProducts, setFilteredProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const URL = process.env.NEXT_PUBLIC_API;

  useEffect(() => {
    const loadData = async () => {
      try {

        console.log(process.env.DB_HOST)
        //const response = await fetch('http://localhost:5000/api/Store');
        //const response = await fetch(process.env.NEXT_PUBLIC_API + '/api/Store');
        const response = await fetch(URL + '/api/Store');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const data = await response.json();
        setStoreProducts(data);
        setCarrusel(data);
        setCategories(data.categoriesList);
        console.log(data)
      } catch (error) {
        throw new Error('Failed to fetch data');
      }

    };

    loadData();
  }, []);


  const [cart, setCart] = useState({
    products: [],
    subtotal: 0,
    taxes: 0.13,
    total: 0.0,
    deliveryAddress: '',
    payMethods: {},
    receipt: '',
    orderNumber: '',
    count: 0
  });

  const handleAddToCart = (product) => {
    if (product === undefined) throw new Error('The product is empty.');

    let productsNotInCart = !cart.products.some(item => item.id === product.id);
    if (productsNotInCart) {
      let updatedProductos = [...cart.products, product];
      let updatedCount = cart.count + 1;
      setCart({
        ...cart,
        products: updatedProductos,
        count: updatedCount
      });
      localStorage.setItem('cartItem', JSON.stringify({ products: updatedProductos, count: updatedCount }));
    }
  };


  useEffect(() => {
    const storedCartData = JSON.parse(localStorage.getItem('cartItem') || '{}');
    setCart({
      ...cart,
      products: storedCartData.products || [],
      count: storedCartData.count || 0
    });
  }, []);

  const handleCategoryChange = async (event) => {
    try {
      const selected = event.target.value;
      setSelectedCategory(selected);
      let productsForCategory = [];
      if (selected === "0") {
        const response = await fetch(URL + '/api/Store');
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        productsForCategory = await response.json();
        setStoreProducts(productsForCategory);
      } else {
        //const response = await fetch(`http://localhost:5207/api/Store/products?category=${selected}`);
        const response = await fetch(URL + `/api/Store/products?category=${selected}`);
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        productsForCategory = await response.json();
        setFilteredProducts(productsForCategory);
      }

      setStoreProducts(productsForCategory);


    } catch (error) {
      throw new Error('Fail handleCategory: ' + error.message);
    }
  };


  return (

    <main className="flex min-h-screen flex-col p-6" style={{ backgroundColor: 'silver' }}>

      <header className="p-3 text-bg-dark">
        <div className="row" style={{ color: 'gray' }}>
          <div className="col-sm-2">
            <Link href="/">
              <img src="Logo1.jpg" style={{ height: '75px', width: '200px', margin: '1.4rem' }} className="img-fluid" />
            </Link>
          </div>
          <div className="col-sm-8 d-flex justify-content-center align-items-center">
            <form className="d-flex justify-content-center">
              <input type="search" name="search" style={{ width: '805%' }} placeholder="Book..."></input>
              <button type="submit">Search</button>
            </form>
          </div>

          <div className="col-sm-2 d-flex justify-content-end align-items-center">
            <div style={{ position: 'relative', display: 'inline-block' }}>
              <div >
                <Link href="/cart">
                  <button className="btn btn-success">
                    <img src="https://miguelrevelles.com/wp-content/uploads/carrito-de-la-compra-1.png"
                      style={{ height: '100px', width: '100px' }} className="img-fluid" />
                  </button>
                </Link>
              </div>
              <div style={{ position: 'absolute', top: '-10px', right: '-10px', backgroundColor: 'green', borderRadius: '50%', width: '20px', height: '20px', textAlign: 'center', color: 'white' }}>
                {cart.count}
              </div>
            </div>
          </div>

        </div>
      </header>


      <div className='container'>
        <h2 className='text-left mt-5 mb-5'>List of Books</h2>

        <select className="btn btn-secondary dropdown-toggle" onChange={handleCategoryChange} value={selectedCategory}>
          <option value={0}>ALL</option>

          {categories && categories.map(category => (
            <option key={category.idCategory} value={category.idCategory}>
              {category.name}
            </option>
          ))}
        </select>


        <div className="container" style={{ display: 'flex', flexWrap: 'wrap' }}>
          {storeProducts && storeProducts.products && storeProducts.products.map(item => (
            <Products key={item.id} product={item} handleAddToCart={handleAddToCart} />
          ))}

          {filteredProducts && filteredProducts.map(item => (
            <Products key={item.id} product={item} handleAddToCart={handleAddToCart} />

          ))}


          <CarruselComponent carrusel={carrusel.products && {
            ...carrusel, productsCarrusel:
              carrusel.products.filter(product => product.productCategory.idCategory === 10)
          }} handleAddToCart={handleAddToCart} />

        </div>
      </div >

      <footer className='footer'>
        <div className="text-center p-3">
          <h5 className="text-light">  Paula's Library</h5>
        </div>
      </footer>

    </main>

  );
}

const Products = ({ product, handleAddToCart }) => {
  if (product === undefined) throw new Error('The product is empty.');

  if (product.productCategory.idCategory !== 10) {
    const { id, name, author, imgUrl, price } = product;

    return (
      <div className="row my-3">
        <div key={id} className='col-sm-3 mb-4' style={{ width: '300px', margin: '0.5rem' }}>
          <div className="card" style={{ background: 'white' }}>
            <img src={imgUrl} className="card-img-top" style={{ margin: '0.4rem', width: '250px' }} alt={name} />
            <div className="card-body">
              <div className='text-center'>
                <h4>{name}</h4>
                <p>Author: {author}</p>
                <p>Price: ₡{price}</p>
                <button className="btn btn-dark" onClick={() => handleAddToCart(product)}>Add to Cart</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  } else {
    // Si el idCategory es igual a 10, devuelve null para no renderizar el producto
    return null;
  }
};


//Carrusel

const CarruselComponent = ({ carrusel, handleAddToCart }) => {
  if (!carrusel) {
    return null; // Devuelve null para no renderizar nada si carrusel es undefined o null
  }
  if (typeof handleAddToCart !== 'function') {
    throw new Error('handleAddToCart must be a function');
  }
  const [currentIndex, setCurrentIndex] = useState(0);

  const handlePrev = () => {
    setCurrentIndex(prevIndex => prevIndex === 0 ? carrusel.productsCarrusel.length - 1 : prevIndex - 1);
  };

  const handleNext = () => {
    setCurrentIndex(prevIndex => prevIndex === carrusel.productsCarrusel.length - 1 ? 0 : prevIndex + 1);
  };


  const handleSlide = index => {
    setCurrentIndex(index);
  };

  return (
    <section style={{ margin: '50px' }}>
      <div id="carrouselReact" className="carousel slide carousel-fade" data-bs-ride="carousel">
        <ol className="carousel-indicators">
          {carrusel && carrusel.productsCarrusel && carrusel.productsCarrusel.map((carruselItem, index) => (
            <button
              key={index}
              type="button"
              data-bs-target="#carouselExample"
              data-bs-slide-to={index}
              className={index === currentIndex ? "active" : ""}
              onClick={() => handleSlide(index)}
              aria-label={`Slide ${index + 1}`}
            ></button>
          ))}
        </ol>

        <div className="carousel-inner">
          {carrusel && carrusel.productsCarrusel && carrusel.productsCarrusel.map((carruselItem, index) => (
            <div
              key={index}
              className={`carousel-item ${index === currentIndex ? "active" : ""}`}
            >
              <img className="d-block w-100" src={carruselItem.imgUrl} width="100%" alt={carruselItem.name} />
              <div className="carousel-caption d-none d-md-block" style={{ color: 'black' }}>
                <h5>{carruselItem.name}</h5>
                <p>{carruselItem.author}</p>
                <p>{carruselItem.price}</p>
                <button className="btn btn-dark" onClick={() => handleAddToCart(carruselItem)}>Add to Cart</button>
              </div>
            </div>
          ))}
        </div>
        <button
          className="carousel-control-prev"
          type="button"
          data-bs-target="#carrouselReact"
          data-bs-slide="prev"
          onClick={handlePrev}
        >
          <span className="carousel-control-prev-icon" aria-hidden="true"></span>
          <span className="sr-only">Previous</span>
        </button>
        <button
          className="carousel-control-next"
          type="button"
          data-bs-target="#carrouselReact"
          data-bs-slide="next"
          onClick={handleNext}
        >
          <span className="carousel-control-next-icon" aria-hidden="true"></span>
          <span className="sr-only">Next</span>
        </button>
      </div>
    </section>
  );
};
