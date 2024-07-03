import { UUID } from "crypto";

export type Product = {
    uuid: UUID;
    name: string;
    description: string;
    imageUrl: string;
    price: number;
}

export type Ad = {
    uuid: UUID;
    date: Date;
    message: string;
}

export type PaymentMethod = {
    paymentType: number;
    name: string;
    isEnabled: boolean;
}

export type MethodPayment = {
    method: boolean;
}

export type InitialStore = {
    products: Product[];
    cart: Cart;
    paymentMethods: PaymentMethod[];
}

export type Cart = {
    cart: {
        products: Product[];
        subtotal: number;
        taxPercentage: number;
        total: number;
        deliveryAddress: string;
        methodPayment: number | null;
    };
}

export type Category = {
    uuid: UUID;
    name: string;
}

export type ProductC = {
    name: string,
    imageUrl: string,
    price: number,
    description: string,
    category: string
}

export type LocationData = {

    boundingbox: [string, string, string, string];
    class: string;
    display_name: string;
    icon: string;
    importance: number;
    lat: string;
    licence: string;
    lon: string;
    osm_id: string;
    osm_type: string;
    place_id: string;
    type: string;

}
