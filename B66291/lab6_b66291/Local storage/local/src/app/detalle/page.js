"use client"
import "../../styles/direccion.css"
import "bootstrap/dist/css/bootstrap.min.css"; 
import Navbar from '../../components/Navbar';

const Detalle = () => {
  const storedData = localStorage.getItem('tienda');
  const dataObject = JSON.parse(storedData);

  const mostrarTextArea = dataObject && dataObject.cart && dataObject.cart.metodosPago === 'Sinpe Movil'; //verifica si que el metodo de pago sea sinpe

  return (
    
    <article>
      <div>
        <Navbar size={dataObject.cart.productos.length}/>
    </div>
      <div className="detalleCompra">
        <p>Número de compra: #{generarNumeroCompra()}</p>
        <p>Número de telefono: {generarNumeroTelefono()}</p>
        {mostrarTextArea && (
          <textarea className="form-control" rows="5" placeholder="Comprobante de pago" />
        )}
        <p>Procesando compra. Espere confirmación del administrador.</p>
      </div>
    </article>
  );
};

function generarNumeroCompra() {
  return Math.floor(Math.random() * 1000000);
}

function generarNumeroTelefono() {
  return Math.floor(Math.random() * 100000000);
}

export default Detalle;

