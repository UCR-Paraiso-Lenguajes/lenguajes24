'use client';
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/global.css';
import Link from 'next/link';

const EjemploPage: React.FC = () => {
  const [carrito, setCarrito] = useState<any[]>([]);
  const [subtotal, setSubtotal] = useState<number>(0);
  const [total, setTotal] = useState<number>(0);

  useEffect(() => {
    const carritoGuardado = JSON.parse(localStorage.getItem('cartItems') || '[]');
    setCarrito(carritoGuardado);
  }, []);

  useEffect(() => {
    const subtotalCalculado = carrito.reduce((total, item) => total + item.price, 0);
    setSubtotal(subtotalCalculado);

    const totalCalculado = carrito.reduce((total, item) => total + (item.price * 1.13), 0);
    setTotal(totalCalculado);
  }, [carrito]);

  return (
    <div>
      <h1>Carrito de Compras</h1>
      <div className="row  my-3">
        {carrito.map(item => (
          <div key={item.id} className="col-sm-3 mb-4">
            <div className="card">
              <img src={item.imageUrl} className="card-img-top" alt={item.name} />
              <div className="card-body">
                <div className="text-center">
                  <h5 className="card-title my-3">{item.name}</h5>
                  <p className="card-text my-3">{item.description}</p>
                  <p className="card-text my-3">Precio: ${item.price}</p>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
      <div className="text-right">
        <h2>Subtotal: ${subtotal.toFixed(2)}</h2>
        <h2 className='my-3'>Total: ${total.toFixed(2)}</h2>
      </div>
      <div className="text-right ">
        {carrito.length > 0 && (
          <Link href="/pay">
            <button className="btn btn-primary" onClick={() => console.log('Compra realizada')}>Comprar</button>
          </Link>
        )}
      </div>
    </div>
  );
};

export default EjemploPage;