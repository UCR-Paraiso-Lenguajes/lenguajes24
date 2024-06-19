'use client'
import React, { useEffect, useState } from "react";
import { Container, Table, Button, Row, Col } from "react-bootstrap";
import 'bootstrap/dist/css/bootstrap.min.css';
import { PencilSquareIcon, TrashIcon } from '@heroicons/react/24/solid';

function App() {

    const [products, setProducts] = useState([])

    debugger
    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const response = await fetch('https://localhost:7194/api/Admin/Products'); // Replace with your API endpoint
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
            const data = await response.json();
            debugger;
            setProducts(data);

        } catch (error) {
            throw error
            setError(error.message);
            setLoading(false);
        }
    };
    // Datos de ejemplo para los productos
    const products1 = [
        {
            id: 1,
            image: "https://via.placeholder.com/50",
            name: "Product 1",
            category: "Category 1",
            price: "$10.00"
        },
        {
            id: 2,
            image: "https://via.placeholder.com/50",
            name: "Product 2",
            category: "Category 2",
            price: "$20.00"
        }
        // Puedes agregar más productos aquí
    ];

    return (
        <Container>
            <h1>Products</h1>
            <Row className="mb-3">
                <Col className="text-end">
                    <Button variant="primary">Add Product</Button>
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
