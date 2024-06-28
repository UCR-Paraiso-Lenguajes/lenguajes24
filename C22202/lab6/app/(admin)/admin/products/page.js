'use client'

import React, { useEffect, useState } from "react";
import { Container, Table, Button, Row, Col } from "react-bootstrap";
import 'bootstrap/dist/css/bootstrap.min.css';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/solid';
import AddEditProduct from "@/app/ui/addProduct";
import ErrorModal from "@/app/ui/ModalError";

function App() {
    const [products, setProducts] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [showError, setShowError] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');


    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        let message = ''
        try {
            const response = await fetch('https://localhost:7194/api/Admin/Products'); // Replace with your API endpoint
            if (!response.ok) {
                handleError(`${response.status} ${response.statusText}`)
            }
            const data = await response.json();
            setProducts(data);
        } catch (error) {
            handleError('Error del servidor, por favor, intenta más tarde')
        }
    };

    const handleError = (error) => {
        setErrorMessage(error);
        setShowError(true);
    };

    const handleCloseError = () => {
        setShowError(false);
    };

    const handleAddProduct = () => {
        setShowModal(true);
    };

    const handleCloseModal = () => {
        setShowModal(false);
    };

    const handleSaveProduct = async (formData) => {

        try {
            const response = await fetch('https://localhost:7194/api/Admin/Product', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });
            if (!response.ok) {
                handleError(`${response.status} ${response.statusText}`)
            }
            fetchData();
        } catch (error) {
            handleError('Error del servidor, por favor, intenta más tarde')
        }
    };

    return (
        <Container>
            <AddEditProduct show={showModal} onClose={handleCloseModal} onSave={handleSaveProduct} />
            <ErrorModal show={showError} handleClose={handleCloseError} errorMessage={errorMessage} />
            <h1>Products</h1>
            <Row className="mb-3">
                <Col className="text-end">
                    <Button variant="primary" onClick={handleAddProduct}>Add Product</Button>
                </Col>
            </Row>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Image</th>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Category</th>
                        <th>Price</th>
                        <th>Edit</th>
                        <th>Delete</th>
                    </tr>
                </thead>
                <tbody>
                    {products.map((product) => (
                        <tr key={product.id}>
                            <td><img src={product.imgSource} alt={product.name} width="100px" className="img-fluid" /></td>
                            <td>{product.id}</td>
                            <td>{product.name}</td>
                            <td>{product.category}</td>
                            <td>{product.price}</td>
                            <td>
                                <Button variant="warning">
                                    <PencilSquareIcon style={{ width: "20px", height: "20px" }} />
                                </Button>
                            </td>
                            <td>
                                <Button variant="danger">
                                    <TrashIcon style={{ width: "20px", height: "20px" }} />
                                </Button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </Container>
    );
}

export default App;
