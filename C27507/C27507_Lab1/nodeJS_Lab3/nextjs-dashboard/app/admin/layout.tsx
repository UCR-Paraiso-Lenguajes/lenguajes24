'use client';

//Componentes
import { useEffect } from 'react';
import { useRouter } from 'next/router';
import { jwtDecode } from 'jwt-decode';


//Recursos
import 'bootstrap/dist/css/bootstrap.min.css';
import VerifyComponent from '../global-components/verify_component';

  export default function RootLayout({
    children,
  }: {
    children: React.ReactNode
  }) {
  
    return (
      <html lang="en">        
        <body>
            <VerifyComponent>{children}</VerifyComponent>
            {/* {children} */}
        </body>
      </html>
    )
  }