'use client';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import Link from 'next/link';
import '/app/ui/global.css';
import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form } from 'react-bootstrap';
import validator from 'validator';

const ProductPage = () => {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const [isNameValid, setIsNameValid] = useState(false);
    const [isAuthorValid, setIsAuthorValid] = useState(false);
    const [isPriceValid, setIsPriceValid] = useState(false);
    const [isCategoryValid, setIsCategoryValid] = useState(false);
    const [isImgUrlValid, setIsImgUrlValid] = useState(true); // Nueva validación
    const [isFormValid, setIsFormValid] = useState(false); // Nuevo estado para la validación del formulario
    const token = sessionStorage.getItem("authToken");

    const URL = process.env.NEXT_PUBLIC_API_URL;
    if (!URL) {
        throw new Error('NEXT_PUBLIC_API_URL is not defined');
    }

    const [newProduct, setNewProduct] = useState({
        Name: '',
        Author: '',
        ImgUrl: '',
        Price: '',
        ProductCategory: {
            Name: '',
            IdCategory: 0
        }
    });

    useEffect(() => {
        if (!token) {
            sessionStorage.removeItem("authToken");
            return;
        }
    }, []);

    useEffect(() => {
        fetchProducts();
    }, []);

    useEffect(() => {
        validateForm(); // Validar el formulario cada vez que cambie algún estado
    }, [newProduct, isNameValid, isAuthorValid, isPriceValid, isCategoryValid, isImgUrlValid]);

    const fetchProducts = async () => {
        try {
            const response = await fetch(`${URL}/api/Store`, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) {
                throw new Error('Failed to fetch products: ' + response.statusText);
            }

            const data = await response.json();
            setProducts(data.products);
            setCategories(data.categoriesList);
        } catch (error) {
            throw new Error('Failed to fetch data');
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;

        if (name === 'CategoryId') {
            const selectedCategory = categories.find(category => category.idCategory === parseInt(value));
            const categoryName = selectedCategory ? selectedCategory.name : '';
            setNewProduct(prevState => ({
                ...prevState,
                ProductCategory: {
                    Name: categoryName,
                    IdCategory: parseInt(value)
                }
            }));
            setIsCategoryValid(!!selectedCategory);
        } else if (name === 'ImgUrl') {
            setNewProduct(prevState => ({
                ...prevState,
                [name]: value
            }));
            setIsImgUrlValid(true); // Permitir cualquier entrada temporalmente
        } else if (name === 'Price') {
            setNewProduct(prevState => ({
                ...prevState,
                [name]: value
            }));
            setIsPriceValid(true); // Permitir cualquier entrada temporalmente
        } else {
            setNewProduct(prevState => ({
                ...prevState,
                [name]: value
            }));

            if (name === 'Name') {
                setIsNameValid(value.trim() !== '');
            }

            if (name === 'Author') {
                setIsAuthorValid(value.trim() !== '');
            }
        }

        validateForm(); // Validar el formulario después de cada cambio de entrada
    };

    const validateForm = () => {
        const isFormValid = isNameValid && isAuthorValid && isPriceValid && isCategoryValid && isImgUrlValid;
        setIsFormValid(isFormValid);
    };

    const handleAddProduct = async () => {
        // Validar URL de la imagen y precio antes de enviar
        if (!validator.isURL(newProduct.ImgUrl)) {
            setIsImgUrlValid(false);
            setErrorMessage('La imagen debe ser una URL válida');
            return;
        }

        const priceRegex = /^\d+(\.\d{1,3})?$/;
        if (!priceRegex.test(newProduct.Price) || parseFloat(newProduct.Price) > 100000.000) {
            setIsPriceValid(false);
            setErrorMessage('El precio debe ser un número con hasta 3 decimales y menor o igual a 100000.000');
            return;
        }

        const productData = {
            Name: newProduct.Name,
            Author: newProduct.Author,
            ImgUrl: newProduct.ImgUrl,
            Price: parseFloat(newProduct.Price),
            ProductCategory: {
                Name: newProduct.ProductCategory.Name,
                IdCategory: newProduct.ProductCategory.IdCategory
            }
        };

        const token = sessionStorage.getItem('authToken');
        if (!token) {
            sessionStorage.removeItem("authToken");
            return;
        }

        try {
            const response = await fetch(`${URL}/api/product`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(productData)
            });

            if (!response.ok) {
                throw new Error('Failed to save product: ' + response.statusText);
            }

            const result = await response.json();
            fetchProducts();
            setShowModal(false);
            setNewProduct({
                Name: '',
                Author: '',
                ImgUrl: '',
                Price: '',
                ProductCategory: {
                    Name: '',
                    IdCategory: 0
                }
            });
        } catch (error) {
            throw new Error('Error al guardar el producto:', error.message);
        }
    };

    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-12 d-flex justify-content-end align-items-center">
                        <Link href="/admin/init">
                            <button className="btn btn-dark">GO Back</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-9">
                        <div className="content">
                            <Button variant="secondary" onClick={() => setShowModal(true)} style={{ margin: '1rem' }}>
                                New Product
                            </Button>

                            <Table striped bordered hover>
                                <thead>
                                    <tr>
                                        <th>ID</th>
                                        <th>Name</th>
                                        <th>Author</th>
                                        <th>Image</th>
                                        <th>Price</th>
                                        <th>Category</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {products.map(product => (
                                        <tr key={product.id}>
                                            <td>{product.id}</td>
                                            <td>{product.name}</td>
                                            <td dangerouslySetInnerHTML={{ __html: product.author }} />
                                            <td><img src={product.imgUrl} alt={product.name} width="50" /></td>
                                            <td>{product.price}</td>
                                            <td>
                                                {categories.find(category => category.idCategory === product.productCategory.idCategory)?.name || 'Unknown'}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </Table>
                        </div>
                    </div>
                </div>
            </div>

            <footer className="footer" style={{ width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light">Biblioteca de Paula</h5>
                </div>
            </footer>

            <Modal show={showModal} onHide={() => setShowModal(false)}>
                <Modal.Header closeButton>
                    <Modal.Title>Add Product</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group>
                            <Form.Label>Name</Form.Label>
                            <Form.Control type="text" name="Name" value={newProduct.Name} onChange={handleInputChange} />
                            {!isNameValid && <div style={{ color: 'red', marginTop: '0.5rem' }}>El nombre es requerido</div>}
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Author</Form.Label>
                            <Form.Control type="text" name="Author" value={newProduct.Author} onChange={handleInputChange} />
                            {!isAuthorValid && <div style={{ color: 'red', marginTop: '0.5rem' }}>El autor es requerido</div>}
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Image</Form.Label>
                            <Form.Control type="text" name="ImgUrl" value={newProduct.ImgUrl} onChange={handleInputChange} />
                            {!isImgUrlValid && <div style={{ color: 'red', marginTop: '0.5rem' }}>La imagen debe ser una URL válida</div>}
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Price</Form.Label>
                            <Form.Control type="text" name="Price" value={newProduct.Price} onChange={handleInputChange} />
                            {!isPriceValid && <div style={{ color: 'red', marginTop: '0.5rem' }}>El precio debe ser un número con hasta 3 decimales y menor o igual a 100000.000</div>}
                        </Form.Group>
                        <Form.Group>
                            <Form.Label>Category</Form.Label>
                            <Form.Control as="select" name="CategoryId" value={newProduct.ProductCategory.IdCategory} onChange={handleInputChange}>
                                <option value="">Seleccione una categoría</option>
                                {categories.map(category => (
                                    <option key={category.idCategory} value={category.idCategory}>
                                        {category.name}
                                    </option>
                                ))}
                            </Form.Control>
                            {!isCategoryValid && <div style={{ color: 'red', marginTop: '0.5rem' }}>La categoría es requerida</div>}
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModal(false)}>
                        Close
                    </Button>
                    <Button
                        variant="success"
                        onClick={handleAddProduct}
                        disabled={!isFormValid} // Deshabilitar el botón si el formulario no es válido
                    >
                        Save
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
};

export default ProductPage;
