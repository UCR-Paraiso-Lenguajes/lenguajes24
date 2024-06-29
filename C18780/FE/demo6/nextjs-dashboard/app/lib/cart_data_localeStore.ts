import { Cart } from './data-definitions';

export function saveInitialCartLocalStorage(cart: Cart): void {
  const dataAsString = JSON.stringify(cart);
  localStorage.setItem('initialCart', dataAsString);
}

export function deleteCartLocalStorage(): void {
  localStorage.removeItem('initialCart');
}

export function getInitialCartLocalStorage(): Cart {
  if (typeof localStorage !== 'undefined') {
    const dataAsString = localStorage.getItem('initialCart');
    if (dataAsString) {
      return JSON.parse(dataAsString);
    }
  }
  const cart: Cart = {
    cart: {
      products: [],
      subtotal: 0,
      taxPercentage: 0,
      total: 0,
      deliveryAddress: "",
      methodPayment: null
    },
  }
  return cart;
}

