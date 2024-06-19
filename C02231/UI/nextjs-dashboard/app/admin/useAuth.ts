// useAuth.tsx
import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import {jwtDecode} from 'jwt-decode';

const useAuth = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const router = useRouter();

    const checkAuthToken = async () => {
        const loginToken = sessionStorage.getItem("authToken");

        if (!loginToken) {
            return false;
        }

        try {
            const tokenFormat: any = jwtDecode(loginToken);
            const todayDate = Math.floor(Date.now() / 1000);

            if (tokenFormat.exp && tokenFormat.exp < todayDate) {
                sessionStorage.removeItem("authToken");
                return false;
            }

            return true;
        } catch (error) {
            return false;
        }
    };

    useEffect(() => {
        const verifyToken = async () => {
            const isValid = await checkAuthToken();
            if (!isValid) {
                router.push("/../admin");
            } else {
                setIsAuthenticated(true);
            }
        };

        verifyToken();
    }, [router]);

    return isAuthenticated;
};

export default useAuth;