'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import Link from 'next/link';
import '/app/ui/global.css';
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'next/navigation';

export default function InitPage() {
    const router = useRouter();
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const loginToken = sessionStorage.getItem("authToken");

    const checkAuthToken = async () => {
        const loginToken = sessionStorage.getItem("authToken");

        if (!loginToken) {
            return false;
        }

        try {
            const tokenFormat = jwtDecode(loginToken);
            const todayDate = Date.now() / 1000;

            if (tokenFormat.exp && tokenFormat.exp < todayDate) {
                sessionStorage.removeItem("authToken");
                return false;
            }

            return true;
        } catch (error) {
            return false;
        }
    };

    useEffect(() => {
        const verifyToken = async () => {
            const isValid = await checkAuthToken();
            if (!isValid) {
                router.push("/../admin");
            } else {
                setIsAuthenticated(true);
            }
        };

        verifyToken();
    }, [router]);

    if (!isAuthenticated) {
        return null;
    }

    const handleRemove = async () => {
        sessionStorage.removeItem("authToken");
    };

    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-12 d-flex justify-content-end align-items-center">
                        <Link href="/admin">
                            <button className="btn btn-dark" onClick={handleRemove}>Cerrar Sesión</button>
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
                            <Link href="/admin/init/products">
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
                            { }
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
