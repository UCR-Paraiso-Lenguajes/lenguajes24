//Componentes
import Link from 'next/link';

//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';

export default function AdminDashboard({
    children,
  }: {
    children: React.ReactNode
  }){
    return(        

        <div className="container-fluid">
            <div className="row">
                {/* Menú lateral - Columna 1 */}
                <header id="menu_v" className="menu_v col-sm-2 bg-dark text-light">
                    <div className="menu_v_logo">
                        <span>Aurora</span>
                    </div>
                    <nav className='menu_v_nav'>
                        <ul>
                            <Link href="/admin/init/sales_report"><button>Reportes de ventas</button></Link>
                            <Link href="/admin/init/products_info"><button>Productos</button></Link>
                            <Link href="/admin/init/notification"><button>Notificaciones</button></Link>
                            <Link href="/admin/init/payment-methods"><button>PaymentMethod</button></Link>
                        </ul>
                    </nav>
                </header>

                <div className="col-sm-10">
                    <div className="row">
                        {/* Menú vertical - Columna 2 */}
                        <header id="menu_h" className="menu_h col-md-12 row bg-secondary text-light">
                            <button className="menu_h_close col-md-1">X</button>
                            <nav  className="menu_h_nav col-md-6">                                
                                <ul>
                                    <button>Notificaciones</button>
                                    <button>Mi usuario</button>
                                    <button>Salir</button>                                    
                                </ul>
                            </nav>
                        </header>

                        {/* Área principal - Columna 2*/}
                        <main id="principal" className="menu_p col-md-12 row bg-light">
                            {children}
                            {/* <h2>Área Principal</h2>
                            <p>Contenido de la página principal</p>
                            <section>                                                        
                            </section>                         */}
                        </main>
                    </div>                    
                    
                </div>
            </div>
        </div>

    );
}