using FluentAssertions;
using Moq;
using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Application.Services.Finnhub;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Finnhub
{
    public class FinnhubStockPriceQuoteServiceTest
    {
        private readonly IFinnhubStockPriceQuoteService _service;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

        public FinnhubStockPriceQuoteServiceTest()
        {
            _finnhubRepositoryMock = new Mock<IFinnhubRepository>();
            _service = new FinnhubStockPriceQuoteService(_finnhubRepositoryMock.Object);
        }

        [Fact]
        public async Task GetStockPriceQuote_WhenSymbolIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            string symbol = string.Empty;

            // Act
            Func<Task> act = async () => await _service.GetStockPriceQuote(symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetStockPriceQuote_WhenSymbolIsWhiteSpace_ThrowsArgumentNullException()
        {
            // Arrange
            string symbol = " ";

            // Act
            Func<Task> act = async () => await _service.GetStockPriceQuote(symbol);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetStockPriceQuote_ValidSymbol_CallsGetStockPriceQuoteAsync()
        {
            // Arrange
            string symbol = "AAPL";

            // Act
            await _service.GetStockPriceQuote(symbol);

            // Assert
            _finnhubRepositoryMock.Verify(x => x.GetStockPriceQuoteAsync(symbol), Times.Once);
        }
    }
}
