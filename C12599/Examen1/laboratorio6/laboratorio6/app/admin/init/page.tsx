'use client'
import { useState } from 'react';
import Link from 'next/link';
import { Menu, X, ShoppingCart, User } from 'react-feather';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Navbar, Nav } from 'react-bootstrap';

const CustomAccordionItem = ({ title, href, icon, onClick }) => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleAccordion = () => {
    setIsOpen(!isOpen);
  };
  return (
    <div className="my-4">
      <div
        className="d-flex justify-content-between align-items-center bg-light p-3"
        style={{ cursor: 'pointer' }}
        onClick={onClick || toggleAccordion}
      >
        <h3 className="mb-0">{title}</h3>
        {isOpen ? <X /> : <Menu />}
      </div>
      {isOpen && (
       <div className="p-3">
       <div className="mb-2">
         <Link href="/ventas">
           <button className="btn btn-secondary btn-sm btn-block">
             Ir a Ventas
           </button>
         </Link>
       </div>
       <div>
         <Link href="/tienda">
           <button className="btn btn-secondary btn-sm btn-block">
             Ir a Productos
           </button>
         </Link>
       </div>
     </div>
      )}
    </div>
  );
};

const Init = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleMenu = () => {
    setIsOpen(!isOpen);
  };



  return (
    <div className="container-fluid">
      <Navbar bg="light" expand="lg">
       
        <Navbar.Toggle onClick={toggleMenu} aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav" className={isOpen ? 'show' : ''}>
          <Nav className="flex-column mt-4">
            <CustomAccordionItem
              title="Panel Administracion"
              href="/ventas"
              icon={<ShoppingCart />}
             
            />
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    </div>
  );
};

export default Init;
