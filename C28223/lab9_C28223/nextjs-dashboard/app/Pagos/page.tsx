import React, { useState } from 'react';

const MetodoPago = () => {
    const [showModal, setShowModal] = useState(false);
    const [formaDePago, setFormaDePago] = useState(0); // 0 para efectivo, 1 para SinpeMóvil
    const [accepted, setAccepted] = useState(false);
    const memoryStore = JSON.parse(localStorage.getItem('tienda'));
    const [numeroCompra, setNumeroCompra] = useState('');

    const handleAceptar = () => {
        if (formaDePago === '') {
            setShowModal(true);
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

    const productIds = memoryStore.productos.map((producto: any) => String(producto.id));
    const dataToSend = {
        ProductIds: productIds,
        Address: memoryStore.carrito.direccionEntrega,
        PaymentMethod: memoryStore.carrito.metodoDePago
    };

    const enviarDatosaAPI = async () => {
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
            } else {
                const errorResponseData = await response.json();
                throw new Error(errorResponseData.message || 'Error al procesar el pago');
            }
    }

    const pagoE = () => {
        return (
            <div className="Compra">
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
                <div className="smsFinal">
                    <div className="text-center">
                        <h5>Número de compra: {numeroCompra}</h5>
                        <h4>Número de SinpeMóvil: +506 3875 8524</h4>
                        <input type="text" placeholder="Número de comprobante"></input>
                        <button className="btn btn-primary" onClick={handleAceptar}>Aceptar</button>
                        <h3>Esperando la confirmación del Administrador</h3>
                        <button className="btn btn-primary" onClick={enviarDatosaAPI}>Confirmar Pago</button>
                    </div>
                </div>
            </div>
        );
    };

    const closeModal = () => {
        setShowModal(false);
    };

    const handleMetodoChange = (metodoDePagoSelec) => {
        setFormaDePago(metodoDePagoSelec.target.value === 'pagoEfectivo' ? 0 : 1);
    };

    return (
        accepted ? (
            <div>
                {formaDePago === 0 ? pagoE() : formaDePago === 1 ? pagoS() : <div className="cart-box"><h1>Ha ocurrido un error</h1></div>}
            </div>
        ) : (

            <div>
                {showModal && <ModalSinMetodoPago closeModal={closeModal} />}
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

const ModalSinMetodoPago = ({ closeModal }) => {
    return (
        <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
            <div className="modal-dialog" role="document">
                <div className="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title">Método de pago</h5>
                        <button type="button" className="close" onClick={closeModal} aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div className="modal-body">
                        <p>Por favor seleccione un método de pago disponible.</p>
                    </div>
                    <div className="modal-footer">
                        <button type="button" className="btn btn-secondary" onClick={closeModal}>Cerrar</button>
                    </div>
                </div>
            </div>
        </div>
    );
};


export default MetodoPago;
