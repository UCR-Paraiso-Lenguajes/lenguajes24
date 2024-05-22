"use client"
import React, { useState, useEffect } from 'react';
import { ProductItem, Product } from './layout';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../HTMLPageDemo.css';
import Link from 'next/link';

export default function Page() {
  const [availableProducts, setAvailableProducts] = useState<ProductItem[]>([]);
  const [cartProducts, setCartProducts] = useState<ProductItem[]>([]);
  const [categories, setCategories] = useState<{ idCategory: number; nameCategory: string; }[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>('');
  const [searchQuery, setSearchQuery] = useState<string>('');

  useEffect(() => {
    const loadProductData = async () => {
      try {
        const productResponse = await fetch('https://localhost:7165/api/Store');
        if (!productResponse.ok) {
          throw new Error('Failed to fetch products');
        }
        const productJson = await productResponse.json();
        if (!Array.isArray(productJson.products)) {
          throw new Error('Invalid product data format');
        }
        setAvailableProducts(productJson.products);
      } catch (error) {
        setError('Error al cargar productos');
      } finally {
        setLoading(false);
      }
    };

    const loadCategoryData = async () => {
      try {
        const categoryResponse = await fetch('https://localhost:7165/api/Store/Categories');
        if (!categoryResponse.ok) {
          throw new Error('Failed to fetch categories');
        }
        const categoryJson = await categoryResponse.json();
        setCategories(categoryJson.map((category: any) => category));
      } catch (error) {
        setError('Error al cargar categorías');
      }
    };

    loadProductData();
    loadCategoryData();

    const savedCartProducts = JSON.parse(localStorage.getItem('cartProducts') || '[]');
    setCartProducts(savedCartProducts);
  }, []);

  const handleCategoryChange = async (category: number) => {
    setSelectedCategory(category);
    setLoading(true);
    setError('');

    try {
      if (category === 0) {
        const productResponse = await fetch('https://localhost:7165/api/Store');
        if (!productResponse.ok) {
          throw new Error('Failed to fetch products');
        }
        const productJson = await productResponse.json();
        setAvailableProducts(productJson.products);
      } else {
        const response = await fetch(`https://localhost:7165/api/Store/Products?categoryId=${category}`);
        if (!response.ok) {
          throw new Error('Failed to fetch products');
        }
        const productJson = await response.json();
        const filteredProducts = productJson.filter((product: any) => product.categoria.idCategory === category);
        setAvailableProducts(filteredProducts);
      }
    } catch (error) {
      setError('Error al cargar productos');
    } finally {
      setLoading(false);
    }
  };

  const addToCart = (product: ProductItem) => {
    setCartProducts(prevProducts => {
      const updatedProducts = [...prevProducts, product];
      localStorage.setItem('cartProducts', JSON.stringify(updatedProducts));
      return updatedProducts;
    });
  };

  const handleSearch = async () => {
    try {
      setLoading(true);
      setError('');
  
      let searchUrl = `https://localhost:7165/api/Store/Search?productName=${encodeURIComponent(searchQuery)}`;
  
      if (selectedCategory !== 0) {
        searchUrl += `&categoryId=${selectedCategory}`;
      }
  
      const response = await fetch(searchUrl);
      if (!response.ok) {
        throw new Error('Failed to fetch products');
      }
  
      const productJson = await response.json();
      setAvailableProducts(productJson);
    } catch (error) {
      setError('Error al cargar productos');
    } finally {
      setLoading(false);
    }
  };

  const filteredProducts = availableProducts.filter(product =>
    product.name.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <main className="flex min-h-screen flex-col p-6">
      <header className="header-container row">
        <div className="search-container col-sm-4 d-flex">
          <input
            type="search"
            className="form-control"
            placeholder="Buscar"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />

          <button className="btn btn-primary" onClick={handleSearch}>
            Buscar
          </button>
          <Link href="/cart" className="col-sm-4 d-flex justify-content-end">
            <button className="btn btn-outline-secondary">
              <img src="/img/carrito.png" className="col-sm-6" />
              {cartProducts.length}
            </button>
          </Link>
        </div>
        <div className="categories-container col-sm-4">
          <select
            className="form-select"
            style={{ height: "auto", maxHeight: "200px", overflowY: "auto" }}
            value={selectedCategory}
            onChange={(e) => handleCategoryChange(Number(e.target.value))}
          >
            <option value="0">Todas las Categorías</option>
            {categories.map(category => (
              <option key={category.idCategory} value={category.idCategory}>
                {category.nameCategory}
              </option>
            ))}
          </select>
        </div>
      </header>

      {loading ? (
        <div>Cargando...</div>
      ) : error ? (
        <div>{error}</div>
      ) : (
        <div>
          <h1>Lista de Productos</h1>
          <div className='row' style={{ display: 'flex', flexWrap: 'wrap' }}>
            {filteredProducts && filteredProducts.map(product =>
              <Product key={product.id} product={product} addToCart={addToCart} />
            )}
          </div>
        </div>
      )}

      <div className="products col-sm-6" style={{ margin: '0 auto' }}>
        <div id="productsCarouselControl" className="carousel" data-bs-ride="carousel">
          <div className="carousel-inner">
            <div className="carousel-item active">
              <img src="/img/Chromecast.jpg" className="d-block w-100" />
            </div>
            <div className="carousel-item">
              <img src="/img/teclado.jpg" className="d-block w-100" />
            </div>
            <div className="carousel-item">
              <img src="/img/Pantalla.jpg" className="d-block w-100" />
            </div>
          </div>
          <button className="carousel-control-prev" type="button" data-bs-target="#productsCarouselControl" data-bs-slide="prev">
            <span className="carousel-control-prev-icon" aria-hidden="true"></span>
            <span className="visually-hidden">Previous</span>
          </button>
          <button className="carousel-control-next" type="button" data-bs-target="#productsCarouselControl" data-bs-slide="next">
            <span className="carousel-control-next-icon" aria-hidden="true"></span>
            <span className="visually-hidden">Next</span>
          </button>
        </div>
      </div>

      <footer className="footer-container">
        <p>Derechos reservados, 2024</p>
      </footer>
    </main>
  );
}