"use client";
import React, { useState, useEffect } from "react";
import Navbar from "../../components/Navbar";
import "../../styles/direccion.css";
import "bootstrap/dist/css/bootstrap.min.css";

const Detalle = () => {
  const [confirmed, setConfirmed] = useState(false);
  const [orderDetails, setOrderDetails] = useState(undefined);
  const [dataObject, setDataObject] = useState(null);

  useEffect(() => {
    const storedData = localStorage.getItem("tienda");
    if (storedData) {
      try {
        const parsedData = JSON.parse(storedData);
        setDataObject(parsedData);
      } catch (error) {
        console.error("Error al parsear datos de localStorage:", error);
      }
    }
  }, []);

  const mostrarTextArea = dataObject && dataObject.cart && dataObject.cart.metodosPago === 1;

  function procesarPago(e) {
    e.preventDefault();
    const updatedCart = {
      ...dataObject.cart,
      necesitaVerificacion: true,
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
  }

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

  const enviarDatosPago = async () => {
    if (!dataObject) {
      console.error('No se han cargado los datos de tienda correctamente.');
      return;
    }

    const idsProductos = dataObject.cart.productos.map((producto) => String(producto.id));

    const dataToSend = {
      productIds: idsProductos,
      address: dataObject.cart.direccionEntrega,
      paymentMethod: dataObject.cart.metodosPago,
    };

    try {
      const response = await fetch("https://localhost:7013/api/Cart", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(dataToSend),
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
          direccionEntrega: "",
          metodosPago: 0,
          ordenCompra: 0,
        };
        const updatedDataObject = { ...dataObject, cart: initialCartState };
        localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
      } else {
        const errorResponseData = await response.json();
        throw new Error(errorResponseData.message || "Error al procesar el pago");
      }
    } catch (error) {
      console.error("Error al procesar el pago:", error);
    }
  };

  if (!dataObject) {
    return <p></p>; // Puedes mostrar un mensaje de carga aquí si lo deseas
  }

  return (
    <article>
      <div>
        <Navbar cantidad_Productos={dataObject.cart.productos.length} />
      </div>
      <div className="container mt-5">
        <div className="row justify-content-center">
          <div className="col-md-8">
            <div className="detalleCompra text-center">
              {orderDetails && (
                <div className="mb-3">
                  <p>Número de compra: {orderDetails.numeroCompra}</p>
                </div>
              )}
              <p>Número de teléfono: {generarNumeroTelefono()}</p>
              {mostrarTextArea && (
                <textarea className="form-control mt-3" rows="5" placeholder="Comprobante de pago" />
              )}
              <button onClick={enviarDatosPago} className="btn btn-info mt-3">
                Confirmar compra
              </button>
              {confirmed && <p className="mt-3">Compra confirmada!</p>}
            </div>
          </div>
        </div>
      </div>
    </article>
  );
};

export default Detalle;