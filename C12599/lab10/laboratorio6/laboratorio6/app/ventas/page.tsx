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
        selectedDate: null,
        pieChartData: [],
    });

    const { transactionsDays, selectedDate, pieChartData } = state;

    useEffect(() => {
        const fetchData = async () => {
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
                selectedDate: selectedDate,
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
        const countByPurchaseNumber = {};
        transactions.forEach(transaction => {
            const { purchaseNumber } = transaction;
            if (!purchaseNumber) {
                throw new Error('El número de compra no puede estar vacío.');
            }
            countByPurchaseNumber[purchaseNumber] = (countByPurchaseNumber[purchaseNumber] || 0) + 1;
        });

        const pieData = Object.keys(countByPurchaseNumber).map((purchaseNumber, index) => ({
            name: purchaseNumber,
            value: countByPurchaseNumber[purchaseNumber],
            color: `hsl(${(index * 360) / Object.keys(countByPurchaseNumber).length}, 70%, 50%)`,
        }));

        return pieData;
    };

    const handleDateChange = (date) => {
        if (!date) {
            throw new Error('La fecha seleccionada no es válida.');
        }
        setState(prevState => ({
            ...prevState,
            selectedDate: date,
        }));
    };

    return (
        <div>
            <h1 className="text-center">Gráfico de Ventas</h1>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <div style={{ flex: '1', marginRight: '20px' }}>
                    <h2>Gráfico semanal</h2>
                    <PieChart width={400} height={400}>
                        <Pie
                            dataKey="value"
                            data={pieChartData}
                            cx={200}
                            cy={200}
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
                </div>
                <div style={{ flex: '1' }}>
                    <h2>Tabla diaria</h2>
                    <div style={{ marginBottom: '20px' }}>
                        <DatePicker
                            selected={selectedDate}
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
                            </tr>
                        </thead>
                        <tbody>
                            {transactionsDays.map((transaction, index) => (
                                <tr key={index}>
                                    <td>{transaction.purchaseNumber}</td>
                                    <td>{transaction.totalAmount}</td>
                                    <td>{new Date(transaction.transactionDate).toLocaleString()}</td>
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
