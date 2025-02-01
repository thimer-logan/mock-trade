using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Portfolios;
using StocksApp.Application.Services.Stocks.Portfolios;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Exceptions;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Portfolios
{
    public class GetPortfolioServiceTest
    {
        private readonly IGetPortfolioService _getPortfolioService;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;

        private readonly IFixture _fixture;

        public GetPortfolioServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
            _portfolioRepository = _portfolioRepositoryMock.Object;

            var logger = Mock.Of<ILogger<GetPortfolioService>>();
            _getPortfolioService = new GetPortfolioService(_portfolioRepository, logger);
        }

        [Fact]
        public async Task GetPortfolioByIdAsync_WhenPortfolioExistsAndUserHasAccess_ReturnsPortfolioResponse()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);

            // Act
            var result = await _getPortfolioService.GetPortfolioByIdAsync(userId, portfolioId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(portfolio.Id);
            result.UserId.Should().Be(portfolio.UserId);
            result.Name.Should().Be(portfolio.Name);
        }

        [Fact]
        public async Task GetPortfolioByIdAsync_WhenPortfolioDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync((Portfolio?)null);

            // Act
            var result = await _getPortfolioService.GetPortfolioByIdAsync(userId, portfolioId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetPortfolioByIdAsync_WhenUserDoesNotHaveAccess_ThrowsUnauthorizedResourceAccessException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = _fixture.Create<string>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);

            // Act
            Func<Task> act = async () => await _getPortfolioService.GetPortfolioByIdAsync(userId, portfolioId);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedResourceAccessException>();
        }

        [Fact]
        public async Task GetPortfolioByIdAsync_PortfolioIdEmpty_ThrowsArgumentException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = Guid.Empty;
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);

            // Act
            Func<Task> act = async () => await _getPortfolioService.GetPortfolioByIdAsync(userId, portfolioId);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetPortfoliosAsync_WhenPortfoliosExist_ReturnsPortfolioResponses()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolios = _fixture.CreateMany<Portfolio>().ToList();
            portfolios.ForEach(p => p.UserId = userId);
            _portfolioRepositoryMock.Setup(x => x.GetPortfoliosByUserAsync(userId)).ReturnsAsync(portfolios);

            // Act
            var result = await _getPortfolioService.GetPortfoliosAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(portfolios.Count);
            result.Should().BeEquivalentTo(portfolios.Select(p => p.ToPortfolioResponse()));
        }

        [Fact]
        public async Task GetPortfoliosAsync_WhenNoPortfoliosExist_ReturnsNull()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfoliosByUserAsync(userId)).ReturnsAsync((IEnumerable<Portfolio>)null);

            // Act
            var result = await _getPortfolioService.GetPortfoliosAsync(userId);

            // Assert
            result.Should().BeNull();
        }
    }
}
