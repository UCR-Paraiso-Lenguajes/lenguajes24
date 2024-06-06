'use client';
import {useState} from 'react';
import {useEffect} from 'react';

//Componentes
import Form from 'react-bootstrap/Form';
import Link from 'next/link';
import Button from 'react-bootstrap/Button';
import { useRouter } from 'next/navigation';

//Interfaces
import { UserAccountAPI } from '../src/models-data/UserAccountAPI';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import './../src/css/login.css'
import './../src/css/fonts_awesome/css/all.min.css'
import { mock } from 'node:test';

//Funciones
import { validateUserAndGetToken } from '../src/api/get-post-api';

//Para manejar tokens JWT
const { default: jwt_decode } = require("jwt-decode");

export default function Login(){

	const router = useRouter();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
	const [formStatus, setFormStatus] = useState(true);	

	const submitForm = () => {
		const emailValid = email && email.trim();
		const passValid = password && password.trim();
		if(emailValid && passValid){
			getLoginCredentials();
		}else{
			//Desplegar errores de formulario			
			setFormStatus(false)				
		}		
    };

	//Para verificar si recibimos un token o un mensaje de error del fetch
	function validTokenFormat(token: string): boolean {		
		const parts = token.split('.');
		
		// Verificar si tiene tres partes
		if (parts.length === 3) {
			return true;
		}
		return false;		
	}

	async function getLoginCredentials(){
		
		try {			
			let userData: UserAccountAPI = {
				userName: email,
				userPassword: password
			};
			let loginToken = await validateUserAndGetToken(userData);

			if(loginToken !== null && loginToken !== undefined){
				if (validTokenFormat(loginToken)) {				
					sessionStorage.setItem("loginToken", loginToken);
					router.push("/admin/init/sales_report");
				}
			}
		} catch (error) {
			//Desplegar errores de formulario			
			setFormStatus(false);
		}
	}

	return(		

		<div className="login_container row">

			<header className="login_header col-sm-12">
				<figure className="logo">
					<img src="" alt="" />				
				</figure>
										
			</header>
			
			<div className="form_container row col-sm-9 mx-auto">
				
				<form className="form row">
					
					<h2>Inicio de Sesion</h2>					

					<div className="campos campo-usuario col-sm-12" >

						<input 
							id="correo" 
							className="prueba" 
							type="text" 
							name="correo" 
							placeholder="Correo:"							
                            onChange={(e) => setEmail(e.target.value)}                            
						/>

						<span className="info_icon iconos"><img src="./../src/icons/contra.svg"/></span>						
					</div>			
					{!formStatus ? (
    					<Form.Control.Feedback type="invalid">
                        	Por favor, ingrese un correo válido.
                    	</Form.Control.Feedback>		
					) : null}
					

					<div className="campos campo-contra col-sm-12">
						<input 
							id="contra" 
							className="prueba" 
							type="password" name="pass" 
							placeholder="Contraseña:"														
                            onChange={(e) => setPassword(e.target.value)}                            
						/>
						<span className="info_icon iconos"><img src=""/></span>						
						<span id="alert_visibility" className="alert_visibility iconos" title="Presione para mostrar/ocultar la contraseña"><i className="far fa-eye"></i></span>						
					</div>				
					{!formStatus ? (
    					<Form.Control.Feedback type="invalid">
        				La contraseña es requerida.
    					</Form.Control.Feedback>
					) : null}
					
					
					<div className="forgotten_pass_container">
						{/* <Link href=""> */}
						<a className="forgotten_pass">¿Olvidó su contraseña?</a>
						{/* </Link> */}
						
					</div>									
					<input id="boton" type="button"value="Iniciar" onClick={submitForm}/>
				</form>

				
			</div>			
		</div>
	);
}