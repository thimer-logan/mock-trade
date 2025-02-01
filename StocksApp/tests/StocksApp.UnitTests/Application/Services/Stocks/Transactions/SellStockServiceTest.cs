using AutoFixture;
using FluentAssertions;
using Moq;
using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Transactions;
using StocksApp.Application.Services.Stocks.Transactions;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Exceptions;
using StocksApp.Domain.Interfaces;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.UnitTests.Application.Services.Stocks.Transactions
{
    public class SellStockServiceTest
    {
        private readonly ISellStockService _sellStockService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;
        private readonly Mock<IHoldingRepository> _holdingRepositoryMock;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

        private readonly IFixture _fixture;

        public SellStockServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _portfolioRepositoryMock = new Mock<IPortfolioRepository>();
            _holdingRepositoryMock = new Mock<IHoldingRepository>();
            _finnhubRepositoryMock = new Mock<IFinnhubRepository>();
            _sellStockService = new SellStockService(_transactionRepositoryMock.Object, _portfolioRepositoryMock.Object,
                _finnhubRepositoryMock.Object, _holdingRepositoryMock.Object);
        }

        [Fact]
        public async Task SellStockAsync_WhenPortfolioIdIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = Guid.Empty;
            var sellStockReq = _fixture.Create<SellStockRequest>();

            // Act
            Func<Task> act = async () => await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task SellStockAsync_WhenUserIdIsInvalid_ThrowsBadRequestException()
        {
            // Arrange
            var userId = string.Empty;
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = _fixture.Create<SellStockRequest>();

            // Act
            Func<Task> act = async () => await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task SellStockAsync_WhenTickerIsNullOrEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = new SellStockRequest { Ticker = string.Empty };

            // Act
            Func<Task> act = async () => await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task SellStockAsync_WhenPortfolioDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = _fixture.Create<SellStockRequest>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(null as Portfolio);

            // Act
            Func<Task> act = async () => await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task SellStockAsync_WhenUserDoesNotHaveAccessToPortfolio_ThrowsUnauthorizedResourceAccessException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = _fixture.Create<SellStockRequest>();
            var portfolio = new Portfolio { UserId = _fixture.Create<string>() };
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);

            // Act
            Func<Task> act = async () => await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedResourceAccessException>();
        }

        [Fact]
        public async Task SellStockAsync_WhenInsufficientSharesToSell_ThrowsInsufficientSharesException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = _fixture.Create<SellStockRequest>();
            var portfolio = new Portfolio { UserId = userId };
            var holding = new Holding { Quantity = sellStockReq.Quantity - 1 };
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _holdingRepositoryMock.Setup(x => x.GetHoldingByTicker(portfolioId, sellStockReq.Ticker)).ReturnsAsync(holding);

            // Act
            Func<Task> act = async () => await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            await act.Should().ThrowAsync<InsufficientSharesException>();
        }

        [Fact]
        public async Task SellStockAsync_WhenStockDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = _fixture.Create<SellStockRequest>();
            var portfolio = new Portfolio { UserId = userId };
            var holding = new Holding { Quantity = sellStockReq.Quantity };
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _holdingRepositoryMock.Setup(x => x.GetHoldingByTicker(portfolioId, sellStockReq.Ticker)).ReturnsAsync(holding);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(sellStockReq.Ticker)).ReturnsAsync(null as StockQuote);

            // Act
            Func<Task> act = async () => await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task SellStockAsync_WhenValidArguments_ReturnsTransactionResponse()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = _fixture.Create<SellStockRequest>();
            var portfolio = new Portfolio { UserId = userId };
            var holding = new Holding { Quantity = sellStockReq.Quantity };
            var quote = new StockQuote { c = _fixture.Create<float>() };

            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _holdingRepositoryMock.Setup(x => x.GetHoldingByTicker(portfolioId, sellStockReq.Ticker)).ReturnsAsync(holding);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(sellStockReq.Ticker)).ReturnsAsync(quote);

            // Act
            var result = await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            result.Should().NotBeNull();
            result!.Quantity.Should().Be(sellStockReq.Quantity);
            result.Ticker.Should().Be(sellStockReq.Ticker);
            result.Price.Should().Be((decimal)quote.c);
            result.Type.Should().Be(TransactionType.Sell);
            result.TransactionDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public async Task SellStockAsync_WhenValidArguments_CreatesTransaction()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var sellStockReq = _fixture.Create<SellStockRequest>();
            var portfolio = new Portfolio { UserId = userId };
            var holding = new Holding { Quantity = sellStockReq.Quantity };
            var quote = new StockQuote { c = _fixture.Create<float>() };
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _holdingRepositoryMock.Setup(x => x.GetHoldingByTicker(portfolioId, sellStockReq.Ticker)).ReturnsAsync(holding);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(sellStockReq.Ticker)).ReturnsAsync(quote);

            // Act
            await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            _transactionRepositoryMock.Verify(x => x.CreateTransactionAsync(It.Is<Transaction>(t =>
                t.PortfolioId == portfolioId &&
                t.Type == TransactionType.Sell &&
                t.Ticker == sellStockReq.Ticker &&
                t.Quantity == sellStockReq.Quantity &&
                t.Price == (decimal)quote.c)), Times.Once);
        }

        [Fact]
        public async Task SellStockAsync_WhenValidArguments_UpdatesHolding()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            decimal holdingQuantity = 25;
            decimal sellQuantity = 10;
            var sellStockReq = _fixture.Build<SellStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, sellQuantity)
                .Create();
            var portfolio = new Portfolio { UserId = userId };
            var quote = new StockQuote { c = _fixture.Create<float>() };
            var holding = _fixture.Build<Holding>()
                .With(x => x.PortfolioId, portfolioId)
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, holdingQuantity)
                .With(x => x.AverageCostBasis, _fixture.Create<decimal>())
                .Create();

            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _holdingRepositoryMock.Setup(x => x.GetHoldingByTicker(portfolioId, sellStockReq.Ticker)).ReturnsAsync(holding);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(sellStockReq.Ticker)).ReturnsAsync(quote);

            var newQuantity = holding.Quantity - sellStockReq.Quantity;

            // Act
            await _sellStockService.SellStockAsync(userId, portfolioId, sellStockReq);

            // Assert
            _holdingRepositoryMock.Verify(x => x.UpdateHoldingAsync(It.Is<Holding>(h =>
                h.PortfolioId == portfolioId &&
                h.Ticker == sellStockReq.Ticker &&
                h.Quantity == newQuantity)), Times.Once);
        }
    }
}
