'use client';
import {
  UserGroupIcon,
  ShoppingCartIcon,
} from '@heroicons/react/24/outline';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import clsx from 'clsx';

// Map of links to display in the side navigation.
// Depending on the size of the application, this would be stored in a database.
interface LinkItem {
  name: string;
  href: string;
  icon: any;
}

const links: LinkItem[] = [
  {
    name: 'Cart', href: '/dashboard/cart', icon: ShoppingCartIcon,
  },
];

export default function NavLinks({ countCart }: { countCart: number }) {
  const pathname = usePathname();

  return (
    <>
      {links.map((link, index) => {
        const LinkIcon = link.icon;
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
              <LinkIcon className="linkIcon" />
              <span className="cart-count">{count}</span>
              <p className="linkName">{link.name}</p>
            </Link >
          </ul >
        );
      })}
    </>
  );
}

