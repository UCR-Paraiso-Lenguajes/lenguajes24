import DataTable, { TableColumn } from "react-data-table-component";
import { Container, Row, Col, Button, Modal, Form } from "react-bootstrap";
import { useRouter } from "next/navigation";
import { FaPlus } from "react-icons/fa";
import { useState } from "react";

export default function Products() {
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    const router = useRouter();

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
            sortable: false
        },
        {
            name: 'Description',
            cell: row => <div dangerouslySetInnerHTML={{ __html: row.description }}></div>,
            sortable: true
        },
        {
            name: 'Image URL',
            selector: row => row.imageUrl,
            sortable: true
        },
    ];

    const data: Product[] = [
        { id: '123', description: 'descr1', imageUrl: 'http1', productLink: 'www.google.com' },
        { id: '456', description: '<h1>Hola hola</h1>', imageUrl: 'http2', productLink: 'www.google.com' },
        { id: '678', description: 'desc3', imageUrl: 'http3', productLink: 'www.google.com' },
    ];

    const handleAddProduct = () => {
        //router.push('/add-product');
    };

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
                            <Form.Control type="text" placeholder="Nombre del producto" />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="description">
                            <Form.Label>Descripción:</Form.Label>
                            <Form.Control as="textarea" rows={3} />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="imageUrl">
                            <Form.Label>Link de imagen:</Form.Label>
                            <Form.Control type="text" placeholder="Imagen del producto" />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Cancelar
                    </Button>
                    <Button variant="primary" onClick={handleClose}>
                        Agregar
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}