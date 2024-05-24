"use client"
import "../../styles/direccion.css"
import "bootstrap/dist/css/bootstrap.min.css"; 
import React, { useState, useEffect } from "react";
import Navbar from '../../components/Navbar';

const Detalle = () => {//validaciones
  const [confirmed, setConfirmed] = useState(false);
  const [orderDetails, setOrderDetails] = useState(undefined); 

  const storedData = localStorage.getItem('tienda');
  const dataObject = JSON.parse(storedData);

  const mostrarTextArea = dataObject && dataObject.cart && dataObject.cart.metodosPago == 1;

  function procesarPago(e) {
    e.preventDefault();
    const updatedCart = {
      ...dataObject.cart,
      necesitaVerificacion: true,
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));

  };

  function actualizarOrden(ordenCompraRespuesta) {
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

    const idsProductos = dataObject.cart.productos.map(function (producto) {
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
      const order = await response.json();
      actualizarOrden(order.numeroCompra);
      setConfirmed(true);
      setOrderDetails(order);
      const initialCartState = {
        productos: [],
        subtotal: 0,
        total: 0,
        direccionEntrega: '',
        metodosPago: 0,
        ordenCompra: 0
      };
      const updatedDataObject = { ...dataObject, cart: initialCartState };
      localStorage.setItem("tienda", JSON.stringify(updatedDataObject));

    } else {
      const errorResponseData = await response.json();
      throw new Error(errorResponseData.message || 'Error al procesar el pago');
    }
  };

  return (
    <article>
      <div>
        <Navbar cantidad_Productos={dataObject.cart.productos.length} />
      </div>
      <div className="detalleCompra">
        {orderDetails && ( 
          <div>
            <p>Número de compra: {orderDetails.numeroCompra}</p>
          </div>
        )}
        <p>Número de telefono: {generarNumeroTelefono()}</p>
        {mostrarTextArea && (
          <textarea className="form-control" rows="5" placeholder="Comprobante de pago" />
        )}
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