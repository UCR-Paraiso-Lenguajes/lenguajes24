using NUnit.Framework;
using System;
using System.Threading.Tasks;
using core;
using core.DataBase;
using core.Models;
using core.Business;
namespace UT;
public class ProductsTest
{
    private Products _prod;

    [SetUp]
    public void Setup()
    {
        _prod = new Products();
    }

    [Test]
    public void FiltrarProductosCategoria_Completado()
    {

        List<Product> listaProductos = new List<Product>
        {
            new Product { id = 1, category = new Categories.Category { id = 1 } },
            new Product { id = 2, category = new Categories.Category { id = 2 } },
            new Product { id = 3, category = new Categories.Category { id = 1 } }
        };


        _prod.FiltrarProductosCategoria(1, listaProductos);


        Assert.IsTrue(_prod.filter.ContainsKey(1));
        Assert.That(_prod.filter[1].Count, Is.EqualTo(2));
    }

    [Test]
    public void FiltrarProductosCategoria_Fallido()
    {
        List<Product> listaProductos = new List<Product>
        {
            new Product { id = 1, category = new Categories.Category { id = 1 } },
            new Product { id = 2, category = new Categories.Category { id = 2 } },
            new Product { id = 3, category = new Categories.Category { id = 1 } }
        };


        _prod.FiltrarProductosCategoria(2, listaProductos);


        Assert.IsFalse(_prod.filter.ContainsKey(4));
    }

    [Test]
    public void FiltrarProductosCategoria_FiltadoCorrectoCategoria()
    {

        int categoryId = 1;
        List<Product> productList = new List<Product>
        {
            new Product { id = 1, name = "Product 1", category = new Categories.Category { id = 1, Name = "Category 1" } },
            new Product { id = 2, name = "Product 2", category = new Categories.Category { id = 2, Name = "Category 2" } }
        };

        _prod.FiltrarProductosCategoria(categoryId, productList);


        Assert.IsTrue(_prod.ObtenerProductosFiltrados(categoryId).Count > 0);
    }

    [Test]
    public void ObtenerProductosFiltrados_CategoriaCorrecta()
    {

        int categoryId = 1;
        List<Product> products = new List<Product>
        {
            new Product { id = 1, name = "Product 1", category = new Categories.Category { id = 1, Name = "Category 1" } },
            new Product { id = 2, name = "Product 2", category = new Categories.Category { id = 1, Name = "Category 1" } }
        };
        _prod.filter.Add(categoryId, products);


        List<Product> filteredProducts = _prod.ObtenerProductosFiltrados(categoryId);

        Assert.That(filteredProducts.Count, Is.EqualTo(2));
        Assert.IsTrue(filteredProducts.All(p => p.category.id == categoryId));
    }

    [Test]
    public void ObtenerProductosFiltrados_CategoriaIncorrecta()
    {

        int invalidCategoryId = -1;

        Assert.Throws<ArgumentException>(() => _prod.ObtenerProductosFiltrados(invalidCategoryId));
    }

    [Test]
    public void FiltrarProductosBusqueda_BusquedaExitosa()
    {
        var categories = new Categories();
        var products = new List<Product>
    {
        new Product { id = 1, name = "Producto 1", category = categories.obtenerCategoria(1) },
        new Product { id = 2, name = "Producto 2", category = categories.obtenerCategoria(2) },
        new Product { id = 3, name = "Producto 3", category = categories.obtenerCategoria(1) }
    };

        var idCat = new[] { 1 };
        var idSearch = "Producto";

        _prod.FiltrarProductosBusqueda(idCat, products, idSearch);

        Assert.IsTrue(_prod.filter.ContainsKey(idCat[0]));
        var filteredProducts = _prod.filter[idCat[0]];
        Assert.That(filteredProducts.Count, Is.EqualTo(2));
        Assert.Contains(products[0], filteredProducts);
        Assert.Contains(products[2], filteredProducts);
    }

    [Test]
    public void FiltrarProductosBusqueda_BusquedaFallida()
    {
        var products = new List<Product>();
        int[]? idCat = null;
        var idSearch = "Producto";

        Assert.Throws<ArgumentException>(() =>
            _prod.FiltrarProductosBusqueda(idCat, products, idSearch));
    }


}



