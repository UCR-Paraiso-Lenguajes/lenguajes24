'use client';
import "../../../../styles/report.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css";
import Navbar from '../../../../components/Navbar';
import React, { useState, useEffect, useRef } from "react";
import { Chart } from 'react-google-charts';
import $ from 'jquery';
import 'bootstrap-datepicker';
import Sidebar from '../../../../components/Sidebar';

const Reports = () => {

  const storedData = localStorage.getItem('tienda');
  const dataObject = storedData ? JSON.parse(storedData) : {};
  const [dailySalesList, setDailySalesList] = useState([]);
  const [weeklySalesList, setWeeklySalesList] = useState([]);

  const datePickerRef = useRef(undefined);

  useEffect(() => {

    const verificarFechaExpiracion = () => {
      const token = sessionStorage.getItem("token");
      if (!token) {
        window.location.href = "/admin";
      }

      const decodedToken = JSON.parse(atob(token.split(".")[1]));
      const expirationDate = new Date(decodedToken.exp * 1000);

      if (new Date() > expirationDate) {
        sessionStorage.removeItem("token");
        window.location.href = "/admin";
      }
    };

    verificarFechaExpiracion();
    const intervalId = setInterval(verificarFechaExpiracion, 2 * 60 * 1000);
    return () => clearInterval(intervalId);
  }, []);

  useEffect(() => {
    $(datePickerRef.current).datepicker({
      format: 'yyyy-mm-dd',
      autoclose: true
    }).on('changeDate', handleDateChange);
  }, []);

  const formatDataForChart = (sales) => {
    return [
      ['PurchaseNumber', 'PurchaseDate', 'Total', 'Cantidad'],
      ...sales.map(report => [report.purchaseNumber, report.purchaseDate.substring(0, 10), report.total, report.pcantidad])
    ];
  };

  const [data, setData] = useState([]);
  useEffect(() => {
    setData(formatDataForChart(dailySalesList));
  }, [dailySalesList]);

  const totalSalesPerDay = {};
  weeklySalesList.forEach(report => {
    const purchaseDate = report.purchaseDate;
    if (totalSalesPerDay[purchaseDate]) {
      totalSalesPerDay[purchaseDate]++;
    } else {
      totalSalesPerDay[purchaseDate] = 1;
    }
  });

  const totalWeeklySales = Object.values(totalSalesPerDay).reduce((acc, curr) => acc + curr, 0);
  const dataForPieChart = Object.entries(totalSalesPerDay).map(([purchaseDate, total]) => [purchaseDate.substring(0, 10), (total / totalWeeklySales) * 100]);
  const headersForPieChart = ['Fecha', 'Porcentaje de Ventas'];

  const handleDateChange = async (event) => {
    const token = sessionStorage.getItem("token");
    if (!token) {
      window.location.href = "/admin";
    } else {
      const selectedDate = event.date;
      const formattedDate = selectedDate.toISOString().slice(0, 10);
      try {
        const response = await fetch(`https://localhost:7013/api/report?date=${formattedDate}`, {
          method: 'GET',
          headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
          }
        });

        if (!response.ok) {
          throw new Error('Failed to fetch data');
        }
        const reports = await response.json();
        if (!reports) {
          throw new Error('No reports found');
        }
        setDailySalesList(reports[0]);
        setWeeklySalesList(reports[1]);
      } catch (error) {
        throw new Error('Error al solicitar reportes ' + error.message);
      }
    }
  };

  return (
    <div style={{ width: '100%', height: '100%' }}>
      <div>
        <Navbar cantidad_Productos={dataObject.cart?.productos?.length || 0} />
      </div>
      <div className="row">
        <div className="col-md-3">
          <Sidebar />
        </div>
        <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4">
          <div className="chart-title" style={{ marginBottom: '10px', marginLeft: '100px', marginTop: '10px' }}>Tabla de ventas diarias</div>
          <Chart
            chartType="Table"
            loader={<div>Cargando tabla por favor espere</div>}
            data={data}
            options={{
              titleTextStyle: { fontSize: 24, color: 'blue' },
              showRowNumber: true,
              cssClassNames: {
                tableCell: 'google-chart-table-cell',
                headerRow: 'google-chart-header-row',
                tableRow: 'google-chart-table-row',
                oddTableRow: 'google-chart-odd-table-row',
                selectedTableRow: 'google-chart-selected-table-row',
                hoverTableRow: 'google-chart-hover-table-row',
                headerCell: 'google-chart-header-cell'
              }
            }}
          />
        </div>
        <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4" >
          <Chart
            chartType="PieChart"
            loader={<div>Cargando gráfico por favor espere</div>}
            data={[headersForPieChart, ...dataForPieChart]}
            options={{
              title: 'Gráfico de ventas semanales',
              titleTextStyle: { fontSize: 15 },
              is3D: true,
            }}
          />
        </div>
        <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4" style={{ marginTop: '30px', marginLeft: '870px' }}>
          <input
            ref={datePickerRef}
            type="text"
            className="form-control datepicker-input text-center"
            placeholder="Ingrese una fecha para mostar los reportes"
            style={{
              borderRadius: '5px',
              padding: '10px',
              fontSize: '16px',
              borderColor: '#ced4da',
              width: '350px'
            }} />
        </div>
      </div>
    </div>
  );
}

export default Reports;