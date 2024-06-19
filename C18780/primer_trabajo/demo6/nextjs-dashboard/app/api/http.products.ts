import { getCookie } from "cookies-next";
import { useEffect } from "react";
import { ProductC } from "../lib/data-definitions";

export function useFetchCreateProduct(product: ProductC | undefined) {
    useEffect(() => {
        console.log(product);
        const postProduct = async () => {
            const token = getCookie('token');
            const res = await fetch(`https://localhost:7099/api/Product`, {
                method: 'POST',
                body: JSON.stringify(product),
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                }
            });
            if (!res.ok) {
                throw new Error('Failed to fetch Create Product.');
            }
        }

        if (product !== undefined) {
            postProduct();
        }
    }, [product]);
}