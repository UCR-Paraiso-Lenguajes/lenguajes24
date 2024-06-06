import React, { useState, useEffect } from 'react';

const MetodoPago = () => {
    const [formaDePago, setFormaDePago] = useState(0); // 0 para efectivo, 1 para SinpeMóvil
    const [accepted, setAccepted] = useState(false);
    const memoryStore = JSON.parse(localStorage.getItem('tienda'));
    const [numeroCompra, setNumeroCompra] = useState('');
    const [numeroComprobante, setNumeroComprobante] = useState('');
    const [modalData, setModalData] = useState(null);
    const [modalData2, setModalData2] = useState(false);

    const closeModal = () => {
        setModalData(null);
    };
    const showModal = (title, content) => {
        if (!title || !content ) {throw new Error('Error: Los argumentos title, content deben existir.');}
        if (title===undefined||content===undefined) {throw new Error('Error: Los argumentos title no pueden ser indefinidos');}
        setModalData({ title, content });
    };
    const showModalAceptado = (title, content) => {
        if (!title || !content ) {throw new Error('Error: Los argumentos title, content deben existir.');}
        if (title==undefined||content==undefined) {throw new Error('Error: Los argumentos title no pueden ser indefinidos');}
        setModalData({ title, content });
    };


    const handleAceptar = () => {
        if (formaDePago === '') {
            showModal('Método de pago', 'Por favor seleccione un método de pago disponible.');
        } else {
            const updatedCart = {
                ...memoryStore,
                carrito: {
                    ...memoryStore.carrito,
                    metodoDePago: formaDePago
                },
                necesitaVerificacion: true
            };
            localStorage.setItem("tienda", JSON.stringify(updatedCart));
            setAccepted(true);
        }
    };

    const productData = memoryStore?.productos?.map((producto) => ({
        ProductId: String(producto.id),
        Quantity: producto.cant
    }));
    const dataToSend = {
        ProductIds: productData,
        Address: memoryStore?.carrito?.direccionEntrega,
        PaymentMethod: memoryStore?.carrito?.metodoDePago
    };

    const enviarDatosaAPI = async () => {
        if (formaDePago === 1 && !numeroComprobante) { // Verificar si el número de comprobante está vacío para SinpeMóvil
            showModal('Sin comprobante de pago', 'Por favor ingrese el comprobante de pago para finalizar la compra.');
            return;
        }
        const response = await fetch('https://localhost:7223/api/Cart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataToSend)
        });
        if (response.ok) {
            const data = await response.json();
            setNumeroCompra(data.numeroCompra);
            setModalData2(true);
            showModalAceptado('Compra exitosa', `¡Su compra  ${data.numeroCompra}  ha sido procesada con éxito! Será redirigido al inicio.`);
        } else {
            const errorResponseData = await response.json();
            throw new Error(errorResponseData.message || 'Error al procesar el pago');
        }
    }

    const pagoE = () => {
        return (
            <div className="Compra">
                {modalData && modalData.title === 'Compra exitosa' && (
                    <ModalPagoAceptado title={modalData.title} content={modalData.content} onClose={closeModal} />)}
                <div className="smsFinal">
                    <div className="text-center">
                        <h5>Número de compra: {numeroCompra}</h5>
                        <h4>Esperando la confirmación del Administrador</h4>
                        <button className="btn btn-primary" onClick={enviarDatosaAPI}>Confirmar Pago</button>
                    </div>
                </div>
            </div>
        );
    };

    const pagoS = () => {
        return (
            <div className="Compra">
                {!modalData2 && modalData && <Modal title={modalData.title} content={modalData.content} onClose={closeModal} />}
                {modalData2 && modalData && modalData.title === 'Compra exitosa' && (
                    <ModalPagoAceptado title={modalData.title} content={modalData.content} onClose={closeModal} />)}
                <div className="smsFinal">
                    <div className="text-center">
                        <h5>Número de compra: {numeroCompra}</h5>
                        <h4>Número de SinpeMóvil: +506 3875 8524</h4>
                        <input type="text" placeholder="Número de comprobante" onChange={handleSinComprobanteChange} required></input>
                        <button className="btn btn-primary" onClick={handleAceptar}>Aceptar</button>
                        <h3>Esperando la confirmación del Administrador</h3>
                        <button className="btn btn-primary" onClick={enviarDatosaAPI}>Confirmar Pago</button>
                    </div>
                </div>
            </div>
        );
    };

    const handleMetodoChange = (metodoDePagoSelec) => {
        if (metodoDePagoSelec==undefined ) {throw new Error('Error: El argumento de metodo de pago no puede ser un null.');}
        setFormaDePago(metodoDePagoSelec.target.value === 'pagoEfectivo' ? 0 : 1);
    };
    const handleSinComprobanteChange = (comprobanteDePago) => {
        if (comprobanteDePago==undefined ) {throw new Error('Error: El argumento de Comprobante de pago no puede ser un null.');}
        setNumeroComprobante(comprobanteDePago.target.value);
    };


    return (
        accepted ? (
            <div>
                {formaDePago === 0 ? pagoE() : formaDePago === 1 ? pagoS() : <div className="cart-box"><h1>Ha ocurrido un error</h1></div>}
            </div>
        ) : (
            <div>
                {modalData && <Modal title={modalData.title} content={modalData.content} onClose={closeModal} />}
                <div className='metodoPago'>
                    <div className="centro">
                        <h1>Métodos de pago</h1>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" id="inlineCheckbox1" value="pagoEfectivo" onChange={handleMetodoChange} checked={formaDePago === 0} required />
                            <label className="form-check-label" htmlFor="inlineCheckbox1">En Efectivo</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" id="inlineCheckbox2" value="pagoSinpe" onChange={handleMetodoChange} checked={formaDePago === 1} required />
                            <label className="form-check-label" htmlFor="inlineCheckbox2">SinpeMóvil</label>
                        </div>
                        <button className="btn btn-primary" onClick={handleAceptar}>Aceptar</button>
                    </div>
                </div>
            </div>
        )
    );
};

const Modal = ({ title, content, onClose, closeButtonText = 'Cerrar', showCloseButton = true }) => {
    if (!title || !content || !onClose || typeof onClose !== 'function') {throw new Error('Error: Los argumentos title, content y onClose son obligatorios y onClose debe ser una función.');}
    if (title==undefined||content==undefined|| onClose==undefined) {throw new Error('Error: Los argumentos title, content no pueden ser indefinidos');}
    return (
        <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
            <div className="modal-dialog" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{title}</h5>
                        {showCloseButton && (
                            <button type="button" className="close" onClick={onClose} aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        )}
                    </div>
                    <div className="modal-body">
                        <p>{content}</p>
                    </div>
                    {showCloseButton && (
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" onClick={onClose}>
                                {closeButtonText}
                            </button>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};
const ModalPagoAceptado = ({ title, content, onClose, closeButtonText = 'Ir al inicio', showCloseButton = true }) => {
    if (!title || !content || !onClose || typeof onClose !== 'function') { throw new Error('Error: Los argumentos title, content y onClose son obligatorios y onClose debe ser una función.');}
    if (title==undefined||content==undefined|| onClose==undefined) {throw new Error('Error: Los argumentos title, content no pueden ser indefinidos');}
    const handleCerrarModal = () => {
        onClose();
        window.location.replace('/');
        localStorage.removeItem("tienda");
        return;
    };
    return (
        <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <div className="modal-dialog" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">{title}</h5>
                        {showCloseButton && (
                            <button type="button" className="close" onClick={handleCerrarModal} aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        )}
                    </div>
                    <div className="modal-body">
                        <p>{content}</p>
                    </div>
                    {showCloseButton && (
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" onClick={handleCerrarModal}>
                                {closeButtonText}
                            </button>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default MetodoPago;
