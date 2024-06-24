import { getCookie } from "cookies-next";
import { useEffect } from "react";

let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export function useFetctCreateAd(message: string) {
    useEffect(() => {
        const postAd = async () => {
            const token = getCookie('token');
            const res = await fetch(`${environmentUrl}/api/Ad`, {
                method: 'POST',
                body: JSON.stringify(message),
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                }
            });
            if (!res.ok) {
                throw new Error('Failed to fetch Create Ad.');
            }
        }
        if (message !== undefined) {
            postAd();
        }
    }, [message]);
}