"use client" 
import "../../styles/direccion.css"
import "bootstrap/dist/css/bootstrap.min.css"; 
import Navbar from '../../components/Navbar';

const Direccion = () => {
 

  const storedData = localStorage.getItem('tienda'); 
  const dataObject = JSON.parse(storedData); 

  function agregarDireccion(e) {
    e.preventDefault();
    const updatedDirection = e.target.direccion.value;
    const updatedCart = {
      ...dataObject.cart,
      direccionEntrega: updatedDirection 
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem('tienda', JSON.stringify(updatedDataObject));
  };

  return (
    <article>
      <div>
         <Navbar cantidad_Productos={dataObject.cart.productos.length}/>
      </div>
      <div className="form_direccion">
        <form onSubmit={agregarDireccion}>
          <label htmlFor="direccion">Dirección de entrega:</label>
          <input
            type="text"
            id="direccion"
            placeholder="Ingresa tu dirección"
          />
          <button className="btnAsignar" type="submit">Asignar</button>
        </form>
        <div className="cart_box">
        <a
          href="/pago" 
          className="btn btn-info mt-3"
        >
          Continuar con la compra
        </a>
      </div> 
      </div>
    </article>
    
  );
};

export default Direccion;