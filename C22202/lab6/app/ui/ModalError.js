import React, { useState } from 'react';
import { Modal, Button, Alert } from 'react-bootstrap';

const ErrorModal = ({ show, handleClose, errorMessage }) => {

    return (
        <Modal show={show} onHide={handleClose}>
            <Modal.Header closeButton>
                <Modal.Title>Error</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Alert variant="danger" className="mt-3">
                    {errorMessage}
                </Alert>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Cerrar
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default ErrorModal;
