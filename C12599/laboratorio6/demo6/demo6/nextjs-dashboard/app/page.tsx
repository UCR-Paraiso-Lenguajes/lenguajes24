'use client'
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import 'bootstrap/dist/css/bootstrap.css';
import '../app/ui/globals.css';
import 'bootstrap-icons/font/bootstrap-icons.css';

const products = [
  {
    id: 1,
    name: "Producto 1",
    description: "Descripción del producto 1",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 10.00
  },
  {
    id: 2,
    name: "Producto 2",
    description: "Descripción del producto 2",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 15.00
  },
  {
    id: 3,
    name: "Producto 3",
    description: "Descripción del producto 3",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 20.00
  },
  {
    id: 4,
    name: "Producto 4",
    description: "Descripción del producto 4",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 25.00
  },
  {
    id: 5,
    name: "Producto 5",
    description: "Descripción del producto 5",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 30.00
  },
  {
    id: 6,
    name: "Producto 6",
    description: "Descripción del producto 6",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 35.00
  },
  {
    id: 7,
    name: "Producto 7",
    description: "Descripción del producto 7",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 40.00
  },
  {
    id: 8,
    name: "Producto 8",
    description: "Descripción del producto 8",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 45.00
  },
  {
    id: 9,
    name: "Producto 9",
    description: "Descripción del producto 9",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 50.00
  },
  {
    id: 10,
    name: "Producto 10",
    description: "Descripción del producto 10",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 55.00
  },
  {
    id: 11,
    name: "Producto 11",
    description: "Descripción del producto 11",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 60.00
  },
  {
    id: 12,
    name: "Producto 12",
    description: "Descripción del producto 11",
    imageUrl: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlgv-oyHOyGGAa0U9W524JKA361U4t22Z7oQ&usqp=CAU",
    price: 70.00
  }
];


const Page = () => {
  
  const [cart, setCart] = useState({
    productos: [], 
    subtotal: 0,
    porcentajeImpuesto: 0.13,
    total: 0.0,
    direccionEntrega: '',
    metodosDePago: {},
    comprobante: '',
    confirmacion: '',
    numeroCompra: 0,
    numeroPago: 0,
  });
  
  const handleAddToCart = (product) => {
    
    if (!cart.productos.some(item => item.id === product.id)) {
     
      const updatedProductos = [...cart.productos, product];
      const updatedCount = cart.count + 1;
      setCart({
        ...cart,
        productos: updatedProductos,
        count: updatedCount
      });
     
      localStorage.setItem('cartData', JSON.stringify({ productos: updatedProductos, count: updatedCount }));
    }
  };

  useEffect(() => {
    const storedCartData = JSON.parse(localStorage.getItem('cartData') || '{}');
    setCart({
      ...cart,
      productos: storedCartData.productos || [],
      count: storedCartData.count || 0
    });
  }, []);

  const Products = ({ products }) => {
    return (
      <div className="row">
        {products.map(product => (
          <div key={product.id} className="col-sm-3 mb-4">
            <div className="card">
              <img src={product.imageUrl} className="card-img-top" alt={product.name} />
              <div className="card-body">
                <div className="text-center">
                  <h5 className="card-title my-3">{product.name}</h5>
                  <p className="card-text my-3">{product.description}</p>
                  <p className="card-text my-3">Precio: ${product.price}</p>
                  <button className="btn btn-primary" onClick={() => handleAddToCart(product)}>Comprar</button>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    );
  };

 
  return (
    <div className="row">
      <div className="col-sm-10 text-center">
        <form action="/buscar" method="GET" className="mt-2">
          <input type="text" name="q" placeholder="Buscar..." />
          <button className="btn btn-primary">Buscar</button>
        </form>
      </div>
      <div className="col-sm-2">
        <div className='my-3' style={{ position: 'relative', display: 'inline-block' }}>
          <div>
            <Link href="/cart">
              <button className="btn btn-primary">
                <i className="bi bi-cart-fill"></i>
              </button>
            </Link>
          </div>
          <div style={{ position: 'absolute', top: '-10px', right: '-10px', backgroundColor: 'red', borderRadius: '50%', width: '20px', height: '20px', textAlign: 'center', color: 'white' }}>
            {cart.count}
          </div>
        </div>
      </div>
      <div className="row">
        <h1 className="mb-0">Lista de Productos</h1>
      </div>
      <div className="row">
        <Products products={products} />
      </div>
    </div>
  );
};

export default Page;
