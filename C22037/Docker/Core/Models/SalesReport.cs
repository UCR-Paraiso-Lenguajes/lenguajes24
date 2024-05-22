using System;
using System.Collections.Generic;
using TodoApi.Business;
using TodoApi.Database;

namespace TodoApi.Models
{
    public sealed class SalesReport
    {
        public List<DailyReport> DailyReport { get; }
        public List<WeeklyReport> WeeklyReport { get; }

        public SalesReport(List<DailyReport> dailylyReport, List<WeeklyReport> weeklyReport)
        {
            DailyReport = dailylyReport;
            WeeklyReport = weeklyReport;
        }
    }
}