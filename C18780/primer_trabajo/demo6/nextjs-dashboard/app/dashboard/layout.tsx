import Footer from './footer';
import 'bootstrap/dist/css/bootstrap.min.css';
import '../ui/styles/Ecomerce.css';
import '../ui/styles/nav.css';
import '../ui/styles/cartPage.css';
import '../ui/styles/checkout.css';
import '../ui/styles/products.css';

export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex h-screen flex-col md:flex-row md:overflow-hidden">
            <div className="w-full flex-none md:w-64">
            </div>
            <div className="flex-grow p-6 md:overflow-y-auto md:p-12">{children}</div>
            <Footer/>
        </div>
    );
}