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
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init(myDbtest);
        store = new StoreDb();
        StoreDb.CrearDatosSync();
    }

    [Test]
    public void ExtraerProductosDB_ListaNoVacia()
    {

        IEnumerable<Product> productList;
        productList = StoreDb.ExtraerProductosDB();
        Assert.IsNotNull(productList);
        Assert.IsNotEmpty(productList);
    }

    [Test]
    public void ExtraerProductosDB_NumeroProductosCorecto()
    {
        var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
        Storage.Init(myDbtest);
        IEnumerable<Product> productList;
        productList = StoreDb.ExtraerProductosDB();
        Assert.That(productList.Count, Is.EqualTo(12));
    }

    [Test]
    public void ExtraerIDMax_ObtenerIDValido()
    {
        StoreDb storeDb = new StoreDb();
        int maxId = storeDb.ExtraerIDMax();
        Assert.IsTrue(maxId > 0); 
    }

    [Test]
    public void ExtraerIDMax_ExtraerIDCorrecto()
    {
        IEnumerable<Product> productList = StoreDb.ExtraerProductosDB();
        int maxIdInDb = productList.Max(p => p.id);
        int nextId = store.ExtraerIDMax();
        Assert.That(nextId, Is.EqualTo(maxIdInDb + 1));
    }
}
