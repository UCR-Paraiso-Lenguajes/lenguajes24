'use client'
import { useState } from 'react';
import Link from 'next/link';
import { ChevronDown, ChevronUp } from 'react-feather';
import 'bootstrap/dist/css/bootstrap.css';

const CustomAccordionItem = ({ title, href }) => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleAccordion = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="my-4">
      <div
        className="d-flex justify-content-between align-items-center bg-light p-3"
        style={{ cursor: 'pointer' }}
        onClick={toggleAccordion}
      >
        <h3 className="mb-0">{title}</h3>
        {isOpen ? <ChevronUp /> : <ChevronDown />}
      </div>
      {isOpen && (
        <div className="p-3">
          <Link href={href}>
            <button className="btn btn-secondary btn-sm btn-block">
              Ir a {title}
            </button>
          </Link>
        </div>
      )}
    </div>
  );
};

const Init = () => {
  return (
    <div className="container-fluid">
      <div className="row">
        <div className="col-sm-auto bg-light sticky-top py-4">
          <h2 className="mt-4 mb-3">Panel de Administración</h2>
          <CustomAccordionItem title="Productos" href="/tienda" />
          <CustomAccordionItem title="Ventas" href="/ventas" />
        </div>
        <div className="col-sm p-3 min-vh-100">
        </div>
      </div>
    </div>
  );
};

export default Init;
