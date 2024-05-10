"use client"
import React, { useState, useEffect } from 'react';
import { Chart } from 'react-google-charts';
import "react-datepicker/dist/react-datepicker.css";

const Graphic = () => {
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [dailySales, setDailySales] = useState([]);
  const [weeklySales, setWeeklySales] = useState<[string, number][]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchData();
  }, [selectedDate]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const formattedDate = selectedDate.toISOString().split('T')[0];
      const response = await fetch(`https://localhost:7165/api/SaleReport/${formattedDate}`);

      if (!response.ok) {
        throw new Error('Failed to fetch sales data');
      }
      
      const data = await response.json();
      
      if (!data || !data.DailySales || !data.WeeklySales) {
        throw new Error('Sales data is empty or missing');
      }

      const formattedDailySales = data.DailySales.map((sale: any) => ({
        id: sale.SaleId,
        purchaseNumber: sale.PurchaseNumber,
        total: sale.Total,
        purchaseDate: new Date(sale.PurchaseDate).toLocaleDateString(),
        product: sale.Product,
        saleByDay: sale.SaleByDay,
        saleCounter: sale.SaleCounter
      }));

      setDailySales(formattedDailySales);

      const formattedWeeklySales: [string, number][] = [
        ['Day', 'Total Sales'],
        ...data.WeeklySales.map((week: any) => [
          week.DayOfWeek,
          week.Total
        ])
      ];

      setWeeklySales(formattedWeeklySales);

      setError('');
    } catch (error) {
      setError('Error fetching sales data');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <h2>Sales Reports</h2>
      <div className="row">
        <div className="col-md-6">
          <label htmlFor="datepicker">Select Date:</label>
          <input
            type="date"
            value={selectedDate.toISOString().split('T')[0]}
            onChange={e => setSelectedDate(new Date(e.target.value))}
          />
          <br />
          <br />
          <h3>Daily Sales</h3>
          {loading && <p>Loading...</p>}
          {error && <p>{error}</p>}
          {!loading && !error && (
            dailySales.length > 0 ? (
              <table className="table">
                <thead>
                  <tr>
                    <th>Sale ID</th>
                    <th>Purchase Number</th>
                    <th>Total</th>
                    <th>Purchase Date</th>
                    <th>Product</th>
                    <th>Sale By Day</th>
                    <th>Sale Counter</th>
                  </tr>
                </thead>
                <tbody>
                  {dailySales.map(({ id, purchaseNumber, total, purchaseDate, product, saleByDay, saleCounter }, index) => (
                    <tr key={index}>
                      <td>{id}</td>
                      <td>{purchaseNumber}</td>
                      <td>{total}</td>
                      <td>{purchaseDate}</td>
                      <td>{product}</td>
                      <td>{saleByDay}</td>
                      <td>{saleCounter}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            ) : (
              <p>No daily sales data available</p>
            )
          )}
        </div>
        <div className="col-md-6">
          <h3>Weekly Sales</h3>
          <Chart
            width={'100%'}
            height={'400px'}
            chartType="PieChart"
            loader={<div>Loading Chart</div>}
            data={weeklySales}
            options={{
              title: 'Weekly Sales',
            }}
          />
        </div>
      </div>
    </div>
  );
};

export default Graphic;