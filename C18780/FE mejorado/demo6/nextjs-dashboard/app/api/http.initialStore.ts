import { useEffect, useState } from 'react';
import { getInitialCartLocalStorage, saveInitialCartLocalStorage } from '../utils/utils';

let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export function useFetchInitialStore({ category, search }: { category: string[], search: string }) {
  const [products, setProducts] = useState([]);
  const cart = getInitialCartLocalStorage();

  useEffect(() => {
    async function getProducts() {
      const categoryParams = category.map(c => `category=${encodeURIComponent(c)}`).join('&');
      const url = `${environmentUrl}/api/Store/Products?${categoryParams}&search=${encodeURIComponent(search)}`;

      const res = await fetch(url);
      if (!res.ok) {
        throw new Error('Failed to fetch products.');
      }
      const data = await res.json();
      setProducts(data.products);
      cart.cart.taxPercentage = data.taxPercentage / 100;

      saveInitialCartLocalStorage(cart);
    }

    getProducts();
  }, [category, search]);
  return products;
}

export default useFetchInitialStore;

