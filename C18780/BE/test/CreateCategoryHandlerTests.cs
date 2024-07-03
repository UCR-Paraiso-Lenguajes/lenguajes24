using Moq;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApiTests
{
    public class CreateCategoryHandlerTests
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private CreateCategoryHandler _createCategoryHandler;

        [SetUp]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _createCategoryHandler = new CreateCategoryHandler(_mockCategoryRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsCategory()
        {
            var command = new CreateCategoryCommand("Test Category");

            var categoryToAdd = new Category
            {
                Uuid = Guid.NewGuid(),
                Name = command.Name
            };
            _mockCategoryRepository.Setup(repo => repo.AddCategoryAsync(It.IsAny<Category>()))
                                  .ReturnsAsync(categoryToAdd);
            var result = await _createCategoryHandler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.AreEqual(categoryToAdd.Uuid, result.Uuid);
            Assert.AreEqual(command.Name, result.Name);
        }

        [Test]
        public void Handle_EmptyName_ThrowsArgumentException()
        {
            var command = new CreateCategoryCommand("");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createCategoryHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The name cannot be empty.", ex.Message);
        }
    }
}
