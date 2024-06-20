import React, { useState, useEffect } from 'react';
import { Modal, Button, Form, Alert } from 'react-bootstrap';

export default function AddEditProduct({ show, onClose, onSave }) {
    const [formData, setFormData] = useState({
        id: 0,
        name: '',
        imgSource: '',
        price: 0,
        category: 0  // Utilizaremos el ID de la categoría seleccionada
    });
    const [categories, setCategories] = useState([]);
    const [errorMessage, setErrorMessage] = useState('');

    useEffect(() => {
        // Obtener categorías de localStorage y configurarlas
        const storedCategories = JSON.parse(localStorage.getItem('Shop')).categories || [];
        setCategories(storedCategories);

        if (!show) {
            clearForm();
        }
    }, [show]);

    const clearForm = () => {
        setFormData({
            id: 0,
            name: '',
            imgSource: '',
            price: 0,
            category: 0
        });
        setErrorMessage('');
    };

    const handleClose = () => {
        onClose();
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = () => {
        if (!validateForm()) {
            return;
        }

        onSave(formData);
        handleClose();
    };

    const validateForm = () => {
        if (!formData.name || !formData.imgSource || formData.price <= 0 || formData.category <= 0) {
            setErrorMessage('Por favor completa todos los campos y asegúrate de seleccionar una categoria y que el Precio sea mayor que 0');
            return false;
        }
        return true;
    };

    return (
        <Modal show={show} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Agregar Producto</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group controlId="formName" className="mb-3">
                        <Form.Label className="mb-0">Nombre</Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Ingrese el nombre"
                            name="name"
                            value={formData.name}
                            onChange={handleChange}
                        />
                    </Form.Group>

                    <Form.Group controlId="formImageLink" className="mb-3">
                        <Form.Label className="mb-0">Link de la Imagen</Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Ingrese el link de la imagen"
                            name="imgSource"
                            value={formData.imgSource}
                            onChange={handleChange}
                        />
                    </Form.Group>

                    <Form.Group controlId="formPrice" className="mb-3">
                        <Form.Label className="mb-0">Precio</Form.Label>
                        <Form.Control
                            type="number"
                            step="0.01"
                            placeholder="Ingrese el precio"
                            name="price"
                            onChange={handleChange}
                        />
                    </Form.Group>

                    <Form.Group controlId="formCategory" className="mb-3">
                        <Form.Label className="mb-0">Categoría</Form.Label>
                        <Form.Control
                            as="select"
                            name="category"
                            value={formData.category}
                            onChange={handleChange}
                        >
                            <option value="">Selecciona una categoría</option>
                            {categories.map((cat) => (
                                <option key={cat.id} value={cat.id}>{cat.name}</option>
                            ))}
                        </Form.Control>
                    </Form.Group>

                    {errorMessage && (
                        <Alert variant="danger" className="mt-3">
                            {errorMessage}
                        </Alert>
                    )}
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Cancelar
                </Button>
                <Button variant="primary" onClick={handleSubmit}>
                    Guardar
                </Button>
            </Modal.Footer>
        </Modal>
    );
}
