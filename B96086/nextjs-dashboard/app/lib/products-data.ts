import { useEffect, useState } from 'react';
import { InitialStore } from './products-data-definitions';
import { GET, POST } from '../api/route'

export function useFetchInitialStore() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    async function getProducts() {
      try {
        const res = await fetch('https://localhost:7295/api/Store');
        if (!res.ok) {
          throw new Error('Failed to fetch products');
        }
        const data = await res.json();
        setProducts(data.products);
      } catch (error) {
        console.error(error);
      }
    }
    
    getProducts();
  }, []);

  return products;
}

function useInitialStore() {
  const products = useFetchInitialStore();

  const initialStore: InitialStore = {
    products: products,
    cart: {
      products: [],
      subtotal: 0,
      taxPercentage: 0.13,
      total: 0,
      deliveryAddress: "",
      methodPayment: null
    }
  };

  return initialStore;
}

export default useInitialStore;
