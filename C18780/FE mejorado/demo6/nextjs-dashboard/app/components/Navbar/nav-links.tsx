'use client';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import clsx from 'clsx';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCartShopping } from '@fortawesome/free-solid-svg-icons';

interface LinkItem {
  name: string;
  href: string;
}

const links: LinkItem[] = [
  {
    name: 'Cart', href: '/cart',
  },
];

export default function NavLinks({ countCart }: { countCart: number }) {
  const pathname = usePathname();

  return (
    <>
      {links.map((link, index) => {
        const className = index === 0 ? "nav-item active" : "nav-item";
        
        const count = index === links.length - 1 ? countCart : "";
        return (
          <ul className={className} key={index}>
            <Link
              href={link.href}
              className={clsx(
                'nav-link',
                {
                  'bg-sky-100 text-blue-600': pathname === link.href,
                },
              )}
            >
              <FontAwesomeIcon icon={faCartShopping} className="fa-icon me-2" />
              <span className="cart-count">{count}</span>
            </Link >
          </ul >
        );
      })}
    </>
  );
}

