'use client';
import React, { useState, useEffect } from 'react';
import Navbar from '@/app/ui/components/header';
import Product from '@/app/ui/components/product';
import Cart_Store from './ui/components/cart';
import CarouselBanner from '@/app/ui/components/carrusel';
import LoginForm from './admin/page';
import "bootstrap/dist/css/bootstrap.min.css";
import '@/app/ui/styles/app.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { decodeToken, checkTokenDate } from './hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const Page = () => {
  const initialState = {
    products: [],
    cart: {
      subtotal: 0,
      subtotalImpuesto: 0,
      total: 0,
      impVentas: 13,
      direccionEntrega: '',
      metodoPago: '',
      cartItems: {}
    },
    necesitaVerifica: false,
    idCompra: ''
  };

  const [tienda, setTienda] = useState(() => {
    const storedTienda = localStorage.getItem('tienda');
    return storedTienda ? JSON.parse(storedTienda) : initialState;
  });

  const [warning, setWarning] = useState(false);
  const [productList, setProductList] = useState([]);
  const [currentProductIndex, setCurrentProductIndex] = useState(0);
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [currentView, setCurrentView] = useState('products');
  const [categories, setCategories] = useState(null);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const URLConection = process.env.NEXT_PUBLIC_API;
  const router = useRouter();

  const [searchParams, setSearchParams] = useState({
    productName: '',
    categoryIds: []
  });

  useEffect(() => {
    const token = sessionStorage.getItem("sessionToken");
    if (token) {
      const decodedToken = decodeToken(token);
      const isTokenAlive = checkTokenDate(decodedToken?.exp);
      if (!isTokenAlive) {
        sessionStorage.removeItem("sessionToken");
        sessionStorage.removeItem("expiracyToken");
        router.push("/admin");
      }
    }
  }, [router]);

  const handleShowLoginForm = () => {
    setCurrentView('login');
  };

  const handleShowCart = () => {
    setCurrentView('cart');
  };

  const handleShowProducts = () => {
    setCurrentView('products');
  };

  const fetchProductsByCategory = async (categoryIds) => {
    try {
      if (categoryIds.length === 0) {
        await fetchAllProducts();
        return;
      }

      const url = new URL(URLConection + '/api/store/products');
      categoryIds.forEach(id => url.searchParams.append('categoryIds', id));

      const response = await fetch(url);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const data = await response.json();
      if (data && data.products) {
        setProductList(data.products);
      } else {
        setProductList([]);
      }
    } catch (error) {
      throw new Error('Failed to fetch data: ' + error.message);
    }
  };

  const fetchAllProducts = async () => {
    try {
      const response = await fetch(URLConection + '/api/store');
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      if (Array.isArray(json.products)) {
        setProductList(json.products);
      } else {
        setProductList([]);
      }
      const categories = json.categorias;
      setCategories(categories);
    } catch (error) {
      throw new Error('Failed to fetch data: ' + error.message);
    }
  };

  const fetchProductsBySearch = async (productName, categoryIds) => {
    try {
      const url = new URL(URLConection + '/api/store/search');
      if (categoryIds) {
        categoryIds.forEach(id => url.searchParams.append('categoryIds', id));
      }
      url.searchParams.append('productName', productName);

      const response = await fetch(url);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const data = await response.json();
      if (data && data.products) {
        setProductList(data.products);
      } else {
        setProductList([]);
      }
    } catch (error) {
      throw new Error('Failed to fetch data: ' + error.message);
    }
  };

  const handleAddToCart = (newProduct) => {
    const isPresent = tienda.products.some(product => product.id === newProduct.id);

    if (isPresent) {
      setWarning(true);
      setTimeout(() => {
        setWarning(false);
      }, 2000);
      return;
    }
    const updatedProducts = [...tienda.products, newProduct];
    const updatedDataObject = { ...tienda, products: updatedProducts };

    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
    setTienda(updatedDataObject);
  };

  const handleCategorySelect = async (selectedCategory) => {
    setSelectedCategory(selectedCategory);
    const updatedCategoryIds = selectedCategory
      ? [selectedCategory.id]
      : [];
    await fetchProductsByCategory(updatedCategoryIds);
    setSearchParams(prev => ({
      ...prev,
      categoryIds: updatedCategoryIds
    }));
  };

  const handleSearch = async (productName, categoryIds) => {
    await fetchProductsBySearch(productName, categoryIds);
    setSearchParams({
      productName,
      categoryIds: categoryIds || []
    });
  };

  useEffect(() => {
    const loadData = async () => {
      await fetchAllProducts();
    };

    loadData();
  }, []);

  useEffect(() => {
    const url = new URL(window.location.href);
    url.searchParams.delete('productName');
    url.searchParams.delete('categoryIds');

    searchParams.categoryIds.forEach(id => url.searchParams.append('categoryIds', id));
    if (searchParams.productName) {
      url.searchParams.append('productName', searchParams.productName);
    }

    window.history.pushState({}, '', url);
  }, [searchParams]);

  return (
    <div>
      <Navbar
        size={tienda.products.length}
        onShowLogin={handleShowLoginForm}
        onShowCart={handleShowCart}
        onShowProducts={handleShowProducts}
        categories={categories}
        onSearch={handleSearch}
        setSelectedCategory={handleCategorySelect}
        fetchProductsByCategory={fetchProductsByCategory}
        fetchProductsBySearch={fetchProductsBySearch}
      />
      {currentView === 'login' && <LoginForm />}
      {warning && <div className='alert'>El producto ya se encuentra en el carrito</div>}
      {currentView === 'products' && productList && productList.length > 0 ? (
        <div className='container'>
          <div id="carouselExampleIndicators" className="carousel slide" data-bs-ride="carousel">
            <div className="carousel-indicators">
              {productList.map((product, i) => (
                <button key={product.id} type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide-to={i} className={i === currentProductIndex ? 'active' : ''} aria-current={i === currentProductIndex ? 'true' : 'false'} aria-label={`Slide ${i + 1}`} />
              ))}
            </div>
            <div className="carousel-inner">
              {productList.map((product, index) => (
                <CarouselBanner key={product.id} banner={product} />
              ))}
            </div>
            <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="prev" onClick={() => setCurrentProductIndex((currentProductIndex + productList.length - 1) % productList.length)}>
              <span className="carousel-control-prev-icon" aria-hidden="true"></span>
              <span className="visually-hidden">Previous</span>
            </button>
            <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleIndicators" data-bs-slide="next" onClick={() => setCurrentProductIndex((currentProductIndex + 1) % productList.length)}>
              <span className="carousel-control-next-icon" aria-hidden="true"></span>
              <span className="visually-hidden">Next</span>
            </button>
          </div>
        </div>
      ) : (
        currentView === 'products' && (
          <div className='container'>
            <div>No hay productos que coincidan con los criterios de búsqueda.</div>
          </div>
        )
      )}
      <div className='AddFromCarousel'>
        {currentView === 'products' && <button className="btn btn-primary" onClick={handleAddToCart}>Add to cart</button>}
      </div>
      {currentView === 'products' && (
        <div className="product-list row">
          {Array.isArray(productList) && productList.map((product) => (
            <Product key={product.id} product={product} handleClick={handleAddToCart} />
          ))}
        </div>
      )}
      {currentView === 'cart' && <Cart_Store />}
      <footer className="bg-dark text-white text-center text-lg-start d-flex justify-content-center align-items-center">
        <div className="row row-cols-4">
          © 2024: Derechos reservados para Kendall Sánchez
        </div>
      </footer>
    </div>
  );
};

export default Page;
