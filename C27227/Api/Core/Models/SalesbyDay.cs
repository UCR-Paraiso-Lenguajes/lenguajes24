public class SalesByDay
{
    public string SaleDayOfWeek { get; private set; }
    public int SaleCount { get;  private set; }

    public SalesByDay() { }

    public SalesByDay(string saleDayOfWeek, int saleCount)
    {
        if (string.IsNullOrWhiteSpace(saleDayOfWeek))
        {
            throw new ArgumentException("El día de la venta de la semana no puede ser nulo ni vacío.", nameof(saleDayOfWeek));
        }

        if (saleCount < 0)
        {
            throw new ArgumentException("El recuento de ventas debe ser mayor o igual a cero.", nameof(saleCount));
        }

        SaleDayOfWeek = saleDayOfWeek;
        SaleCount = saleCount;
    }
}
