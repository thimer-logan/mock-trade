using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Domain.Interfaces;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Services.Finnhub
{
    public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<StockQuote?> GetStockPriceQuote(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return await _finnhubRepository.GetStockPriceQuoteAsync(symbol);
        }
    }
}
