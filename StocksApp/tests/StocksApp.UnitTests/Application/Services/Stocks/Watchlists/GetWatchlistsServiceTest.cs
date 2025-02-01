using AutoFixture;
using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Application.Services.Stocks.Watchlists;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Watchlists
{
    public class GetWatchlistsServiceTest
    {
        private readonly IGetWatchlistsService _service;
        private readonly Mock<IWatchlistRepository> _watchlistRepositoryMock;

        private readonly IFixture _fixture;

        public GetWatchlistsServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _watchlistRepositoryMock = new Mock<IWatchlistRepository>();
            _service = new GetWatchlistsService(_watchlistRepositoryMock.Object);
        }

        [Fact]
        public async Task GetWatchlistByIdAsync_WithValidData_ReturnsWatchlistResponse()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var watchlist = _fixture.Create<Watchlist>();
            watchlist.Id = watchlistId;
            watchlist.UserId = userId;
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistByIdAsync(watchlistId))
                .ReturnsAsync(watchlist);

            // Act
            var result = await _service.GetWatchlistByIdAsync(userId, watchlistId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(watchlistId);
            result.UserId.Should().Be(userId);
            result.Name.Should().Be(watchlist.Name);
            result.CreatedAt.Should().Be(watchlist.CreatedAt);
        }

        [Fact]
        public async Task GetWatchlistByIdAsync_WithInvalidUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = string.Empty;
            var watchlistId = _fixture.Create<Guid>();

            // Act
            Func<Task> act = async () => await _service.GetWatchlistByIdAsync(userId, watchlistId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetWatchlistByIdAsync_WithInvalidWatchlistId_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _service.GetWatchlistByIdAsync(userId, watchlistId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetWatchlistByIdAsync_WithUnauthorizedUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var watchlist = _fixture.Create<Watchlist>();
            watchlist.UserId = _fixture.Create<string>();
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistByIdAsync(watchlistId))
                .ReturnsAsync(watchlist);

            // Act
            Func<Task> act = async () => await _service.GetWatchlistByIdAsync(userId, watchlistId);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task GetWatchlistItemAsync_WithValidData_ReturnsWatchlistItemResponse()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();
            var watchlistItem = _fixture.Create<WatchlistItem>();
            watchlistItem.WatchlistId = watchlistId;
            watchlistItem.Ticker = symbol;
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistItemAsync(watchlistId, symbol))
                .ReturnsAsync(watchlistItem);

            // Act
            var result = await _service.GetWatchlistItemAsync(userId, watchlistId, symbol);

            // Assert
            result.Should().NotBeNull();
            result.Ticker.Should().Be(symbol);
        }

        [Fact]
        public async Task GetWatchlistItemAsync_WithEmptyUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = string.Empty;
            var watchlistId = _fixture.Create<Guid>();
            var symbol = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => await _service.GetWatchlistItemAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetWatchlistItemAsync_WithEmptySymbol_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlistId = _fixture.Create<Guid>();
            var symbol = string.Empty;

            // Act
            Func<Task> act = async () => await _service.GetWatchlistItemAsync(userId, watchlistId, symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetWatchlistsAsync_WithValidData_ReturnsWatchlistResponses()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var watchlists = _fixture.CreateMany<Watchlist>(3).ToList();
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistsAsync(userId))
                .ReturnsAsync(watchlists);

            // Act
            var result = await _service.GetWatchlistsAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(watchlists.Count);
            result.Should().BeEquivalentTo(watchlists, options => options.Excluding(w => w.Items));
        }

        [Fact]
        public async Task GetWatchlistsAsync_WithEmptyUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = string.Empty;

            // Act
            Func<Task> act = async () => await _service.GetWatchlistsAsync(userId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetWatchlistsAsync_WithNoWatchlists_ReturnsEmptyList()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            _watchlistRepositoryMock.Setup(x => x.GetWatchlistsAsync(userId))
                .ReturnsAsync(new List<Watchlist>());

            // Act
            var result = await _service.GetWatchlistsAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
