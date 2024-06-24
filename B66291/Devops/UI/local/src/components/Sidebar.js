import React from 'react';
import Side from '../styles/sidebar.css';

const Sidebar = () => {
  return (
    <div className="sidebar">
      <a href="#home">
        Opciones de Productos
        <svg xmlns="http://www.w3.org/2000/svg" 
        width="25" 
        height="25" 
        fill="currentColor" 
        className="bi bi-box-fill" v
        iewBox="0 0 16 16">
          <path fillRule="evenodd" d="M15.528 2.973a.75.75 0 0 1 .472.696v8.662a.75.75 0 0 1-.472.696l-7.25 2.9a.75.75 0 0 1-.557 0l-7.25-2.9A.75.75 0 0 1 0 12.331V3.669a.75.75 0 0 1 .471-.696L7.443.184l.004-.001.274-.11a.75.75 0 0 1 .558 0l.274.11.004.001zm-1.374.527L8 5.962 1.846 3.5 1 3.839v.4l6.5 2.6v7.922l.5.2.5-.2V6.84l6.5-2.6v-.4l-.846-.339Z"/>
        </svg>
      </a>
      <a href='/admin/products/reports'>
        Reportes de ventas 
        <svg xmlns="http://www.w3.org/2000/svg" 
        width="24" 
        height="24" 
        fill="currentColor" 
        className="bi bi-graph-up" viewBox="0 0 16 16">
          <path fillRule="evenodd" d="M0 0h1v15h15v1H0zm14.817 3.113a.5.5 0 0 1 .07.704l-4.5 5.5a.5.5 0 0 1-.74.037L7.06 6.767l-3.656 5.027a.5.5 0 0 1-.808-.588l4-5.5a.5.5 0 0 1 .758-.06l2.609 2.61 4.15-5.073a.5.5 0 0 1 .704-.07"/>
        </svg>
      </a>
      
    </div>
  );
};

export default Sidebar;