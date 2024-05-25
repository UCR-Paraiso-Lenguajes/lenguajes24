namespace UT;
using core.Models;
using core.DataBase;
using core.Business;
using MySqlConnector;

public class StoreDbTesting //falla geekStoreDB.products
{
    private StoreDb store;

    [SetUp]
    public void Setup()
    {
        store = new StoreDb();
    }

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
    public async Task ExtraerProductosDB_ListaNoVacia()
    {
        await CrearDatosAsyncProducts();
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init(myDbtest);
        List<Product> productList;
        productList = StoreDb.ExtraerProductosDB();
        Assert.IsNotNull(productList);
        Assert.IsNotEmpty(productList);
    }

    [Test]
    public async Task ExtraerProductosDB_NumeroProductosCorecto()
    {
        await CrearDatosAsyncProducts();
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init(myDbtest);
        List<Product> productList;
        productList = StoreDb.ExtraerProductosDB();
        Assert.That(productList.Count, Is.EqualTo(12));
    }
}
