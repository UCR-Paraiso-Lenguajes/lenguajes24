import React, { useState, useEffect } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; // Importar CSS de Bootstrap

export const Header = ({ goToPage }) => {
    const [active, setActive] = useState(false);
    const [store, setStore] = useState(() => {
        const storedStore = localStorage.getItem("tienda");
        return JSON.parse(storedStore) || { productos: [], carrito: { subtotal: 0, total: 0 } };
    });

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
    };


    const onCleanCart = () => {
        localStorage.removeItem("tienda");
        setStore({ productos: [], carrito: { subtotal: 0, total: 0 } });

    };

	useEffect(() => {
        const contadorProductos = document.getElementById('contador-productos');
        if (contadorProductos) {
            contadorProductos.textContent = store.productos.length;
        }
    }, [store.productos]); 

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

                <div
                    className={`container-cart-products ${active ? '' : 'hidden-cart'
                        }`}
                >
                    {true ? (
                        <>
                            <div className='row-product'>
                                {store.productos.map(product => (
                                    <div className='cart-product' key={product.id}>
                                        <div className='info-cart-product'>

                                            <p className='titulo-producto-carrito'>
                                                {product.name}
                                            </p>
                                            <span className='precio-producto-carrito'>
                                                ₡{product.price}
                                            </span>
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
                                <span className='total-pagar'>{<span className='total-pagar'>₡{store.carrito.subtotal}</span>}</span>
                                <h6>Sin Impuestos</h6>
                            </div>
                            <button className='btn-payment' onClick={() => goToPage(1)}>
                                Pagar
                            </button>

                            <button className='btn-clear-all' onClick={onCleanCart}>
                                Vaciar Carrito
                            </button>
                        </>
                    ) : (
                            <>

                                <p className='cart-empty'>El carrito está vacío</p>


                            </>

                        )}
                </div>
            </div>
        </header>
    );
};
