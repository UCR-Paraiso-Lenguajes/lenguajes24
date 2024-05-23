import { getInitialCartLocalStorage } from '../lib/cart_data_localeStore';


export async function useFetchCartPurchase() {
    const cart = getInitialCartLocalStorage();

    let purchase = {
        "productIds": cart.cart.products.map(product => product.uuid),
        "address": cart.cart.deliveryAddress,
        "paymentMethod": cart.cart.methodPayment
    }
    try {
        const res = await fetch('https://localhost:7099/api/Cart', {
            method: 'POST',
            body: JSON.stringify(purchase),
            headers: {
                'content-type': 'application/json'
            }
        })
        if (res.ok) {
            const data = await res.json();
            return data.purchaseNumber;
        }
    } catch (error) {
        throw new Error('Failed to fetch purchase');
    }
}


export default useFetchCartPurchase;
