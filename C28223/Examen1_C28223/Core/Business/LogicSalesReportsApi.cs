using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using storeApi.DataBase;
using storeApi.Models.Data;

namespace storeApi.Models;
public sealed class LogicSalesReportsApi
{
    public LogicSalesReportsApi() { } // Se usa para la creacion de getSalesReport() 
    private readonly SaleDataBase saleDataBase = new SaleDataBase();
    public async Task<SalesReport> GetSalesReportAsync(DateTime date)
    {
        if (date == null) { throw new ArgumentException($"La variable {nameof(date)} no puede estar vacía o nula."); }
        if (date > DateTime.Now) { return new SalesReport(); }
        if (date == DateTime.MinValue || date == DateTime.MaxValue) { throw new ArgumentException($"La variable {nameof(date)} no puede ser DateTime.MinValue o DateTime.MaxValue."); }
        Task<IEnumerable<SalesData>> salesByDateTask = saleDataBase.GetSalesByDateAsync(date);
        Task<IEnumerable<SaleAnnotation>> salesWeekTask = saleDataBase.GetSalesWeekAsync(date);
        await Task.WhenAll(salesByDateTask, salesWeekTask);
        IEnumerable<SalesData> listSales = await salesByDateTask;
        IEnumerable<SaleAnnotation> weekSales = await salesWeekTask;
        return new SalesReport (listSales, weekSales);
    }
}

public class SalesReport
{
    public IEnumerable<SalesData> Sales { get;  private set; }
    public IEnumerable<SaleAnnotation> SalesDaysWeek { get; private set; }

    public SalesReport(){}
     public SalesReport(IEnumerable<SalesData> sales, IEnumerable<SaleAnnotation> salesDaysWeek){// para este caso la listas si pueden estar vacias
        if (sales==null) { throw new ArgumentException($"El parametro {nameof(sales)} no puede ser nulo."); }
        if (salesDaysWeek==null) { throw new ArgumentException($"El parametro {nameof(salesDaysWeek)} no puede ser nulo."); }
        this.Sales=sales;
        this.SalesDaysWeek=salesDaysWeek;
    }
}