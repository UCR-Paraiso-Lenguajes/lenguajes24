"use client"
import React, { useState, useEffect } from 'react';
import {jwtDecode} from 'jwt-decode'; 
import DatePicker from "react-datepicker";
import 'chart.js/auto';
import { Pie } from 'react-chartjs-2';
import "react-datepicker/dist/react-datepicker.css";
import { Chart } from 'react-google-charts';


function Reports() {
  const [weekDate, setWeekDate] = useState(new Date());
  const [dailyDate, setDailyDate] = useState(new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate())));


  const [weekSaleData, setWeekSaleData] = useState([]);
  const [dailySaleData, setDailySaleData] = useState([['Purchase Number', 'Total']]);


  useEffect(() => {
    const token = sessionStorage.getItem('token');
    if (!token) {
      window.location.href = '/Admin';
      return;
    }


    try {
      const decoded = jwtDecode(token);
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (role !== 'Admin') {
        window.location.href = '/Admin';
        return;
      }
    } catch (error) {
      console.error('Error decoding token:', error);
      window.location.href = '/Admin';
      return;
    }


    fetchData();
  }, [weekDate, dailyDate]);


  const fetchData = async () => {
    const token = sessionStorage.getItem('token');
    const datesPayload = {
      weekDate: weekDate,
      dailyDate: dailyDate
    };


    try {
      const response = await fetch('http://localhost:7280/api/Sale', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(datesPayload)
      });


      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();


      if (data) {
        setWeekSaleData(Object.entries(data.weekSales).map(([day, total]) => ({ day, total })));
        setDailySaleData([['Purchase Number', 'Total']].concat(Object.entries(data.dailySales).map(([day, total]) => [day, total])));
      } else {
        throw new Error('Empty data received');
      }
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };


  // Config for pie chart
  const pieChartData = {
    labels: weekSaleData.map(item => item.day),
    datasets: [{
      data: weekSaleData.map(item => item.total),
      backgroundColor: weekSaleData.map(() => randColor())
    }]
  };


  const chartOptions = {
    responsive: true
  };


  return (
    <div className="product-list-container">
      <h2>Reports</h2>
      <div>
        Seleccionar Fecha:
        <DatePicker selected={weekDate} onChange={(date) => setWeekDate(date)} />
      </div>
      <div style={{ display: "flex", justifyContent: "space-between" }}>
        <div>
          <Chart
            width={"100%"}
            height={"300px"}
            chartType="Table"
            loader={<div>Loading Chart</div>}
            data={dailySaleData}
            options={{ title: "Weekly Sales" }}
          />
        </div>
        <div className="chartContainer">
          <h6 className="centered">Ventas Semanales:</h6>
          <Pie data={pieChartData} options={chartOptions} />
        </div>
      </div>
    </div>
  );
}


function randColor() {
  const characters = '0123456789ABCDEF';
  let color = '#';
  for (let i = 0; i < 6; i++) {
    color += characters[Math.floor(Math.random() * 16)];
  }
  return color;
}


export default Reports;





