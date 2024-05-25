namespace UT;
using core.Models;
using core.DataBase;
using core.Business;

public class StoreDbTesting
{
    private StoreDb store;

    [SetUp]
    public void Setup()
    {
        store = new StoreDb();
        StoreDb.CrearDatosSync();
    }

    [Test]
    public void ExtraerProductosDB_ListaNoVacia()
    {
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init(myDbtest);
        List<Product> productList;
        productList = StoreDb.ExtraerProductosDB();
        Assert.IsNotNull(productList);
        Assert.IsNotEmpty(productList);
    }

    [Test]
    public void ExtraerProductosDB_NumeroProductosCorecto()
    {
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init(myDbtest);
        List<Product> productList;
        productList = StoreDb.ExtraerProductosDB();
        Assert.That(productList.Count, Is.EqualTo(12));
    }
}
