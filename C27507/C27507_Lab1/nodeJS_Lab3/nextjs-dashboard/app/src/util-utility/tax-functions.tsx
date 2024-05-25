//Calcular el total y manejarlo con stateUse para tenerlo en todos los componentes
export const totalPriceNoTax = (allProduct: { price: number; quantity: number; }[]) => {
    let total = 0;
    allProduct.map((item) => {                    
        total += ( item.price * item.quantity );
    });
    return total;
}
export const totalPriceTax = (allProduct: { price: number; quantity: number; }[]) => {
    let total = 0;
    allProduct.map((item) => {                                
        total += ((item.price * 0.13 + item.price) * item.quantity);
    });
    return total;
}