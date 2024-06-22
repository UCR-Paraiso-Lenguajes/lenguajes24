"use client";
import React, { useState, useEffect, ChangeEvent } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import { Parser } from 'html-to-react';

interface Product {
    id: number;
    name: string;
    category: { idCategory: number, nameCategory: string };
    price: number;
    description: string;
    imageURL: string;
}

interface Category {
    id: number;
    name: string;
}

declare module 'html-to-react' {
    export class Parser {
        constructor(options?: any);
        parse: (html: string) => any; 
    }
}

const htmlParser = new Parser();

export default function ProductCrud() {
    const [products, setProducts] = useState<Product[]>([]);
    const [categoryListFromStore, setCategoryListFromStore] = useState<Category[]>([
        { id: 1, name: "Electrónica" },
        { id: 2, name: "Hogar y oficina" },
        { id: 3, name: "Entretenimiento" },
        { id: 4, name: "Tecnología" }
    ]);
    const [newProduct, setNewProduct] = useState<Omit<Product, 'id' | 'category'> & { category: string, description: string }>({
        name: '',
        category: '',
        price: 0,
        description: '',
        imageURL: ''
    });
    const [showModal, setShowModal] = useState(false);
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

    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
    
        if (name === 'price') {
            const newValue = value.replace(/\D/g, ''); 
            const limitedValue = newValue.substring(0, 6); 
            setNewProduct({ ...newProduct, [name]: parseInt(limitedValue) }); 
        } else if (name === 'description') {
            const parsedHtml = htmlParser.parse(value);
            setNewProduct({ ...newProduct, [name]: parsedHtml }); 
        } else {
            setNewProduct({ ...newProduct, [name]: value });
        }
    };

    const handleInsertProduct = async () => {
        if (!newProduct.name || !newProduct.category || !newProduct.price || !newProduct.imageURL) {
            alert("Todos los campos son obligatorios.");
            return;
        }
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
                setProducts([...products, { ...newProduct, id: result.newProductId, category: { idCategory: 0, nameCategory: newProduct.category } }]);
                setShowModal(false);
                clearFormFields();
            }
        } catch (error) {
            throw new Error('Failed to insert product:');
        }
    };

    const handleDeleteProduct = async (productId: number) => {
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
            throw new Error('Failed to delete product:');
        }
    };

    const clearFormFields = () => {
        setNewProduct({
            name: '',
            category: '',
            price: 0,
            description: '',
            imageURL: ''
        });
    };

    return (
        <main className="container">
            <header className="my-4">
                <h1>CRUD de Productos</h1>
            </header>

            <button className="btn btn-primary mb-4" onClick={() => setShowModal(true)}>Añadir Producto</button>

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
                                <th>Descripción</th>
                                <th>Imagen</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                            {products.map((product) => (
                                <tr key={product.id}>
                                    <td>{product.id}</td>
                                    <td>{product.name}</td>
                                    <td>{product.category && product.category.nameCategory}</td>
                                    <td>{product.price}</td>
                                    <td dangerouslySetInnerHTML={{ __html: product.description }}></td>
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

            {showModal && (
                <div className="modal show" style={{ display: 'block' }}>
                    <div className="modal-dialog">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">Añadir Nuevo Producto</h5>
                                <button type="button" className="close" onClick={() => { setShowModal(false); clearFormFields(); }}>
                                    <span>&times;</span>
                                </button>
                            </div>
                            <div className="modal-body">
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
                                            inputMode="numeric" 
                                        />
                                    </div>
                                    <div className="mb-3">
                                        <label htmlFor="description" className="form-label">Descripción</label>
                                        <textarea
                                            className="form-control"
                                            id="description"
                                            name="description"
                                            value={newProduct.description}
                                            onChange={handleInputChange}
                                            rows={3}
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
                                    <div className="modal-footer">
                                        <button type="button" className="btn btn-secondary" onClick={() => { setShowModal(false); clearFormFields(); }}>Cerrar</button>
                                        <button type="button" className="btn btn-warning" onClick={clearFormFields}>Limpiar</button>
                                        <button type="submit" className="btn btn-primary">Añadir Producto</button>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </main>
    );
}