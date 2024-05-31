import { getCookie } from "cookies-next";
import { useEffect, useState } from "react";

export function useFetchReports(dateTime: Date) {
    const [dailyReports, setDailyReports] = useState([]);
    const [weeklyReports, setWeeklyReports] = useState([]);
    const token = getCookie('token');


    useEffect(() => {
        const getReportsSales = async () => {
            const formattedDate = dateTime.toISOString().slice(0, 10);
            const res = await fetch(`https://localhost:7099/api/Reports/Date?dateTime=${formattedDate}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json',
                }
            });
            if (!res.ok) {
                throw new Error('Failed to fetch Reports.');
            }
            const data = await res.json();
            const { dailySalesList, weeklySalesList } = data;
            setDailyReports(dailySalesList);
            setWeeklyReports(weeklySalesList);
        }
        getReportsSales();
    }, [dateTime]);

    return { dailyReports, weeklyReports };
}