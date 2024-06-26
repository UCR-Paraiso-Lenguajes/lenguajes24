import React, { useState, useEffect } from 'react';

function AddProduct() {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [imageURL, setImageURL] = useState('');
  const [price, setPrice] = useState('');
  const [category, setCategory] = useState({ id: '', name: '' });
  const [errorMessage, setErrorMessage] = useState('');
  const [categories, setCategories] = useState(() => {
    const storedListNames = localStorage.getItem('listNames');
    return storedListNames ? JSON.parse(storedListNames) : [];
  });
  const token = sessionStorage.getItem('token');


  const URL = process.env.NEXT_PUBLIC_API_URL;
  if (!URL) {
    throw new Error('NEXT_PUBLIC_API_URL is not defined');
  }

  const addToLocalStorage = (product) => {
    let products = JSON.parse(localStorage.getItem('productList')) || [];
    products.push(product);
    localStorage.setItem('productList', JSON.stringify(products));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!/^\d+$/.test(price.replace('.', ''))) {
      setErrorMessage('El precio debe contener solo números y un punto decimal si es necesario.');
      return;
    }
    const priceNumber = parseFloat(price);
    if (isNaN(priceNumber) || priceNumber <= 0 || priceNumber > 5000000) {
      setErrorMessage(priceNumber > 5000000 ? 'El precio no puede superar los 5 millones.' : 'Por favor ingrese un precio válido.');
      return;
    }

    const newProduct = {
      name,
      description,
      imageURL,
      price: priceNumber,
      category
    };

    try {
      const response = await fetch(process.env.NEXT_PUBLIC_API_URL + '/api/Product', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(newProduct)
      });

      if (response.ok) {
        const addedProduct = await response.json(); // Asegúrate de que la API devuelva el producto creado
        addToLocalStorage(addedProduct); // Añadir el producto directamente
        setName('');
        setDescription('');
        setImageURL('');
        setPrice('');
        setCategory({ id: '', name: '' });
        setErrorMessage('');
      } else {
        throw new Error('Failed to add product');
      }
    } catch (error) {
      setErrorMessage('Error adding product: ' + error.message);
    }
  };

  const handleCategoryChange = (event) => {
    const selectedCategory = categories.find(cat => cat.id === parseInt(event.target.value));
    setCategory(selectedCategory ? { id: selectedCategory.id, name: selectedCategory.name } : { id: '', name: '' });
  };

  const isEmptyInfo = !name.trim() || !description.trim() || !imageURL.trim() || !price.trim() || !category.id;

  return (
    <div className="add-product-container">
      <h2>Agregar Producto</h2>
      {errorMessage && <h3 style={{ color: 'red' }}>{errorMessage}</h3>}  {/* Display error message */}
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Nombre</label>
          <input
            type="text"
            className="form-control"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Descripción</label>
          <textarea
            className="form-control"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Imagen URL</label>
          <input
            type="text"
            className="form-control"
            value={imageURL}
            onChange={(e) => setImageURL(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Precio</label>
          <input
            type="text"
            className="form-control"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Categoría</label>
          <select
            className="form-control"
            value={category.id}
            onChange={handleCategoryChange}
            required
          >
            <option value="">Selecciona una categoría</option>
            {categories.map(cat => (
              <option key={cat.id} value={cat.id}>{cat.name}</option>
            ))}
          </select>
        </div>
        <button type="submit" className="btn btn-success" disabled={isEmptyInfo}>Guardar</button>
      </form>
    </div>
  );
}

export default AddProduct;
