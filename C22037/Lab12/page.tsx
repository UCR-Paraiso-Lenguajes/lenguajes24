"use client"; // Para utilizar el cliente en lugar del servidor
import { useState, useEffect } from 'react';
import Link from 'next/link';
import { Chart } from 'react-google-charts';
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'next/navigation';

interface CustomJwtPayload {
    exp: number;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: string;
}

interface Product {
    id: number;
    name: string;
    price: number;
}

export default function Init() {
    const router = useRouter();
    const [selectedOption, setSelectedOption] = useState('');
    const [selectedDay, setSelectedDay] = useState(new Date());
    const [weeklySalesData, setWeeklySalesData] = useState([['Day', 'Total']]);
    const [dailySalesData, setDailySalesData] = useState([['Purchase Date', 'Purchase Number', 'Total']]);
    const [isVerified, setIsVerified] = useState(false);
    const [products, setProducts] = useState<Product[]>([]);
    const [showDeleteProducts, setShowDeleteProducts] = useState(false);
    const [showInsertProducts, setShowInsertProducts] = useState(false);
    const [newProduct, setNewProduct] = useState({ name: '', description: '', price: '', imageUrl: '' });

    const token = typeof window !== 'undefined' ? sessionStorage.getItem('token') : null;
    let userRole = null;
    if (token) {
        const decodedToken = jwtDecode<CustomJwtPayload>(token);
        userRole = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    }

    const checkTokenValidity = () => {
        if (!token) {
            router.push('/admin');
            return;
        }

        const decodedToken = jwtDecode<CustomJwtPayload>(token);
        const exp = decodedToken.exp * 1000;

        if (Date.now() >= exp || userRole !== "Admin") {
            sessionStorage.removeItem('token');
            router.push('/admin');
        } else {
            setIsVerified(true);
        }
    };

    useEffect(() => {
        checkTokenValidity();
        const interval = setInterval(checkTokenValidity, 10000);
    
        return () => clearInterval(interval);
    }, [token]);

    useEffect(() => {
        fetchData();
    }, [selectedDay]);

    const fetchData = async () => {
        if (!token || userRole !== "Admin") {
            router.push('/admin');
            return;
        }

        try {
            const formattedDate = selectedDay.toISOString().split('T')[0];
            const response = await fetch(`https://localhost:7067/api/sale?date=${formattedDate}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error('Failed to fetch data.');
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
    };

    const fetchProducts = async () => {
        if (!token || userRole !== "Admin") {
            router.push('/admin');
            return;
        }

        try {
            const response = await fetch('https://localhost:7067/api/store/products', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error('Failed to fetch products.');
            }

            const data = await response.json();
            setProducts(data.products);
        } catch (error) {
            throw new Error("Error loading products.");
        }
    };

    useEffect(() => {
        if (showDeleteProducts) {
            fetchProducts();
        }
    }, [showDeleteProducts]);

    const handleDayChange = (date: Date | null) => {
        if (!date) {
            throw new Error("Invalid date argument.");
        }
        const adjustedDay = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
        setSelectedDay(adjustedDay);
    };

    const handleDeleteProduct = async (productId: number) => {
        if (!confirm('Are you sure you want to delete this product?')) {
            return;
        }

        try {
            const response = await fetch(`https://localhost:7067/api/store/products/${productId}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error('Failed to delete product.');
            }

            setProducts(products.filter(product => product.id !== productId));
        } catch (error) {
            throw new Error("Error deleting product.");
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setNewProduct({ ...newProduct, [name]: value });
    };

    const handleInsertProduct = async () => {
        if (!token || userRole !== "Admin") {
            router.push('/admin');
            return;
        }

        try {
            const response = await fetch('https://localhost:7067/api/store/products', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(newProduct)
            });

            if (!response.ok) {
                throw new Error('Failed to insert product.');
            }

            const data = await response.json();
            setProducts([...products, data.product]);
            setNewProduct({ name: '', description: '', price: '', imageUrl: '' });
        } catch (error) {
            throw new Error("Error inserting product.");
        }
    };

    if (!isVerified) {
        return null;
    }

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
                        <button className="Button" onClick={() => { setSelectedOption("Products"); setShowDeleteProducts(false); setShowInsertProducts(false); }}>Products</button>
                        <button className="Button" onClick={() => setSelectedOption("Reports")}>Reports</button>
                    </div>
                </div>

                <div className="content">
                    {selectedOption === "Products" && (
                        <div>
                            <h2>Products</h2>
                            <div>
                                <button className="Button" onClick={() => { setShowDeleteProducts(true); setShowInsertProducts(false); }}>Delete Products</button>
                                <button className="Button" onClick={() => { setShowInsertProducts(true); setShowDeleteProducts(false); }}>Insert Products</button>
                            </div>
                            {showDeleteProducts && (
                                <div>
                                    {products.map(product => (
                                        <div key={product.id}>
                                            <span>{product.name} - ${product.price}</span>
                                            <button onClick={() => handleDeleteProduct(product.id)}>Delete</button>
                                        </div>
                                    ))}
                                </div>
                            )}
                            {showInsertProducts && (
                                <div>
                                    <h3>Insert New Product</h3>
                                    <div>
                                        <input
                                            type="text"
                                            name="name"
                                            placeholder="Name"
                                            value={newProduct.name}
                                            onChange={handleInputChange}
                                        />
                                    </div>
                                    <div>
                                        <textarea
                                            name="description"
                                            placeholder="Description"
                                            value={newProduct.description}
                                            onChange={handleInputChange}
                                        />
                                    </div>
                                    <div>
                                        <input
                                            type="text"
                                            name="price"
                                            placeholder="Price"
                                            value={newProduct.price}
                                            onChange={handleInputChange}
                                        />
                                    </div>
                                    <div>
                                        <input
                                            type="text"
                                            name="imageUrl"
                                            placeholder="Image URL"
                                            value={newProduct.imageUrl}
                                            onChange={handleInputChange}
                                        />
                                    </div>
                                    <button className="Button" onClick={handleInsertProduct}>Submit</button>
                                </div>
                            )}
                        </div>
                    )}
                    {selectedOption === "Reports" && (
                        <div>
                            <h2>Reports</h2>
                            <h3>Select a day</h3>
                            <DatePicker
                                selected={selectedDay}
                                onChange={handleDayChange}
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