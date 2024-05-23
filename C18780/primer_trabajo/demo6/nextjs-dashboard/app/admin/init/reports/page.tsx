'use client'
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import Charts from '../../charts';
import Pie from '../../pie';
import MyCalendar from '../../calendar';
import { useEffect, useState } from 'react';
import { useFetchReports } from '@/app/api/http.reports';

export default function Reports() {
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [dataWeeklySales, setDataWeeklySales] = useState<(string | number)[][]>([]);
  const [dataDailySales, setDataDailySales] = useState<(string | number)[][]>([]);

  const { dailyReports, weeklyReports } = useFetchReports(selectedDate);

  const handleOnDay = (selectedDay: Date) => {
    setSelectedDate(selectedDay);
  }

  useEffect(() => {
    setDataDailySales([
      ["Date", "PaymentMethod", "NameProduct", "SubTotal", "Quantity", "Total"],
      ...dailyReports.map(({ date, paymentMethod, nameProduct, subTotal, quantity, total }) => [new Date(date).toISOString().slice(0, 10), paymentMethod, nameProduct, subTotal, quantity, total])
    ]);
    setDataWeeklySales([
      ["Week", "Total"],
      ...weeklyReports.map(({ date, total }) => [date, total])
    ]);
  }, [selectedDate, dailyReports, weeklyReports]);

  return (
    <>
      <div className='container'>
        <div className='row align-items-center'>
          <div className='col-sm-12 col-md-6 col-lg-6'>
            <MyCalendar onSelectDay={handleOnDay} />
          </div>
          <div className='col-sm-12 col-md-6 col-lg-6'>
            <h2>Venta por semana</h2>
            <Pie data={dataWeeklySales} />
          </div>
        </div>
        <div className='row align-items-center'>
          <div className='col'>
            <h2>Venta por día</h2>
            <Charts data={dataDailySales} />
          </div>
        </div>
      </div>
    </>
  );
}
