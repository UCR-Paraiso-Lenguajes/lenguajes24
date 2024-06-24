import SideNav from "@/app/ui/admin/sidenav";
import '../ui/styles/reports.css';
import '../ui/styles/login.css';
import '../ui/styles/navAdmin.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-calendar/dist/Calendar.css';

export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="container">
            <div className="row">
                <div className="col-ms">
                    {children}
                </div>
                <div className="col-ms col-md col-xl">
                    <SideNav />
                </div>
            </div>
        </div>
    );
}