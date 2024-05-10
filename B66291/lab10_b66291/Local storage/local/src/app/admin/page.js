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


  const procesarForm = (e) => {
    e.preventDefault();
    if(e == undefined){
      throw new Error("error al ingresar informacion de logueo");
    }
    if (!username || !password) {
      setError("complete la informacion por favor");
    } else {
       window.location.href = "/admin/products";
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
              placeholder="Ingresa tu correo"
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