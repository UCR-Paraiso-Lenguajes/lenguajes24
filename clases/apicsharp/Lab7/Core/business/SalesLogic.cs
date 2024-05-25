using TodoApi.db;
using TodoApi.models;

namespace Core;

public class SalesLogic
{
    internal delegate void OnEachRow(decimal precio);    
    internal List<Report> Calculate()//O(n)
    {
        SaleDB saleDB= new SaleDB();

        decimal totalPrecios = 0.0m;

        // Definir la función 'foo' que acumula precios
        OnEachRow onEachRow = (precio) =>
        {
            totalPrecios += precio;
        };

        List<Report>  result = saleDB.GetSales(onEachRow);//O(n)

        return result;

    }
 
}
public class Report
{
    public decimal Total { get; internal set; }
}