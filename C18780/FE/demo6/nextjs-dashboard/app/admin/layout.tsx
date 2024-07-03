'use client'
import SideNav from "../components/NavbarAdmin/sidenav";
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="container">
            <div className="row">
                <div className="col-ms">
                    {children}
                </div>
                <div className="col-ms col-md col-xl">
                    <SideNav/>
                </div>
            </div>
        </div>
    );
}