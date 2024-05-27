'use client'
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { getCookie } from 'cookies-next';
import Link from 'next/link';

export default function Page() {
  const token = getCookie('token');
  return (
    <>
      {
        token
      }
    </>
  );
}