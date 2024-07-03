using System;

namespace TodoApi.Models
{
    public sealed class WeeklyReport
    {
        public string Day { get; }
        public decimal Total { get; }

        public WeeklyReport(string day, decimal total)
        {
            if (day == null) throw new ArgumentNullException("Day cannot be null.");
            if (total < 0) throw new ArgumentOutOfRangeException("Total must be positive.");

            Day = day;
            Total = total;
        }
    }
}