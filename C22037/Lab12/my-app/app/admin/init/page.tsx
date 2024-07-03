"use client";
import { useState, useEffect } from 'react';
import Link from 'next/link';
import { Chart } from 'react-google-charts';
import DatePicker from "react-datepicker";
import "../../../public/styles.css";
import "react-datepicker/dist/react-datepicker.css";
import { jwtDecode } from 'jwt-decode';
import { useRouter } from 'next/navigation';
import DataTable from 'react-data-table-component';
import { HubConnectionBuilder } from '@microsoft/signalr';

interface CustomJwtPayload {
    exp: number;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: string;
}

interface Category {
    id: number;
    name: string;
}

interface Product {
    id: number;
    name: string;
    description: string;
    price: number;
    imageUrl: string;
    category: Category;
}

interface Message {
    id: number;
    content: string;
    timestamp: string;
}

export default function Init() {
    const router = useRouter();
    const [selectedOption, setSelectedOption] = useState('');
    const [selectedDay, setSelectedDay] = useState(new Date());
    const [weeklySalesData, setWeeklySalesData] = useState([['Day', 'Total']]);
    const [dailySalesData, setDailySalesData] = useState([['Purchase Date', 'Purchase Number', 'Total']]);
    const [isVerified, setIsVerified] = useState(false);
    const [products, setProducts] = useState<Product[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);
    const [showDeleteProducts, setShowDeleteProducts] = useState(false);
    const [showInsertProducts, setShowInsertProducts] = useState(false);
    const [newProduct, setNewProduct] = useState({
        name: '',
        description: '',
        price: '',
        imageUrl: '',
        categoryId: ''
    });
    const [messages, setMessages] = useState<Message[]>([]);
    const [newMessage, setNewMessage] = useState('');
    const URL = process.env.NEXT_PUBLIC_API_URL;
    const [errors, setErrors] = useState<string[]>([]);
    const [connection, setConnection] = useState(null);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(URL + "/campaignHub")
            .withAutomaticReconnect()
            .build();

        newConnection.start();

        newConnection.on("ReceiveAllMessagesAsync", (messages) => {
            setMessages(messages);
        });

        setConnection(newConnection);

        return () => {
            newConnection.stop();
        };
    }, []);

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
        fetchCategories();
    }, [selectedDay]);

    const fetchData = async () => {
        if (!token || userRole !== "Admin") {
            router.push('/admin');
            return;
        }

        try {
            const formattedDate = selectedDay.toISOString().split('T')[0];
            const response = await fetch(URL + `/api/sale?date=${formattedDate}`, {
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

    const fetchCategories = async () => {
        try {
            const response = await fetch(URL + '/api/store/categories', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error('Failed to fetch categories.');
            }

            const data = await response.json();
            setCategories(data.categories);
            localStorage.setItem('categories', JSON.stringify(data.categories));
        } catch (error) {
            throw new Error("Error loading categories.");
        }
    };

    const fetchProducts = async (categories = "null") => {
        if (!token || userRole !== "Admin") {
            router.push('/admin');
            return;
        }

        try {
            const response = await fetch(URL + `/api/store/products?categories=${categories}`, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error('Failed to fetch products.');
            }

            const data = await response.json();
            setProducts(data.products);
            localStorage.setItem('productList', JSON.stringify(data.products));
        } catch (error) {
            throw new Error("Error loading products.");
        }
    };

    const handleSendMessage = async () => {
        if (!newMessage.trim() || newMessage.length > 5000) {
            return;
        }

        try {
            if (connection) {
                await connection.invoke("SendMessageAsync", newMessage);
                setNewMessage('');
            }
        } catch (error) {
            throw new Error('Error sending message.');
        }
    };

    const handleDeleteMessage = async (messageId: number) => {
        if (!confirm('Are you sure you want to delete this message?')) {
            return;
        }

        try {
            if (connection) {
                await connection.invoke("DeleteMessageAsync", messageId);
            }
        } catch (error) {
            throw new Error("Error deleting message.");
        }
    };

    useEffect(() => {
        if (showDeleteProducts) {
            fetchProducts();
        }
    }, [showDeleteProducts]);

    const handleDayChange = (date: Date | null) => {
        if (!date) {
            return;
        }
        const adjustedDay = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
        setSelectedDay(adjustedDay);
    };

    const handleDeleteProduct = async (productId: number) => {
        if (!confirm('Are you sure you want to delete this product?')) {
            return;
        }

        try {
            const response = await fetch(URL + `/api/store/products/${productId}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });

            if (!response.ok) {
                throw new Error('Failed to delete product.');
            }

            const updatedProducts = products.filter(product => product.id !== productId);
            setProducts(updatedProducts);
            localStorage.setItem('productList', JSON.stringify(updatedProducts));
        } catch (error) {
            throw new Error("Error deleting product.");
        }
    };

    const handleCategoryChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const { value } = e.target;
        const category = categories.find(cat => cat.id.toString() === value);
        if (category) {
            setNewProduct({ ...newProduct, categoryId: category.id.toString() });
        }
    };

    const validateProductData = () => {
        const errors: string[] = [];

        if (!newProduct.name.trim()) {
            errors.push("Name is required.");
        }

        if (!newProduct.description.trim()) {
            errors.push("Description is required.");
        }

        if (!newProduct.price.trim() || isNaN(Number(newProduct.price)) || Number(newProduct.price) <= 0) {
            errors.push("Price must be a positive number.");
        } else if (Number(newProduct.price) > 1000000) {
            errors.push("Price cannot exceed 1,000,000.");
        }

        if (!newProduct.imageUrl.trim()) {
            errors.push("Image URL is required.");
        }

        if (!newProduct.categoryId.trim()) {
            errors.push("Category is required.");
        }

        setErrors(errors);
        return errors.length === 0;
    };

    const handleAddProduct = async () => {
        if (!validateProductData()) {
            return;
        }

        const productData = {
            name: newProduct.name,
            description: newProduct.description,
            price: parseFloat(newProduct.price),
            imageUrl: newProduct.imageUrl,
            category: parseInt(newProduct.categoryId)
        };

        try {
            const response = await fetch(URL + '/api/product/store/add', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(productData)
            });

            if (!response.ok) {
                throw new Error('Failed to add product.');
            }

            const data = await response.json();

            const updatedProducts = data.products;

            const storeData = JSON.parse(localStorage.getItem('store') || '{}');
            storeData.products = updatedProducts;
            localStorage.setItem('store', JSON.stringify(storeData));
            setProducts(updatedProducts);
            setNewProduct({
                name: '',
                description: '',
                price: '',
                imageUrl: '',
                categoryId: ''
            });
        } catch (error) {
            throw new Error('Error adding product.');
        }
    };

    const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setNewProduct({ ...newProduct, name: e.target.value });
    };

    const handleDescriptionChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
        setNewProduct({ ...newProduct, description: e.target.value });
    };

    const handlePriceChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setNewProduct({ ...newProduct, price: e.target.value });
    };

    const handleImageUrlChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setNewProduct({ ...newProduct, imageUrl: e.target.value });
    };

    const columns = [
        {
            name: 'Name',
            selector: (row: Product) => row.name,
            sortable: true,
        },
        {
            name: 'Description',
            selector: (row: Product) => <div dangerouslySetInnerHTML={{ __html: row.description }} />,
            sortable: true,
        },
        {
            name: 'Price',
            selector: (row: Product) => row.price,
            sortable: true,
        },
        {
            name: 'Category',
            selector: (row: Product) => row.category.name,
            sortable: true,
        },
        {
            cell: (row: Product) => <button onClick={() => handleDeleteProduct(row.id)}>Delete</button>,
            ignoreRowClick: true,
            allowOverflow: true,
            button: true,
        },
    ];

    if (!isVerified) {
        return null;
    }

    return (
        <div>
            <div className="header">
                <Link href="/">
                    <h1>Store</h1>
                </Link>
            </div>

            <div className="body">
                <div className="sidebar">
                    <h2>Menu</h2>
                    <div>
                        <button className="Button" onClick={() => { setSelectedOption("Products"); setShowDeleteProducts(false); setShowInsertProducts(false); }}>Products</button>
                        <button className="Button" onClick={() => setSelectedOption("Reports")}>Reports</button>
                        <button className="Button" onClick={() => setSelectedOption("Campaigns")}>Campaigns</button>
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
                                <DataTable
                                    columns={columns}
                                    data={products}
                                    pagination
                                />
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
                                            onChange={handleNameChange}
                                        />
                                    </div>
                                    <div>
                                        <textarea
                                            name="description"
                                            placeholder="Description"
                                            value={newProduct.description}
                                            onChange={handleDescriptionChange}
                                        />
                                    </div>
                                    <div>
                                        <input
                                            type="text"
                                            name="price"
                                            placeholder="Price"
                                            value={newProduct.price}
                                            onChange={handlePriceChange}
                                        />
                                    </div>
                                    <div>
                                        <input
                                            type="text"
                                            name="imageUrl"
                                            placeholder="Image URL"
                                            value={newProduct.imageUrl}
                                            onChange={handleImageUrlChange}
                                        />
                                    </div>
                                    <div>
                                        <select
                                            name="categoryId"
                                            value={newProduct.categoryId}
                                            onChange={handleCategoryChange}
                                        >
                                            <option value="">Select Category</option>
                                            {categories.map(category => (
                                                <option key={category.id} value={category.id}>
                                                    {category.name}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                    {errors.length > 0 && (
                                        <div className="error-messages">
                                            <ul>
                                                {errors.map((error, index) => (
                                                    <li key={index}>{error}</li>
                                                ))}
                                            </ul>
                                        </div>
                                    )}
                                    <button className="Button" onClick={handleAddProduct}>Submit</button>
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
                    {selectedOption === "Campaigns" && (
                        <div>
                            <h2>Campaigns</h2>
                            <div className="chat-container">
                                {messages.length > 0 ? (
                                    <table className="message-table">
                                        <thead>
                                            <tr>
                                                <th>Message</th>
                                                <th>Actions</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {messages.map(message => (
                                                <tr key={message.id} className="message-row">
                                                    <td dangerouslySetInnerHTML={{ __html: message.content }}></td>
                                                    <td>
                                                        <button onClick={() => handleDeleteMessage(message.id)}>Delete</button>
                                                    </td>
                                                </tr>
                                            ))}
                                        </tbody>
                                    </table>
                                ) : (
                                    <p>No messages.</p>
                                )}
                            </div>
                            <div className="message-input">
                                <textarea
                                    placeholder="Type your message here..."
                                    value={newMessage}
                                    onChange={(e) => setNewMessage(e.target.value)}
                                    maxLength={5000}
                                />
                                <button onClick={handleSendMessage}>Send</button>
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