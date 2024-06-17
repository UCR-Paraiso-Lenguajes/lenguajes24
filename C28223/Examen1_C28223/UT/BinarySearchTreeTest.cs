namespace UT;

using Core;
using storeApi.Models;
using storeApi.Models.Data;
public class BinarySearchTreeTest
{
    [SetUp]
    public void Setup(){}

    [Test]
    public void InsertProduct_InsertsProductIntoTree()
    {
        var tree2 = new BinarySearchTree();
        Product product = new Product { name = "Producto de prueba", description = "Descripción de prueba" };
        tree2.InsertProduct(product);
        var foundProducts = tree2.Search("prueba");
        Assert.AreEqual(2, foundProducts.Count());// coincide en nombre y descripcion
    }

    [Test]
    public void Search_ReturnsCorrectProducts()
    {
        var tree2 = new BinarySearchTree();
        // Creamos varios productos y los insertamos en el árbol
        Product product1 = new Product { name = "Producto 1", description = "Descripción del  1" };
        Product product2 = new Product { name = "Producto 2", description = "Descripción del  2" };
        Product product3 = new Product { name = "Producto 3", description = "Descripción del  3" };

        tree2.InsertProduct(product1);
        tree2.InsertProduct(product2);
        tree2.InsertProduct(product3);

        // Realizamos una búsqueda para encontrar productos que contengan "Produ" en su nombre o descripción
        var foundProducts = tree2.Search("Prod");

        // Verificamos que se hayan encontrado todos los productos que coinciden con el criterio de búsqueda
        Assert.AreEqual(3, foundProducts.Count());
        Assert.Contains(product1, foundProducts.ToList());
        Assert.Contains(product2, foundProducts.ToList());
        Assert.Contains(product3, foundProducts.ToList());
    }

    [Test]
    public void Search_ReturnsEmptyListIfNoMatchFound()
    {
        var tree2 = new BinarySearchTree();
        // Act
        var foundProducts = tree2.Search("No existo");
        // Assert
        Assert.IsEmpty(foundProducts);
    }

    [Test]
    public void InsertProduct_NullProduct_ThrowsException()
    {
        // Arrange
        BinarySearchTree binarySearchTree = new BinarySearchTree();
        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => binarySearchTree.InsertProduct(null));
    }

    [Test]
    public void InsertProduct_EmptyProductName_ThrowsException()
    {
        // Arrange
        BinarySearchTree binarySearchTree = new BinarySearchTree();
        Product product = new Product { name = "", description = "Description" };
        // Act and Assert
        Assert.Throws<ArgumentException>(() => binarySearchTree.InsertProduct(product));
    }

    [Test]
    public void Search_NullTextToSearch_ThrowsException()
    {
        // Arrange
        BinarySearchTree binarySearchTree = new BinarySearchTree();
        // Act and Assert
        Assert.Throws<ArgumentException>(() => binarySearchTree.Search(null));
    }

    [Test]
    public void Search_EmptyTextToSearch_ThrowsException()
    {
        // Arrange
        BinarySearchTree binarySearchTree = new BinarySearchTree();
        // Act and Assert
        Assert.Throws<ArgumentException>(() => binarySearchTree.Search(""));
    }

    [Test]
    public void Search_ProductsFound_ReturnsMatchingProducts()
    {
        // Arrange
        BinarySearchTree binarySearchTree = new BinarySearchTree();
        Product product1 = new Product { name = "Apple", description = "una fruta roja" };
        Product product2 = new Product { name = "Banana", description = "una fruta amarilla" };
        Product product3 = new Product { name = "Orange", description = "una fruta rica" };

        binarySearchTree.InsertProduct(product1);
        binarySearchTree.InsertProduct(product2);
        binarySearchTree.InsertProduct(product3);
        // Act
        var result = binarySearchTree.Search("fruta").ToList();
        // Assert
        Assert.Contains(product1, result);
        Assert.Contains(product2, result);
        Assert.Contains(product3, result);
    }

    [Test]
    public void Search_NoProductsFound_ReturnsEmptyList()
    {
        // Arrange
        BinarySearchTree binarySearchTree = new BinarySearchTree();
        Product product1 = new Product { name = "Apple", description = "una fruta roja" };
        Product product2 = new Product { name = "Banana", description = "una fruta amarilla" };
        Product product3 = new Product { name = "Orange", description = "una fruta rica" };


        binarySearchTree.InsertProduct(product1);
        binarySearchTree.InsertProduct(product2);
        binarySearchTree.InsertProduct(product3);
        // Act
        var result = binarySearchTree.Search("car");
        // Assert
        Assert.IsEmpty(result);
    }

    [Test]
    public void Search_ProductsFound_ReturnsACorrectDescription()
    {
        // Arrange
        BinarySearchTree binarySearchTree = new BinarySearchTree();
        Product product1 = new Product { name = "Apple", description = "una fruta roja" };
        Product product2 = new Product { name = "Banana", description = "una fruta amarilla" };
        Product product3 = new Product { name = "Orange", description = "una fruta rica" };


        binarySearchTree.InsertProduct(product1);
        binarySearchTree.InsertProduct(product2);
        binarySearchTree.InsertProduct(product3);
        // Act
        var result = binarySearchTree.Search("amarilla");
        // Assert
        Assert.IsNotEmpty(result);
    }
}
