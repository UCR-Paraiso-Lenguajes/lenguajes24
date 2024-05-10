import { useEffect, useState } from "react";

export function useFetchCategoriesList() {
    const [category, setCategory] = useState([]);
    useEffect(() => {
        async function getCategory() {
            const rest = await fetch('https://localhost:7099/api/Category');
            if(!rest.ok){
                throw new Error('Failed to fetch category.');
            }
            const data = await rest.json();
            setCategory(data);
        }
        getCategory();
    }),[];
    return category;
}