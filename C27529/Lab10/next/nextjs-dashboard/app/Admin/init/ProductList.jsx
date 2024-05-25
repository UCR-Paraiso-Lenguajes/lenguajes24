import React from 'react';

function ProductList() {
  return (
    <div className="product-list-container">
      <h2>ProductList</h2>
      <div className="d-flex justify-content-center">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Cargando...</span>
        </div>
      </div>


    </div>
  );
}

export default ProductList;
