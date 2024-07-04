import React, { useEffect, useRef, useState } from 'react';
import L from 'leaflet';
import 'leaflet/dist/leaflet';
import '../styles/map.css';

const MapComponent = ({ onAddress }: { onAddress: any }) => {
    const [lat, setLatitude] = useState<number>(0);
    const [lng, setLongitude] = useState<number>(0);
    const [map, setMap] = useState<L.Map | null>(null);
    const [address, setAddress] = useState<string>('');
    const mapInit = useRef<boolean>(false);
    const markerRef = useRef<L.Marker | null>(null);

    useEffect(() => {
        navigator.geolocation.getCurrentPosition(
            (position) => {
                setLatitude(position.coords.latitude);
                setLongitude(position.coords.longitude);
            },
            (error) => {
                console.error('Error getting geolocation:', error);
            }
        );
    }, []);

    const [markerPosition, setMarkerPosition] = useState<[number, number]>([lat, lng]);

    useEffect(() => {
        if (!mapInit.current && lat !== 0 && lng !== 0) {
            mapInit.current = true;
            const newMap = L.map('map').setView([lat, lng], 15);
            setMap(newMap);

            L.tileLayer('https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png', {
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>',
                subdomains: 'abcd',
                maxZoom: 20
            }).addTo(newMap);

            const marker = L.marker([lat, lng], { draggable: true }).addTo(newMap)
                .bindPopup(`<b>¡SOY EL CENTRO!</b><br>Location: LAT: ${lat}, LNG: ${lng}`)
                .on('dragend', handleMarkerDragEnd);

            markerRef.current = marker;
            updateAddress(lat, lng);
        }
    }, [lat, lng]);

    const handleMarkerDragEnd = () => {
        if (markerRef.current) {
            const newPos = markerRef.current.getLatLng();
            setMarkerPosition([newPos.lat, newPos.lng]);
            updateAddress(newPos.lat, newPos.lng);
        }
    };

    const updateAddress = async (lat: number, lng: number) => {
        try {
            const response = await fetch(`https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}&zoom=18&addressdetails=1`);
            const data = await response.json();
            if (data && data.display_name) {
                onAddress(data.display_name);
            } else {
                onAddress('Address not found');
            }
        } catch (error) {
            console.error('Error fetching address:', error);
            onAddress('Error fetching address');
        }
    };

    return (
        <div>
            <div id="map" style={{ height: '400px' }}></div>
        </div>
    );
};

export default MapComponent;
