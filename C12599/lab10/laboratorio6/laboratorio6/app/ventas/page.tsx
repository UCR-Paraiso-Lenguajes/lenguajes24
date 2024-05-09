'use client'
import React, { useState, useEffect } from 'react';
import { PieChart, Pie, Legend, Tooltip, Cell } from 'recharts';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import 'bootstrap/dist/css/bootstrap.css';
import '../ui/globals.css';

const Ventas = () => {
    const [state, setState] = useState({
        transactionsDays: [],
        selectedDate: new Date(),
        pieChartData: [],
    });

    const { transactionsDays, selectedDate, pieChartData } = state;

    useEffect(() => {
        const fetchData = async () => {
            if (!selectedDate) {
                return; // Manejar el caso en que selectedDate sea null
            }

            const formattedDate = selectedDate.toISOString().split('T')[0];
            const url = `https://localhost:7043/api/Sales/transactions?date=${formattedDate}`;
                const response = await fetch(url);
                if (!response.ok) {
                    throw new Error('Error al obtener los datos.');
                }
                const json = await response.json();

                setState(prevState => ({
                    ...prevState,
                    transactionsDays: json.transactionsDays || [],
                }));

                const pieData = generatePieChartData(json.transactionsWeeks || []);
                setState(prevState => ({
                    ...prevState,
                    pieChartData: pieData,
                }));
         
        };

        fetchData();
    }, [selectedDate]);

    const generatePieChartData = (transactions) => {
        if (!transactions || !Array.isArray(transactions)) {
            throw new Error('Los datos de transacciones no son válidos.');
        }

        const countByDayOfWeek = {};
        transactions.forEach(transaction => {
            const dayOfWeek = new Date(transaction.transactionDate).getDay(); 
            countByDayOfWeek[dayOfWeek] = (countByDayOfWeek[dayOfWeek] || 0) + 1;
        });

        const totalTransactions = transactions.length;

        const pieData = Object.keys(countByDayOfWeek).map(dayOfWeek => {
            const dayName = getDayName(parseInt(dayOfWeek, 10)); 
            const percentage = (countByDayOfWeek[dayOfWeek] / totalTransactions) * 100;
            return {
                name: `${dayName} (${percentage.toFixed(2)}%)`,
                value: countByDayOfWeek[dayOfWeek],
                color: getRandomColor(),
            };
        });

        return pieData;
    };

    const getDayName = (dayOfWeek) => {
        const days = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];
    
        // Validar que dayOfWeek sea un número entre 0 y 6 (inclusive)
        if (typeof dayOfWeek !== 'number' || dayOfWeek < 0 || dayOfWeek > 6) {
            throw new Error('El argumento dayOfWeek debe ser un número entre 0 y 6.');
        }
    
        return days[dayOfWeek];
    };
    

    const getRandomColor = () => {
        const randomColor = `hsl(${Math.random() * 360}, 70%, 50%)`;
        return randomColor;
    };

    const handleDateChange = (date) => {
        if (!date) {
            throw new Error('La fecha seleccionada no es válida.');
        }


        const adjustedDate = new Date(date);
        adjustedDate.setDate(adjustedDate.getDate() - 1); 
        setState(prevState => ({
            ...prevState,
            selectedDate: adjustedDate,
        }));
    };


    const formattedDisplayDate = new Date(selectedDate);
    formattedDisplayDate.setDate(formattedDisplayDate.getDate() + 1); 

    return (
        <div>
            <h1 className="text-center">Gráfico de Ventas</h1>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <div style={{ flex: '1', marginRight: '20px' }}>
                    <h2>Gráfico semanal</h2>
                    {pieChartData.length > 0 ? (
                        <PieChart width={400} height={550}>
                            <Pie
                                dataKey="value"
                                data={pieChartData}
                                cx={200}
                                cy={150}
                                outerRadius={100}
                                innerRadius={60}
                                label
                            >
                                {pieChartData.map((entry, index) => (
                                    <Cell key={`cell-${index}`} fill={entry.color} />
                                ))}
                            </Pie>
                            <Tooltip formatter={(value, name) => [value, name]} />
                            <Legend />
                        </PieChart>
                    ) : (
                        <p>No hay datos disponibles para mostrar el gráfico.</p>
                    )}
                </div>
                <div style={{ flex: '1' }}>
                    <h2>Tabla diaria</h2>
                    <div style={{ marginBottom: '20px' }}>
                        <DatePicker
                            selected={formattedDisplayDate}
                            onChange={handleDateChange}
                            dateFormat="yyyy-MM-dd"
                            placeholderText="Selecciona una fecha"
                        />
                    </div>
                    <table className="table">
                        <thead>
                            <tr>
                                <th>Número de Compra</th>
                                <th>Monto Total</th>
                                <th>Fecha de Transacción</th>
                                <th>Productos</th>
                            </tr>
                        </thead>
                        <tbody>
                            {transactionsDays.map((transaction, index) => (
                                <tr key={index}>
                                    <td>{transaction.purchaseNumber}</td>
                                    <td>{transaction.totalAmount}</td>
                                    <td>{new Date(transaction.transactionDate).toLocaleDateString()}</td>
                                    <td>
                                        <ul>
                                            {transaction.products.map((product, index) => (
                                                <li key={index}>{product}</li>
                                            ))}
                                        </ul>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    );
};

export default Ventas;
