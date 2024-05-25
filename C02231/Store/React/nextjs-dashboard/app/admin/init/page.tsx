import React from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import Link from 'next/link';
import '/app/ui/global.css';

export default function InitPage() {
    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-12 d-flex justify-content-end align-items-center">
                        <Link href="/admin">
                            <button className="btn btn-dark">Cerrar Sesión</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-3 bg-custom">
                        <div className="sidebar d-flex flex-column justify-content-between align-items-center" style={{ height: '100vh', marginTop: '200px' }}>
                              <Link href="/admin/init/report">
                                    <button className="btn btn-dark mb-3">Reportes de Ventas</button>
                                </Link>
                                <Link href="/admin/init">
                                    <button className="btn btn-dark mb-3">Productos</button>
                                </Link>
                                <Link href="/admin/init">
                                    <button className="btn btn-dark ">Payment Methods</button>
                                </Link>
                              
                           
                            <div className="flex-grow-1"></div>
                        </div>
                    </div>

                    <div className="col-md-9">
                        <div className="content">
                            {}
                        </div>
                    </div>
                </div>
            </div>

            <footer className="footer" style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light">Biblioteca de Paula</h5>
                </div>
            </footer>
        </div>
    );
}
