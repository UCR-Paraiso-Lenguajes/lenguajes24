"use client" 
import "../../../styles/direccion.css"
import "bootstrap/dist/css/bootstrap.min.css"; 
import Navbar from '../../../components/Navbar';
import Sidebar from '../../../components/Sidebar';

const products = () => {
 
  const storedData = localStorage.getItem('tienda'); 
  const dataObject = JSON.parse(storedData); 

  return (
    <article>
        <div className="row">
          <div>
            <Navbar cantidad_Productos={dataObject.cart.productos.length} />
          </div>    
        </div>
        <div className="col-md-3">
            <Sidebar />
        </div>
    </article>   
  );
};

export default products;