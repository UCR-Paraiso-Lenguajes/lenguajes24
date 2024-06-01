"use client"; // Para utilizar el cliente en lugar del servidor
import { useState, useEffect } from 'react';
import Link from 'next/link';
import { Chart } from 'react-google-charts';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'next/navigation';

export default function Init() {
    const router = useRouter();
    const [selectedOption, setSelectedOption] = useState(null);
    const [selectedDay, setSelectedDay] = useState(new Date());
    const [weeklySalesData, setWeeklySalesData] = useState([['Day', 'Total']]);
    const [dailySalesData, setDailySalesData] = useState([['Purchase Date', 'Purchase Number', 'Total']]);

    const token = typeof window !== 'undefined' ? sessionStorage.getItem('token') : null;
    let userRole = null;
    if (token) {
        const decodedToken = jwtDecode(token);
        userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }
    
    useEffect(() => {
        fetchData();
    }, [selectedDay]);

    const checkTokenValidity = () => {
        if (token) {
            const decodedToken = jwtDecode(token);
            const exp = decodedToken.exp * 1000;

            if (Date.now() >= exp || userRole !== "Admin") {
                sessionStorage.removeItem('token');
                router.push('/admin');
            }
        } else {
            router.push('/admin');
        }
    };
    
    useEffect(() => {
        checkTokenValidity();
        const interval = setInterval(checkTokenValidity, 10000);
    
        return () => clearInterval(interval);
    }, []);

    const fetchData = async () => {
        if (token) {
            if (userRole === "Admin") {
                try {
                    const formattedDate = selectedDay.toISOString().split('T')[0];
                    const response = await fetch(`https://localhost:7067/api/sale?date=${formattedDate}`, {
                        headers: {
                            'Authorization': `Bearer ${token}`
                        }
                    });

                    if (!response.ok) {
                        throw new Error('Failed to fetch data');
                    }

                    const data = await response.json();

                    const newWeeklyData = [['Day', 'Total']];
                    for (const item of data.weeklyReport) {
                        newWeeklyData.push([item.day, item.total]);
                    }
                    setWeeklySalesData(newWeeklyData);

                    const newDailyData = [['Purchase Date', 'Purchase Number', 'Total']];
                    for (const item of data.dailyReport) {
                        newDailyData.push([item.purchaseDate, item.purchaseNumber, item.total]);
                    }
                    setDailySalesData(newDailyData);
                } catch (error) {
                    throw new Error("Error loading data.");
                }
            } else {
                router.push('/admin');
            }
        } else {
            router.push('/admin');
        }
    };

    const handleDayChange = (date) => {
        if (!date || !(date instanceof Date)) {
            throw new Error("Invalid date argument");
        }
        const adjustedDay = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
        setSelectedDay(adjustedDay);
    };

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Tienda</h1>
                </Link>
            </div>

            <div className="body">
                <div className="sidebar">
                    <h2>Menu</h2>
                    <div>
                        <button className="Button" onClick={() => setSelectedOption("Products")}>Products</button>
                        <button className="Button" onClick={() => setSelectedOption("Reports")}>Reports</button>
                    </div>
                </div>

                <div className="content">
                    {selectedOption === "Products" && (
                        <div>
                            <h2>Products</h2>
                        </div>
                    )}
                    {selectedOption === "Reports" && (
                        <div>
                            <h2>Reports</h2>
                            <DatePicker
                                label="Select a day"
                                selected={selectedDay}
                                onChange={handleDayChange}
                                renderInput={(params) => <input {...params} />}
                            />

                            <div className="chartContainer">
                                <Chart
                                    width={"100%"}
                                    height={"300px"}
                                    chartType="Table"
                                    loader={<div>Loading Chart</div>}
                                    data={dailySalesData}
                                    options={{
                                        title: "Daily Sales"
                                    }}
                                />

                                <Chart
                                    width={"100%"}
                                    height={"300px"}
                                    chartType="PieChart"
                                    loader={<div>Loading Chart</div>}
                                    data={weeklySalesData}
                                    options={{
                                        title: "Weekly Sales",
                                    }}
                                />
                            </div>
                        </div>
                    )}
                </div>
            </div>

            <div className="footer">
                <h2>Tienda.com</h2>
            </div>
        </div>
    );
}