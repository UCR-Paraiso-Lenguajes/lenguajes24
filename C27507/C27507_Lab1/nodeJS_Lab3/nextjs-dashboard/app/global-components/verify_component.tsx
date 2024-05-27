"use client";
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';


export default function  VerifyComponent({children,}: {children: React.ReactNode}) {
    const router = useRouter();       
    useEffect(()=>{        

        //Verificamos si existe un item en el localStorage
        const isLoggued = sessionStorage.getItem("loginToken");
        console.log("Hola desde verify_component: " + isLoggued);
        if(isLoggued === null){            
            //nos vamos al login
            router.push("/../admin");
        }
    },[])
    return children;
}