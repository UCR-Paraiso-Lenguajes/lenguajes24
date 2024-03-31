import React from "react";
import '../styles/products.css'

const Products = ({item, handleClickAdd}) => { //5. paso la lista y la funcion
    const { name, description, precio, imageUrl } = item;
    return (
        <div className="productContent">
        <div className="border p-3 text-center">
        <h3>{name}</h3>
        <h5>{description}</h5>
        <img src={imageUrl} style={{ height: '220px', width: '100%' }} className="imgProduct" alt="Product Image" />
        <h5 style={{paddingTop: '20px' }}>${precio}</h5>
        <button type="button" className="btn btn-success mt-3" onClick={()=>handleClickAdd(item)}> Agregar al carrito</button> 
      </div>
      </div>
    );  
  };

  export default Products;