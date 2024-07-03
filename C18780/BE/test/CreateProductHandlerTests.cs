using Moq;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApiTests
{
    public class CreateProductHandlerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private CreateProductHandler _createProductHandler;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _createProductHandler = new CreateProductHandler(_mockProductRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsProduct()
        {
            var command = new CreateProductCommand("Test Product", "https://example.com/image.jpg", 99, "Test Description", Guid.NewGuid());
            var productToAdd = new Product
            {
                Uuid = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                ImageUrl = command.ImageUrl,
                Price = command.Price,
                Category = command.Category
            };
            _mockProductRepository.Setup(repo => repo.AddProductAsync(It.IsAny<Product>()))
                                  .ReturnsAsync(productToAdd);
            var result = await _createProductHandler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.AreEqual(productToAdd.Uuid, result.Uuid);
            Assert.AreEqual(command.Name, result.Name);
            Assert.AreEqual(command.Description, result.Description);
            Assert.AreEqual(command.ImageUrl, result.ImageUrl);
            Assert.AreEqual(command.Price, result.Price);
            Assert.AreEqual(command.Category, result.Category);
        }

        [Test]
        public void Handle_EmptyName_ThrowsArgumentException()
        {
            var command = new CreateProductCommand("", "https://example.com/image.jpg", 99, "Test Description", Guid.NewGuid());
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createProductHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The name cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_EmptyDescription_ThrowsArgumentException()
        {
            var command = new CreateProductCommand("Test Product", "https://example.com/image.jpg", 99, "", Guid.NewGuid());
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createProductHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The description cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_EmptyImageUrl_ThrowsArgumentException()
        {
            var command = new CreateProductCommand("Test Product", "", 99, "Test Description", Guid.NewGuid());
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createProductHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The picture cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_PriceZero_ThrowsArgumentException()
        {
            var command = new CreateProductCommand("Test Product", "https://example.com/image.jpg", 0, "Test Description", Guid.NewGuid());
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createProductHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The price must be greater than zero.", ex.Message);
        }

        [Test]
        public void Handle_EmptyCategory_ThrowsArgumentException()
        {
            var command = new CreateProductCommand("Test Product", "https://example.com/image.jpg", 99, "Test Description", Guid.Empty);
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createProductHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The uuid category cannot be empty.", ex.Message);
        }
    }
}
