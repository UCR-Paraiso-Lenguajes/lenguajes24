using System.Collections;
using StoreApi.Models;

namespace StoreApi.utils
{
    public sealed class Utils
    {
        public static string GetPurchaseNumber()
        {
            DateTime today = DateTime.Today;
            Random random = new Random();
            string number = "";
            for (int i = 0; i < 2; i++)
            {
                number += (char)random.Next(65, 91);
                number += random.Next(0, 100);
            }

            return today.Year.ToString("0000") + today.Month.ToString("00") + today.Day.ToString("00") + number;
        }
        //devuelve el domingo
        public static DateTime GetFirstDayOfTheWeek(DateTime dateTime)
        {
            if (dateTime.Date == DateTime.MinValue)
            {
                throw new ArgumentException("The dateTime cannot be empty.");
            }
            int sunday = DayOfWeek.Sunday - dateTime.DayOfWeek;
            return dateTime.AddDays(sunday);
        }
    }
}