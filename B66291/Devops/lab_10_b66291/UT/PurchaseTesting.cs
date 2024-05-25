using NUnit.Framework;
using core;
using System;
using System.Threading.Tasks;
using core.DataBase;
using core.Models;
using core.Business;
using MySqlConnector;
namespace UT;

public class PurchaseTesting //falla geekStoreDB.products
{ 

    public static async Task CrearDatosAsyncProducts()
    {
        using (var connection = new MySqlConnection(Storage.Instance.ConnectionStringMyDb))
        {
            await connection.OpenAsync();

            using (var transaction = await connection.BeginTransactionAsync())
            {
                try
                {
                    string createTableQuery = @"
                    DROP TABLE IF EXISTS products;
                    CREATE TABLE IF NOT EXISTS products (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        name VARCHAR(100),
                        description VARCHAR(255),
                        price DECIMAL(10, 2),
                        imageURL VARCHAR(255),
                        pcant INT,
                        idCat INT 
                    );";

                    using (var command = new MySqlCommand(createTableQuery, connection, transaction))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }

    

    [Test]
    public async Task PurchaseTest_ExcenarioExitoso() 
    {
        await CrearDatosAsyncProducts();
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init( myDbtest);
        
        var cart = new Cart
        {
            ProductIds = new List<string> { "1", "2" },
            Address = "Cachi, Cartago",
            PaymentMethod = PaymentMethods.Type.CASH
        };

        var saleInfo = new StoreLogic(); 

        var sale = await saleInfo.PurchaseAsync(cart);

        Assert.IsNotNull(sale);
        Assert.AreEqual(2, sale.Products.Count());
        Assert.AreEqual("Cachi, Cartago", sale.Address); 
        Assert.Greater(sale.Amount, 0); 
        Assert.AreEqual(PaymentMethods.Type.CASH, sale.PaymentMethod);
        Assert.IsFalse(string.IsNullOrWhiteSpace(sale.PurchaseNumber)); 
    }

    [Test]
    public async Task Purchase_CartIncompleto()
    {
        await CrearDatosAsyncProducts();
        var cart = new Cart
        {
            ProductIds = new List<string> { "1", "2" },
            Address = "",
            PaymentMethod = PaymentMethods.Type.CASH,
        };
        var purchaseService = new StoreLogic(); 
        Assert.ThrowsAsync<ArgumentException>(async () => await purchaseService.PurchaseAsync(cart));
    }
}
