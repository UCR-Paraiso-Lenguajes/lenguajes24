using Moq;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApiTests
{
    public class CreateSalesLineHandlerTests
    {
        private Mock<ISalesLineRepository> _mockSalesLineRepository;
        private CreateSalesLineHandler _createSalesLineHandler;

        [SetUp]
        public void Setup()
        {
            _mockSalesLineRepository = new Mock<ISalesLineRepository>();
            _createSalesLineHandler = new CreateSalesLineHandler(_mockSalesLineRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsSalesLine()
        {
            var command = new CreateSalesLineCommand(5, 50.0m, Guid.NewGuid(), Guid.NewGuid());
            var salesLineToAdd = new SalesLine
            {
                Uuid = Guid.NewGuid(),
                Quantity = command.Quantity,
                Subtotal = command.Subtotal,
                UuidProduct = command.UuidProduct,
                UuidSales = command.UuidSales
            };

            _mockSalesLineRepository.Setup(repo => repo.AddSalesLineAsync(It.IsAny<SalesLine>()))
                                    .ReturnsAsync(salesLineToAdd);
            var result = await _createSalesLineHandler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.AreEqual(salesLineToAdd.Uuid, result.Uuid);
            Assert.AreEqual(command.Quantity, result.Quantity);
            Assert.AreEqual(command.Subtotal, result.Subtotal);
            Assert.AreEqual(command.UuidProduct, result.UuidProduct);
            Assert.AreEqual(command.UuidSales, result.UuidSales);
        }

        [Test]
        public void Handle_InvalidQuantity_ThrowsArgumentException()
        {
            var command = new CreateSalesLineCommand(-5, 50.0m, Guid.NewGuid(), Guid.NewGuid());
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesLineHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The quantity must be greater than zero.", ex.Message);
        }

        [Test]
        public void Handle_NegativeSubtotal_ThrowsArgumentException()
        {
            var command = new CreateSalesLineCommand(5, -50.0m, Guid.NewGuid(), Guid.NewGuid());
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesLineHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The subtotal must be greater than zero.", ex.Message);
        }

        [Test]
        public void Handle_EmptyUuidProduct_ThrowsArgumentException()
        {
            var command = new CreateSalesLineCommand(5, 50.0m, Guid.Empty, Guid.NewGuid());
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesLineHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The uuid product cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_EmptyUuidSales_ThrowsArgumentException()
        {
            var command = new CreateSalesLineCommand(5, 50.0m, Guid.NewGuid(), Guid.Empty);
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesLineHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The uuid sales cannot be empty.", ex.Message);
        }
    }
}
