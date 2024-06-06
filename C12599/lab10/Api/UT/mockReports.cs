using System.Collections.Generic;
using System.Threading.Tasks;
using storeapi.Database;

namespace storeapi.UT
{
    // Implementación de mock manual para SalesDB
    public class MockSalesDB : SalesDB
    {
        // Override del método GetForWeekAsync para retornar datos simulados de semana con totales
        public override async Task<IEnumerable<string[]>> GetForWeekAsync(string date)
        {
            // Datos simulados para transacciones semanales con totales
            var mockData = new List<string[]>
            {
                new string[] { "100", "2024-04-29", "123", "Cash" },
                new string[] { "150", "2024-04-30", "124", "Credit Card" },
            };

            return mockData;
        }

        // Override del método GetForDayAsync para retornar datos simulados de día con totales
        public override async Task<IEnumerable<string[]>> GetForDayAsync(string date)
        {
            // Datos simulados para transacciones diarias con totales
            var mockData = new List<string[]>
            {
                new string[] { "200", "2024-04-30", "125", "PayPal" },
            };

            return mockData;
        }
    }
}
