'use client'
import "bootstrap/dist/css/bootstrap.min.css";
import 'react-calendar/dist/Calendar.css';
import { useEffect, useState } from "react";
import { Alert } from "react-bootstrap";
import Calendar from "react-calendar";
import { Chart } from 'react-google-charts';
import { checkTokenDate } from "@/app/hooks/jwtHooks";
import { useRouter } from 'next/navigation';
import Products from "./products";
import Campaigns from "./campaigns/page";

export default function MainAdmin() {
    const [path, setPath] = useState<string>('/');
    const [errorMessage, setErrorMessage] = useState('');
    const router = useRouter();

    let environmentUrl = process.env.NEXT_PUBLIC_NODE_ENV;

    useEffect(() => {
        if(checkTokenStatus()){
            const handlePopState = () => {
                setPath(window.location.pathname);
            };
    
            setPath(window.location.pathname);

            window.addEventListener('popstate', handlePopState);
    
            return () => {
                window.removeEventListener('popstate', handlePopState);
            };
        }
    }, []);

    const navigateTo = (newPath: string) => {
        window.history.pushState({}, '', newPath);
        setPath(newPath);
    };

    function checkTokenStatus() {
        var token = sessionStorage.getItem("sessionToken");
        var expiracyDate = sessionStorage.getItem("expiracyToken");
        var isTokenAlive = checkTokenDate(Number(expiracyDate));
        if (!isTokenAlive || token == null) router.push("/Admin");
        else return true;
    }

    const Reports = () => {
        const [salesOfTheDayObtained, setSalesOfTheDayObtained] = useState([]);
        const [salesOfTheWeekObtained, setSalesOfTheWeekObtained] = useState([]);
        const [selectedDate, setSelectedDate] = useState(new Date());
        const salesData = [
            ['Datetime', 'Purchase Number', 'Price', 'Amount of Products', { role: 'annotation' }],
            ...salesOfTheDayObtained.map(sale => [
                parseDate(sale.saleDate),
                sale.purchaseNumber,
                sale.amount,
                sale.products.length,
                JSON.stringify(sale.products.map(product => ({
                    name: product.name,
                })))
            ])
        ];
        const weeklySalesData = [
            ['Week', 'Sales'],
            ...salesOfTheWeekObtained.map(day => [day.key, day.value])
        ];

        function onChangeDate(date: any) {
            setSelectedDate(date);
        }

        async function getData() {
            var token = sessionStorage.getItem("sessionToken");
            try {
                const res = await fetch(`${environmentUrl}/api/sale/sales?dateToFind=${selectedDate.toISOString().split('T')[0]}`,
                    {
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json',
                            'authorization': `Bearer ${token}`
                        }
                    })
                if (!res.ok) {
                    throw new Error('Failed to fetch data');
                }
                return await res.json()
            } catch (error) {
                setErrorMessage(String(error));
            }
        }

        useEffect(() => {
            if (checkTokenStatus()) {
                const fetchData = async () => {
                    try {
                        const result = await getData();
                        setSalesOfTheDayObtained(result.salesOfTheDay);
                        setSalesOfTheWeekObtained(result.salesOfTheWeek);
                    } catch (error: any) {
                        setErrorMessage(String(error))
                    }
                };
                fetchData();
            }
        }, [selectedDate]);

        function parseDate(dateString: string) {
            const parts = dateString.split('/');

            const day = parseInt(parts[0], 10);
            const month = parseInt(parts[1], 10) - 1;
            const year = parseInt(parts[2], 10);

            return new Date(year, month, day);
        }

        return <>
            <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center' }}>
                <div>
                    <div>
                        <h2>Sales Chart</h2>
                        <Chart
                            width={'700px'}
                            height={'200px'}
                            chartType="Table"
                            loader={<div className="progress">
                                <div className="progress-bar progress-bar-striped progress-bar-animated"
                                    role="progressbar" aria-valuenow={75} aria-valuemin={0}
                                    aria-valuemax={100} style={{ width: '100%' }}></div>
                            </div>}
                            data={salesData}
                            options={{
                                showRowNumber: true,
                                cssClassNames: {
                                    tableRow: 'chart-row',
                                    headerRow: 'chart-header-row',
                                    tableCell: 'chart-cell',
                                },
                                allowHtml: true,
                                page: true,
                                pagingButtons: 'both'
                            }}
                        />
                    </div>
                </div>

                <div style={{ marginLeft: '10px', marginTop: '10px' }}>
                    <Calendar onChange={onChangeDate} value={selectedDate} />
                </div>
            </div>
            <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center' }}>
                <div>
                    <div>
                        <h2>Weekly Sales Pie Chart</h2>
                        <Chart
                            width={'400px'}
                            height={'300px'}
                            chartType="PieChart"
                            loader={<div className="progress">
                                <div className="progress-bar progress-bar-striped progress-bar-animated"
                                    role="progressbar" aria-valuenow={75} aria-valuemin={0}
                                    aria-valuemax={100} style={{ width: '100%' }}></div>
                            </div>}
                            data={weeklySalesData}
                            options={{
                                title: 'Weekly Sales',
                                sliceVisibilityThreshold: 0
                            }}
                        />
                    </div>
                </div>
            </div>
        </>
    }

    const PaymentMethods = () => {
        return <>
            Soy métodos de pago
        </>
    }

    return <>
        <div className="d-flex" style={{ height: "100vh" }}>
            <div className="p-3 text-white bg-dark h-100" style={{ width: "200px", height: "100%", flex: "0 0 auto" }}>
                <a href="/" className="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none">
                    <img src="https://cdn.pixabay.com/photo/2016/03/31/19/56/avatar-1295397__340.png"
                        className="img-fluid img-thumbnail rounded-circle me-3"
                        style={{
                            height: "50px",
                            width: "50px",
                            objectFit: "cover"
                        }}
                        alt="profile" />
                    <span className="fs-4">Menú</span>
                </a>
                <ul className="list-unstyled ps-0">
                    <li className="border-top my-3"></li>
                    <li className="mb-1">
                        <button className="btn btn-toggle align-items-center rounded collapsed text-white"
                            data-bs-toggle="collapse" data-bs-target="#home-collapse" aria-expanded="true"
                            onClick={() => navigateTo('/Admin/init/products')}>
                            Productos
                        </button>
                    </li>
                    <li className="mb-1">
                        <button className="btn btn-toggle align-items-center rounded collapsed text-white"
                            data-bs-toggle="collapse" data-bs-target="#home-collapse" aria-expanded="true"
                            onClick={() => navigateTo('/Admin/init/reports')}>
                            Reporte de ventas
                        </button>
                    </li>
                    <li className="mb-1">
                        <button className="btn btn-toggle align-items-center rounded collapsed text-white"
                            data-bs-toggle="collapse" data-bs-target="#home-collapse" aria-expanded="true"
                            onClick={() => navigateTo('/Admin/init/paymentMethods')}>
                            Métodos de pago
                        </button>
                    </li>
                    <li className="mb-1">
                        <button className="btn btn-toggle align-items-center rounded collapsed text-white"
                            data-bs-toggle="collapse" data-bs-target="#home-collapse" aria-expanded="true"
                            onClick={() => navigateTo('/Admin/init/campannas')}>
                            Campañas
                        </button>
                    </li>
                </ul>
            </div>
            <div className="flex-grow-1">
            {path === '/Admin/init/products' && <Products />}
            {path === '/Admin/init/reports' && <Reports />}
            {path === '/Admin/init/payment-methods' && 'Soy métodos de pago'}
            {path === '/Admin/init/campannas' && <Campaigns />}
            </div>
            {errorMessage ?
                <div
                    style={{
                        position: 'fixed',
                        bottom: 20,
                        right: 20,
                        zIndex: 9999,
                    }}
                >
                    <Alert variant="danger" onClose={() => setErrorMessage('')} dismissible>
                        <Alert.Heading>Error</Alert.Heading>
                        <p>{errorMessage.toString()}</p>
                    </Alert>
                </div> : ''
            }
        </div>
    </>
}