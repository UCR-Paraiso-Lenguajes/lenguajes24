import React, { useState, useEffect } from 'react';

function AddProduct() {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [imageURL, setImageURL] = useState('');
  const [price, setPrice] = useState('');
  const [category, setCategory] = useState({ id: '', name: '' });
  const [categories, setCategories] = useState(() => {
    const storedListNames = localStorage.getItem('listNames');
    return storedListNames ? JSON.parse(storedListNames) : [];
  });

  const handleSubmit = async (event) => {
    event.preventDefault();

    const priceNumber = parseFloat(price);
    if (isNaN(priceNumber) || priceNumber <= 0) {
      alert('Por favor ingrese un precio válido.');
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
      const response = await fetch('https://localhost:7280/api/Product', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newProduct),
      });

      if (response.ok) {
        setName('');
        setDescription('');
        setImageURL('');
        setPrice('');
        setCategory({ id: '', name: '' });
      } else {
        throw new Error('Failed to add product');
      }
    } catch (error) {
      throw new Error('Error adding product:', error);
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
