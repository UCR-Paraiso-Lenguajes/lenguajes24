"use client";
import React, { useState } from 'react';
import Link from 'next/link';
import "bootstrap/dist/css/bootstrap.min.css";

const Sidebar = () => {
  const [showOffcanvas, setShowOffcanvas] = useState(false);

  const toggleOffcanvas = () => {
    setShowOffcanvas(!showOffcanvas);
  };

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
            Menu
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