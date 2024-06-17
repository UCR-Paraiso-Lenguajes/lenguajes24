"use client"
import AcmeLogo from '@/app/ui/acme-logo';
import { ArrowRightIcon } from '@heroicons/react/24/outline';
import Link from 'next/link';
import { lusitana } from '@/app/ui/fonts';
import React, { useState } from 'react';

const IncrementButton = ({ setItem }) => {
  const handleClick = () => {
    setItem(prevItem => ({ ...prevItem, count: prevItem.count + 1 }));
  };

  return (
    <button onClick={handleClick}>Incrementar</button>
  );
};

const DisplayItem = ({ item }) => {
  return (
    <div>
      <h2>Objeto en el Estado:</h2>
      <p>Conteo: {item.count}</p>
    </div>
  );
};

export default function Page() {
  console.log("----------------------")
  console.log(process.env.NEXT_PUBLIC_API)
  const [item, setItem] = useState({ count: 0 });
  return (
    <>
      <div>
        <h1>Ejemplo de Next.js con Componentes de React</h1>
        <IncrementButton setItem={setItem} />
        <DisplayItem item={item} />
      </div>
    
      <p
      className={`${lusitana.className} text-xl text-gray-800 md:text-3xl md:leading-normal`}
    >
      <strong>Welcome to Acme.</strong> This is the example for the{' '}
      <a href="https://nextjs.org/learn/" className="text-blue-500">
        Next.js Learn Course
      </a>
      , brought to you by Vercel.
    </p>
    </>
    // ...

    // ...
  );
}