"use client";
import React, { useState, useEffect } from 'react';
import Navbar from '../../components/Navbar';
import "../../styles/pago.css";
import "bootstrap/dist/css/bootstrap.min.css";

const Pago = () => { //los metodos de pago deben venir de la consulta en base de datos no de local storage ahora
  const [dataObject, setDataObject] = useState(null);
  const [pagoIngresado, setPagoIngresado] = useState(false);

  useEffect(() => {
    if (typeof localStorage !== 'undefined') {
      const storedData = localStorage.getItem("tienda");
      if (storedData) {
        try {
          const parsedData = JSON.parse(storedData);
          setDataObject(parsedData);
        } catch (error) {
          console.error('Error al parsear datos de localStorage:', error);
        }
      }
    }
  }, []);

  function agregarPago(e) {
    e.preventDefault();

    if (!dataObject) {
      console.error('No se han cargado los datos de tienda correctamente.');
      return;
    }

    let metodoPago = e.target.pago.value;
    let pago = 0; 

    if (metodoPago === "Sinpe Movil") {
      pago = 1;
    }

    const updatedCart = {
      ...dataObject.cart,
      metodosPago: pago,
    };

    const updatedDataObject = { ...dataObject, cart: updatedCart };

    try {
      localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
      setPagoIngresado(true);
    } catch (error) {
      console.error('Error al guardar en localStorage:', error);
      throw new Error('No se pudo guardar el método de pago.');
    }
  };

  if (!dataObject) {
    return <p></p>;
  }

  return (
    <article>
      <div>
        <Navbar cantidad_Productos={dataObject.cart.productos.length} />
      </div>
      <div className="form_pago">
        <form onSubmit={agregarPago}>
          <div className="form-check">
            <input
              type="radio"
              id="pago1"
              name="pago"
              value="Efectivo"
              className="form-check-input"
            />
            <label htmlFor="pago1" className="form-check-label">
              Efectivo
            </label>
          </div>
          <div className="form-check">
            <input
              type="radio"
              id="pago2"
              name="pago"
              value="Sinpe Movil"
              className="form-check-input"
            />
            <label htmlFor="pago2" className="form-check-label">
              Sinpe Movil
            </label>
          </div>
          <button type="submit" className="btn btn-primary mt-3">
            Seleccionar tipo de pago
          </button>
        </form>
        {pagoIngresado && (
          <p className="infoPago" style={{ fontSize: '0.8em' }}>Método de pago ingresado exitosamente.</p>
        )}
        <div className="cart_box">
          <a
            href="/detalle"
            className="btn btn-info mt-3"
          >
            Continuar con la compra
          </a>
        </div>
      </div>
    </article>
  );
};

export default Pago;