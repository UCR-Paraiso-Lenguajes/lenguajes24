import React from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import Link from 'next/link';
import '/app/ui/global.css';

export default function Products() {
    //Get/admin/products --api Get/products
    //Datatable
    //imagenes en http (también puede ser local, github) varias imagenes en un producto, list de img
    //btn de pantalla que abre pantalla en admin/product, qie tinene un form para poner nombre, descrp, imagenes y que se bean los links y btn de quitar 
    //hacer borrado lógico de los products, activado o borrado, porque después lo puede volver a activar
    //al guardar el new product, el api lo recibe como un post
    //el crud no  usa caché, hace la consulta directa a la BD
    //para new product es un post, para modificar es un put 

    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-12 d-flex justify-content-end align-items-center">
                        <Link href="/admin/init">
                            <button className="btn btn-dark">GO Back</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div className="container-fluid">
                <div className="row">
                    <div className="col-md-3 bg-custom">
                        <div className="sidebar d-flex flex-column justify-content-between align-items-center" style={{ height: '100vh', marginTop: '200px' }}>
                             
                              
                           
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
