using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Domain.Interfaces;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Services.Finnhub
{
    public class FinnhubStockSearchService : IFinnhubStockSearchService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubStockSearchService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public Task<StockSearch?> SearchStocks(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}
