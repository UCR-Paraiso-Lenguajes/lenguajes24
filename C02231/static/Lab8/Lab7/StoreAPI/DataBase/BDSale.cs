using System;
using MySqlConnector;
using StoreAPI;
using StoreAPI.models;

namespace StoreAPI.Database;

public sealed class BDSale
{

    public void Save(Sale sale)
    {

      //  TableExists();
        using (var connection = new MySqlConnection("Server=localhost;Database=store;Port=3306;Uid=root;Pwd=123456;"))
        {
            connection.Open();

            string insertQuery = @"
                INSERT INTO sales (purchase_date, total, payment_method, purchase_number)
                VALUES (@date, @total, @PaymentMethod, @orderNumber);";


            using (var command = new MySqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@total", sale.Amount);
                command.Parameters.AddWithValue("@PaymentMethod", sale.PaymentMethod);
                command.Parameters.AddWithValue("@orderNumber", sale.NumberOrder);

                command.ExecuteNonQuery();
            }
        }
    }

}

