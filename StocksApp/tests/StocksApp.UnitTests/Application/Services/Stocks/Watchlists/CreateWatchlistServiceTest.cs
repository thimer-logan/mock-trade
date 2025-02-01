using AutoFixture;
using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Stocks.Watchlists;
using StocksApp.Application.Services.Stocks.Watchlists;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Watchlists
{
    public class CreateWatchlistServiceTest
    {
        private readonly ICreateWatchlistService _service;
        private readonly Mock<IWatchlistRepository> _watchlistRepositoryMock;

        private readonly IFixture _fixture;

        public CreateWatchlistServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _watchlistRepositoryMock = new Mock<IWatchlistRepository>();
            _service = new CreateWatchlistService(_watchlistRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateWatchlistAsync_WithValidData_ReturnsWatchlistResponse()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var name = _fixture.Create<string>();
            var watchlist = _fixture.Build<Watchlist>()
                .With(x => x.UserId, userId)
                .With(x => x.Name, name)
                .Create();
            _watchlistRepositoryMock.Setup(x => x.CreateWatchlistAsync(It.IsAny<Watchlist>()))
                .ReturnsAsync(watchlist);

            // Act
            var result = await _service.CreateWatchlistAsync(userId, name);

            // Assert
            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.Name.Should().Be(name);
        }

        [Fact]
        public async Task CreateWatchlistAsync_WithEmptyUserId_ThrowsArgumentException()
        {
            // Arrange
            var userId = string.Empty;
            var name = _fixture.Create<string>();
            // Act
            Func<Task> act = async () => await _service.CreateWatchlistAsync(userId, name);
            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task CreateWatchlistAsync_WithEmptyName_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var name = string.Empty;
            // Act
            Func<Task> act = async () => await _service.CreateWatchlistAsync(userId, name);
            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }
    }
}
