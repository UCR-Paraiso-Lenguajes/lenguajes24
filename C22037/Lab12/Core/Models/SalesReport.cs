using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public sealed class SalesReport
    {
        public List<DailyReport> DailyReport { get; private set; }
        public List<WeeklyReport> WeeklyReport { get; private set; }

        public SalesReport(List<DailyReport> dailyReport, List<WeeklyReport> weeklyReport)
        {
            if (dailyReport == null) throw new ArgumentNullException("DailyReport cannot be null.");
            if (weeklyReport == null) throw new ArgumentNullException("WeeklyReport cannot be null.");

            DailyReport = dailyReport;
            WeeklyReport = weeklyReport;
        }
    }
}