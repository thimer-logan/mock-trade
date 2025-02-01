using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Application.Services.Finnhub;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Finnhub
{
    public class FinnhubStockSearchServiceTest
    {
        private readonly IFinnhubStockSearchService _service;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

        public FinnhubStockSearchServiceTest()
        {
            _finnhubRepositoryMock = new Mock<IFinnhubRepository>();
            _service = new FinnhubStockSearchService(_finnhubRepositoryMock.Object);
        }

        [Fact]
        public async Task SearchStocks_WhenSymbolIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            string symbol = string.Empty;

            // Act
            Func<Task> act = async () => await _service.SearchStocks(symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SearchStocks_WhenSymbolIsWhiteSpace_ThrowsArgumentNullException()
        {
            // Arrange
            string symbol = " ";

            // Act
            Func<Task> act = async () => await _service.SearchStocks(symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SearchStocks_ValidSymbol_CallsSearchStocksAsync()
        {
            // Arrange
            string symbol = "AAPL";

            // Act
            await _service.SearchStocks(symbol);

            // Assert
            _finnhubRepositoryMock.Verify(x => x.SearchStocksAsync(symbol), Times.Once);
        }
    }
}
