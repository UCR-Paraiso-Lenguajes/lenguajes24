"use client"; 
import "../../styles/pago.css"; 
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from '../../components/Navbar';
import React, { useState } from 'react';

const Pago = () => {//validaciones
  const storedData = localStorage.getItem("tienda");
  const dataObject = JSON.parse(storedData);
  const [pagoIngresado, setPagoIngresado] = useState(false);

  function agregarPago(e){
    e.preventDefault();
    
    let pago = 0; 
    if(e.target.pago.value == "Efectivo"){
      pago = 0;
    }else{
      pago = 1;
    }
  
    const updatedCart = {
      ...dataObject.cart,
      metodosPago: pago, 
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
    setPagoIngresado(true);
  };

  return (
    <article>
      <div>
          <Navbar cantidad_Productos={dataObject.cart.productos.length}/>
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
          <p className="infoPago" style={{fontSize: '0.8em'}}>Método de pago ingresado exitosamente.</p>
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