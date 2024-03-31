"use client"; // Va con comillas dobles
import React from "react";
import "../../styles/pago.css"; // Cambiado a pago.css según el nombre del componente
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from '../../components/Navbar';

const Pago = () => {
  const storedData = localStorage.getItem("tienda");
  const dataObject = JSON.parse(storedData);

  const handleSubmit = (e) => {
    e.preventDefault();
    const pago = e.target.pago.value; // Obtener el valor del radio button seleccionado
    const updatedCart = {
      ...dataObject.cart,
      metodosPago: pago, //variable actualizada
      necesitaVerificacion: true,
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem("tienda", JSON.stringify(updatedDataObject));
    // Actualizar el estado o redirigir si es necesario
    window.location.reload(); // Recargar la página para reflejar los cambios
  };

  const Footer = () => (
    <footer className="bg-body-tertiary text-center text-lg-start">
      <div className="text-center p-3" style={{ backgroundColor: 'black' }}>
        <a className="text-white">© 2024: Condiciones de uso</a>
      </div>
    </footer>
  );

  return (
    <article>
      <div>
        <Navbar size={dataObject.cart.productos.length}/>
      </div>
      <div className="form_pago">
        <form onSubmit={handleSubmit}>
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
          {/* Agrega más opciones de pago según sea necesario */}
          <button type="submit" className="btn btn-primary mt-3">
            Seleccionar tipo de pago
          </button>
        </form>
        <div className="cart_box">
        <a
          href="/detalle" // URL a la página de checkout
          className="btn btn-info mt-3"
        >
          Continuar con la compra
        </a>
      </div>
      </div>
      <div>
        <Footer></Footer>
    </div>
    </article>
  );
};

export default Pago;