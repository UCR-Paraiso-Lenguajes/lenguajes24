using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class DailySalesRepository : IDailySalesRepository
    {
        private readonly IConfiguration _configuration;
        public DailySalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<DailySales>> GetDailySalesListAsync(DateTime dateTime)
        {
            using (var dbContext = new DbContextClass(_configuration))
            {
                var dailySalesList = new List<DailySales>();

                var salesForDate = await dbContext.Sales
                    .Where(s => s.Date.Date == dateTime.Date)//paso 1: todos los sales que tenga el mismo dateTime
                    .ToListAsync();

                foreach (var sale in salesForDate)
                {
                    var salesLinesForSale = await dbContext.SalesLine//paso 2: sacar todos los salesLine que tengan los sales
                        .Where(sl => sl.UuidSales == sale.Uuid)
                        .ToListAsync();

                    foreach (var salesLine in salesLinesForSale)
                    {
                        var productName = await dbContext.Product//paso 4: obtendo el nombre del producto que esta en el salesLine
                            .Where(p => p.Uuid == salesLine.UuidProduct)
                            .Select(p => p.Name)
                            .FirstOrDefaultAsync();

                        var dailySales = new DailySales//paso 3: el objeto que voy a guardar
                        {
                            Date = sale.Date,
                            PaymentMethod = sale.PaymentMethod,
                            NameProduct = productName,
                            SubTotal = salesLine.Subtotal,
                            Quantity = salesLine.Quantity,
                            Total = salesLine.Quantity * salesLine.Subtotal
                        };

                        dailySalesList.Add(dailySales);

                    }
                }

                return dailySalesList;
            }
        }
    }
}