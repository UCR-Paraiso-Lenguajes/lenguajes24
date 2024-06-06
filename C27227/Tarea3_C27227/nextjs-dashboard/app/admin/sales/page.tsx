"use client"
import React, { useState, useEffect, useRef } from "react";
import DatePicker from 'react-datepicker';
import Pagination from 'react-bootstrap/Pagination';
import Chart from 'chart.js/auto';
import Sidebar from "../init/page";
import "chartjs-plugin-datalabels";
import "bootstrap/dist/css/bootstrap.min.css";
import "react-datepicker/dist/react-datepicker.css";
import '@/app/ui/styles/SalesReport.css';

const TableWithPaginationAndChart = () => {
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(5);
  const [dailySales, setDailySales] = useState([]);
  const [weeklySales, setWeeklySales] = useState([]);
  const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];
  const pieChartRef = useRef<Chart | null>(null);

  useEffect(() => {
    fetchData(selectedDate);
  }, [selectedDate]);

  const fetchData = async (date) => {
    try {
        if (!date) {
            throw new Error('La fecha es requerida.');
        }
        
        const formattedDate = date.toISOString().slice(0, 10);
        const response = await fetch(`http://localhost:5072/api/Sale?date=${formattedDate}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error('Error en la solicitud: ' + response.statusText);
        }

        const data = await response.json();

        if (!data || !data.sales || !data.salesByWeek) {
            throw new Error('Los datos recibidos no son válidos.');
        }

        setDailySales(data.sales);
        setWeeklySales(data.salesByWeek);
    } catch (error) {
        throw new Error('Error al enviar datos: ' + error.message);
    }
};

  const handleDateChange = (date) => {
    setSelectedDate(date);
    setCurrentPage(1);
  };

  const drawPieChart = (pieOptions: Chart.ChartOptions) => {
    const pieCtx = document.getElementById('myPieGraph')?.getContext('2d');
  
    if (!pieCtx) return;

    if (pieChartRef.current) {
      pieChartRef.current.destroy();
    }

    pieChartRef.current = new Chart(pieCtx, {
      type: 'pie',
      data: {
        labels: weeklySales.map(item => item.saleDayOfWeek), 
        datasets: [{
          data: weeklySales.map(item => item.saleCount),
          backgroundColor: COLORS,
          hoverOffset: 4,
        }],
      },
      options: pieOptions
    });
  };

  useEffect(() => {
    if (weeklySales.length > 0) {
      drawPieChart(pieOptions);
    }
  }, [weeklySales]);

  const pieOptions: ChartOptions = {
    plugins: {
      datalabels: {
        anchor: "center",
        formatter: (dato) => dato + "%",
        color: "black",
        font: {
          family: '"Times New Roman", Times, serif',
          size: "28",
          weight: "bold",
        },
      }
    }
  };

  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = dailySales.slice(indexOfFirstItem, indexOfLastItem);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

return (
  <div>
    <Sidebar/>
    <div className="datePicker">
      <DatePicker selected={selectedDate} onChange={handleDateChange} />
    </div>
    <h2>Ventas Diarias</h2>
    <div className="table">
      <table>
        <thead>
          <tr>
            <th>Número de compra</th>
            <th>Total de venta</th>
            <th>Fecha</th>
            <th>Producto</th>
            <th>Cantidad</th>
          </tr>
        </thead>
        <tbody>
          {currentItems.map(item => (
            <tr key={item.purchaseNumber}>
              <td>{item.purchaseNumber}</td>
              <td>{item.total}</td>
              <td>{item.purchaseDate}</td>
              <td>{item.product}</td>
              <td>{item.quantity}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
    <Pagination>
      {[...Array(Math.ceil(dailySales.length / itemsPerPage))].map((_, index) => (
        <Pagination.Item key={index + 1} active={index + 1 === currentPage} onClick={() => paginate(index + 1)}>
          {index + 1}
        </Pagination.Item>
      ))}
    </Pagination>
    {(dailySales.length === 0 && weeklySales.length === 0) && (
      <div className="noSalesMessage">
        <p>No hay ventas en esta fecha.</p>
      </div>
    )}
    {(dailySales.length > 0 || weeklySales.length > 0) && (
      <div className="salesChart">
        <h2>Ventas semanales</h2>
        <canvas id="myPieGraph" style={{ maxHeight: '200px' }}></canvas>
      </div>
    )}
  </div>
);
};

export default TableWithPaginationAndChart;
