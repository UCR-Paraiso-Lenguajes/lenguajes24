using Moq;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApiTests
{
    public class CreateSalesHandlerTests
    {
        private Mock<ISalesRepository> _mockSalesRepository;
        private CreateSalesHandler _createSalesHandler;

        [SetUp]
        public void Setup()
        {
            _mockSalesRepository = new Mock<ISalesRepository>();
            _createSalesHandler = new CreateSalesHandler(_mockSalesRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsSales()
        {
            var command = new CreateSalesCommand(DateTime.Now, 1, "Credit Card", 100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var salesToAdd = new Sales
            {
                Uuid = Guid.NewGuid(),
                Date = command.Date,
                Confirmation = command.Confirmation,
                PaymentMethod = command.PaymentMethods,
                Total = command.Total,
                Address = command.Address,
                PurchaseNumber = command.PurchaseNumber
            };
            _mockSalesRepository.Setup(repo => repo.AddSalesAsync(It.IsAny<Sales>()))
                                .ReturnsAsync(salesToAdd);
            var result = await _createSalesHandler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.AreEqual(salesToAdd.Uuid, result.Uuid);
            Assert.AreEqual(command.Date, result.Date);
            Assert.AreEqual(command.Confirmation, result.Confirmation);
            Assert.AreEqual(command.PaymentMethods, result.PaymentMethod);
            Assert.AreEqual(command.Total, result.Total);
            Assert.AreEqual(command.Address, result.Address);
            Assert.AreEqual(command.PurchaseNumber, result.PurchaseNumber);
        }

        [Test]
        public void Handle_EmptyDate_ThrowsArgumentException()
        {
            var command = new CreateSalesCommand(DateTime.MinValue, 1, "Credit Card", 100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The Date cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_InvalidConfirmation_ThrowsArgumentException()
        {
            var command = new CreateSalesCommand(DateTime.Now, 2, "Credit Card", 100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("Confirmation error.", ex.Message);
        }

        [Test]
        public void Handle_EmptyPaymentMethods_ThrowsArgumentException()
        {
            var command = new CreateSalesCommand(DateTime.Now, 1, "", 100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The Paymet Methods cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_NegativeTotal_ThrowsArgumentException()
        {
            var command = new CreateSalesCommand(DateTime.Now, 1, "Credit Card", -100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The total must be greater than zero.", ex.Message);
        }

        [Test]
        public void Handle_InvalidAddress_ThrowsArgumentException()
        {
            var command = new CreateSalesCommand(DateTime.Now, 1, "Credit Card", 100, "Invalid Address", "PUR-001");
            _createSalesHandler = new CreateSalesHandler(_mockSalesRepository.Object);
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSalesHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The address is invalid.", ex.Message);
        }
    }
}
