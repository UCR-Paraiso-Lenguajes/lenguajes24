"use client"
import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; // Importar CSS de Bootstrap

export const Address = ({
 goToPage
}) => {
  const [deliveryAddress, setDeliveryAddress] = useState('');

  const [storeData, setStoreData] = useState(() => {
    const storedStoreData = localStorage.getItem("tienda");
    return JSON.parse(storedStoreData); 
  });

  var hasStoredData =storeData && storeData.carrito.direccionEntrega;
  
  useEffect(() => {
    if (hasStoredData) {
      setDeliveryAddress(storeData.carrito.direccionEntrega);
    }
  }, [storeData]);

  const handleSubmit = (event) => {
    event.preventDefault();

    const updatedStoreData = {
      ...storeData,
      carrito: {
        ...storeData.carrito,
        direccionEntrega: deliveryAddress
      }
    };

    localStorage.setItem("tienda", JSON.stringify(updatedStoreData));
    setStoreData(updatedStoreData);
    goToPage(3);
  };  

  const isDeliveryAddressEmpty = deliveryAddress.trim() === '';

  return (
    <>
      <div className='address'>
        <form onSubmit={handleSubmit}>
          <label>
            Dirección de Entrega:
            <input
              type="text"
              value={deliveryAddress}
              onChange={(e) => setDeliveryAddress(e.target.value)}
            />
          </label>

          <button type="submit" className='btn-cartPayment' disabled={isDeliveryAddressEmpty}>
            Continuar
          </button>
        </form>
      </div>
    </>
  );
};
