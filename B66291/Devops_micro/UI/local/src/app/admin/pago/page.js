"use client"
import "../../../styles/direccion.css"
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from '../../../components/Navbar';
import React, { useState } from 'react';

const ModificaPago = () => {

    const storedData = localStorage.getItem('tienda');
    const dataObject = JSON.parse(storedData);

    return (
        <article>
            <div>
                <Navbar cantidad_Productos={dataObject.cart.productos.length} />
            </div>
        </article>
    );
};

export default ModificaPago;