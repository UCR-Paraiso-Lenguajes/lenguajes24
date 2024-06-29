using StoreApi.Models;
using StoreApi.Repositories;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Moq;
using System;

namespace StoreApiTests
{
    public class ProductsTest
    {
        private IConfiguration _configuration;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;

        [SetUp]
        public void Setup()
        {
            // Configuración del IConfiguration
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Configuración de los mocks
            var productRepositoryMock = new Mock<IProductRepository>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            // Configuración de los métodos simulados de IProductRepository
            productRepositoryMock.Setup(repo => repo.AddProductAsync(It.IsAny<Product>()))
                                 .ReturnsAsync(new Product() { Uuid = Guid.NewGuid() });

            productRepositoryMock.Setup(repo => repo.GetProductListAsync())
                                 .ReturnsAsync(new List<Product>() { new Product(), new Product() });

            productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(It.IsAny<Guid>()))
                                 .ReturnsAsync(new Product() { Uuid = Guid.NewGuid() });

            productRepositoryMock.Setup(repo => repo.UpdateProductAsync(It.IsAny<Product>()))
                                 .ReturnsAsync(1);

            productRepositoryMock.Setup(repo => repo.DeleteProductAsync(It.IsAny<Guid>()))
                                 .ReturnsAsync(1);

            productRepositoryMock.Setup(repo => repo.AddProductAsync(It.IsAny<Product>())).ReturnsAsync((Product product) => product);

            productRepositoryMock.Setup(repo => repo.GetProductByCategoryAsync(It.IsAny<Guid>()))
                                 .ReturnsAsync(new List<Product> { /* Lista de productos simulados */ });

            // Configuración de los métodos simulados de ICategoryRepository
            categoryRepositoryMock.Setup(repo => repo.AddCategoryAsync(It.IsAny<Category>()))
                                  .ReturnsAsync(new Category() { Uuid = Guid.NewGuid() });

            categoryRepositoryMock.Setup(repo => repo.GetCategoryListAsync())
                                  .ReturnsAsync(new List<Category>() { new Category(), new Category() });

            categoryRepositoryMock.Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<Guid>()))
                                  .ReturnsAsync(new Category() { Uuid = Guid.NewGuid() });

            categoryRepositoryMock.Setup(repo => repo.DeleteCategoryAsync(It.IsAny<Guid>()))
                                  .ReturnsAsync(1);

            // Inicialización de los repositorios con los mocks configurados
            _productRepository = productRepositoryMock.Object;
            _categoryRepository = categoryRepositoryMock.Object;
        }

        [Test]
        public async Task AddProductAsync()
        {
            var product = new Product
            {
                Uuid = Guid.Parse("ab12cd34-56ef-78ab-90cd-12ef345678ab"),
                Name = "Test Product",
                ImageUrl = "test_image.jpg",
                Price = 50000,
                Description = "Test description",
                Category = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18")
            };


            var result = await _productRepository.AddProductAsync(product);

            Assert.NotNull(result);
            Assert.AreEqual(product.Name, result.Name);
        }


        [Test]
        public async Task AddProductWithPriceZeroAsync()
        {
            // Crea un nuevo producto con precio cero para la prueba
            var product = new Product
            {
                Uuid = Guid.NewGuid(),
                Name = "Test Product",
                ImageUrl = "test_image.jpg",
                Price = 0,
                Description = "Test description",
                Category = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18")
            };

            try
            {
                var result = await _productRepository.AddProductAsync(product);
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo("The price must be greater than zero."));
            }
        }

        [Test]
        public async Task GetProductByCategoryAsync()
        {
            Guid category = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18");
            var result = await _productRepository.GetProductByCategoryAsync(category);
            Assert.NotNull(result);
        }

        [Test]
        public async Task GetProductByIdAsync()
        {
            // Ejecuta el método de prueba utilizando el repositorio simulado
            var result = await _productRepository.GetProductByIdAsync(Guid.NewGuid());

            // Realiza las afirmaciones
            Assert.NotNull(result);
        }

        [Test]
        public async Task UpdateProductAsync()
        {
            // Crea un producto para actualizar
            var product = new Product
            {
                Uuid = Guid.NewGuid(),
                Name = "Test New Product",
                ImageUrl = "test_new_image.jpg",
                Price = 50001,
                Description = "Test new description",
                Category = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18")
            };

            // Ejecuta el método de prueba utilizando el repositorio simulado
            var result = await _productRepository.UpdateProductAsync(product);

            // Realiza las afirmaciones
            Assert.NotZero(result);
        }

        [Test]
        public async Task GetProductListAsync()
        {
            // Ejecuta el método de prueba utilizando el repositorio simulado
            var result = await _productRepository.GetProductListAsync();

            // Realiza las afirmaciones
            Assert.NotNull(result);
        }

        [Test]
        public async Task DeleteProductAsync()
        {
            // Ejecuta el método de prueba utilizando el repositorio simulado
            var result = await _productRepository.DeleteProductAsync(Guid.NewGuid());

            // Realiza las afirmaciones
            Assert.NotZero(result);
        }

        // Asegúrate de limpiar los datos después de las pruebas si es necesario
        [TearDown]
        public void TearDown()
        {
            // Agrega aquí la lógica para limpiar la base de datos si es necesario
        }
    }
}
