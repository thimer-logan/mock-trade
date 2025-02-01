using AutoFixture;
using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Application.Services.Stocks.Watchlists;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Watchlists
{
    public class DeleteWatchlistServiceTest
    {
        private readonly IDeleteWatchlistService _service;
        private readonly Mock<IWatchlistRepository> _watchlistRepositoryMock;

        private readonly IFixture _fixture;

        public DeleteWatchlistServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _watchlistRepositoryMock = new Mock<IWatchlistRepository>();
            _service = new DeleteWatchlistService(_watchlistRepositoryMock.Object);
        }

        [Fact]
        public async Task DeleteWatchlistAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var watchlist = _fixture.Build<Watchlist>()
                .With(x => x.Id, watchlistId)
                .Create();
            _watchlistRepositoryMock.Setup(x => x.DeleteWatchlistAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteWatchlistAsync(userId, watchlistId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteWatchlistAsync_WithEmptyUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = string.Empty;
            var watchlistId = _fixture.Create<Guid>();

            // Act
            Func<Task> act = async () => await _service.DeleteWatchlistAsync(userId, watchlistId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task DeleteWatchlistAsync_WithEmptyWatchlistId_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _service.DeleteWatchlistAsync(userId, watchlistId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
