using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Domain.Interfaces;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Services.Finnhub
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubStocksService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<IEnumerable<Stock>?> GetStocks()
        {
            try
            {
                return await _finnhubRepository.GetStocksAsync();
            }
            catch (Exception ex)
            {
                // Log error
                return null;
            }
        }
    }
}
