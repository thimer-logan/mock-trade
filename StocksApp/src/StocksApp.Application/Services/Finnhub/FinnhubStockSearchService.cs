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

        public async Task<IEnumerable<StockSearch>?> SearchStocks(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return await _finnhubRepository.SearchStocksAsync(symbol);
        }
    }
}
