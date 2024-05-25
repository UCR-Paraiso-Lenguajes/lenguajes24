using MySqlConnector;
namespace KEStoreApi;
public sealed class DatabaseLines{
 public void addLine(Sale sale)
    {

        using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=store;Uid=root;Pwd=123456;"))
        {
            connection.Open();

            string insertQuery = @"
                INSERT INTO Lines (id_Sale, id_Product, price)
                VALUES (@sale_ID, @product_ID, @price_Line);";

            using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}