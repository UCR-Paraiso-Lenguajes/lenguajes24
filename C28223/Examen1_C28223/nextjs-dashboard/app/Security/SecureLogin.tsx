"use client";
import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { jwtDecode } from 'jwt-decode';

export default function ValidationLoginSession({ children, }: { children: React.ReactNode }) {
    const router = useRouter();
    useEffect(() => {
        const tokenFromStorage = sessionStorage.getItem("token");
        if (!tokenFromStorage) { //si no hay token entonces lo manda a logearse
           router.push("/../Admin");
            return;
        }
        try {
            var decodedTokenStorage = jwtDecode(tokenFromStorage);
            var nowTimeComparation = Date.now() / 1000;// reinicio de sesion si el token expira
            if (decodedTokenStorage.exp < nowTimeComparation) {
                sessionStorage.removeItem("token");
                router.push("/../Admin");
                return;
            }
        } catch (error) {
            sessionStorage.removeItem("token");
            router.push("/../Admin");
            return;
        }
    }, [router]);
    
    return children;
}