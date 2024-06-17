"use client";
import React, { useState, useEffect } from 'react';
import { Chart } from 'react-google-charts';
import "bootstrap/dist/css/bootstrap.min.css";
import "@/app/ui/styles.css";
import Menu from "@/app/Admin/init/page";
import { jwtDecode } from 'jwt-decode';


const SalesCharAdmin = () => {
  const [selectedDate, setSelectedDate] = useState(new Date().toISOString().split('T')[0]); // Fecha por defecto: hoy
  const [salesData2, setSalesData2] = useState([['Datetime', 'Purchase Number', 'Price', 'Amount of Products', { role: 'annotation' }]]);
  const [weeklySalesData, setWeeklySalesData] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [showModal2, setShowModal2] = useState(false);
  const [charge, setCharge] = useState(false);
  var nombresDias = ['Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo'];
  const URLConection = process.env.NEXT_PUBLIC_API;
  var token = sessionStorage.getItem("token");
  useEffect(() => {
    if(!token){
      setShowModal2(true);
      return;
    }
    fetchData(); // Cargar datos iniciales al cargar el componente
  }, [selectedDate, charge]);
  useEffect(() => {
  }, [weeklySalesData, salesData2, showModal]);

  const fetchData = async () => {
    try {
      if (token) {
        const decodedTokenStorage = jwtDecode(token);
        const nowTime = Date.now() / 1000;
        if (decodedTokenStorage.exp < nowTime) {
          setShowModal2(true);
          sessionStorage.removeItem("token");
          return;
        }
      }
      const response = await fetch(URLConection + `/api/sales/date?date=${selectedDate}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          "Authorization": `Bearer ${token}`
        }
      });
      if (response.status === 403 || response.status === 401) {
        setShowModal2(true);
        return;
      }
      if (response.ok) {
        const data = await response.json();
        var salesAreEmpty = data.sales === null || data.sales.length === 0;
        var weekSalesAreEmpty = data.salesDaysWeek === null || data.salesDaysWeek.length === 0;
        if (salesAreEmpty && weekSalesAreEmpty) {
          setShowModal(true);
          setCharge(true);
          setSalesData2(data)
        } else {
          if (salesAreEmpty) { setShowModal(true); updateSalesData2(data) }
          const weeklyData = [['Week', 'Sales']];
          data.salesDaysWeek.forEach(day => {
            var dayOfWeekNumber = day.dayOfWeek;
            var indiceDia = (dayOfWeekNumber - 1 + 7) % 7;
            weeklyData.push([nombresDias[indiceDia], day.total]);
          });
          updateSalesData2(data);
          setWeeklySalesData(weeklyData);
          setCharge(false);
        }
      } else {
        throw new Error('Error al obtener datos de ventas');
      }
    } catch (error) {
      throw new Error('Error de fetch en obtener los datos de ventas'); // si ocurre un error en el fetching
    }

  };
  const updateSalesData2 = (data: any) => {
    if (data == undefined) { throw new Error('Error: data es nulo o indefinido'); }
    let productsString = '';
    const newData = data.sales.map(sale => {
      const productsInfo = sale.productsAnnotation.map(product => {
        return `${product.productId}, cantidad: ${product.quantity}`;
      });
      productsString = productsInfo.join('  \n'); // Unir los productos con saltos de línea
      return [
        sale.purchaseDate.toString().split('T')[0],
        sale.purchaseNumber,
        sale.total,
        sale.amountProducts,
        productsString // Convertir el objeto en una cadena JSON
      ];
    });
    setSalesData2([['Datetime', 'Purchase Number', 'Price', 'Amount of Products', { role: 'annotation' }], ...newData]); // Actualizar el estado con los nuevos datos y el encabezado
  };
  return (
    <div className='col' >
      <Menu />
      {showModal && <ModalSinVentas closeModal={() => setShowModal(false)} />}
      {showModal2 && (<ModalNoAutorizado closeModal={() => setShowModal(false)} />)}
      <div>
        <div>
          <label htmlFor="fecha">Selecciona una fecha:</label>
          <input
            type="date"
            id="fecha"
            min={'2000-01-01'}
            value={selectedDate}
            onChange={(e) => setSelectedDate(e.target.value)}
          />
        </div>
        <h2>Sales Chart</h2>
        <Chart
          chartType="Table"
          loader={<div className="text-center">
            <div className="spinner-border" role="status">
              <span className="visually-hidden">Loading...</span>
            </div>
          </div>}
          data={salesData2}
          options={{
            showRowNumber: true,
            cssclassNameNames: {
              tableRow: 'chart-row',
              headerRow: 'chart-header-row',
              tableCell: 'chart-cell',
            },
            allowHtml: true,
            pageSize: 20,
          }}
        />
      </div>
      <div className='row' style={{ textAlign: 'center' }}>
        <h2>Weekly Sales Pie Chart</h2>
        {(!charge ? (<Chart
          width={'400px'}
          height={'300px'}
          chartType="PieChart"
          loader={<div className="text-center">
            <div className="spinner-border" role="status">
              <span className="visually-hidden">Loading...</span>
            </div>
          </div>}
          data={weeklySalesData}
          options={{
            title: 'Weekly Sales',
          }}
        />) :
          <div>
            <div className="spinner-border" role="status">
              <span className="visually-hidden">Loading...</span>
            </div>
            <span>Sin datos para esta semana.</span>
          </div>)}
      </div>
    </div>
  );
};

const ModalSinVentas = ({ closeModal }: any) => {
  if (closeModal == undefined) { throw new Error('Error: CloseModal es nulo o indefinido'); }
  return (
    <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Sin ventas</h5>
            <button type="button" className="close" onClick={closeModal} aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div className="modal-body">
            <p>Este dia no presenta ventas.</p>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" onClick={closeModal}>Cerrar</button>
          </div>
        </div>
      </div>
    </div>
  );
};
const ModalNoAutorizado = ({ closeModal }: any) => {
  if (closeModal == undefined) { throw new Error('Error: CloseModal es nulo o indefinido'); }
  return (
    <div className="modal" tabIndex="-1" role="dialog" style={{ display: 'block' }}>
      <div className="modal-dialog" role="document">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">No autorizado</h5>
          </div>
          <div className="modal-body">
            <p>No estás autorizado para acceder a este recurso.</p>
          </div>
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" onClick={closeModal}><a href="/">Volver al inicio</a></button>
          </div>
        </div>
      </div>
    </div>
  );
};
export default SalesCharAdmin;
