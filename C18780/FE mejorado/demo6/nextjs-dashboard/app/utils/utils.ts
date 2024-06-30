import { UUID } from "crypto";
import { Cart, Product } from "./data-definitions";

export const findProductsDuplicates = (products: Product[]) => {
  const duplicates: Product[] = [];
  const uniqueIds: UUID[] = [];
  products.forEach(product => {
    if (!uniqueIds.includes(product.uuid)) {
      duplicates.push(product);
      uniqueIds.push(product.uuid);
    }
  });

  return duplicates;
};

export const getProductQuantity = (product: Product, products: Product[]) => {
  return products.filter(p => p.uuid === product.uuid).length;
};

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