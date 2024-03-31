"use client";
import React, { useEffect, useState } from 'react';
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import 'bootstrap/dist/css/bootstrap.min.css'; // Importar los estilos de Bootstrap
import 'bootstrap/dist/js/bootstrap.bundle.min'; // Importar los scripts de Bootstrap
import Link from 'next/link';
import { useRouter } from 'next/router';



export default function Pago() {
    const [seccionHabilitada, setSeccionHabilitada] = useState(false);
    const [seccionEfectivo, setSeccionEfectivo] = useState(false);
    const [seccionSinpe, setSeccionSinpe] = useState(false);
    const [num, setNum] = useState(0);//Genera un número aleatorio de compra
    const handleContinuarClick = () => {
        const direccionIngresada = document.getElementById('address').value.trim();
        if (direccionIngresada !== '') {
            setSeccionHabilitada(true);
        } else {
            alert('Por favor ingrese una dirección antes de continuar.');
        }
    };

    //Genera el número aleatorio de compra
    const randomNumberInRange = (min, max) => {
        return Math.floor(Math.random()
            * (max - min + 1)) + min;
    };

    //Muestra la sección de pago seleccionada
    const paymentMethod = () => {

        if (document.getElementById('opcion1').checked) {
            setSeccionEfectivo(true);
            setNum(randomNumberInRange(51234, 789065));
        }
        if (document.getElementById('opcion2').checked) {
            setSeccionSinpe(true);
            setNum(randomNumberInRange(51234, 789065));
        }
    };
    const message =()=>{
        if(document.getElementById('voucher').value.trim()!==''){
            alert('Espere la confirmación del administrador')
        }else{
            alert('Por favor ingrese el número de transacción')
        }
        
    }
    
 

    return (
        <div className='container'>
            <Link href={"/cart"}>
                <img src='https://cdn.pixabay.com/photo/2020/03/22/15/19/arrow-4957487_1280.png' style={{ height: "1cm" }} />
            </Link>
            <h2>Pago</h2>
            <form>
                <div className="form-group">
                    <h4>1. Ingrese su Dirección</h4>
                    <input type="text" className="form-control" id="address" style={{ width: "15cm" }} />
                </div>
                <a href="#MetodoPago">
                    <button type="button" style={{backgroundColor: "black", color: "white" }}  className="button" onClick={handleContinuarClick}>Continuar</button>
                </a>
            </form>
            <br></br>
            <section id="MetodoPago" style={{ display: seccionHabilitada ? 'block' : 'none' }}>
                <h4>2. Seleccione un método de Pago</h4>
                <div className='radio'><h6><input type="radio" name="optradio" id='opcion1'/>Efectivo</h6></div>
                <div className='radio'><h6><input type="radio" name="optradio" id='opcion2' />Sinpe</h6></div>
                <button className='button' style={{backgroundColor: "black", color: "white" }} onClick={paymentMethod}>Seleccionar</button>
            </section>
            <br></br>

            {seccionEfectivo && (
                <div>
                    <h4>3.Detalle de pago </h4>
                    <p>Número de compra: {num}</p>
                    <p>Espere la confirmación del administrador</p>
                </div>
            )}
            {seccionSinpe && (
                <div>
                    <h4>Detalle de pago </h4>
                    <h6>Número de compra: {num}</h6>
                    <h6>Número sinpe: 8976-3456</h6>
                    <input type="text" placeholder="Número de transacción" id='voucher'/>
                    <br></br>
                    <Link href={'/'}><button className='button' style={{backgroundColor: "black", color: "white" }} onClick={message}>Enviar</button></Link>
                </div>
            )}
        </div>
    );
}
