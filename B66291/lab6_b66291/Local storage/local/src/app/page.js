"use client" //va con doble comilla
import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; 
import Navbar from '../components/Navbar';
import StorePage from '../components/StorePage';


export default function Home() {

  const initialState = {
    impVentas: 13,
    cart: { productos: [], subtotal: 0, total: 0, direccionEntrega: '', metodosPago : '', necesitaVerificacion: false},
  };
  
  // Obtener el estado de tienda desde localStorage o usar el estado inicial
  const [tienda, setTienda] = useState(() => {
    const storedTienda = localStorage.getItem('tienda');
    return storedTienda ? JSON.parse(storedTienda) : initialState;
  });

  // Guardar el estado de tienda en localStorage cuando cambie
  useEffect(() => {
    localStorage.setItem('tienda', JSON.stringify(tienda));
  }, [tienda]);

const handleClickAdd=(item)=>{ //1. por medio de esta funcion puedo enviar un objeto por el boton
  const isPresent = tienda.cart.productos.some(producto => producto.id === item.id);

  if (!isPresent) {
    // Si el item no está presente, actualizar el estado
    const nuevosProductos = [...tienda.cart.productos, item];
    //se encarga de sumar los precios en los productos
    const nuevoSubtotal = nuevosProductos.reduce((total, producto) => total + producto.precio, 0);
    const nuevoTotal = nuevoSubtotal * (1 + tienda.impVentas / 100);
    const direccionS = ' ';
    const necesitaVerificacionS = false

    setTienda({
      ...tienda,
      cart: {
        ...tienda.cart,
        productos: nuevosProductos,
        subtotal: nuevoSubtotal,
        total: nuevoTotal,
        direccionEntrega: direccionS,
        necesitaVerificacion: necesitaVerificacionS
      }
    });
  }
};

const Footer = () => (
  <footer className="bg-body-tertiary text-center text-lg-start">
    <div className="text-center p-3" style={{ backgroundColor: 'black' }}>
      <a className="text-white">© 2024: Condiciones de uso</a>
    </div>
  </footer>
);

return ( 
    <div>
    {/*header*/}
      <div>
        <Navbar size={tienda.cart.productos.length}/>
      </div>

    {/*pagina principal*/}
      <main>
      <div className="container" >
        <div>
          <StorePage 
          handleClickAdd={handleClickAdd}/> {/*El handle click azul define el proximo en product*/}
        </div>

      </div>
    </main>
    {/*footer*/}
    <div>
        <Footer></Footer>
    </div>
    </div>
  );
}
