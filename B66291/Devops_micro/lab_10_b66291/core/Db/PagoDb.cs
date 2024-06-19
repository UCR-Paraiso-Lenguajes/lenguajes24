using System;
using System.Data.Common;
using System.IO.Compression;
using MySqlConnector;
using core.Models;
using System.Security.Cryptography;
namespace core.DataBase;
public sealed class PagoDb
{
     public static List<Product> ObtenerMetodosDB()//adaptar para metodos de pago
    {
        List<Product> productList = new List<Product>();
        Categories cat = new Categories();

        using (var connection = new MySqlConnection(Storage.Instance.ConnectionStringMyDb))
        {
            connection.Open();

            string selection = "SELECT id, name, description, price, imageUrl, pcant, idCat FROM products";

            using (var command = new MySqlCommand(selection, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productList.Add(new Product
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            name = reader.GetString(reader.GetOrdinal("name")),
                            description = reader.GetString(reader.GetOrdinal("description")),
                            price = reader.GetDecimal(reader.GetOrdinal("price")),
                            imageUrl = reader.GetString(reader.GetOrdinal("imageUrl")),
                            pcant = reader.GetInt32(reader.GetOrdinal("pcant")),
                            category = cat.obtenerCategoria(reader.GetInt32(reader.GetOrdinal("idCat"))),
                        });
                    }
                }
            }
            connection.Close();
        }
        return productList;
    }
}







