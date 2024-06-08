"use client";
import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars } from '@fortawesome/free-solid-svg-icons';
import { decodeToken, checkTokenDate } from '../../hooks/jwtHooks';
import { useRouter } from 'next/navigation';

const Sidebar = () => {
  const [showOffcanvas, setShowOffcanvas] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const toggleOffcanvas = () => {
    setShowOffcanvas(!showOffcanvas);
  };
  const router = useRouter();

  useEffect(() => {
    try {
      const token = sessionStorage.getItem("sessionToken");
      if (token) {
        const decodedToken = decodeToken(token);
        const isTokenAlive = checkTokenDate(decodedToken?.exp);
        if (!isTokenAlive) {
          sessionStorage.removeItem("sessionToken");
          sessionStorage.removeItem("expiracyToken");
          setErrorMessage('La sesión ha expirado, por favor inicie sesión nuevamente.');
          router.push("/admin");
        }
      } else {
        setErrorMessage('No se encontró el token, por favor inicie sesión.');
        router.push("/admin");
      }
    } catch (error) {
      console.error("Error validating token:", error);
      setErrorMessage('Ocurrió un error al validar la sesión, por favor inicie sesión nuevamente.');
      router.push("/admin");
    }
  }, [router]);

  return (
    <div className="container-fluid">
      <div className="row flex-nowrap">
        <div className="col py-3">
          <button
            className="btn btn-primary"
            type="button"
            data-bs-toggle="offcanvas"
            data-bs-target="#offcanvasExample"
            aria-controls="offcanvasExample"
            onClick={toggleOffcanvas}
          >
            <FontAwesomeIcon icon={faBars} />
          </button>

          <div
            className={`offcanvas offcanvas-start ${showOffcanvas ? 'show' : ''}`}
            tabIndex="-1"
            id="offcanvasExample"
            aria-labelledby="offcanvasExampleLabel"
          >
            <div className="offcanvas-header">
              <h5 className="offcanvas-title" id="offcanvasExampleLabel">
                Menu
              </h5>
              <button
                type="button"
                className="btn-close text-reset"
                data-bs-dismiss="offcanvas"
                aria-label="Close"
                onClick={toggleOffcanvas}
              ></button>
            </div>
            <div className="offcanvas-body">
              <ul className="nav flex-column">
                <li className="nav-item">
                  <Link href="/admin/init" passHref>
                    <span className="nav-link">Home</span>
                  </Link>
                </li>
                <li className="nav-item">
                  <Link href="/admin/sales" passHref>
                    <span className="nav-link">Sales Reports</span>
                  </Link>
                </li>
                <li className="nav-item">
                  <a href="#" className="nav-link">
                    Products
                  </a>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Sidebar;
