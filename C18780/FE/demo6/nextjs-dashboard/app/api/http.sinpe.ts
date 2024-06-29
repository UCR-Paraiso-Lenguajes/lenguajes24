let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export async function useFetchSinpePurchase(uuidSales: string, confirmationNumber: string) {

    let sinpe = {
        "purchaseNumber": uuidSales,
        "confirmationNumber": confirmationNumber
    }
    try {
        const res = await fetch(`${environmentUrl}/api/Sinpe`, {
            method: 'POST',
            body: JSON.stringify(sinpe),
            headers: {
                'content-type': 'application/json'
            }
        })
        if (res.ok) {
            const data = await res.json();
            return data;
        }
    } catch (error) {
        throw new Error('Failed to fetch purchase');
    }
}


export default useFetchSinpePurchase;
