import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css";
import { HubConnectionBuilder } from '@microsoft/signalr';
import eventEmitter from './eventEmitter';

export const Header = ({ goToPage }) => {
    const [active, setActive] = useState(false);
    const [campaignActive, setCampaignActive] = useState(false);
    const [newMessages, setNewMessages] = useState(0);
    const [messages, setMessages] = useState([]);
    const [connection, setConnection] = useState(null);
    const [store, setStore] = useState(() => {
        if (typeof window !== 'undefined') {
            const storedStore = localStorage.getItem("tienda");
            return JSON.parse(storedStore) || { productos: [], carrito: { subtotal: 0, total: 0 } };
        }
        return { productos: [], carrito: { subtotal: 0, total: 0 } };
    });

    const URL = process.env.NEXT_PUBLIC_API_URL;
    if (!URL) {
        throw new Error('NEXT_PUBLIC_API_URL is not defined');
    }

    useEffect(() => {
        const connectToHub = async () => {
            const connection = new HubConnectionBuilder()
                .withUrl(`${URL}/Campannas`)
                .withAutomaticReconnect()
                .build();

            connection.on("UpdateMessages", (receivedMessages) => {
                setMessages(receivedMessages);
                setNewMessages(receivedMessages.length);
            });

            try {
                await connection.start();
                setConnection(connection);
            } catch (error) {
                console.error('Connection failed: ', error);
            }
        };

        connectToHub();

        return () => {
            if (connection) {
                connection.stop();
            }
        };
    }, [URL]);

    useEffect(() => {
        const updateStore = (updatedStore) => {
            setStore(updatedStore);
        };

        eventEmitter.on('cartUpdated', updateStore);

        return () => {
            eventEmitter.off('cartUpdated', updateStore);
        };
    }, []);

    const onDeleteProduct = product => {
        const updatedProductos = store.productos.filter(item => item.id !== product.id);
        const updatedSubtotal = store.carrito.subtotal - product.price;
        const updatedTotal = store.carrito.total - product.price;

        const updatedStore = {
            ...store,
            productos: updatedProductos,
            carrito: {
                ...store.carrito,
                subtotal: updatedSubtotal,
                total: updatedTotal
            }
        };

        setStore(updatedStore);
        localStorage.setItem("tienda", JSON.stringify(updatedStore));
        eventEmitter.emit('cartUpdated', updatedStore);
    };

    const onCleanCart = () => {
        localStorage.removeItem("tienda");
        const updatedStore = { productos: [], carrito: { subtotal: 0, total: 0 } };
        setStore(updatedStore);
        eventEmitter.emit('cartUpdated', updatedStore);
    };

    return (
        <header>
            <h1>GreatestBuy</h1>
            <nav className="navbar navbar-expand-lg bg-body-tertiary">
                <div className="container-fluid">
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup"
                        aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNavAltMarkup">
                        <div className="navbar-nav">
                            <div className="secciones">
                                <div className="row">
                                    <div className="col-sm-2">
                                        <button className="nav-link"> <i className="fa-solid fa-list-ul"></i> Todo</button>
                                    </div>
                                    <div className="col-sm-2">
                                        <button className="nav-link"> Ofertas del día</button>
                                    </div>
                                    <div className="col-sm-2">
                                        <button className="nav-link"> Servicio al Cliente</button>
                                    </div>
                                    <div className="col-sm-2">
                                        <button className="nav-link"> Listas</button>
                                    </div>
                                    <div className="col-sm-2">
                                        <button className="nav-link">Tarjetas de Regalo</button>
                                    </div>
                                    <div className="col-sm-2">
                                        <button className="nav-link"> Vender</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
            <div className='container-icon'>
                <div
                    className='container-cart-icon'
                    onClick={() => setActive(!active)}
                >
                    <svg
                        xmlns='http://www.w3.org/2000/svg'
                        fill='none'
                        viewBox='0 0 24 24'
                        strokeWidth='1.5'
                        stroke='currentColor'
                        className='icon-cart'
                    >
                        <path
                            strokeLinecap='round'
                            strokeLinejoin='round'
                            d='M15.75 10.5V6a3.75 3.75 0 10-7.5 0v4.5m11.356-1.993l1.263 12c.07.665-.45 1.243-1.119 1.243H4.25a1.125 1.125 0 01-1.12-1.243l1.264-12A1.125 1.125 0 015.513 7.5h12.974c.576 0 1.059.435 1.119 1.007zM8.625 10.5a.375.375 0 11-.75 0 .375.375 0 01.75 0zm7.5 0a.375.375 0 11-.75 0 .375.375 0 01.75 0z'
                        />
                    </svg>
                    <div className='count-products'>
                        <span id='contador-productos'>{store.productos.length}</span>
                    </div>
                </div>
                <div className='container-campaign-icon' onClick={() => setCampaignActive(!campaignActive)}>
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth="1.5"
                        stroke="currentColor"
                        className="icon-campaign"
                        style={{ width: '32px', height: '32px' }}
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            d='M12 8v8m4-4H8m8-4a8 8 0 11-16 0 8 8 0 0116 0z'
                        />
                    </svg>
                    <div className='count-messages'>
                        <span id='contador-mensajes'>{newMessages}</span>
                    </div>
                </div>
                {campaignActive && (
                    <div className={`container-campaign-messages ${campaignActive ? '' : 'hidden-campaign'}`} style={{ maxHeight: '300px', overflowY: 'auto' }}>
                        {messages.length > 0 ? (
                            <div className='campaign-messages'>
                                {messages.slice().reverse().map((message, index) => (
                                    <div className='message-item' key={index}>
                                        <div dangerouslySetInnerHTML={{ __html: message.content || "Mensaje vacío" }}></div>
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <p className='campaign-empty'>No hay mensajes</p>
                        )}
                    </div>
                )}
            </div>
            <div
                className={`container-cart-products ${active ? '' : 'hidden-cart'
                    }`}
            >
                {store.productos.length > 0 ? (
                    <>
                        <div className='row-product'>
                            {store.productos.map(product => (
                                <div className='cart-product' key={product.id}>
                                    <div className='info-cart-product'>
                                        <p className='titulo-producto-carrito'>{product.name}</p>
                                        <span className='precio-producto-carrito'>₡{product.price}</span>
                                    </div>
                                    <svg
                                        xmlns='http://www.w3.org/2000/svg'
                                        fill='none'
                                        viewBox='0 0 24 24'
                                        strokeWidth='1.5'
                                        stroke='currentColor'
                                        className='icon-close'
                                        onClick={() => onDeleteProduct(product)}
                                    >
                                        <path
                                            strokeLinecap='round'
                                            strokeLinejoin='round'
                                            d='M6 18L18 6M6 6l12 12'
                                        />
                                    </svg>
                                </div>
                            ))}
                        </div>
                        <div className='cart-total'>
                            <h3>Subtotal:</h3>
                            <span className='total-pagar'>₡{store.carrito.subtotal}</span>
                            <h6>Sin Impuestos</h6>
                        </div>
                        <button className='btn-payment' onClick={() => goToPage(1)}>Pagar</button>
                        <button className='btn-clear-all' onClick={onCleanCart}>Vaciar Carrito</button>
                    </>
                ) : (
                    <p className='cart-empty'>El carrito está vacío</p>
                )}
            </div>
        </header>
    );
};
