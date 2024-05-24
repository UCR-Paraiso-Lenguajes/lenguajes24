'use client';
import "../../../../styles/report.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css"; 
import Navbar from '../../../../components/Navbar';
import React, { useState, useEffect, useRef } from "react"; 
import { Chart } from 'react-google-charts';
import $ from 'jquery'; 
import 'bootstrap-datepicker'; 

const Reports = () => {
  
  const storedData = localStorage.getItem('tienda'); 
  const dataObject = storedData ? JSON.parse(storedData) : {};
  const [dailySalesList, setDailySalesList] = useState([]);
  const [weeklySalesList, setWeeklySalesList] = useState([]);

   const datePickerRef = useRef(undefined);

   useEffect(() => {
     $(datePickerRef.current).datepicker({
       format: 'yyyy-mm-dd', 
       autoclose: true
     }).on('changeDate', handleDateChange);
   }, []);

 
  const formatDataForChart = (sales) => {
    return [
      ['PurchaseNumber', 'PurchaseDate', 'Total', 'Cantidad'],
      ...sales.map(report => [report.purchaseNumber, report.purchaseDate, report.total, report.pcantidad])
    ];
  };

  const [data, setData] = useState([]);
  useEffect(() => {
    setData(formatDataForChart(dailySalesList));
  }, [dailySalesList]);

   const dataForPieChart = weeklySalesList.map(report => [report.purchaseDate, report.dayOfWeek]);
   const headersForPieChart = ['PurchaseNumber', 'Total'];

   const handleDateChange = async (event) => {
    const selectedDate = event.date;
    const formattedDate = selectedDate.toISOString().slice(0, 10);
      try {
      const response = await fetch(`https://localhost:7013/api/report?date=${formattedDate}`);
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
        throw error;
    }
  };

   return (
    <div style={{ width: '100%', height: '100%' }}>
      <div>
        <Navbar cantidad_Productos={dataObject.cart?.productos?.length || 0} />
      </div>
      <div className="row">
        <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4">
          <Chart
            chartType="Table"
            loader={<div>Cargando tabla por favor espere</div>}
            data={data}
            options={{
              title: 'Tabla de ventas diarias',
              titleTextStyle: { fontSize: 24, color: 'blue' }, 
              showRowNumber: true,     
            }}
          />
        </div>
        <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4" >
          <Chart
            chartType="PieChart"
            loader={<div>Cargando gráfico por favor espere</div>}
            data={[headersForPieChart, ...dataForPieChart]}
            options={{
              title: 'Ventas por Producto semanales', 
              is3D: true,
            }}
          />
        </div>
        <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4" style={{marginTop: '30px'}}>
          <input
           ref={datePickerRef} 
           type="text" 
           className="form-control datepicker-input" 
           placeholder="Ingresa una fecha" 
           style={{ 
               borderRadius: '5px', 
               padding: '10px', 
               fontSize: '16px', 
               borderColor: '#ced4da' 
           }}  />
        </div>
      </div>
    </div>
  );
}

export default Reports;