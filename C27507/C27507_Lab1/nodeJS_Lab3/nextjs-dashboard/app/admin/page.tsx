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

export default function Login(){
	const router = useRouter();       
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
	const [formStatus, setFormStatus] = useState(true);	

	const submitForm = () => {        		
		formValidations()		
    };

	async function formValidations(){

		const isEmailValid = email && email.trim();
		const isPassValid = password && password.trim();
		if(!isEmailValid || !isPassValid){			
			setFormStatus(false)			
		}else{
			setFormStatus(true)
			// Creación de un objeto de tipo UserAPI
			let userData: UserAccountAPI = {
				userName: "ejemploUsuario",
				userPassword: "ejemploContraseña"
			};
			let userToken = await validateUserAndGetToken(userData);

			if(userToken !== null && userToken !== undefined){

				sessionStorage.setItem("userToken",userToken);
				router.push("/admin/init/sales_report")
				// sessionStorage.setItem("loginToken","loginToken");
				// router.push("/admin/init/sales_report")
			}						
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