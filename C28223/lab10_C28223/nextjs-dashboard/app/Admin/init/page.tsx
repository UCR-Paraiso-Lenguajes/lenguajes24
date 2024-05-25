"use client";
import "bootstrap/dist/css/bootstrap.min.css"; // Import bootstrap CSS
import React, { useState, useEffect } from 'react';
import "@/app/ui/styles.css";

const init = () => {

  return (
    <div>
      <title>Menú</title>
      <nav className="navbar  navbar-light">
      <button className="navbar-toggler" type="button" data-bs-toggle="offcanvas" data-bs-target="#menuLateral" >
            <span className="navbar-toggler-icon"></span>
          </button>
        <div className="container-fluid">
          <section className="offcanvas offcanvas-start" id="menuLateral">
            <div className="offcanvas-header" data-bs-theme="light">
              <h5 className="offcanvas-title">Menú</h5>
              <button className="btn-close" type="button" aria-label="Close" data-bs-dismiss="offcanvas"></button>
            </div>
            <div className="offcanvas-body d-flex flex-column justify-content-between px-0">
              <ul className="navbar-nav fs-5 justify-content-evenly">
                <li className="nav-item p-3 py-md-1">
                  <a href="/Admin/reports" className="nav-link">Reportes de ventas</a>
                </li>
                <li className="nav-item p-3 py-md-1">
                  <a href="" className="nav-link" >Opciones de Productos</a>
                </li>
                <li className="nav-item p-3 py-md-1">
                  <a href="/" className="nav-link">Volver</a>
                </li>
              </ul>
            </div>
          </section>
        </div>
      </nav>
      <script
        src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"
        integrity="sha384-w76AqPfDkMBDXo30jS1Sgez6pr3x5MlQ1ZAGC+nuZB+EYdgRZgiwxhTBTkF7CXvN"
        crossOrigin="anonymous"
      ></script>
    </div>
  );
};

export default init;