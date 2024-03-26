import React from 'react';
import Link from 'next/link';

export default function CartPage() {
  return (
    <div>
      <h1>¡Esta es la página del carrito de compras!</h1>
      <Link href="/">Volver a la página de inicio</Link>
    </div>
  );
}
