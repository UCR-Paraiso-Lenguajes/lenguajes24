namespace UT
{
    using core.Models;
    using core.DataBase;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class StoreTesting
    {
        private StoreDb store;
        [SetUp]
        public void Setup()
        {
            var myDbtest = "Server=localhost;Database=geekStoreDB;Uid=root;Pwd=123456;";
            Storage.Init(myDbtest);
            store = new StoreDb();
            StoreDb.CrearDatosSync();
            Store.Instance.SetProducts(new List<Product>());
        }

        [Test]
        public void SetProducts_Exitoso()
        {

            var initialProducts = new List<Product>
            {
                new Product { id = 1, name = "Product 1", description = "Description 1", price = 10.0m, imageUrl = "url1", pcant = 5, category = new Categories.Category { id = 1 }},
                new Product { id = 2, name = "Product 2", description = "Description 2", price = 20.0m, imageUrl = "url2", pcant = 2, category = new Categories.Category { id = 2 }}
            };

            var newProducts = new List<Product>
            {
                new Product { id = 3, name = "Product 3", description = "Description 3", price = 30.0m, imageUrl = "url3", pcant = 15, category = new Categories.Category { id = 3 }},
                new Product { id = 4, name = "Product 4", description = "Description 4", price = 40.0m, imageUrl = "url4", pcant = 20, category = new Categories.Category { id = 4 }}
            };

            Store.Instance.SetProducts(newProducts);
            Assert.That(Store.Instance.Products, Is.EqualTo(newProducts));
        }

        [Test]
        public void SetProducts_ProductosVacios()
        {
            List<Product> newProducts = null;
            var exception = Assert.Throws<ArgumentNullException>(() => Store.Instance.SetProducts(newProducts));
            Assert.That(exception.ParamName, Is.EqualTo("newProducts"));
        }

        [Test]
        public void AgregarProducto_Existoso()
        {
            var initialProducts = new List<Product>
            {
                new Product { id = 1, name = "Product 1", description = "Description 1", price = 10.0m, imageUrl = "url1", pcant = 5, category = new Categories.Category { id = 1 }},
                new Product { id = 2, name = "Product 2", description = "Description 2", price = 20.0m, imageUrl = "url2", pcant = 2, category = new Categories.Category { id = 2 }}
            };
            Store.Instance.SetProducts(initialProducts);
            var newProduct = new Product { id = 3, name = "Product 3", description = "Description 3", price = 30.0m, imageUrl = "url3", pcant = 15, category = new Categories.Category { id = 3 } };
            Store.Instance.AgregarProducto(newProduct);
            Assert.IsTrue(Store.Instance.Products.Contains(newProduct));
        }

        [Test]
        public void AgregarProducto_ListaVacia()
        {
            List<Product> products = null;
            var exception = Assert.Throws<ArgumentNullException>(() => Store.Instance.SetProducts(products));
            Assert.That(exception.ParamName, Is.EqualTo("newProducts"));
        }

    }
}