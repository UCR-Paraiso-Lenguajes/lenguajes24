import { getInitialCartLocalStorage } from "../utils/utils";

let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export async function useFetchCartPurchase() {
    const cart = getInitialCartLocalStorage();

    let purchase = {
        "productIds": cart.cart.products.map((product: { uuid: any; }) => product.uuid),
        "address": cart.cart.deliveryAddress,
        "paymentMethod": cart.cart.methodPayment
    }
    try {
        const res = await fetch(`${environmentUrl}/api/Cart`, {
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
