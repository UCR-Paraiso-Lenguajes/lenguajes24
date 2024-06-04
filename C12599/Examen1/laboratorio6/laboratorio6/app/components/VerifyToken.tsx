"use client";
import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { jwtDecode } from 'jwt-decode';

export default function VerifyComponent({ children }: { children: React.ReactNode }) {
    const router = useRouter();

    useEffect(() => {
        const loginToken = sessionStorage.getItem("authToken");
        
        if (!loginToken) {
            router.push("/../admin");
            return;
        }

        const tokenFormat = jwtDecode(loginToken);
        const todayDate = Date.now() / 1000;

        if (tokenFormat.exp && tokenFormat.exp < todayDate) {
            sessionStorage.removeItem("authToken");
            router.push("/../admin");
            return;
        }
        
    }, [router]);

    return <>{children}</>;
}