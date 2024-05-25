"use client"
import React, { createContext, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import Reports from './Reports';
import ProductList from './ProductList';


const SidebarContext = createContext();
function Page() {
  const [selected, setSelected] = useState();
  const [expanded, setExpanded] = useState(true);


  const isSelected = (pageNumber) => {
    if(pageNumber!=null && (pageNumber==0||pageNumber==1)){
      setSelected(pageNumber);
    }
      
  };

  return (
    <div className="container-fluid gray-background" style={{ height: "100vh" }}>

      <div className="row">
        <div className="col-sm-3">
          <nav className="col-md-2 d-none d-md-block bg-light nav-sidebar">

            <div className="sidebar-sticky">
              <ul className="nav flex-column">
                <li className={"nav-item $ { expanded ? '' : 'top-2'}"} >
                  <a className="nav-link" aria-current="page" onClick={() => isSelected(0)}>
                    Productos
                  </a>
                </li>
                <li className="nav-item">
                  <a className="nav-link" onClick={() => isSelected(1)}>
                    Reportes de Ventas
                  </a>
                </li>
                <li className="nav-item">
                  <a className="nav-link disabled" aria-disabled="true" href="#">
                    Ofertas
                  </a>
                </li>
              </ul>
            </div>
          </nav>
        </div>
        <div className="col-sm-9">
          <main role="main" className="col-md-10 ml-md-auto px-4">
            {selected == 0 ? (<ProductList />) : (<Reports />)}
          </main>

        </div>


      </div>


    </div >
  );
}

export default Page;
