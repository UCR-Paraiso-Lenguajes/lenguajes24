using MySqlConnector;
namespace KEStoreApi;
public sealed class DatabaseSale
{    public void Save(Sale sale)
{
    using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=store;Uid=root;Pwd=123456;"))
    {
        connection.Open();
        // Inicia la transacción
        using (MySqlTransaction transaction = connection.BeginTransaction())
        {
            try
            {
                string insertQuery = @"
                    INSERT INTO Sales (purchase_date, total, payment_method, purchaseNumber)
                    VALUES (@purchase_date, @total, @payment_method, @purchase_number);";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    // Asocia la transacción al comando
                    command.Transaction = transaction;
                    command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                    command.Parameters.AddWithValue("@total", sale.amount);
                    command.Parameters.AddWithValue("@payment_method", (int)sale.PaymentMethod);
                    command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
                    command.ExecuteNonQuery();

                    // Insertar los detalles de la venta en la tabla Lines_Sales
                    foreach (var product in sale.Products)
                    {
                        string insertLineQuery = @"
                            INSERT INTO Lines_Sales (id_Sale, id_Product, price)
                            VALUES (@id_Sale, @id_Product, @price);";

                        using (MySqlCommand lineCommand = new MySqlCommand(insertLineQuery, connection, transaction))
                        {
                            lineCommand.Parameters.AddWithValue("@id_Sale", sale.PurchaseNumber);
                            lineCommand.Parameters.AddWithValue("@id_Product", product.Id);
                            lineCommand.Parameters.AddWithValue("@price", product.Price);
                            lineCommand.ExecuteNonQuery();
                        }
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, realiza un rollback para deshacer los cambios
                transaction.Rollback();
                throw new Exception("Error al guardar la venta en la base de datos", ex);
            }
        }
    }
    }
}
