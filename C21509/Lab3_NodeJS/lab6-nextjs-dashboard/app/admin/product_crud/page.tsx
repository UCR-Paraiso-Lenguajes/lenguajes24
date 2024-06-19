"use client";
import React, { useState, useEffect } from 'react';
import { Parser } from 'html-to-react';
import 'bootstrap/dist/css/bootstrap.min.css';

const htmlToReactParser = new Parser();

export default function ProductCrud() {
    const [products, setProducts] = useState([]);
    const [categoryListFromStore, setCategoryListFromStore] = useState([]);
    const [newProduct, setNewProduct] = useState({
        name: '',
        category: '',
        price: '',
        imageURL: ''
    });
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const loadDataProductAPI = async () => {
            try {
                const response = await fetch('https://localhost:7165/api/Store'); 
                const dataFromStore = await response.json();
                if (dataFromStore) {
                    setProducts(dataFromStore.productsFromStore);
                    setCategoryListFromStore(dataFromStore.categoriesFromStore);
                }
            } catch (error) {
                setError('Failed to fetch data: ' + error);
            } finally {
                setLoading(false);
            }
        };
        loadDataProductAPI();
    }, []);

    const handleInputChange = (e:any) => {
        const { name, value } = e.target;
        setNewProduct({ ...newProduct, [name]: value });
    };

    const handleInsertProduct = async () => {
        try {
            const response = await fetch('https://localhost:7165/api/Store/product/insert', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newProduct),
            });

            const result = await response.json();
            if (result.insertedProductStatus) {
                setProducts([...products, newProduct]);
                setNewProduct({
                    name: '',
                    category: '',
                    price: '',
                    imageURL: ''
                });
            }
        } catch (error) {
            console.error('Failed to insert product:', error);
        }
    };

    const handleDeleteProduct = async (productId:number) => {
        try {
            const response = await fetch('https://localhost:7165/api/Store/product/delete', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ id: productId }),
            });

            const result = await response.json();
            if (result.deleteProductStatus) {
                setProducts(products.filter(product => product.id !== productId));
            }
        } catch (error) {
            console.error('Failed to delete product:', error);
        }
    };

    return (
        <main className="container">
            <header className="my-4">
                <h1>CRUD de Productos</h1>
            </header>
            
            <section className="mb-4">
                <h2>Agregar Nuevo Producto</h2>
                <form onSubmit={e => { e.preventDefault(); handleInsertProduct(); }}>
                    <div className="mb-3">
                        <label htmlFor="name" className="form-label">Nombre del Producto</label>
                        <input
                            type="text"
                            className="form-control"
                            id="name"
                            name="name"
                            value={newProduct.name}
                            onChange={handleInputChange}
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="category" className="form-label">Categoría</label>
                        <select
                            className="form-select"
                            id="category"
                            name="category"
                            value={newProduct.category}
                            onChange={handleInputChange}
                            required
                        >
                            <option value="">Selecciona una categoría</option>
                            {categoryListFromStore.map(category => (
                                <option key={category.id} value={category.name}>
                                    {category.name}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="price" className="form-label">Precio</label>
                        <input
                            type="number"
                            className="form-control"
                            id="price"
                            name="price"
                            value={newProduct.price}
                            onChange={handleInputChange}
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="imageURL" className="form-label">URL de la Imagen</label>
                        <input
                            type="text"
                            className="form-control"
                            id="imageURL"
                            name="imageURL"
                            value={newProduct.imageURL}
                            onChange={handleInputChange}
                            required
                        />
                    </div>
                    <button type="submit" className="btn btn-primary">Agregar Producto</button>
                </form>
            </section>

            {loading ? (
                <div>Cargando...</div>
            ) : error ? (
                <div>{error}</div>
            ) : (
                <section>
                    <h2>Lista de Productos</h2>
                    <table className="table table-bordered">
                        <thead className="thead-light">
                            <tr>
                                <th>Código</th>
                                <th>Nombre del Producto</th>
                                <th>Categoría</th>
                                <th>Precio</th>
                                <th>Imagen</th>
                            </tr>
                        </thead>
                        <tbody>
                            {products.map((product) => (
                                <tr key={product.id}>
                                    <td>{product.id}</td>
                                    <td>{product.name}</td>
                                    <td>{product.category && product.category.name}</td>
                                    <td>{product.price}</td>
                                    <td><img src={product.imageURL} alt={product.name} className="img-thumbnail" style={{ width: '100px' }} /></td>
                                    <td>
                                        <button className="btn btn-warning mr-2">Editar</button>
                                        <button className="btn btn-danger" onClick={() => handleDeleteProduct(product.id)}>Eliminar</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </section>
            )}
        </main>
    );
}