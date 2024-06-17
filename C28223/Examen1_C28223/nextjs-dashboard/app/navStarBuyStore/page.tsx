"use client";
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import React, { useState, useEffect } from 'react';
import "@/app/ui/styles.css";
import Link from 'next/link';
import { Dropdown, Form } from 'react-bootstrap';

const Header = ({ size, setShow, fetchData, category }) => {
  let isSizeUndefined = size === undefined; 
  let isSetShowUndefined = setShow === undefined; 
  let isFetchDataUndefined = fetchData === undefined; 
  let isCategoryUndefined = category === undefined;
  let areArgumentsUndefined = isSizeUndefined || isSetShowUndefined || isFetchDataUndefined || isCategoryUndefined;
  if(areArgumentsUndefined){throw new Error('Los argumentos para mostrar el header no pueden ser indefinidos.');}
  const [searchText, setSearchText] = useState('');
  const [categorySelected, setCategorySelected] = useState([]);
  const [showModal, setShowModal] = useState(false);

  const handleReloadPage = () => {
    window.location.reload();
  };

  const handleSearch = () => {
    const trimmedSearchText = searchText.trim();
    const elTextoABuscarEsVacio = trimmedSearchText === '';
    const noHayCategoria = categorySelected.length === 0;
    if (elTextoABuscarEsVacio && noHayCategoria) {
      setShowModal(true);
    } else {
      fetchData(searchText, categorySelected);
    }
  };
  useEffect(() => {
  }, [categorySelected]);

  const closeModal = () => {
    setShowModal(false);
  };

  const handleCategoryChange = (categoryID) => {
    if(category==undefined){throw new Error('Los argumentos para handleCategoryChange no pueden ser indefinidos.');}
    const updatedCategories = categorySelected.includes(categoryID)
      ? categorySelected.filter(cat => cat !== categoryID)
      : [...categorySelected, categoryID];
    setCategorySelected(updatedCategories);
  };

  return (
    <div>
      <div className='col'>
        <div className='barra' >
          <title >StarBuyStore</title>
          <h1 onClick={handleReloadPage}>StarBuyStore</h1>
          <div className='search'>
            <input
              id="search"
              type="search"
              placeholder="Buscar"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
            />
            <button className="btn btn-outline-dark mr-2" onClick={handleSearch}>Buscar</button>
          </div>
          <div className="access">
            <div className='carro' onClick={() => setShow(false)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="black" className="bi bi-cart" viewBox="0 0 16 16">
                <path d="M0 1.5A.5.5 0 0 1 .5 1H2a.5.5 0 0 1 .485.379L2.89 3H14.5a.5.5 0 0 1 .491.592l-1.5 8A.5.5 0 0 1 13 12H4a.5.5 0 0 1-.491-.408L2.01 3.607 1.61 2H.5a.5.5 0 0 1-.5-.5M3.102 4l1.313 7h8.17l1.313-7zM5 12a2 2 0 1 0 0 4 2 2 0 0 0 0-4m7 0a2 2 0 1 0 0 4 2 2 0 0 0 0-4m-7 1a1 1 0 1 1 0 2 1 1 0 0 1 0-2m7 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2" />
              </svg>
              <span>{size}</span>
            </div>
            <div>
              <Dropdown>
                <Dropdown.Toggle variant="success" id="dropdown-basic">
                  Categorías
                </Dropdown.Toggle>
                <Dropdown.Menu>
                  {category.map(cat => (
                    <Form.Check
                      key={cat.categoryID}
                      type="checkbox"
                      id={`category-${cat.categoryID}`}
                      label={cat.nameCategory}
                      checked={categorySelected.includes(cat.categoryID)}
                      onChange={() => handleCategoryChange(cat.categoryID)}
                    />
                  ))}
                </Dropdown.Menu>
              </Dropdown>
            </div>
          </div>
          <Link href={'/Admin'}><button className="btn btn-outline-dark">Perfil</button></Link>
        </div>
      </div>
      {showModal && (
        <Modal
          title="Atención"
          content="Por favor, ingrese un texto de búsqueda o seleccione una categoría."
          onClose={closeModal}
        />
      )}
    </div>
  );
};

const Modal = ({ title, content, onClose, closeButtonText = 'Cerrar', showCloseButton = true }) => {
  if (!title || !content || !onClose || typeof onClose !== 'function') {
    throw new Error('Error: Los argumentos title, content y onClose son obligatorios y onClose debe ser una función.');
  }
  if (title === undefined || content === undefined || onClose === undefined) {
    throw new Error('Error: Los argumentos title, content no pueden ser indefinidos');
  }
  return (
    <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">{title}</h5>
            {showCloseButton && (
              <button type="button" className="close" onClick={onClose} aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            )}
          </div>
          <div className="modal-body">
            <p>{content}</p>
          </div>
          {showCloseButton && (
            <div className="modal-footer">
              <button type="button" className="btn btn-secondary" onClick={onClose}>
                {closeButtonText}
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Header;