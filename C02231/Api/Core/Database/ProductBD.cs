using System;
using Core;
using MySqlConnector;
using StoreAPI.models;
using StoreAPI.Business;

namespace StoreAPI.Database
{
    public sealed class ProductBD
    {
        internal async Task<Product> InsertProductAsync(Product product, ProductLogic.OnNewProduct onNewProduct)
        {
            if (product == null) throw new ArgumentNullException($"The {nameof(product)} object cannot be null.");

            using (var connection = new MySqlConnection(Storage.Instance.ConnectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string insertQuery = @"
                        INSERT INTO products (name, author, price, idCategory, imgURL)
                        VALUES (@name, @author, @price, @idCategory, @imgURL);";

                        using (var insertCommand = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@name", product.Name);
                            insertCommand.Parameters.AddWithValue("@author", product.Author);
                            insertCommand.Parameters.AddWithValue("@price", product.Price);
                            insertCommand.Parameters.AddWithValue("@idCategory", product.ProductCategory.IdCategory);
                            insertCommand.Parameters.AddWithValue("@imgURL", product.ImgUrl);
                            await insertCommand.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        if (transaction.Connection != null) // Verifica que la conexión aún esté abierta
                        {
                            await transaction.RollbackAsync();
                        }
                        throw new Exception("Error saving the product in the database.");
                    }
                }
            }
            // Notificar sobre el nuevo producto fuera de la transacción
            onNewProduct(product);

            return product;
        }
    }
}
