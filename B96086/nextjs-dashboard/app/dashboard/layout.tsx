import '@/app/ui/style/products.css';
import '@/app/ui/style/Ecomerce.css';
import '@/app/ui/style/cart.css';
import '@/app/ui/style/checkout.css';



export default function Layout({ children }: { children: React.ReactNode }) {
  return (
    <div className="flex h-screen flex-col md:flex-row md:overflow-hidden">
      <div className="flex-grow p-6 md:overflow-y-auto md:p-12" style={{ marginTop: '80px' }}>
        {children}
      </div>
    </div>
  );
}
