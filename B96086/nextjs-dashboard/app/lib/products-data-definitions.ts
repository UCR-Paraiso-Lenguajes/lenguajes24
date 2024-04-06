import { UUID } from "crypto";

export type Product = {
    uuid: UUID;
    name: string;
    description: string;
    imageUrl: string;
    price: number;
}
export type MethodPayment = {
    method: boolean;
}

export type InitialStore = {
    products: Product[];
    cart: {
        products: Product[];
        subtotal: number;
        taxPercentage: number;
        total: number;
        deliveryAddress: string;
        methodPayment: MethodPayment | null;
    };
}