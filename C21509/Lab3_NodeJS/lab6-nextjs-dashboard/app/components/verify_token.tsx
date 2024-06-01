"use client";
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { jwtDecode } from 'jwt-decode';

export default function  VerifyToken({children,}: {children: React.ReactNode}) {
    const router = useRouter();       
    useEffect(()=>{        
        const loginToken = sessionStorage.getItem("loginToken");                
        if(!loginToken){
            router.push("/../admin");
            return;       
        }

        try {            
            const tokenFormat = jwtDecode(loginToken); 
            console.log(tokenFormat);           
            const todayDate = Date.now() / 1000;            
            if(tokenFormat.exp && tokenFormat.exp < todayDate){
                router.push("/../admin");
                return;            
            }
        }catch(error){
            router.push("/../admin");
            return;
        }

    },[router])
    return children;
}