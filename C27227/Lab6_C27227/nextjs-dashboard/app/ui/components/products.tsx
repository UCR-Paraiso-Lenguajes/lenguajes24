import React from 'react';
import products from '@/app/ui/data';
import Product from '@/app/ui/components/product';
import '../styles/app.css';

const Products_Store = ({handleClick}) => {


    return(
        <div className="product-list row">
        {products.map((product) => (
          <Product key={product.id} product={product} handleClick={handleClick} />
        ))}
      </div>
    )

}

export default Products_Store;