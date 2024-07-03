import { useEffect, useState } from 'react';
import { getInitialCartLocalStorage, getPaymentMethodLocalStorage, saveInitialCartLocalStorage, savePaymentMethodLocalStorage } from '../utils/utils';
import { getCookie } from 'cookies-next';
import { PaymentMethod } from '../utils/data-definitions';
import * as signalR from '@microsoft/signalr';

let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export function useFetchInitialStore({ category, search }: { category: string[], search: string }) {
  const [products, setProducts] = useState([]);
  const cart = getInitialCartLocalStorage();
  let paymentMethod = getPaymentMethodLocalStorage();

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
      paymentMethod = data.paymentMethods;
      savePaymentMethodLocalStorage(paymentMethod);
      saveInitialCartLocalStorage(cart);
    }

    getProducts();
  }, [category, search]);
  return products;
}

export function useFetchGetPaymentMethods() {
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
  useEffect(() => {
    const getPaymentMethods = async () => {
      const token = getCookie('token');
      const res = await fetch(`${environmentUrl}/api/Store/PaymentMethods`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        }
      });
      if (!res.ok) {
        throw new Error('Failed to fetch payment methods.');
      }
      const data = await res.json();
      setPaymentMethods(data);
    }
    getPaymentMethods();
  }, []);
  return paymentMethods;
}

export function useFetchUpdatePaymentIsEnabled(paymentMethod: number | null) {
  useEffect(() => {
    if (paymentMethod !== null) {
      const updatePaymentIsEnabled = async () => {
        const token = getCookie('token');
        const res = await fetch(`${environmentUrl}/api/Store/UpdatePaymentEnabled`, {
          method: 'POST',
          body: JSON.stringify(paymentMethod),
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
          }
        });
        if (!res.ok) {
          throw new Error('Failed to fetch update payment method.');
        }
      }
      updatePaymentIsEnabled();
    }
  }, [paymentMethod]);
}

export function useSignalRGetPaymentMethods() {
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${environmentUrl}/paymentMethodsHub`)
      .build();

    connection.on("PaymentMethodsChange", (methods: PaymentMethod[]) => {
      setPaymentMethods(methods);
    });

    connection.start()
      .then(() => console.log('SignalR Connected'))
      .catch(err => console.error('SignalR Connection Error: ', err));

    return () => {
      connection.stop()
        .then(() => console.log('SignalR Disconnected'))
        .catch(err => console.error('SignalR Disconnection Error: ', err));
    };
  }, []);

  return { paymentMethods };
}
