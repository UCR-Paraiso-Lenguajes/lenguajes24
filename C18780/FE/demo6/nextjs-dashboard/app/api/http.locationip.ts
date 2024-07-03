import { useEffect, useState } from "react";
import { LocationData } from "../utils/data-definitions";

const key = 'pk.907e2c27b4df9c13243827db78332485';
const apiUrl = 'https://us1.locationiq.com/v1/search';

export function useFetchLocation({ address }: { address: string }) {
    const [location, setLocation] = useState<LocationData[]>([]);

    useEffect(() => {
        const fetchLocation = async () => {
            try {
                const encodedAddress = encodeURIComponent(address);
                const options = {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json'
                    }
                };
                const response = await fetch(`${apiUrl}?q=${encodedAddress}&key=${key}&format=json`, options);
                const data = await response.json();
                setLocation(data);
            } catch (error) {
                console.error('Error fetching location:', error);
            }
        };

        if (address) {
            fetchLocation();
        }
    }, [address]);

    return { location };
}
