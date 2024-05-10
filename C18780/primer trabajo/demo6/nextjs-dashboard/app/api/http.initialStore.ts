import { useEffect, useState } from 'react';
import { getInitialCartLocalStorage, saveInitialCartLocalStorage } from '../lib/cart_data_localeStore';

export function useFetchInitialStore(category: string) {
  const [products, setProducts] = useState([]);
  const cart = getInitialCartLocalStorage();
  
  useEffect(() => {
    async function getProducts() {
        const res = await fetch(`https://localhost:7099/api/Store/category?name=${category}`);
        if (!res.ok) {
          throw new Error('Failed to fetch products.');
        }
        const data = await res.json();
        console.log(data);
        setProducts(data.products);
        cart.cart.taxPercentage = data.taxPercentage/100;

        saveInitialCartLocalStorage(cart);
        
    }
    
    getProducts();
  }, [category]);
  return products;
}

export default useFetchInitialStore;
