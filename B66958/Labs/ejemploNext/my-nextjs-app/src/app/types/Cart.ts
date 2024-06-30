export interface CartItem {
    uuid: string;
    quantity: number;
    name: string;
    imageUrl: string;
    price: number;
    description: string;
}
  
export interface Carrito {
    productos: CartItem[];
    subtotal: number;
    porcentajeImpuesto: number;
    total: number;
    direccionEntrega: string;
    metodoDePago: number;
}
  
export interface CartState {
    carrito: Carrito;
    metodosDePago: any[];
    necesitaVerificacion: boolean;
}  