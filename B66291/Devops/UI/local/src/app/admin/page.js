"use client";
import React, { useState } from "react";
import "../../styles/login.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Navbar from "../../components/Navbar";

const Login = () => {
  const storedData = localStorage.getItem("tienda");
  const dataObject = JSON.parse(storedData);

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");


  const procesarForm = async (e) => {
    e.preventDefault();
    if (!username || !password) {
      setError("Complete la información por favor");
    } else {
      try {
        const response = await fetch("https://localhost:7013/api/auth/login", {
          method: "POST",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            username: username,
            password: password
          })
        });

        if (response.ok) {
          const data = await response.json();
          const token = data.token;
          await sessionStorage.setItem("token", token);
          window.location.href = "/admin/products";
        } else {
          setError("Credenciales incorrectas");
        }
      } catch (error) {
        setError("Ocurrió un error al procesar la solicitud");
      }
    }
  };

  return (
    <article>
      <div>
        <Navbar cantidad_Productos={dataObject.cart.productos.length} />
      </div>
      <div className="form_login">
        <form onSubmit={procesarForm}>
          <div>
            <label htmlFor="email">Usuario:</label>
            <input
              type="text"
              name="username"
              className="form-control"
              placeholder="Ingresa tu usuario"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div>
            <label htmlFor="password">Contraseña:</label>
            <input
              type="password"
              name="password"
              className="form-control"
              placeholder="Ingresa tu contraseña"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
          {error && <p className="error">{error}</p>}
          <div>
            <button className="btnAsignar" type="submit">
              Iniciar sesión
            </button>
          </div>
        </form>
      </div>
    </article>
  );
};

export default Login;