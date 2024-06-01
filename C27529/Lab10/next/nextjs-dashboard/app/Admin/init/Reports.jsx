"use client"
import React from 'react'
import { useState, useEffect } from 'react';
import DatePicker from "react-datepicker";
import 'chart.js/auto';
import { Pie } from 'react-chartjs-2';
import "react-datepicker/dist/react-datepicker.css";
import { Chart } from 'react-google-charts';
import { useHref } from 'react-router-dom';




function Reports() {

  const [weekDate, setweekDate] = useState(new Date());
  const [dailyDate, setDailyDate] = useState(new Date(Date.UTC(new Date().getFullYear(), new Date().getMonth(), new Date().getDate())));

  const [weekSaleData, setWeekSaleData] = useState([]);
  const [dailySaleData, setDailySaleData] = useState([['Purchase Number', 'Total']]);

  const fetchData = async () => {
  const token = typeof window !== 'undefined' ? sessionStorage.getItem('token') : null;
     

    if (!token) {
      window.location.href = '/Admin';
      throw new Error('No token available');
      return; 
    }

    const datesPayload = {
      weekDate: weekDate,
      dailyDate: dailyDate
    };

    try {
      const response = await fetch('https://localhost:7280/api/Sale', {
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

      if (data != null) {
        //Weekly
        const newDataWeek = Object.entries(data.weekSales).map(([day, total]) => ({ day, total }));
        setWeekSaleData(newDataWeek);

        //Daily
        const dailySalesArray = [['Purchase Number', 'Total']];
        for (const day in data.dailySales) {
          if (Object.hasOwnProperty.call(data.dailySales, day)) {
            dailySalesArray.push([day, data.dailySales[day]]);
          }
        }
        setDailySaleData(dailySalesArray);
      } else {
        throw new Error('Empty data received');
      }
    } catch (error) {
      throw new Error('Error fetching data:', error);
    }
  };


  // config pie charts
  const data = {
    labels: weekSaleData.map(item => item.day),
    datasets: [{
      data: weekSaleData.map(item => item.total),
      backgroundColor: weekSaleData.map(() => randColor())
    }]
  };

  const opciones = {
    responsive: true
  }

  useEffect(() => {
    fetchData();

  }, [weekDate]);

  return (
    <div className="product-list-container">
      <h2>Reports</h2>
      <div>
        Seleccionar Fecha:
        <DatePicker selected={weekDate} onChange={(date) => setweekDate(date)} />
      </div>
      <div style={{ display: "flex", justifyContent: "space-between" }}>
        <div>
          <Chart
            width={"100%"}
            height={"300px"}
            chartType="Table"
            loader={<div>Loading Chart</div>}
            data={dailySaleData}
            options={{
              title: "Weekly Sales",
            }}
          />
        </div>
        <div className="chartContainer">
          <h6 className="centered">Ventas Semanales:</h6>
          <Pie data={data} options={opciones} />
        </div>
      </div>
    </div>



  )
}


function randColor() {
  const characters = '0123456789ABCDEF';
  let color = '#';

  for (let i = 0; i < 6; i++) {
    color += characters[Math.floor(Math.random() * 16)];
  }

  return color;
}

export default Reports