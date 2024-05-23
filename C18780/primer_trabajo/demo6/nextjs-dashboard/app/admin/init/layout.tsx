import SideNav from "@/app/ui/admin/sidenav";
import '../../ui/styles/reports.css';
export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="container">
            <div className="row">
                <div className="col-sm-3">
                    <SideNav />
                </div>
                <div className="col-sm-9">
                    {children}
                </div>
            </div>
        </div>
    );
}