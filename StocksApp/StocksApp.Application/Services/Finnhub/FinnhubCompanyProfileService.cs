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
            try
            {
                return await _finnhubRepository.GetCompanyProfileAsync(symbol);
            }
            catch (Exception ex)
            {
                //FinnhubException finnhubException = new FinnhubException("Unable to connect to finnhub", ex);
                //throw finnhubException;
            }

            return null;
        }
    }
}
