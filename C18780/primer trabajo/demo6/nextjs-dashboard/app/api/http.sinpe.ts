export async function useFetchSinpePurchase(uuidSales: string, confirmationNumber: string) {

    let sinpe = {
        "purchaseNumber": uuidSales,
        "confirmationNumber": confirmationNumber
    }
    try {
        const res = await fetch('https://localhost:7099/api/Sinpe', {
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
