using MySqlConnector;
using System;

namespace geekstore_api.DataBase
{
    public class CartDb
    {
        private readonly string connectionString = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";

        public void procesarOrden(Sale sale)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            command.Transaction = transaction;

                            InsertSale(command, sale);
                            InsertSaleLines(command, sale);

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
        }

        public void InsertSale(MySqlCommand command, Sale sale)
        {
            command.CommandText = @"
                INSERT INTO sales (purchase_date, total, payment_type, purchase_number)
                VALUES (@purchase_date, @total, @payment_type, @purchase_number);";
            
            command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);
            command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
            command.Parameters.AddWithValue("@total", sale.Amount);
            command.Parameters.AddWithValue("@payment_type", (int)sale.PaymentMethod);

            command.ExecuteNonQuery();
        }

        internal void InsertSaleLines(MySqlCommand command, Sale sale)
        {
            foreach (var product in sale.Products)
            {
                command.CommandText = @"
                    INSERT INTO salesLine (purchase_id,  product_id, price)
                    VALUES (@purchase_id, @product_id, @price);";

                command.Parameters.AddWithValue("@purchase_id", sale.PurchaseNumber);
                command.Parameters.AddWithValue("@product_id", product.id);
                command.Parameters.AddWithValue("@price", product.price);

                command.ExecuteNonQuery();
                command.Parameters.Clear(); 
            }
        }
    }
}