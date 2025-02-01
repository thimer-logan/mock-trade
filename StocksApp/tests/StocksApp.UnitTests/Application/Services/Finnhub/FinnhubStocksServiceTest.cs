using Moq;
using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Application.Services.Finnhub;
using StocksApp.Domain.Interfaces;

namespace StocksApp.UnitTests.Application.Services.Finnhub
{
    public class FinnhubStocksServiceTest
    {
        private readonly IFinnhubStocksService _service;
        private readonly Mock<IFinnhubRepository> _finnhubRepositoryMock;

        public FinnhubStocksServiceTest()
        {
            _finnhubRepositoryMock = new Mock<IFinnhubRepository>();
            _service = new FinnhubStocksService(_finnhubRepositoryMock.Object);
        }

        [Fact]
        public async Task GetStocks_CallsGetStocksAsync()
        {
            // Act
            await _service.GetStocks();

            // Assert
            _finnhubRepositoryMock.Verify(x => x.GetStocksAsync(), Times.Once);
        }
    }
}
