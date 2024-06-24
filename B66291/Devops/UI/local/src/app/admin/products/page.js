"use client";
import React, { useEffect } from "react";
import "../../../styles/direccion.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from '../../../components/Navbar';
import Sidebar from '../../../components/Sidebar';

const Products = () => {

  useEffect(() => {
    
    const verificarFechaExpiracion = () => {
      const token = sessionStorage.getItem("token");
      if (!token) {
        window.location.href = "/admin";
      }

      const decodedToken = JSON.parse(atob(token.split(".")[1]));
      const expirationDate = new Date(decodedToken.exp * 1000);

      if (new Date() > expirationDate) {
        sessionStorage.removeItem("token");
        window.location.href = "/admin";
      }
    };

    verificarFechaExpiracion();
    const intervalId = setInterval(verificarFechaExpiracion, 2 * 60 * 1000);
    return () => clearInterval(intervalId);
  }, []);

  const storedData = localStorage.getItem('tienda');
  const dataObject = JSON.parse(storedData);

  return (
    <article>
      <div className="row">
        <div>
          <Navbar cantidad_Productos={dataObject.cart.productos.length} />
        </div>
      </div>
      <div className="col-md-3">
        <Sidebar />
      </div>
    </article>
  );
};

export default Products;