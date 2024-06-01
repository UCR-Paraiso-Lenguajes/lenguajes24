"use client"
import React, { useState, useEffect } from 'react';
import { Chart } from 'react-google-charts';
import "react-datepicker/dist/react-datepicker.css";
import VerifyToken from '@/app/components/verify_token';

const Graphic = () => {
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [dailySales, setDailySales] = useState([]);
  const [weeklySales, setWeeklySales] = useState<[string, string][]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchData();
  }, [selectedDate]);

  const fetchData = async () => {
    setLoading(true);
    try {
      const formattedDate = selectedDate.toISOString().split('T')[0];
      const response = await fetch(`https://localhost:7165/api/SalesReport?date=${formattedDate}`);

      if (!response.ok) {
        throw new Error('Failed to fetch sales data');
      }

      const data = await response.json();

      if (!data || !data.DailySales || !data.WeeklySales) {
        throw new Error('Sales data is empty or missing');
      }

      const formattedDailySales = data.DailySales.map((sale: any) => ({
        id: sale.saleId,
        purchaseNumber: sale.purchaseNumber,
        total: sale.total,
        purchaseDate: new Date(sale.purchaseDate).toLocaleDateString(),
        product: sale.product,
        dailySale: sale.dailySale,
        saleCounter: sale.saleCounter
      }));

      setDailySales(formattedDailySales);

      const weeklySalesData: [string, string][] = data.WeeklySales.reduce((acc: any[], current: any) => {
        if (current.dailySale && current.total) {
          acc.push([current.dailySale, current.total]);
        }
        return acc;
      }, []);

      weeklySalesData.unshift(['Day', 'Total Sales']);

      setWeeklySales(weeklySalesData);

      setError('');
    } catch (error) {
      setError('Error fetching sales data');
    } finally {
      setLoading(false);
    }
  };

  return (
    <VerifyToken>

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
                    {dailySales.map(({ id, purchaseNumber, total, purchaseDate, product, dailySale, saleCounter }, index) => (
                      <tr key={index}>
                        <td>{id}</td>
                        <td>{purchaseNumber}</td>
                        <td>{total}</td>
                        <td>{purchaseDate}</td>
                        <td>{product}</td>
                        <td>{dailySale}</td>
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


    </VerifyToken>

  );
};

export default Graphic;