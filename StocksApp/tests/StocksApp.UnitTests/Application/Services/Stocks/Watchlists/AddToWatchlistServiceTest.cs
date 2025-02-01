using AutoFixture;
using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Application.Services.Stocks.Watchlists;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Watchlists
{
    public class AddToWatchlistServiceTest
    {
        private readonly IAddToWatchlistService _service;
        private readonly Mock<IWatchlistRepository> _watchlistRepositoryMock;

        private readonly IFixture _fixture;

        public AddToWatchlistServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _watchlistRepositoryMock = new Mock<IWatchlistRepository>();
            _service = new AddToWatchlistService(_watchlistRepositoryMock.Object);
        }

        [Fact]
        public async Task AddToWatchlistAsync_WithValidData_ReturnsWatchlistItemResponse()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();
            var watchlist = _fixture.Create<Watchlist>();
            watchlist.UserId = userId;
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistByIdAsync(watchlistId))
                .ReturnsAsync(watchlist);
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistItemAsync(watchlistId, symbol))
                .ReturnsAsync(null as WatchlistItem);

            var watchlistItem = _fixture.Build<WatchlistItem>()
                .With(x => x.WatchlistId, watchlistId)
                .With(x => x.Ticker, symbol)
                .Create();
            _watchlistRepositoryMock.Setup(x => x.AddToWatchlistAsync(It.IsAny<WatchlistItem>()))
                .ReturnsAsync(watchlistItem);

            // Act
            var result = await _service.AddToWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            result.Should().NotBeNull();
            result.Ticker.Should().Be(symbol);
        }

        [Fact]
        public async Task AddToWatchlistAsync_WithEmptyUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = string.Empty;
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => await _service.AddToWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("User ID cannot be empty*");
        }

        [Fact]
        public async Task AddToWatchlistAsync_WithEmptyWatchlistId_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = Guid.Empty;
            var symbol = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => await _service.AddToWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Watchlist ID cannot be empty*");
        }

        [Fact]
        public async Task AddToWatchlistAsync_WithEmptySymbol_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = string.Empty;

            // Act
            Func<Task> act = async () => await _service.AddToWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Symbol cannot be empty*");
        }

        [Fact]
        public async Task AddToWatchlistAsync_WithNonExistingWatchlist_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistByIdAsync(watchlistId))
                .ReturnsAsync(null as Watchlist);

            // Act
            Func<Task> act = async () => await _service.AddToWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddToWatchlistAsync_WithUnauthorizedUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();
            var watchlist = _fixture.Create<Watchlist>();
            watchlist.UserId = _fixture.Create<string>();
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistByIdAsync(watchlistId))
                .ReturnsAsync(watchlist);

            // Act
            Func<Task> act = async () => await _service.AddToWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task AddToWatchlistAsync_WithExistingWatchlistItem_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();
            var watchlist = _fixture.Create<Watchlist>();
            watchlist.UserId = userId;
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistByIdAsync(watchlistId))
                .ReturnsAsync(watchlist);
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistItemAsync(watchlistId, symbol))
                .ReturnsAsync(_fixture.Create<WatchlistItem>());

            // Act
            Func<Task> act = async () => await _service.AddToWatchlistAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
