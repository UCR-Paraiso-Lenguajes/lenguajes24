import DataTable, { TableColumn } from "react-data-table-component";
import { Container, Row, Col, Button, Modal, Form, Alert } from "react-bootstrap";
import { FaPlus } from "react-icons/fa";
import { useEffect, useState } from "react";

export default function Products() {
    const [show, setShow] = useState(false);
    const [categories, setCategories] = useState([]);
    const [products, setProducts] = useState([]);
    const [data, setData] = useState<Product[]>([]);
    const [isErrorShowing, setIsErrorShowing] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');

    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [imageUrl, setImageUrl] = useState("");
    const [price, setPrice] = useState("");
    const [category, setCategory] = useState("");

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const categoriesArePresent = categories !== undefined && categories.length > 0;

    interface Product {
        id: string,
        description: string,
        imageUrl: string,
        productLink: string
    }

    const headers: TableColumn<Product>[] = [
        {
            name: 'ID',
            selector: row => row.id,
            sortable: true
        },
        {
            name: 'Product Link',
            cell: row => <a href={row.productLink} target="_blank" rel="noopener noreferrer">Link</a>,
            width: '150px'
        },
        {
            name: 'Description',
            cell: row => <div dangerouslySetInnerHTML={{ __html: row.description }}></div>,
            selector: row => row.description,
            sortable: true
        },
        {
            name: 'Image URL',
            selector: row => row.imageUrl,
        },
    ];

    const handleAddProduct = async () => {
        const newProduct = {
            name: name,
            description: description,
            imageUrl: imageUrl,
            price: price,
            category: category
        };
        var token = sessionStorage.getItem("sessionToken");
        try {
            const res = await fetch('https://localhost:7151/api/product', {
                method: 'POST',
                body: JSON.stringify(newProduct),
                headers: {
                    'content-type': 'application/json',
                    'authorization': `Bearer ${token}`
                },
            })
            if (res.ok) {
                var createdProduct = await res.json();
                setErrorMessage(`Producto ${createdProduct.name} creado`);
            }
            else { setErrorMessage("Error al realizar la compra"); }
        } catch (error) {
            setErrorMessage(error);
        }
        setShow(false);
    };

    async function getData() {
        const res = await fetch('https://localhost:7151/api/store');
        if (!res.ok) {
            throw new Error('Failed to fetch data');
        }
        return res.json();
    }

    useEffect(() => {
        const fetchData = async () => {
            try {
                const result = await getData();
                setCategories(result.categoriesInStore);
                setProducts(result.productsInStore);
                const formattedData = result.productsInStore.map((product: any) => ({
                    id: product.uuid,
                    description: product.description,
                    imageUrl: product.imageUrl,
                    productLink: `/product/${product.uuid}`
                }));
                setData(formattedData);
            } catch (error) {
                setErrorMessage(error)
                setIsErrorShowing(true)
            }
        };
        fetchData();
    }, []);

    return (
        <>
            <Container>
                <Row className="my-4">
                    <Col>
                        <h1 className="text-center">Listado de productos</h1>
                    </Col>
                </Row>
                <Row className="mb-4">
                    <Col className="d-flex justify-content-end">
                        <Button variant="primary" onClick={handleShow}>
                            <FaPlus /> {/* Add the icon here */}
                        </Button>
                    </Col>
                </Row>
                <Row>
                    <Col>
                        <DataTable
                            columns={headers}
                            data={data}
                            pagination
                            highlightOnHover
                            responsive
                        />
                    </Col>
                </Row>
            </Container>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Nuevo producto</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="name">
                            <Form.Label>Nombre:</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Nombre del producto"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mb3" controlId="description">
                            <Form.Label>Descripción:</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={3}
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mb3" controlId="imageUrl">
                            <Form.Label>Link de imagen:</Form.Label>
                            <Form.Control
                                type="text"
                                placeholder="Imagen del producto"
                                value={imageUrl}
                                onChange={(e) => setImageUrl(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mb3" controlId="price">
                            <Form.Label>Precio:</Form.Label>
                            <Form.Control
                                type="number"
                                placeholder="Precio del producto"
                                value={price}
                                onChange={(e) => setPrice(e.target.value)}
                            />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="category">
                            <Form.Label>Categoría:</Form.Label>
                            <Form.Select
                                aria-label="Default select example"
                                value={category}
                                onChange={(e) => setCategory(e.target.value)}
                            >
                                {categories.map(category => (
                                    <option key={category.id} value={category.id}>{category.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Cancelar
                    </Button>
                    <Button variant="primary" onClick={handleAddProduct} disabled={!categories.length}>
                        Agregar
                    </Button>
                </Modal.Footer>
            </Modal>
            {
                isErrorShowing ?
                    <div
                        style={{
                            position: 'fixed',
                            bottom: 20,
                            right: 20,
                            zIndex: 9999,
                        }
                        }
                    >
                        <Alert variant="danger" onClose={() => setIsErrorShowing(false)} dismissible>
                            <Alert.Heading>Error</Alert.Heading>
                            <p>{errorMessage.toString()}</p>
                        </Alert>
                    </div > : ''
            }
        </>
    );
}