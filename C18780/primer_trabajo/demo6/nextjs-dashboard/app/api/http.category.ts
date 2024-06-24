import { useEffect, useState } from "react";

let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export function useFetchCategoriesList() {
    const [category, setCategory] = useState([]);
    useEffect(() => {
        async function getCategory() {
            const rest = await fetch(`${environmentUrl}/api/Category`);
            if(!rest.ok){
                throw new Error('Failed to fetch category.');
            }
            const data = await rest.json();
            setCategory(data);
        }
        getCategory();
    },[]);
    return category;
}