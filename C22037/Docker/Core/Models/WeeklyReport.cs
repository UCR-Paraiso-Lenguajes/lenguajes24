using System;
using System.Collections.Generic;
using TodoApi.Business;
using TodoApi.Database;

namespace TodoApi.Models
{
    public sealed class WeeklyReport
    {
        public string Day { get; }
        public decimal Total { get; }

        public WeeklyReport(string day, decimal total)
        {
            Day = day;
            Total = total;
        }
    }
}