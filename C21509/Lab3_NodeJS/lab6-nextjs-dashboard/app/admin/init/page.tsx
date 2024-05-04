"use client"
import React from 'react';
import Link from 'next/link';
import 'bootstrap/dist/css/bootstrap.min.css';

const InitPage = () => {
  return (
    <div className="container-fluid">
      <div className="row">
        <nav className="col-md-3 col-lg-2 d-md-block bg-light sidebar">
          <div className="position-sticky pt-3">
            <ul className="nav flex-column">
              <li className="nav-item">
                <Link href="/product">
                  Productos
                </Link>
              </li>
              <li className="nav-item">
                <Link href="/admin/graphic">
                  Reportes de Ventas
                </Link>
              </li>
            </ul>
          </div>
        </nav>

        <main className="col-md-9 ms-sm-auto col-lg-10 px-md-4">
          <div className="pt-3">
            <h2>Bienvenido al Panel de Administrador</h2>
          </div>
        </main>
      </div>
    </div>
  );
};

export default InitPage;