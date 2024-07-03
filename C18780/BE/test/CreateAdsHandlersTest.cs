using StoreApi.Handler;
using StoreApi.Models;
using StoreApi.Repositories;
using StoreApi.Commands;
using Moq;

namespace StoreApiTests
{
    public class CreateAdsHandlersTest
    {
        private Mock<IAdRepository> _mockAdRepository;
        private CreateAdHandler _createAdHandler;

        [SetUp]
        public void Setup()
        {
            _mockAdRepository = new Mock<IAdRepository>();
            _createAdHandler = new CreateAdHandler(_mockAdRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsAd()
        {
            var command = new CreateAdCommand("Test message", DateTime.Now);
            var adToAdd = new Ad
            {
                Uuid = Guid.NewGuid(),
                Date = command.Date,
                Message = command.Message
            };
            _mockAdRepository.Setup(repo => repo.AddAdAsync(It.IsAny<Ad>()))
                             .ReturnsAsync(adToAdd);
            var result = await _createAdHandler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.AreEqual(adToAdd.Uuid, result.Uuid);
            Assert.AreEqual(command.Date, result.Date);
            Assert.AreEqual(command.Message, result.Message);
        }

        [Test]
        public void Handle_EmptyMessage_ThrowsArgumentException()
        {
            var command = new CreateAdCommand("", DateTime.Now);
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _createAdHandler.Handle(command, CancellationToken.None));
            Assert.AreEqual("The message cannot be empty.", ex.Message);
        }
    }
}
