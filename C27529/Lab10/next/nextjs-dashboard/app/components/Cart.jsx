import React, { useState } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS

export const Cart = ({
    goToPage
}) => {

    const [store, setStore] = useState(() => {
        const storedStore = localStorage.getItem("tienda");
        return (JSON.parse(storedStore))
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

    return (
        <>
        
            {store.productos.map((product) => (
                <div className='cart-product' key={product.id}>
                    <div className='info-cart-product'>
                        <p className='titulo-producto-carrito'>{product.name}</p>
                        <p> {product.description}</p>
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
                        <path strokeLinecap='round' strokeLinejoin='round' d='M6 18L18 6M6 6l12 12' />
                    </svg>
                </div>
            ))}

            <div className='total-Payment'>
                <div className='total-container'>
                    <h3>Subtotal:</h3>
                    <span className='total-pagar'>₡{store.carrito.subtotal}</span>
                    <h3>Total:</h3>
                    <span className='total-pagar'>₡{store.carrito.total}</span>
                </div>
                <div className='btn-cartPayment'>
                <boton onClick={() => goToPage(2)}>Continuar</boton>
                </div>
                <div className='btn-cartPayment'>
                <boton onClick={() => goToPage(0)}>Atrás</boton>
                </div>
            </div>



        </>
    );
};
