using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;

namespace storeApi.DataBase
{
    public sealed class SaleDataBase
    {
        public void Save(Sale sale)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=store;Uid=root;Pwd=123456;"))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            command.Transaction = transaction;

                            command.CommandText = @"
                                    INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                                    VALUES (@purchase_date, @total, @payment_method, @purchase_number);";

                            command.Parameters.AddWithValue("@purchase_date", DateTime.Now);
                            command.Parameters.AddWithValue("@total", sale.Amount);
                            command.Parameters.AddWithValue("@payment_method", (int)sale.PaymentMethod);
                            command.Parameters.AddWithValue("@purchase_number", sale.PurchaseNumber);

                            command.ExecuteNonQuery();

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}