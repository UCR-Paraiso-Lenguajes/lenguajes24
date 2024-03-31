"use client" //va con doble comilla
import "../../styles/direccion.css"
import "bootstrap/dist/css/bootstrap.min.css"; 
import Navbar from '../../components/Navbar';

const Direccion = () => {
  const storedData = localStorage.getItem('tienda'); // Obtiene datos locales
  // console.log(storedData);
  const dataObject = JSON.parse(storedData); // Convierte a objeto JavaScript para acceder a la propiedad

  const handleSubmit = (e) => {
    e.preventDefault();
    const updatedDirection = e.target.direccion.value;
    const updatedCart = {
      ...dataObject.cart,
      direccionEntrega: updatedDirection 
    };
    const updatedDataObject = { ...dataObject, cart: updatedCart };
    localStorage.setItem('tienda', JSON.stringify(updatedDataObject));
    // Actualizar el estado o redirigir si es necesario
    window.location.reload(); // Recargar la página para reflejar los cambios
  };

  return (
    <article>
      <div>
        <Navbar size={dataObject.cart.productos.length}/>
      </div>
      <div className="form_direccion">
        <form onSubmit={handleSubmit}>
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
          href="/pago" // URL a la página de checkout
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