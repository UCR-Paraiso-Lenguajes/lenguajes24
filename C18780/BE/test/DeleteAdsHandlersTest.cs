using Moq;
using StoreApi.Handler;
using StoreApi.Repositories;
using StoreApi.Commands;
using StoreApi.Models;

namespace StoreApiTests
{
    public class DeleteAdHandlerTests
    {
        private Mock<IAdRepository> _mockAdRepository;
        private DeleteAdHandler _deleteAdHandler;

        [SetUp]
        public void Setup()
        {
            _mockAdRepository = new Mock<IAdRepository>();
            _deleteAdHandler = new DeleteAdHandler(_mockAdRepository.Object);
        }

        [Test]
        public async Task Handle_ExistingAd_ReturnsOne()
        {
            var command = new DeleteAdCommand
            {
                Uuid = Guid.NewGuid()
            };
            var adToDelete = new Ad
            {
                Uuid = command.Uuid,
                Date = DateTime.Now,
                Message = "Test Message"
            };
            _mockAdRepository.Setup(repo => repo.GetAdByIdAsync(command.Uuid))
                             .ReturnsAsync(adToDelete);

            _mockAdRepository.Setup(repo => repo.DeleteAdAsync(adToDelete.Uuid))
                             .ReturnsAsync(1);
            var result = await _deleteAdHandler.Handle(command, CancellationToken.None);
            Assert.AreEqual(1, result);
        }

        [Test]
        public async Task Handle_NonExistingAd_ReturnsDefault()
        {
            var command = new DeleteAdCommand
            {
                Uuid = Guid.NewGuid()
            };
            _mockAdRepository.Setup(repo => repo.GetAdByIdAsync(command.Uuid))
                             .ReturnsAsync((Ad)null);
            var result = await _deleteAdHandler.Handle(command, CancellationToken.None);
            Assert.AreEqual(default(int), result);
        }

        [Test]
        public void Handle_EmptyUuid_ThrowsArgumentException()
        {
            var command = new DeleteAdCommand
            {
                Uuid = Guid.Empty
            };
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _deleteAdHandler.Handle(command, CancellationToken.None));

            Assert.AreEqual("The uuid cannot be empty.", ex.Message);
        }
    }
}
