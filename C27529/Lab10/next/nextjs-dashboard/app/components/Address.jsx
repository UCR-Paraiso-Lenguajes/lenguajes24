"use client"
import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css";

export const Address = ({ goToPage }) => {
  const [deliveryAddress, setDeliveryAddress] = useState('');
  const [additionalAddress, setAdditionalAddress] = useState('');
  const [selectedProvince, setSelectedProvince] = useState('');
  const [selectedCanton, setSelectedCanton] = useState('');
  const [isAddressValid, setIsAddressValid] = useState(true);

  const [storeData, setStoreData] = useState(() => {
    const storedStoreData = localStorage.getItem("tienda");
    return JSON.parse(storedStoreData) || { carrito: { direccionEntrega: '' } };
  });

  const hasStoredData = storeData && storeData.carrito.direccionEntrega;

  useEffect(() => {
    if (hasStoredData) {
      setDeliveryAddress(storeData.carrito.direccionEntrega);
    }
  }, [storeData]);

  useEffect(() => {
    const completeAddress = `${selectedProvince}, ${selectedCanton}, ${additionalAddress}`;
    setDeliveryAddress(completeAddress);
    setIsAddressValid(validateAddress(completeAddress));
  }, [selectedProvince, selectedCanton, additionalAddress]);

  const provincias = {
    "San José": ["San José", "Escazú", "Desamparados", "Puriscal", "Tarrazú", "Aserrí", "Mora", "Goicoechea", "Santa Ana", "Alajuelita", "Vázquez de Coronado", "Acosta", "Tibás", "Moravia", "Montes de Oca", "Turrubares", "Dota", "Curridabat", "Pérez Zeledón", "León Cortés"],
    "Alajuela": ["Alajuela", "San Ramón", "Grecia", "San Mateo", "Atenas", "Naranjo", "Palmares", "Poás", "Orotina", "San Carlos", "Zarcero", "Sarchí", "Upala", "Los Chiles", "Guatuso", "Río Cuarto"],
    "Cartago": ["Cartago", "Paraíso", "La Unión", "Jiménez", "Turrialba", "Alvarado", "Oreamuno", "El Guarco"],
    "Heredia": ["Heredia", "Barva", "Santo Domingo", "Santa Bárbara", "San Rafael", "San Isidro", "Belén", "Flores", "San Pablo", "Sarapiquí"],
    "Guanacaste": ["Liberia", "Nicoya", "Santa Cruz", "Bagaces", "Carrillo", "Cañas", "Abangares", "Tilarán", "Nandayure", "La Cruz", "Hojancha"],
    "Puntarenas": ["Puntarenas", "Esparza", "Buenos Aires", "Montes de Oro", "Osa", "Quepos", "Golfito", "Coto Brus", "Parrita", "Corredores", "Garabito"],
    "Limón": ["Limón", "Pococí", "Siquirres", "Talamanca", "Matina", "Guácimo"]
  };

  const handleSubmit = (event) => {
    event.preventDefault();

    if (!isAddressValid || !selectedProvince || !selectedCanton) {
      console.error("Invalid address:", deliveryAddress);
      return;
    }

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

  const validateAddress = (address) => {
    // Simplified validation: address should have at least 10 characters
    return address.length >= 10;
  };

  const handleProvinceChange = (e) => {
    setSelectedProvince(e.target.value);
    setSelectedCanton(''); // Reset cantons when province changes
  };

  const handleCantonChange = (e) => {
    setSelectedCanton(e.target.value);
  };

  const handleAdditionalAddressChange = (e) => {
    setAdditionalAddress(e.target.value);
  };

  return (
    <div className='address'>
      <form onSubmit={handleSubmit}>
        <label>
          Provincia:
          <select value={selectedProvince} onChange={handleProvinceChange} className="form-select">
            <option value="">Seleccione una provincia</option>
            {Object.keys(provincias).map((province) => (
              <option key={province} value={province}>{province}</option>
            ))}
          </select>
        </label>
        {selectedProvince && (
          <label>
            Cantón:
            <select value={selectedCanton} onChange={handleCantonChange} className="form-select">
              <option value="">Seleccione un cantón</option>
              {provincias[selectedProvince].map((canton) => (
                <option key={canton} value={canton}>{canton}</option>
              ))}
            </select>
          </label>
        )}
        <label>
          Dirección Adicional:
          <input
            type="text"
            value={additionalAddress}
            onChange={handleAdditionalAddressChange}
            className={isAddressValid ? "" : "is-invalid"}
          />
        </label>
        <label>
          Dirección Completa:
          <input
            type="text"
            value={deliveryAddress}
            className={isAddressValid ? "" : "is-invalid"}
            disabled
          />
        </label>
        <button type="submit" className='btn-cartPayment' disabled={!isAddressValid || !selectedProvince || !selectedCanton}>
          Continuar
        </button>
      </form>
    </div>
  );
};
