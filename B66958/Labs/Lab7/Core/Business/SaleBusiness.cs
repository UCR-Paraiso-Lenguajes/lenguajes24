namespace ApiLab7;

public class SaleBusiness
{
    private SaleData sd;

    public SaleBusiness()
    {
        sd = new SaleData();
    }

    public async Task<IEnumerable<Sale>> GetSalesAsync(DateTime dateToFind)
    {
        ValidateDate(dateToFind);
        return await sd.GetSalesByDateAsync(dateToFind);
    }

    public async Task<IEnumerable<KeyValuePair<string, decimal>>> GetTotalSalesAsync(DateTime dateToFind)
    {
        ValidateDate(dateToFind);
        return await sd.GetSalesByWeekAsync(dateToFind);
    }

    private void ValidateDate(DateTime dateToValidate)
    {
        if (dateToValidate == DateTime.MinValue)
            throw new ArgumentException("A valid date is expected");
    }
}
