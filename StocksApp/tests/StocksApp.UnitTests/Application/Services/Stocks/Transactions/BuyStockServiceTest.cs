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
    public class BuyStockServiceTest
    {
        private readonly IBuyStockService _buyStockService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;
        private readonly Mock<IHoldingRepository> _holdingRepositoryMock;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

        private readonly IFixture _fixture;

        public BuyStockServiceTest()
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

            _buyStockService = new BuyStockService(_transactionRepositoryMock.Object, _portfolioRepositoryMock.Object,
                _finnhubRepositoryMock.Object, _holdingRepositoryMock.Object);
        }

        [Fact]
        public async Task BuyStockAsync_UserIdInvalid_ThrowsBadRequestException()
        {
            // Arrange
            var userId = string.Empty;
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();

            // Act
            Func<Task> act = async () => await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task BuyStockAsync_WhenPortfolioIdIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = Guid.Empty;
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();

            // Act
            Func<Task> act = async () => await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task BuyStockAsync_TickerIsNull_ThrowsBadRequestException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, null as string)
                .With(x => x.Quantity, 1)
                .Create();

            // Act
            Func<Task> act = async () => await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task BuyStockAsync_WhenPortfolioDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(null as Portfolio);

            // Act
            Func<Task> act = async () => await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task BuyStockAsync_UserDoesNotHaveAccessToPortfolio_ThrowsUnauthorizedResourceAccessException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = _fixture.Create<string>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);

            // Act
            Func<Task> act = async () => await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedResourceAccessException>();
        }

        [Fact]
        public async Task BuyStockAsync_WhenStockDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(ticker)).ReturnsAsync(null as StockQuote);

            // Act
            Func<Task> act = async () => await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task BuyStockAsync_WhenStockExists_CreatesTransaction()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            var stockQuote = _fixture.Create<StockQuote>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(ticker)).ReturnsAsync(stockQuote);

            // Act
            var result = await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().NotBeEmpty();
            result.Type.Should().Be(TransactionType.Buy);
            result.Ticker.Should().Be(ticker);
            result.Quantity.Should().Be(quantity);
            result.Price.Should().Be((decimal)stockQuote.c);
            result.TransactionDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public async Task BuyStockAsync_WhenStockExists_CreatesNewHolding()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;

            var stockQuote = _fixture.Create<StockQuote>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(ticker)).ReturnsAsync(stockQuote);

            // Act
            await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            _holdingRepositoryMock.Verify(x => x.CreateHoldingAsync(It.Is<Holding>(h =>
                h.PortfolioId == portfolioId &&
                h.Ticker == ticker &&
                h.Quantity == quantity &&
                h.AverageCostBasis == (decimal)stockQuote.c)), Times.Once);
        }

        [Fact]
        public async Task BuyStockAsync_WhenStockExistsAndHoldingExists_UpdatesHolding()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();

            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;

            var stockQuote = _fixture.Create<StockQuote>();
            var holding = _fixture.Build<Holding>()
                .With(x => x.PortfolioId, portfolioId)
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, _fixture.Create<decimal>())
                .With(x => x.AverageCostBasis, _fixture.Create<decimal>())
                .Create();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(ticker)).ReturnsAsync(stockQuote);
            _holdingRepositoryMock.Setup(x => x.GetHoldingByTicker(portfolioId, ticker)).ReturnsAsync(holding);

            var newQuantity = holding.Quantity + quantity;
            var newAverageCostBasis = ((holding.Quantity * holding.AverageCostBasis) + (quantity * (decimal)stockQuote.c)) / newQuantity;

            // Act
            await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            _holdingRepositoryMock.Verify(x => x.UpdateHoldingAsync(It.Is<Holding>(h =>
                h.PortfolioId == portfolioId &&
                h.Ticker == ticker &&
                h.Quantity == newQuantity &&
                h.AverageCostBasis == newAverageCostBasis)), Times.Once);
        }

        [Fact]
        public async Task BuyStockAsync_WhenStockExistsAndHoldingExists_CreatesTransaction()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var ticker = _fixture.Create<string>();
            var quantity = _fixture.Create<decimal>();
            var buyStockReq = _fixture.Build<BuyStockRequest>()
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, quantity)
                .Create();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            var stockQuote = _fixture.Create<StockQuote>();
            var holding = _fixture.Build<Holding>()
                .With(x => x.PortfolioId, portfolioId)
                .With(x => x.Ticker, ticker)
                .With(x => x.Quantity, _fixture.Create<decimal>())
                .With(x => x.AverageCostBasis, _fixture.Create<decimal>())
                .Create();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _finnhubRepositoryMock.Setup(x => x.GetStockPriceQuoteAsync(ticker)).ReturnsAsync(stockQuote);
            _holdingRepositoryMock.Setup(x => x.GetHoldingByTicker(portfolioId, ticker)).ReturnsAsync(holding);

            // Act
            await _buyStockService.BuyStockAsync(userId, portfolioId, buyStockReq);

            // Assert
            _transactionRepositoryMock.Verify(x => x.CreateTransactionAsync(It.Is<Transaction>(t =>
                t.PortfolioId == portfolioId &&
                t.Type == TransactionType.Buy &&
                t.Ticker == ticker &&
                t.Quantity == quantity &&
                t.Price == (decimal)stockQuote.c)), Times.Once);
        }
    }
}
