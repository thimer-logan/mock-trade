using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Domain.Interfaces;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.Services.Finnhub
{
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository)
        {
            _finnhubRepository = finnhubRepository;
        }

        public async Task<CompanyProfile?> GetCompanyProfile(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return await _finnhubRepository.GetCompanyProfileAsync(symbol);
        }
    }
}
