import React from "react";
import productsList from '../app/data' //aprender a importar
import Products from './Producs';

const StorePage = ({handleClickAdd}) => { //agrego de parametro
    return (
        <section className="container-fluid">
          <div className="row product-container">
            {productsList.map((item) => ( //extrae info
              <div key={item.id} className="col-3 col-md-3 col-lg-3 mt-2 product-item">
                <Products item={item} key={item.id} handleClickAdd={handleClickAdd}/> {/*lo agrego a cada producto*/}
              </div>
            ))}
          </div>
        </section>
      );
   
}

export default StorePage;