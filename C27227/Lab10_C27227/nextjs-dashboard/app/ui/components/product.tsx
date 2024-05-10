import React from 'react';
import { Button } from '../button';
import '../styles/product.css';


const Product = ({ product, handleClick}) => {
    if (product === undefined || handleClick === undefined) {
      throw new Error("Product or handleClick is undefined");
    }
    const { name, imageUrl, price } = product;
    if (!name || !imageUrl || price === undefined) {
      throw new Error("Required product attributes are undefined");
    }
    return (
      <div className="col-md-3 mb-4">
        <div className="border p-4 product-container">
          <h3>{name}</h3>
          <p>$ {price}</p>
          <img src={imageUrl} alt={name} />
          <button className="add-to-cart-btn" onClick={()=>handleClick(product)} >Add to Cart</button>
        </div>
      </div>
    );
  };

  export default Product