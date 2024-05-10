import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css";
import Carousel from 'react-bootstrap/Carousel';


export const Products = ({ }) => {

  const [productList, setProductList] = useState([]);
  useEffect(() => {
    const loadData = async () => {
      try {
        const response = await fetch(`https://localhost:7280/api/Store`);
        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const json = await response.json();
        setProductList(json);
      } catch (error) {
        throw new Error('Failed to fetch data');
      }
    };

    loadData(); // Llamar a loadData() solo una vez al montar el componente

  }, []); 

  const [storeData, setStoreData] = useState(() => {
    const storedStoreData = localStorage.getItem("tienda");
    return JSON.parse(storedStoreData);
  });

  const [showModal, setShowModal] = useState(false);

  const closeModal = () => {
    setShowModal(false);
  };



  const onAddProduct = (product) => {
    if (product == undefined) throw new Error('Invalid Parameter');
    if (storeData.productos.some(item => item.id === product.id)) {
      setShowModal(true);
      
    } else {
      const updatedStore = {
        ...storeData,
        carrito: {
          ...storeData.carrito,
          subtotal: storeData.carrito.subtotal + product.price,
          total: ((storeData.carrito.subtotal + product.price) * storeData.carrito.porcentajeImpuesto) + (storeData.carrito.subtotal + product.price)
        },
        productos: [...storeData.productos, product]
      };
      
      setStoreData(updatedStore);
      localStorage.setItem("tienda", JSON.stringify(updatedStore));
    }
  };


  const Product = ({ product, onAddProduct }) => {
    const { name, description, imageURL, price } = product;
    
    return (
      <div className="col-sm-3">
        <div className='info-product'>
          <h2>{name}</h2>
          <div className='price'>{description}</div>
          <div className='price'>Precio: ₡{price}</div>
          <img src={imageURL} alt={name} />
          <button onClick={() => onAddProduct(product)}>Agregar al Carrito</button>
        </div>
      </div>

    );
  };

  const ProductCarrusel = ({ product }) => {
    const { name,  imageURL  } = product;
    return (
      <div className="col-sm-3">
        <div className='info-product'>
          <h2>{name}</h2>
          <img src={imageURL} alt={name} />
        </div>
      </div>
    );
  };
  const Carrusel=({productos}) =>{
    return (
      <Carousel data-bs-theme="dark">
        {productos && productos.products && productos.products.map(product => (
           <Carousel.Item interval={1000}>
          <div className="carrusel-item">
          <ProductCarrusel key={product.id} product={product}  />
          </div>
          </Carousel.Item>
        ))}
      </Carousel>
    );
  };

return (
  
    <div>
    {showModal && <ModalError closeModal={closeModal} />}
    <div className="row">
      {productList && productList.products && productList.products.map(product => (
        <Product key={product.id} product={product} onAddProduct={onAddProduct} />
        
        ))}
    </div>

    <Carousel data-bs-theme="dark">
        {productList && productList.products && productList.products.map(product => (
          <Carousel.Item key={product.id} interval={1000}>
            <div className="info-product">
              <h2>{product.name}</h2>
              <img src={product.imageURL} alt={product.name} />
            </div>
          </Carousel.Item>
        ))}
      </Carousel>
    
  </div>
);
}

const ModalError = ({ closeModal }) => {
  return (
    <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Producto ya agregado</h5>
            <button type="button" onClick={closeModal} className="close" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div className="modal-body">
            <p>Este producto ya ha sido añadido al carrito.</p>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" onClick={closeModal}>Cerrar</button>
          </div>
        </div>
      </div>
    </div>
  );
};