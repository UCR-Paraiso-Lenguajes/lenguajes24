"use client"
import "../../styles/direccion.css"
import "bootstrap/dist/css/bootstrap.min.css"; 
import React, { useState, useEffect } from "react";
import Navbar from '../../components/Navbar';

const Detalle = () => {
  const [confirmed, setConfirmed] = useState(false); 

  const storedData = localStorage.getItem('tienda');
  const dataObject = JSON.parse(storedData);

  const mostrarTextArea = dataObject && dataObject.cart && dataObject.cart.metodosPago === 1; 

  function procesarPago(e) {
    e.preventDefault();
    const updatedCart = {
      ...dataObject.cart, 
      necesitaVerificacion: true,
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
   
  };

  function actualizarOrden(ordenCompraRespuesta){
    console.log(ordenCompraRespuesta);
    const updatedCart = {
      ...dataObject.cart, 
      ordenCompra: ordenCompraRespuesta,
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
  }
  
  function generarNumeroTelefono() {
    return Math.floor(Math.random() * 100000000);
  }

 const EnviarDatosPago = async () => {

  const idsProductos = dataObject.cart.productos.map(function(producto) {
    return String(producto.id);
  });

  const dataToSend = {
    productIds: idsProductos,
    address: dataObject.cart.direccionEntrega,
    paymentMethod: dataObject.cart.metodosPago,
  };

    const response = await fetch('https://localhost:7013/api/Cart', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(dataToSend)
    });

    if (response.ok) {
      const order = await (response.json());       
      actualizarOrden(order.numeroCompra);
      setConfirmed(true); 
    } else {
      const errorResponseData = await response.json();
      throw new Error(errorResponseData.message || 'Error al procesar el pago');
    }
};
  
  return (
    <article>
      <div>
        <Navbar cantidad_Productos={dataObject.cart.productos.length}/>
      </div>
      <div className="detalleCompra">
        <p>Número de compra: {dataObject.cart.ordenCompra}</p> 
        <p>Número de telefono: {generarNumeroTelefono()}</p>
        {mostrarTextArea && (
          <textarea className="form-control" rows="5" placeholder="Comprobante de pago" />
        )}
        <button
          onClick={procesarPago} 
          className="btn btn-info mt-3" 
        >
          Continuar con la compra
        </button>
        <button
          onClick={EnviarDatosPago} 
          className="btn btn-info mt-3" 
        >
          Confirmar compra
        </button>
        {confirmed && <p>Compra confirmada!</p>}
      </div>
    </article>
  );
};

export default Detalle;
