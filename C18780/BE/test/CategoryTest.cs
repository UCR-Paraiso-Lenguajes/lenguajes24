using Microsoft.Extensions.Configuration;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApiTests
{
    public class CategoryRepositoryTests
    {
        private IConfiguration _configuration;
        private ICategoryRepository _categoryRepository;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _categoryRepository = new CategoryRepository(_configuration);
        }

        [Test, Order(1)]
        public async Task AddCategoryAsync_ValidCategory_ReturnsCategory()
        {
            // Arrange
            var category = new Category
            {
                Uuid = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18"),
                Name = "Test Category"
            };

            // Act
            var result = await _categoryRepository.AddCategoryAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(category.Uuid, result.Uuid);
        }

        [Test, Order(2)]
        public async Task GetCategoryByIdAsync_ExistingId_ReturnsCategory()
        {
            // Arrange
            var categoryId = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18");

            // Act
            var result = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(categoryId, result.Uuid);
        }

        [Test, Order(3)]
        public async Task GetCategoryByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingCategoryId = Guid.NewGuid();

            // Act
            var result = await _categoryRepository.GetCategoryByIdAsync(nonExistingCategoryId);

            // Assert
            Assert.Null(result);
        }

        [Test, Order(4)]
        public async Task GetCategoryByNameAsync_ExistingName_ReturnsCategory()
        {
            // Arrange
            var categoryName = "Case";

            // Act
            var result = await _categoryRepository.GetCategoryByNameAsync(categoryName);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(categoryName, result.Name);
        }

        [Test, Order(5)]
        public async Task DeleteCategoryAsync_ExistingId_ReturnsOne()
        {
            // Arrange
            var existingCategoryId = Guid.Parse("4a8c74b4-cf8e-4fbf-81a2-3d11e1e37d18");

            // Act
            var result = await _categoryRepository.DeleteCategoryAsync(existingCategoryId);

            // Assert
            Assert.AreEqual(1, result);
        }
    }
}
