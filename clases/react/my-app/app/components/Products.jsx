import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css";
import Carousel from 'react-bootstrap/Carousel';


export const Products = ({ }) => {

  const [productList, setProductList] = useState([]);
  const [listNames, setListNames] = useState([]);

  const loadData = async () => {
    try {
      const response = await fetch(`https://localhost:7280/api/store`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      if (json == null )throw new Error('Failed to fetch data, null response');
      var nameData = json.categoriesNames;

      setListNames(nameData);
      setProductList(json);

    } catch (error) {
      throw new Error('Failed to fetch data');
    }
  };
  useEffect(() => {

    loadData();


  }, []);

  const loadFilteredData = async (category) => {
    try {
      const response = await fetch(`https://localhost:7280/api/store/products?category=${category}`);
      if (!response.ok) {
        throw new Error('Failed to fetch data');
      }
      const json = await response.json();
      if (json == null )throw new Error('Failed to fetch data, null response');
      var data = json.products;
      
      setProductList(data);
    } catch (error) {
      throw new Error('Failed to fetch data');
    }
  };

  const selectCategory = (category) => {
    loadFilteredData(category);

  }

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
    const { name, description, imageURL, price, category } = product;

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




  return (
    <div className="container-fluid vh-100">
    <nav className="navbar navbar-expand-lg bg-body-tertiary">
      <div className="container-fluid">
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup"
          aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
          <div className="navbar-nav">
            <div className="secciones">
              <div className="row">
                <button className="nav-link btn btn-primary" onClick={() => loadData()}> Página Principal</button>
                {listNames.map((category) => (
                  <div key={category.id} className="col-sm-2">
                    <button className="nav-link btn btn-primary" onClick={() => selectCategory(category.id)}> {category.name} </button>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>
  
    {showModal && <ModalError closeModal={closeModal} />}
    <div className="row flex-grow-1">
      {productList && Array.isArray(productList.products) && productList.products.map(product => (
        <Product key={product.id} product={product} onAddProduct={onAddProduct} />
      ))}
    </div>
  
    <Carousel data-bs-theme="dark" className="flex-grow-1">
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