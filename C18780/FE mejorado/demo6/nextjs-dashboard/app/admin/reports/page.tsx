'use client'
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import Charts from '../../components/Charts/charts';
import Pie from '../../components/Charts/pie';
import MyCalendar from '../../components/Charts/calendar';
import { useEffect, useState } from 'react';
import { useFetchReports } from '@/app/api/http.reports';
import { useRouter } from 'next/navigation';
import { getCookie } from 'cookies-next';

export default function Reports() {
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [dataWeeklySales, setDataWeeklySales] = useState<(string | number)[][]>([]);
  const [dataDailySales, setDataDailySales] = useState<(string | number)[][]>([]);
  const router = useRouter();

  const { dailyReports, weeklyReports } = useFetchReports(selectedDate);

  const handleOnDay = (selectedDay: Date) => {
    const token = getCookie('token');
    if (!token) {
      router.push('/login');
    } else {
      setSelectedDate(selectedDay);
    }
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
          <div className='col-sm-12 col-md-6 col-lg-6'>
            <h2>Venta por día</h2>
            <Charts data={dataDailySales} />
          </div>
        </div>
      </div>
    </>
  );
}
