'use client'
import React, { useState, useEffect } from 'react';
import 'bootstrap/dist/css/bootstrap.css';
import '/app/ui/global.css';
import Link from 'next/link';
import { Chart } from 'react-google-charts';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css'
import { format } from 'date-fns';
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'next/navigation';
import useAuth from '../../useAuth';


export default function ReportPage() {
    const isAuthenticated = useAuth();
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [weeklySalesData, setWeeklySalesData] = useState([['Day', 'Total']]);
    const [dailySalesData, setDailySalesData] = useState([['Day', 'Total']]);
    const router = useRouter();
    const storage = sessionStorage.getItem("authToken");

    const URL = process.env.NEXT_PUBLIC_API_URL;
    if (!URL) {
        throw new Error('NEXT_PUBLIC_API_URL is not defined');
    }
    useEffect(() => {
        if (isAuthenticated) {
            fetchData();
        }
    }, [isAuthenticated]);

    useEffect(() => {
        fetchData();
    }, [selectedDate]);

    useEffect(() => {
        if (isAuthenticated) {
            fetchData();
        }
    }, [isAuthenticated, selectedDate]);


    const fetchData = async () => {
        try {

            const token = sessionStorage.getItem('authToken');

            const year = selectedDate.getFullYear();
            const month = String(selectedDate.getMonth() + 1).padStart(2, '0');
            const day = String(selectedDate.getDate()).padStart(2, '0');
            const formattedDate = `${year}-${month}-${day}`;


            const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/Sale?date=${formattedDate}`, {
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

            router.push("/../admin");
        }
    };


    const handleDayChange = (selectedDay: Date | null) => {
        if (selectedDay !== null) {
            const utcDate = new Date(selectedDay.toUTCString());
            const serverTimeZoneDate = new Date(selectedDay.toLocaleString('en-US', { timeZone: 'America/Costa_Rica' }));
            setSelectedDate(serverTimeZoneDate);
        }
    };

    if (!isAuthenticated) {
        return null;
    }

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
                                        allowHtml: true,
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