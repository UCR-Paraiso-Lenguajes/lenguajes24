using Moq;
using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;

namespace StoreApiTests
{
    public class CreateSinpeHandlerTests
    {
        private Mock<ISinpeRepository> _mockSinpeRepository;
        private CreateSinpeHandler _createSinpeHandler;

        [SetUp]
        public void Setup()
        {
            _mockSinpeRepository = new Mock<ISinpeRepository>();
            _createSinpeHandler = new CreateSinpeHandler(_mockSinpeRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsSinpe()
        {
            var command = new CreateSinpeCommand(Guid.NewGuid(), "123456");
            var sinpeToAdd = new Sinpe
            {
                Uuid = Guid.NewGuid(),
                SalesUuid = command.SalesUuid,
                ConfirmationNumber = command.ConfirmationNumber
            };

            _mockSinpeRepository.Setup(repo => repo.AddSinpeAsync(It.IsAny<Sinpe>()))
                                .ReturnsAsync(sinpeToAdd);
            var result = await _createSinpeHandler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.AreEqual(sinpeToAdd.Uuid, result.Uuid);
            Assert.AreEqual(command.SalesUuid, result.SalesUuid);
            Assert.AreEqual(command.ConfirmationNumber, result.ConfirmationNumber);
        }

        [Test]
        public void Handle_EmptySalesUuid_ThrowsArgumentException()
        {
            var command = new CreateSinpeCommand(Guid.Empty, "123456");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSinpeHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The uuid cannot be empty.", ex.Message);
        }

        [Test]
        public void Handle_EmptyConfirmationNumber_ThrowsArgumentException()
        {
            var command = new CreateSinpeCommand(Guid.NewGuid(), "");
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createSinpeHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The confirmation number cannot be empty.", ex.Message);
        }
    }
}
