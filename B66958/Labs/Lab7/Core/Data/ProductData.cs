using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace ApiLab7;

public class ProductData
{
    internal async Task<Product> InsertProductAsync(Product product, ProductBusiness.OnNewProduct onNewProduct)
    {
        string insertProductQuery =
            @"USE andromeda_store;
            INSERT INTO dbo.products (id, name, description, image_Url, price, category) 
            VALUES (@id, @name, @description, @imageUrl, @price, @category)";

        using (SqlConnection connection = new SqlConnection(Db.Instance.DbConnectionString))
        {
            connection.OpenAsync();
            using (SqlCommand command = new SqlCommand(insertProductQuery, connection))
            {
                command.Parameters.AddWithValue("@id", product.Uuid);
                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@description", product.Description);
                command.Parameters.AddWithValue("@imageUrl", product.ImageUrl);
                command.Parameters.AddWithValue("@price",product.Price);
                command.Parameters.AddWithValue("@category", product.Category.Id);

                await command.ExecuteNonQueryAsync();
                onNewProduct(product);
                return product;
            }
        }
    }
}