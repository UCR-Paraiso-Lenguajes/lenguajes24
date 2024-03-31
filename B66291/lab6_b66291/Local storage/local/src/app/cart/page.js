"use client"
import React from "react";
import "../../styles/cart.css"
import "bootstrap/dist/css/bootstrap.min.css"; 
import Navbar from '../../components/Navbar';

const Cart = () => {

const storedData = localStorage.getItem('tienda');   //obtiene local
//console.log(storedData);
const dataObject = JSON.parse(storedData);     //convierte a js para acceder a la propiedad

const handleDeleteProduct = (productId) => {
  const updatedProducts = dataObject.cart.productos.filter(product => product.id !== productId);
  const updatedCart = {
    ...dataObject.cart,
    productos: updatedProducts
  };

  // Calcular el nuevo subtotal
  const newSubtotal = updatedProducts.reduce((total, product) => total + product.precio, 0);

  // Calcular el nuevo total con impuestos
  const newTotal = newSubtotal * (1 + dataObject.impVentas / 100);

  const updatedCartWithTotals = {
    ...updatedCart,
    subtotal: newSubtotal,
    total: newTotal
  };

  const updatedDataObject = { ...dataObject, cart: updatedCartWithTotals };
  localStorage.setItem('tienda', JSON.stringify(updatedDataObject));
  // Actualizar el estado o redirigir si es necesario
  window.location.reload(); // Recargar la página para reflejar los cambios
};

const isCartEmpty = dataObject.cart.productos.length === 0; //verifica que no este vacio

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
    {dataObject.cart.productos.map((item) => (
      <div className="cart_box" key={item.id}>
        <div className="cart_id">
          <span>{item.name}</span>
        </div>
        <div className="cart_description">
          <span>{item.description}</span>
        </div>
        <div className="cart_precio">
          <span>${item.precio}</span>
        </div>
        <div className="cart_image">
          <img
            src={item.imageUrl}
            alt="Product Image"
            style={{ height: '70px', width: '80%' }}
            className="imgProduct"
          />
        </div>
        <button
          className="btn btn-danger mt-3"
          onClick={() => handleDeleteProduct(item.id)}
        >
          Eliminar producto
        </button>
      </div>
    ))}

    {/* Nueva fila */}
    <div className="cart_box" style={{ flex: 1, justifyContent: 'flex-end' }}>
      <span>subtotal sin impuesto: ${dataObject.cart.subtotal}</span>
    </div>

    {/* Nueva fila */}
    <div className="cart_box" style={{ flex: 1, justifyContent: 'flex-end' }}>
      <span>total con impuesto: ${dataObject.cart.total.toFixed(2)}</span>
    </div>

    {/* Nueva fila */}
    <div className="cart_box" style={{ flex: 1, justifyContent: 'flex-end' }}>
    <a
    href="/direccion" // URL a la página de checkout
    className="btn btn-info mt-3"
    disabled={isCartEmpty} // Deshabilitar el enlace si el carrito está vacío
    >
    Continuar con la compra
    </a>
    </div>
    <div>
        <Footer></Footer>
    </div>
  </article>
);
}

export default Cart


