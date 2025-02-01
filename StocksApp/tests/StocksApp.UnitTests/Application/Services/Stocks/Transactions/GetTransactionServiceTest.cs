using AutoFixture;
using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Stocks.Transactions;
using StocksApp.Application.Services.Stocks.Transactions;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Exceptions;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Stocks.Transactions
{
    public class GetTransactionServiceTest
    {
        private readonly IGetTransactionService _getTransactionService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IPortfolioRepository> _portfolioRepositoryMock;

        private readonly IFixture _fixture;

        public GetTransactionServiceTest()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _portfolioRepositoryMock = new Mock<IPortfolioRepository>();

            _getTransactionService = new GetTransactionService(_transactionRepositoryMock.Object, _portfolioRepositoryMock.Object);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_WhenTransactionExistsAndUserHasAccess_ReturnsTransactionResponse()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var transactionId = _fixture.Create<Guid>();
            var transaction = _fixture.Create<Transaction>();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            transaction.PortfolioId = portfolioId;
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _transactionRepositoryMock.Setup(x => x.GetTransactionByIdAsync(transactionId)).ReturnsAsync(transaction);

            // Act
            var result = await _getTransactionService.GetTransactionByIdAsync(userId, portfolioId, transactionId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(transaction.Id);
            result.Ticker.Should().Be(transaction.Ticker);
            result.Quantity.Should().Be(transaction.Quantity);
            result.Price.Should().Be(transaction.Price);
        }

        [Fact]
        public async Task GetTransactionByIdAsync_WhenTransactionDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var transactionId = _fixture.Create<Guid>();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _transactionRepositoryMock.Setup(x => x.GetTransactionByIdAsync(transactionId)).ReturnsAsync(null as Transaction);

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionByIdAsync(userId, portfolioId, transactionId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetTransactionByIdAsync_WhenPortfolioDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var transactionId = _fixture.Create<Guid>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(null as Portfolio);

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionByIdAsync(userId, portfolioId, transactionId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetTransactionByIdAsync_UserDoesNotHaveAccessToPortfolio_ThrowsUnauthorizedResourceAccessException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var transactionId = _fixture.Create<Guid>();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = _fixture.Create<string>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionByIdAsync(userId, portfolioId, transactionId);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedResourceAccessException>();
        }

        [Fact]
        public async Task GetTransactionByIdAsync_InvalidUserId_ThrowsBadRequestException()
        {
            // Arrange
            var userId = string.Empty;
            var portfolioId = _fixture.Create<Guid>();
            var transactionId = _fixture.Create<Guid>();

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionByIdAsync(userId, portfolioId, transactionId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task GetTransactionByIdAsync_InvalidPortfolioId_ThrowsBadRequestException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = Guid.Empty;
            var transactionId = _fixture.Create<Guid>();

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionByIdAsync(userId, portfolioId, transactionId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task GetTransactionByIdAsync_InvalidTransactionId_ThrowsBadRequestException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var transactionId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionByIdAsync(userId, portfolioId, transactionId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task GetTransactionsAsync_WhenPortfolioExistsAndUserHasAccess_ReturnsTransactionResponses()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var transactions = _fixture.CreateMany<Transaction>().ToList();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = userId;
            transactions.ForEach(t => t.PortfolioId = portfolioId);
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);
            _transactionRepositoryMock.Setup(x => x.GetTransactionsByPortfolioAsync(portfolioId)).ReturnsAsync(transactions);

            // Act
            var result = await _getTransactionService.GetTransactionsAsync(userId, portfolioId);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(transactions.Count);
            result.Should().BeEquivalentTo(transactions, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetTransactionsAsync_WhenPortfolioDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(null as Portfolio);

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionsAsync(userId, portfolioId);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetTransactionsAsync_DoesNotHaveAccessToPortfolio_ThrowsUnauthorizedResourceAccessException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = _fixture.Create<Guid>();
            var portfolio = _fixture.Create<Portfolio>();
            portfolio.UserId = _fixture.Create<string>();
            _portfolioRepositoryMock.Setup(x => x.GetPortfolioAsync(portfolioId)).ReturnsAsync(portfolio);

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionsAsync(userId, portfolioId);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedResourceAccessException>();
        }

        [Fact]
        public async Task GetTransactionsAsync_InvalidUserId_ThrowsBadRequestException()
        {
            // Arrange
            var userId = string.Empty;
            var portfolioId = _fixture.Create<Guid>();

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionsAsync(userId, portfolioId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task GetTransactionsAsync_InvalidPortfolioId_ThrowsBadRequestException()
        {
            // Arrange
            var userId = _fixture.Create<string>();
            var portfolioId = Guid.Empty;

            // Act
            Func<Task> act = async () => await _getTransactionService.GetTransactionsAsync(userId, portfolioId);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }
    }
}
