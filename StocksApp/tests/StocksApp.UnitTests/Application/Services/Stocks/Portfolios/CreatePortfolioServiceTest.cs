using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StocksApp.Application.Interfaces.Stocks.Portfolios;
using StocksApp.Application.Services.Stocks.Portfolios;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Portfolios
{
    public class CreatePortfolioServiceTest
    {
        private readonly ICreatePortfolioService _createPortfolioService;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;

        private readonly IFixture _fixture;

        public CreatePortfolioServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
            _portfolioRepository = _portfolioRepositoryMock.Object;

            var logger = Mock.Of<ILogger<CreatePortfolioService>>();
            _createPortfolioService = new CreatePortfolioService(_portfolioRepository, logger);
        }

        [Fact]
        public async Task CreatePortfolioAsync_ValidArguments_CreatesPortfolio()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioName = _fixture.Create<string>();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            portfolio.Name = portfolioName;

            _portfolioRepositoryMock.Setup(x => x.CreatePortfolioAsync(It.IsAny<Portfolio>())).ReturnsAsync(portfolio);

            // Act
            var result = await _createPortfolioService.CreatePortfolioAsync(userId, portfolioName);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(portfolio.Id);
            result.UserId.Should().Be(portfolio.UserId);
            result.Name.Should().Be(portfolio.Name);
        }

        [Fact]
        public async Task CreatePortfolioAsync_InvalidUserId_ThrowsArgumentNullException()
        {
            // Arrange
            var userId = string.Empty;
            var portfolioName = _fixture.Create<string>();

            // Act
            Func<Task> act = async () => await _createPortfolioService.CreatePortfolioAsync(userId, portfolioName);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreatePortfolioAsync_InvalidPortfolioName_ThrowsArgumentNullException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioName = string.Empty;

            // Act
            Func<Task> act = async () => await _createPortfolioService.CreatePortfolioAsync(userId, portfolioName);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
