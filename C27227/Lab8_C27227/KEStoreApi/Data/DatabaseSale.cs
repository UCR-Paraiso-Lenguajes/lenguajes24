using MySqlConnector;
namespace KEStoreApi;
public sealed class DatabaseSale{
 public void save(Sale sale)
    {

        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=store;Uid=root;Pwd=123456;"))
        {
            connection.Open();

            string insertQuery = @"
                INSERT INTO Sales (purchase_date, total, payment_method, purchaseNumber)
                VALUES (@purchase_date, @total, @payment_method, @purchase_number);";

            using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                command.Parameters.AddWithValue("@total", sale.amount);
                command.Parameters.AddWithValue("@payment_method", (int) sale.PaymentMethod);
                command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);

                command.ExecuteNonQuery();
            }
        }
    }
}