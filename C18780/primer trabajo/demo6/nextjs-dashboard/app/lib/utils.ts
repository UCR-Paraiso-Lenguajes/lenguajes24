import { UUID } from "crypto";
import { Product } from "./data-definitions";

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