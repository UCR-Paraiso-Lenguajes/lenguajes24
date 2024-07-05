using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApiLab7;

public class CartData
{
    public async Task SaveAsync(Sale sale)
    {
        string insertSaleQuery =
            @"USE andromeda_store;
        INSERT INTO dbo.sales (address, purchase_amount, payment_method_id, sale_date, purchase_number, confirmed) 
        VALUES (@address, @purchaseAmount, @paymentMethod, @saleDate, @purchaseNumber, 0); SELECT SCOPE_IDENTITY()";

        string insertSaleLineQuery =
            @"USE andromeda_store;
        INSERT INTO sale_line(sale_id, product_Id, unit_price, quantity) 
        VALUES(@id, @product, @price, @quantity);";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            await connection.OpenAsync();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    int generatedSaleId;
                    using (
                        SqlCommand command = new SqlCommand(
                            insertSaleQuery,
                            connection,
                            transaction
                        )
                    )
                    {
                        command.Parameters.AddWithValue("@address", sale.Address);
                        command.Parameters.AddWithValue("@purchaseAmount", sale.Amount);
                        command.Parameters.AddWithValue(
                            "@paymentMethod",
                            sale.PaymentMethod.PaymentType
                        );
                        command.Parameters.AddWithValue("@saleDate", DateTime.Today);
                        command.Parameters.AddWithValue("@purchaseNumber", sale.PurchaseNumber);

                        generatedSaleId = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }

                    using (
                        SqlCommand command = new SqlCommand(
                            insertSaleLineQuery,
                            connection,
                            transaction
                        )
                    )
                    {
                        foreach (CartProduct product in sale.CartProducts)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", generatedSaleId);
                            command.Parameters.AddWithValue("@product", product.Id);
                            command.Parameters.AddWithValue("@price", product.Price);
                            command.Parameters.AddWithValue("@quantity", product.Quantity);
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    if (sale.PaymentMethod.PaymentType == PaymentMethods.Type.SINPE)
                    {
                        string insertSinpeConfirmationNumber =
                            @"USE andromeda_store;
                            INSERT INTO sinpe_confirmation_number(id, confirmation_number) 
                            VALUES(@saleId, @confirmationNumber)";
                        using (
                            SqlCommand command = new SqlCommand(
                                insertSinpeConfirmationNumber,
                                connection,
                                transaction
                            )
                        )
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue(
                                "@confirmationNumber",
                                sale.ConfirmationNumber
                            );
                            command.Parameters.AddWithValue("@saleId", generatedSaleId);
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction?.RollbackAsync();
                    throw ex;
                }
            }
        }
    }
}
