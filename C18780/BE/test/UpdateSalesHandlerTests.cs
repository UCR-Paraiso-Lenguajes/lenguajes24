using Moq;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApiTests
{
    public class UpdateSalesHandlerTests
    {
        private Mock<ISalesRepository> _mockSalesRepository;
        private UpdateSalesHandler _updateSalesHandler;

        [SetUp]
        public void Setup()
        {
            _mockSalesRepository = new Mock<ISalesRepository>();
            _updateSalesHandler = new UpdateSalesHandler(_mockSalesRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsUpdatedSalesCount()
        {
            var command = new UpdateSalesCommand(Guid.NewGuid(), DateTime.Now, 1, "Credit Card", 100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var existingSales = new Sales
            {
                Uuid = command.Uuid,
                Date = DateTime.Now.AddDays(-1),
                Confirmation = 0,
                PaymentMethod = "Cash",
                Total = 50.00m,
                Address = "456 Elm St",
                PurchaseNumber = "PUR-002"
            };
            _mockSalesRepository.Setup(repo => repo.GetSalesByIdAsync(command.Uuid))
                                .ReturnsAsync(existingSales);
            var result = await _updateSalesHandler.Handle(command, CancellationToken.None);
            Assert.AreEqual(command.Date, existingSales.Date);
            Assert.AreEqual(command.Confirmation, existingSales.Confirmation);
            Assert.AreEqual(command.PaymentMethods, existingSales.PaymentMethod);
            Assert.AreEqual(command.Total, existingSales.Total);
            Assert.AreEqual(command.Address, existingSales.Address);
            Assert.AreEqual(command.PurchaseNumber, existingSales.PurchaseNumber);
        }

        [Test]
        public void Handle_EmptyDate_ThrowsArgumentException()
        {
            var command = new UpdateSalesCommand(Guid.NewGuid(), DateTime.MinValue, 1, "Credit Card", 100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _updateSalesHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The Date cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_InvalidConfirmation_ThrowsArgumentException()
        {
            var command = new UpdateSalesCommand(Guid.NewGuid(), DateTime.Now, 2, "Credit Card", 100, "Avenida 3, Barrio San José, Gravilias, Cantón Desamparados, San Jose Province, 10301, Costa Rica", "PUR-001");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _updateSalesHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("Confirmation error.", ex.Message);
        }

    }
}
