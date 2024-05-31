'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '/app/ui/global.css';
import Link from 'next/link';
import { Chart } from 'react-google-charts';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import {jwtDecode} from 'jwt-decode';
import router, { useRouter } from 'next/router';

export default function ReportPage() {

    const [selectedDate, setSelectedDate] = useState(new Date());
    const [weeklySalesData, setWeeklySalesData] = useState([['Day', 'Total']]);
    const [dailySalesData, setDailySalesData] = useState([['Day', 'Total']]);
    const URL = process.env.NEXT_PUBLIC_NODE_ENV;
    //const URL = "http://localhost:5207";
    
    useEffect(() => {
        fetchData();
    }, [selectedDate]);

    const isTokenExpired = (token: string) => {
        try {
            const decodedToken = jwtDecode<{ exp: number }>(token);
            const currentTime = Math.floor(Date.now() / 1000);
            return decodedToken.exp < currentTime;
        } catch (error) {
            throw new Error('Error decoding token:');
        }
    };
    const fetchData = async () => {
        try {
            //debugger
            const token = sessionStorage.getItem('authToken');
            console.log(token);
            if (!token) {
                throw new Error('No se encontró el token en el session storage');
            }

           /* if (isTokenExpired(token)) {
                sessionStorage.clear();
                router.push('/admin');
                return;
            }*/

            //const formattedDate = selectedDate.toISOString().split('T')[0]; //fecha en formato ISO 8601 sin la hora
            const year = selectedDate.getFullYear();
            const month = String(selectedDate.getMonth() + 1).padStart(2, '0');
            const day = String(selectedDate.getDate()).padStart(2, '0');
            const formattedDate = `${year}-${month}-${day}`;
           // const response = await fetch(`http://localhost:5207/api/Sale?date=${formattedDate}`);
            const url = URL+'/api/Sale?date=${formattedDate}'

            const response = await fetch(URL + `/api/Sale?date=${formattedDate}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                }
            });


            if (!response.ok) {
                throw new Error('Failed to fetch data');
            }
            const data = await response.json();

            const weeklyData = [['Day', 'Total']];
            const dailyData = [['Purcharse Date', 'Purcharse Number', 'Quantity', 'Total', 'Products']];

            for (const item of data.weeklySales) {
                weeklyData.push([item.dayOfWeek, item.total]);
            }

            for (const item of data.dailySales) {
                dailyData.push([item.purchaseDate, item.purchaseNumber, item.quantity, item.total, item.products]);
                dailyData.push([item.purchaseDate, item.purchaseNumber, item.quantity, item.total, item.products]);
            }


            setDailySalesData(dailyData)
            setWeeklySalesData(weeklyData);

        } catch (error) {
            if (error.message.includes('token')) {
                sessionStorage.clear();
                await router.push('/admin');
            } else {
                throw new Error('Error al obtener los datos');
            }
        }
    };


    const handleDayChange = (selectedDay: Date | null) => {
        if (selectedDay !== null) {
            const utcDate = new Date(selectedDay.toUTCString());
            const serverTimeZoneDate = new Date(selectedDay.toLocaleString('en-US', { timeZone: 'America/Costa_Rica' }));
            setSelectedDate(serverTimeZoneDate);
        }
    };


    return (
        <div>
            <header className="p-3 text-bg-dark">
                <div className="row" style={{ color: 'gray' }}>
                    <div className="col-sm-3">
                        <h1 style={{ color: 'white' }}>Reports</h1>
                    </div>
                    <div className="col-sm-9 d-flex justify-content-end align-items-center">
                        <Link href="/admin/init">
                            <button className="btn btn-dark">Go Back</button>
                        </Link>
                    </div>
                </div>
            </header>

            <div style={{ marginLeft: '50px' }}>
                <label style={{ margin: '10px' }}>Select a Day</label>
                <DatePicker
                    selected={selectedDate}
                    onChange={handleDayChange}
                    onKeyDown={(e) => e.preventDefault()}
                />
            </div>

            <div className="container">
                <div className="row">
                    <div className="col-md-8">
                        <div style={{ display: 'flex' }}>
                            <div>
                                <h2>Sales Chart</h2>
                                <Chart
                                    width={'100%'}
                                    height={'300px'}
                                    chartType="Table"
                                    loader={<div>Loading Chart</div>}
                                    data={dailySalesData}
                                    options={{
                                        showRowNumber: true,
                                        cssClassNames: {
                                            tableRow: 'chart-row',
                                            headerRow: 'chart-header-row',
                                            tableCell: 'chart-cell',
                                        },
                                        allowHtml: true, // Allows HTML content in cells
                                        pageSize: 20,
                                    }}
                                />
                            </div>
                        </div>
                    </div>
                    <div className="col-md-4">
                        <div style={{ display: 'flex', flexDirection: 'column' }}>
                            <h2>Weekly Sales Pie Chart</h2>
                            <Chart
                                width={'100%'}
                                height={'300px'}
                                chartType="PieChart"
                                loader={<div>Loading Chart</div>}
                                data={weeklySalesData}
                                options={{
                                    title: 'Weekly Sales',
                                }}
                            />
                        </div>
                    </div>
                </div>
            </div>

            <footer className='footer' style={{ position: 'fixed', bottom: '0', width: '100%', zIndex: '9999' }}>
                <div className="text-center p-3">
                    <h5 className="text-light">Paula's Library</h5>
                </div>
            </footer>
        </div>
    );
}