export interface CartItem {
    id: string;
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