"use client";
import React, { useState, useEffect, ChangeEvent } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import VerifyToken, { useTokenContext } from '@/app/components/verify_token';
import { useRouter } from 'next/navigation';

interface Product {
    id: number;
    name: string;
    Categoria: { IdCategory: number, NameCategory: string };
    price: number;
    description: string;
    imageURL: string;
}

interface Category {
    id: number;
    name: string;
}

const ProductCrud = () => {
    const { isValidToken, isVerifying } = useTokenContext();
    const router = useRouter();

    const [products, setProducts] = useState<Product[]>([]);
    const [newProduct, setNewProduct] = useState<Omit<Product, 'id' | 'Categoria'> & { Categoria: number }>({
        name: '',
        Categoria: 0,
        price: 0,
        description: '',
        imageURL: ''
    });
    const [showModal, setShowModal] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [alert, setAlert] = useState<{ message: string, type: 'success' | 'danger' } | null>(null);

    const [categoryListFromStore, setCategoryListFromStore] = useState<Category[]>([
        { id: 1, name: "Electrónica" },
        { id: 2, name: "Hogar y oficina" },
        { id: 3, name: "Entretenimiento" },
        { id: 4, name: "Tecnología" }
    ]);

    const loadProductData = async () => {
        try {
            const productResponse = await fetch('https://localhost:7165/api/Store');
            if (!productResponse.ok) {
                throw new Error('Failed to fetch products');
            }
            const productJson = await productResponse.json();
            if (!Array.isArray(productJson.products)) {
                throw new Error('Invalid product data format');
            }
            setProducts(productJson.products);
        } catch (error) {
            setError('Error al cargar productos');
            setProducts([]);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (isValidToken && !isVerifying) {
            loadProductData();
        }
    }, [isValidToken, isVerifying]);

    useEffect(() => {
        if (!isValidToken && !isVerifying) {
            router.push("/../admin");
        }
    }, [isValidToken, isVerifying, router]);

    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;

        if (name === 'price') {
            const newValue = value.replace(/\D/g, '');
            const limitedValue = newValue.substring(0, 6);
            setNewProduct({ ...newProduct, [name]: parseInt(limitedValue) });
        } else if (name === 'Categoria') {
            setNewProduct({ ...newProduct, [name]: parseInt(value) });
        } else {
            setNewProduct({ ...newProduct, [name]: value });
        }
    };

    const handleInsertProduct = async () => {
        if (!newProduct.name || !newProduct.Categoria || !newProduct.price || !newProduct.imageURL) {
            setAlert({ message: "Todos los campos son obligatorios.", type: 'danger' });
            return;
        }

        const selectedCategory = categoryListFromStore.find(Categoria => Categoria.id === newProduct.Categoria);
        if (!selectedCategory) {
            setAlert({ message: "Categoría no válida.", type: 'danger' });
            return;
        }

        const categoryName = selectedCategory.name;

        try {
            const productToInsert = {
                ...newProduct,
                Categoria: { IdCategory: newProduct.Categoria, NameCategory: categoryName }
            };

            const response = await fetch('https://localhost:7165/api/Product/product/insert', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(productToInsert)
            });

            if (!response.ok) {
                const errorResponse = await response.json();
                throw new Error(errorResponse.errors ? JSON.stringify(errorResponse.errors) : 'Failed to insert product');
            }

            const insertedProduct = await response.json();

            setProducts(prevProducts => [...prevProducts, insertedProduct]);

            setShowModal(false);
            clearFormFields();
        } catch (error) {
            setError('Error al insertar el producto');
        }
    };

    const handleDeleteProduct = async (productId: number) => {
        try {
            const response = await fetch('https://localhost:7165/api/Product/product/delete', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id: productId })
            });

            if (!response.ok) {
                throw new Error('Failed to delete product');
            }

            setProducts(products.filter(product => product.id !== productId));
        } catch (error) {
            setError('Error al eliminar el producto');
        }
    };

    const clearFormFields = () => {
        setNewProduct({
            name: '',
            Categoria: 0,
            price: 0,
            description: '',
            imageURL: ''
        });
    };

    const renderDescription = (description: string) => {
        return { __html: description };
    };

    if (isVerifying || !isValidToken) {
        return <p></p>;
    }

    return (
        <main className="container">
            <header className="my-4">
                <h1>CRUD de Productos</h1>
            </header>

            {alert && (
                <div className={`alert alert-${alert.type}`} role="alert">
                    {alert.message}
                </div>
            )}

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
                                    <td>{product.Categoria && product.Categoria.NameCategory}</td>
                                    <td>{product.price}</td>
                                    <td dangerouslySetInnerHTML={renderDescription(product.description)}></td>
                                    <td><img src={product.imageURL} alt={product.name} className="img-thumbnail" style={{ width: '100px' }} /></td>
                                    <td>
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
                                        <label htmlFor="Categoria" className="form-label">Categoría</label>
                                        <select
                                            className="form-select"
                                            id="Categoria"
                                            name="Categoria"
                                            value={newProduct.Categoria}
                                            onChange={handleInputChange}
                                            required
                                        >
                                            <option value={0}>Selecciona una categoría</option>
                                            {categoryListFromStore.map(category => (
                                                <option key={category.id} value={category.id}>
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
};

const WrappedProductCrud = () => (
    <VerifyToken>
        <ProductCrud />
    </VerifyToken>
);

export default WrappedProductCrud;