using AutoFixture;
using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Application.Services.Stocks.Watchlists;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Watchlists
{
    public class RemoveFromWatchlistServiceTest
    {
        private readonly IRemoveFromWatchlistService _service;
        private readonly Mock<IWatchlistRepository> _watchlistRepositoryMock;

        private readonly IFixture _fixture;

        public RemoveFromWatchlistServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _watchlistRepositoryMock = new Mock<IWatchlistRepository>();
            _service = new RemoveFromWatchlistService(_watchlistRepositoryMock.Object);
        }

        [Fact]
        public async Task RemoveFromWatchlistAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();
            _watchlistRepositoryMock.Setup(x => x.RemoveFromWatchlistAsync(watchlistId, symbol))
                .ReturnsAsync(true);

            // Act
            var result = await _service.RemoveFromWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveFromWatchlistAsync_WithInvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = string.Empty;
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => await _service.RemoveFromWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task RemoveFromWatchlistAsync_WithEmptyWatchlistId_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = Guid.Empty;
            var symbol = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => await _service.RemoveFromWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task RemoveFromWatchlistAsync_WithEmptySymbol_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = string.Empty;

            // Act
            Func<Task> act = async () => await _service.RemoveFromWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
