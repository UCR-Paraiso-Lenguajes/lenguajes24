"use client"
import React, { useState, useEffect } from 'react';
import { Chart } from 'react-google-charts';
import VerifyToken, { useTokenContext } from '@/app/components/verify_token';
import { useRouter } from 'next/navigation';

interface SalesAttribute {
  saleId: number;
  purchaseNumber: string;
  total: number;
  purchaseDate: string; // Changed to string based on backend response
  product: string;
  dailySale: string;
  saleCounter: number;
}

const Graphic = () => {
  const { isValidToken, isVerifying } = useTokenContext();
  const router = useRouter();
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [dailySales, setDailySales] = useState<SalesAttribute[]>([]);
  const [weeklySales, setWeeklySales] = useState<[string, string][]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const fetchData = async () => {
    if (!isValidToken) {
      return;
    }

    setLoading(true);
    try {
      const formattedDate = selectedDate.toISOString().split('T')[0]; // Formato yyyy-MM-dd
      const response = await fetch(`https://localhost:7165/api/SalesReport?date=${formattedDate}`);

      if (!response.ok) {
        throw new Error('Failed to fetch sales data');
      }

      const data = await response.json();

      if (!data || !data.dailySales || !data.weeklySales) {
        throw new Error('Sales data is empty or missing');
      }

      const formattedDailySales: SalesAttribute[] = data.dailySales.map((sale: any) => ({
        saleId: sale.saleId,
        purchaseNumber: sale.purchaseNumber,
        total: sale.total,
        purchaseDate: new Date(sale.purchaseDate).toLocaleDateString(),
        product: sale.product,
        dailySale: sale.dailySale,
        saleCounter: sale.saleCounter
      }));

      setDailySales(formattedDailySales);

      const weeklySalesData: [string, string][] = data.weeklySales.reduce((acc: any[], current: any) => {
        if (current.dailySale && current.total) {
          acc.push([current.dailySale, current.total.toString()]); // Asegúrate de que total sea string para el Chart
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

  const handleDateChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedDate(new Date(e.target.value));
  };

  useEffect(() => {
    if (isValidToken && !isVerifying) {
      fetchData();
    }
  }, [isValidToken, isVerifying, selectedDate]);

  useEffect(() => {
    if (!isValidToken && !isVerifying) {
      router.push("/../admin");
    }
  }, [isValidToken, isVerifying, router]);

  if (isVerifying || !isValidToken) {
    return <p></p>;
  }

  return (
    <div className="container">
      <h2>Sales Reports</h2>
      <div className="row">
        <div className="col-md-6">
          <label htmlFor="datepicker">Select Date:</label>
          <input
            type="date"
            value={selectedDate.toISOString().split('T')[0]}
            onChange={handleDateChange}
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
                  {dailySales.map(({ saleId, purchaseNumber, total, purchaseDate, product, dailySale, saleCounter }, index) => (
                    <tr key={index}>
                      <td>{saleId}</td>
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
  );
};

const WrappedGraphic = () => (
  <VerifyToken>
    <Graphic />
  </VerifyToken>
);

export default WrappedGraphic;