import { getCookie } from "cookies-next";
import { UUID } from "crypto";
import { useEffect, useState } from "react";
import { Ad } from "../lib/data-definitions";
import * as signalR from '@microsoft/signalr';

let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV || 'https://localhost:7099';

export function useFetchDeleteAd(id: UUID | null) {
    useEffect(() => {
        if (id) {
            const deleteAd = async () => {
                const token = getCookie('token');
                const res = await fetch(`${environmentUrl}/api/Ad/?uuid=${id}`, {
                    method: 'DELETE',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    }
                });
                if (!res.ok) {
                    throw new Error('Failed to fetch Delete Ad.');
                }
            }
            deleteAd();
        }
    }, [id]);

}

export function useFetchGetAd() {
    const [message, setMessage] = useState<Ad[]>([]);
    useEffect(() => {
        const getAd = async () => {
            const token = getCookie('token');
            const res = await fetch(`${environmentUrl}/api/Ad`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                }
            });
            if (!res.ok) {
                throw new Error('Failed to fetch Get Ad.');
            }
            const data = await res.json();
            setMessage(data);
        }
        getAd();
    }, []);
    return { message };
}

export function useFetchCreateAd(message: string) {
    useEffect(() => {
        if (message.trim()) {
            const postAd = async () => {
                const token = getCookie('token');
                const res = await fetch(`${environmentUrl}/api/Ad`, {
                    method: 'POST',
                    body: JSON.stringify(message),
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    }
                });
                if (!res.ok) {
                    throw new Error('Failed to fetch Create Ad.');
                }
            }
            if (message !== undefined) {
                postAd();
            }
        }
    }, [message]);
}

export function useSignalRGetAds() {
    const [message, setMessage] = useState<Ad[]>([]);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${environmentUrl}/hub`)
            .build();

        connection.on("AdCreated", (ad: Ad) => {
            setMessage(prevMessages => [...prevMessages, ad]);
        });

        connection.start()
            .then(() => console.log("SignalR Connected"))
            .catch(err => console.error("Error while establishing connection: ", err));

        return () => {
            connection.stop()
                .then(() => console.log("SignalR Disconnected"))
                .catch(err => console.error("Error while disconnecting: ", err));
        };
    }, []);

    return { message };
}