using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApiLab7;

public class CartData
{
    public void save(Sale sale)
    {
        string insertQuery = @"USE andromeda_store;
            INSERT INTO dbo.sales (address, purchase_amount, payment_method, sale_date, purchase_number) 
            VALUES (@address, @purchaseAmount, @paymentMethod, @saleDate, @purchaseNumber)";

        using (SqlConnection connection = new SqlConnection(Db.DbConnectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {

                command.Parameters.AddWithValue("@address", sale.Address);
                command.Parameters.AddWithValue("@purchaseAmount", sale.Amount);
                command.Parameters.AddWithValue("@paymentMethod", sale.PaymentMethod.PaymentType);
                command.Parameters.AddWithValue("@saleDate", DateTime.Today);
                command.Parameters.AddWithValue("@purchaseNumber", sale.PurchaseNumber);

                command.ExecuteNonQuery();
            }
        }
    }
}