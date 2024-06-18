"use client";
import "bootstrap/dist/css/bootstrap.min.css";
import '@/app/ui/Styles/login.css';
import { useState, useEffect } from "react";
import { decodeToken, checkTokenDate } from '../hooks/jwtHooks';
export default function LoginForm() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [warningUser, setWarningUser] = useState(false);
    const [warningPassword, setWarningPassword] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const URLConection = process.env.NEXT_PUBLIC_API;
    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();
  
      if (username.trim() === '') {
        setWarningUser(true);
        setTimeout(() => setWarningUser(false), 2000);
        return;
      }
  
      if (password.trim() === '') {
        setWarningPassword(true);
        setTimeout(() => setWarningPassword(false), 2000);
        return;
      }
        await authenticateUser();
    }

    async function authenticateUser() {
        try {
            const response = await fetch(URLConection + '/api/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    name: username,
                    password: password,
                }),
            })

            if (!response.ok) {
              setErrorMessage('Inicio de sesión incorrecto');
            } else {
                const tokenString = await response.json();
                const token = tokenString.token;
                sessionStorage.removeItem("sessionToken");
                sessionStorage.removeItem("expiracyToken");

                sessionStorage.setItem("sessionToken", token);
                const decodedToken = decodeToken(token);
                const expiracyDate = decodedToken?.exp;
                const userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

                sessionStorage.setItem("expiracyToken", String(expiracyDate));

                if (userRole === "Admin") {
                    window.location.href = "/admin/init";
                } else {
                    window.location.href = "/";
                }
            }
        } catch (error) {
          setErrorMessage('Error en el inicio de sesión');
        }
    }

    useEffect(() => {
        const token = sessionStorage.getItem("sessionToken");
        if (token) {
            const decodedToken = decodeToken(token);
            const isTokenAlive = checkTokenDate(decodedToken?.exp);
            if (!isTokenAlive) {
                sessionStorage.removeItem("sessionToken");
                sessionStorage.removeItem("expiracyToken");
                setErrorMessage('La sesión ha expirado, por favor inicie sesión nuevamente.');
            }
        }
    }, []);

    return (
        <div className="login">
            <h4>Login</h4>
            <form onSubmit={handleSubmit}>
                <div className="text_area">
                    <input
                        type="text"
                        id="username"
                        name="username"
                        className="text_input"
                        onChange={(e) => setUsername(e.target.value)}
                    />
                    {warningUser && <div className='alert'>Usuario Incorrecto, por favor ingrese su usuario.</div>}
                </div>
                <div className="text_area">
                    <input
                        type="password"
                        id="password"
                        name="password"
                        className="text_input"
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    {warningPassword && <div className='alert'>Contraseña incorrecta, por favor ingrese su contraseña.</div>}
                </div>
                {errorMessage && <div className='alert'>{errorMessage}</div>}
                <input
                    type="submit"
                    value="LOGIN"
                    className="btn"
                />
            </form>
        </div>
    );
}
