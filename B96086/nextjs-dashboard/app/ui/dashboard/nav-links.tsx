'use client'
import { FaShoppingCart } from 'react-icons/fa'; // Importa un ícono de carrito 
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import clsx from 'clsx';

// Map of links to display in the side navigation.
// Depending on the size of the application, this would be stored in a database.
interface LinkItem {
  href: string;
  icon: () => JSX.Element; // Define el tipo de icono como una función que devuelve JSX.Element
}

const links: LinkItem[] = [
  {
     href: '/dashboard/cart', icon: () => <FaShoppingCart size={30} />, // Cambia el valor de icon a una función que devuelve el icono
  },
];

export default function NavLinks({ countCart }: { countCart: number }) {
  const pathname = usePathname();

  return (
    <>
      {links.map((link, index) => {
        const LinkIcon = link.icon
        const className = index === 0 ? "nav-item active" : "nav-item";
        
        const count = index === links.length - 1 ? countCart : "";
        return (
          <li className={className} key={index}>
            <Link
              href={link.href}
              className={clsx(
                'nav-link',
                {
                  'bg-sky-100 text-blue-600': pathname === link.href,
                }
              )}
            >
            <span className="btn btn-light cart-icon">
                <LinkIcon className='linkIcon' />
                <span className ='badge bg-secondary'>{count}</span> 
              </span>
            </Link >
          </li >
        );
      })}
    </>
  );
}
